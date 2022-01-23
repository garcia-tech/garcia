using Dapper;
using GarciaCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GarciaCore.Domain;

namespace GarciaCore.ORM;

public class GarciaCoreRepository
{
    protected List<Type> cachingEnabledTypes = new List<Type>()
    {
    };

    public bool CachingEnabled
    {
        get
        {
            return _settings.CacheExpirationTimeInMinutes != 0;
        }
    }

    protected Settings _settings;
    protected GarciaCoreMemoryCache cache;
    protected DatabaseConnection _databaseConnection;
    protected DatabaseSettings _databaseSettings;

    protected GarciaCoreRepository(IOptions<Settings> settings, IOptions<DatabaseSettings> databaseSettings)
    {
        _settings = settings.Value;
        _databaseSettings = databaseSettings.Value;
        cache = new GarciaCoreMemoryCache(settings);
        _databaseConnection = new DatabaseConnectionFactory(databaseSettings).GetConnection();
    }

    public async Task InitializeCacheAsync<T>()
        where T : Entity
    {
        if (this.IsCachingEnabled<T>())
        {
            var items = await GetItemsFromDbAsync<T>(null);
            cache.SetItem(typeof(T).Name, items);
        }
    }

    public void ClearCache()
    {
        foreach (var type in cachingEnabledTypes)
        {
            cache.Remove(type.Name);
        }
    }

    protected virtual bool IsCachingEnabled<T>()
        where T : Entity
    {
        return CachingEnabled && cachingEnabledTypes.Contains(typeof(T));
    }

    public virtual async Task<List<T>> GetItemsAsync<T>(Dictionary<string, object> parameters)
        where T : Entity
    {
        if (IsCachingEnabled<T>())
        {
            return await GetItemsFromCacheAsync<T>(parameters);
        }

        return await GetItemsFromDbAsync<T>(parameters);
    }

    public virtual async Task<List<T>> GetItemsAsync<T>(string key, object value)
        where T : Entity
    {
        var parameters = new Dictionary<string, object>();
        parameters.Add(key, value);
        return await GetItemsAsync<T>(parameters);
    }

    public virtual async Task<List<T>> GetItemsAsync<T>()
        where T : Entity
    {
        return await GetItemsAsync<T>(null);
    }

    public virtual async Task<T> GetItemAsync<T>(int id)
        where T : Entity
    {
        if (id == 0)
        {
            return null;
        }

        var parameters = new Dictionary<string, object>();
        parameters.Add("Id", id);

        if (IsCachingEnabled<T>())
        {
            return (await GetItemsFromCacheAsync<T>(parameters)).FirstOrDefault();
        }
        else
        {
            return (await GetItemsFromDbAsync<T>(parameters)).FirstOrDefault();
        }
    }

    protected virtual async Task<List<T>> GetItemsFromDbAsync<T>(Dictionary<string, object> parameters, string order = null)
        where T : Entity
    {
        var items = new List<T>();
        var sql = new GarciaCoreStringBuilder("select * from ");
        var values = new GarciaCoreStringBuilder();
        sql += _databaseConnection.TablePrefix;
        sql += typeof(T).Name;
        sql += _databaseConnection.TablePrefix;
        sql += string.Format(" where {0}DeleteTime{0} is null", _databaseConnection.TablePrefix);

        if (parameters != null && parameters.Count != 0)
        {
            sql += " and ";
            var ienum = parameters.GetEnumerator();
            var count = 0;

            while (ienum.MoveNext())
            {
                var key = ienum.Current.Key;
                sql += _databaseConnection.TablePrefix;
                sql += key;
                sql += _databaseConnection.TablePrefix;
                sql += "=@";
                sql += key;
                values += key;

                if (count != parameters.Count - 1)
                {
                    sql += " and ";
                }

                count++;
            }
        }

        if (!string.IsNullOrEmpty(order))
        {
            sql += " order by ";
            sql += _databaseConnection.TablePrefix;
            sql += order;
            sql += _databaseConnection.TablePrefix;
            sql += " ";
        }
        else
        {
            sql += " order by ";
            sql += _databaseConnection.IdKeyword;
            sql += " ";
        }

        using (var connection = _databaseConnection.CreateConnection())
        {
            connection.Open();
            items = (await connection.QueryAsync<T>(sql.ToString(), parameters)).ToList();
            connection.Close();
        }

        return items;
    }

    protected virtual async Task<List<T>> GetItemsFromCacheAsync<T>()
        where T : Entity
    {
        if (!IsCachingEnabled<T>())
        {
            return await GetItemsFromDbAsync<T>(null);
        }

        var typeName = typeof(T).Name;

        if (cache.Get<List<T>>(typeName) == null)
        {
            var items = await GetItemsFromDbAsync<T>(null);
            cache.SetItem(typeName, items);
        }

        return cache.Get<List<T>>(typeName);
    }

    protected virtual async Task<List<T>> GetItemsFromCacheAsync<T>(Dictionary<string, object> Parameters)
        where T : Entity
    {
        var AllItems = await GetItemsFromCacheAsync<T>();

        if (Parameters == null || Parameters.Count == 0)
        {
            return AllItems;
        }

        var FilteredItems = new List<T>();

        foreach (Entity item in AllItems)
        {
            var addItem = true;
            IDictionaryEnumerator ienum = Parameters.GetEnumerator();

            while (ienum.MoveNext())
            {
                var p = item.GetType().GetProperty(ienum.Key.ToString(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (p == null)
                {
                    p = item.GetType().GetProperty(ienum.Key.ToString().Replace("Id", ""), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }

                if (p == null)
                {
                    throw new InvalidOperationException();
                }

                var value = p.GetValue(item, null);

                if (value == null || !value.Equals(ienum.Value))
                {
                    addItem = false;
                    break;
                }
            }

            if (addItem)
            {
                FilteredItems.Add(item as T);
            }
        }

        return FilteredItems;
    }

    public async Task<OperationResult<T>> SaveAsync<T>(T item)
        where T : Entity
    {
        string script = string.Empty;
        var parameters = this.GetParameters(item);
        bool success = false;

        if (item.Id == 0)
        {
            script = this.GenerateInsertScript(item);

            using (var connection = _databaseConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    //SqlCommand command = new SqlCommand(script.ToString(), connection);
                    //IDbCommand command = DatabaseConnectionFactory.CreateCommand(script.ToString(), connection);
                    //command.Parameters.AddRange(parameters.Select(x => new SqlParameter(x.Key, x.Value)).ToArray());
                    //var command = _databaseConnection.CreateCommand(script.ToString(), connection, parameters);
                    //object idString = command.ExecuteScalar();
                    var idString = await connection.ExecuteScalarAsync(script.ToString(), parameters);
                    var id = Helpers.GetValueFromObject<int>(idString);
                    item.Id = id;
                    success = id != 0;
                }
                catch (Exception ex)
                {
                    return new OperationResult<T>(item, ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        else if (item.IsDeleted)
        {
            script = this.GenerateDeleteScript(item);

            using (var connection = _databaseConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    //SqlCommand command = new SqlCommand(script.ToString(), connection);
                    //command.Parameters.AddWithValue("Id", item.Id);
                    //var command = _databaseConnection.CreateCommand(script.ToString(), connection, new Dictionary<string, object>() { { "Id", item.Id } });
                    //int affectedRows = command.ExecuteNonQuery();
                    var affectedRows = await connection.ExecuteAsync(script.ToString(), new Dictionary<string, object>() { { "Id", item.Id } });
                    success = affectedRows != 0;
                }
                catch (Exception ex)
                {
                    return new OperationResult<T>(item, ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        else
        {
            script = this.GenerateUpdateScript(item);
            parameters.Add("@Id", item.Id);

            using (var connection = _databaseConnection.CreateConnection())
            {
                try
                {
                    connection.Open();
                    //SqlCommand command = new SqlCommand(script.ToString(), connection);
                    //command.Parameters.AddRange(parameters.Select(x => new SqlParameter(x.Key, x.Value)).ToArray());
                    //var command = _databaseConnection.CreateCommand(script.ToString(), connection, parameters);
                    //int affectedRows = command.ExecuteNonQuery();
                    var affectedRows = await connection.ExecuteAsync(script.ToString(), parameters);
                    success = affectedRows != 0;
                }
                catch (Exception ex)
                {
                    return new OperationResult<T>(item, ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        return new OperationResult<T>(item) { Success = success };
    }

    protected virtual string GenerateDeleteScript<T>(T item) where T : Entity
    {
        var sb = new GarciaCoreStringBuilder("update ");
        string entityTypeName = this.GetTypeName(item);
        sb += entityTypeName;
        sb += string.Format(" set {0}DeleteTime{0} = ", _databaseConnection.TablePrefix);
        sb += _databaseConnection.GetDateSqlStatement;
        sb += string.Format(" where {0}Id{0} = @Id and {0}DeleteTime{0} is null", _databaseConnection.TablePrefix);
        return sb.ToString();
    }

    protected virtual PropertyInfo[] GetProperties(Entity Item)
    {
        var properties = Item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        return properties.Where(x => !Attribute.IsDefined(x, typeof(NotSavedAttribute)) && !x.PropertyType.IsIList() && !x.PropertyType.IsArray && x.CanRead && x.CanWrite &&
            (x.PropertyType.IsPrimitive || x.PropertyType.IsEnum || x.PropertyType.Equals(typeof(String)) || x.PropertyType.Equals(typeof(Decimal)) || (Nullable.GetUnderlyingType(x.PropertyType) != null && (Nullable.GetUnderlyingType(x.PropertyType).IsPrimitive || Nullable.GetUnderlyingType(x.PropertyType).IsEnum || Nullable.GetUnderlyingType(x.PropertyType).Equals(typeof(Decimal)))))
            && x.Name != "Id").ToArray();
    }

    private string GetTypeName(object item)
    {
        //return TablePrefix + item.GetType().Name.ToLowerInvariant() + TablePrefix;
        return _databaseConnection.TablePrefix + item.GetType().Name.PrepareName() + _databaseConnection.TablePrefix;
    }

    private string GenerateInsertScript(Entity item)
    {
        string script = String.Empty;
        PropertyInfo[] properties = this.GetProperties(item);
        //properties = properties.Where(x => !Attribute.IsDefined(x, typeof(NotSelectedAttribute))).ToArray();
        var sb = new GarciaCoreStringBuilder(_databaseConnection.BeginTranSqlStatement);
        sb += "insert into ";
        string entityTypeName = this.GetTypeName(item);
        //sb += ColumnPrefix;
        sb += entityTypeName;
        //sb += ColumnPpostfix;
        sb += " (";
        var propertyNames = new GarciaCoreStringBuilder();
        var values = new GarciaCoreStringBuilder();

        foreach (var property in properties)
        {
            propertyNames += _databaseConnection.ColumnPrefix;
            propertyNames += property.Name.PrepareName();
            propertyNames += _databaseConnection.ColumnPpostfix;
            propertyNames += ",";
            values += "@";
            values += property.Name.PrepareName();
            values += ", ";
        }

        sb += propertyNames.ToString().Trim().TrimEnd(',');
        sb += ") values (";
        sb += values.ToString().Trim().TrimEnd(',');
        sb += ")";
        sb += _databaseConnection.IdentitySqlStatement;
        sb += _databaseConnection.CommitTranSqlStatement;
        script = sb.ToString();
        return script;
    }

    private string GenerateUpdateScript(Entity item)
    {
        string script = String.Empty;
        PropertyInfo[] properties = this.GetProperties(item);
        var sb = new GarciaCoreStringBuilder("update ");
        string entityTypeName = this.GetTypeName(item);
        sb += entityTypeName;
        sb += " set ";
        var propertyNames = new GarciaCoreStringBuilder();
        var values = new GarciaCoreStringBuilder();

        foreach (var property in properties)
        {
            propertyNames += _databaseConnection.ColumnPrefix;
            propertyNames += property.Name.PrepareName();
            propertyNames += _databaseConnection.ColumnPpostfix;
            propertyNames += "=@";
            propertyNames += property.Name.PrepareName();
            propertyNames += ", ";
        }

        sb += propertyNames.ToString().Trim().TrimEnd(',');
        sb += string.Format(" where {0}Id{0}=@Id", _databaseConnection.TablePrefix);
        script = sb.ToString();
        return script;
    }

    internal virtual Dictionary<string, object> GetParameters(Entity t)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        PropertyInfo[] properties = this.GetProperties(t);

        foreach (PropertyInfo propertyInfo in properties)
        {
            object propertyValue = propertyInfo.GetValue(t);
            var builder = new GarciaCoreStringBuilder("@");
            builder += propertyInfo.Name;
            string parameterName = builder.ToString();

            if (propertyInfo.PropertyType.IsEnum)
                propertyValue = (int)propertyValue;

            if (!parameters.ContainsKey(parameterName))
                parameters.AddParameterValue(parameterName, propertyValue);
        }

        return parameters;
    }
}