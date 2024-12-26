using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Repositories.Models;

public abstract class EntityBase<TKey> : BaseEntity
{
    [Key]
    public TKey Id { get; set; }
    
    protected EntityBase(TKey id, DateTime dateOfCreate)
    {
        Id = id;
        DateOfCreate = dateOfCreate;
        DateOfUpdate = dateOfCreate;
    }
}
