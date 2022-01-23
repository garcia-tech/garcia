using GarciaCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarciaCore.ORM;

public class BaseRepository<T> : IAsyncRepository<T> where T : Entity
{
    public BaseRepository()
    {
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<IReadOnlyList<T>> ListAllAsync()
    {
        throw new NotImplementedException();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<T> SaveAsync(T entity)
    {
        if (entity.Id == 0)
        {
            await AddAsync(entity);
        }
        else if (entity.IsDeleted)
        {
            await DeleteAsync(entity);
        }
        else
        {
            await UpdateAsync(entity);
        }

        return entity;
    }

    public virtual async Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size)
    {
        throw new NotImplementedException();
    }
}
