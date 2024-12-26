using System.ComponentModel.DataAnnotations;

namespace RabbitMqContracts.Mail.Dtos;

public class AttachmentDto
{
    [MaxLength(128)]
    public required string FileName { get; set; }
    
    public required string FileUri { get; set; }
    
    public required string ContentType { get; set; }
}