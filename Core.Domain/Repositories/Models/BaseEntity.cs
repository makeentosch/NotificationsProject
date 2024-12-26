namespace Core.Domain.Repositories.Models;

public abstract class BaseEntity 
{
    public DateTime DateOfCreate { get; set; }
    
    public DateTime DateOfUpdate { get; set; }
}