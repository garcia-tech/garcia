namespace GarciaCore.Domain;

public interface IEntity
{
    int Id { get; set; }
    bool Active { get; set; }
    bool Deleted { get; set; }
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset DeletedOn { get; set; }
}