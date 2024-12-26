namespace Notification.Domain.Models;

public class Attachment
{
    public string FileName { get; private set; }
    public string FileUri { get; private set; }
    public string ContentType { get; private set; }

    public Attachment(string fileName, string fileUri, string contentType)
    {
        FileName = fileName;
        FileUri = fileUri;
        ContentType = contentType;
    }
}