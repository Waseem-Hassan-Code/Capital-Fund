public class FileContent
{
    public byte[] Content { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }

    public FileContent(byte[] content, string contentType, string fileName)
    {
        Content = content;
        ContentType = contentType;
        FileName = fileName;
    }
}

public static class ContentTypeHelper
{
    // Dictionary to map file extensions to MIME types
    private static readonly Dictionary<string, string> _contentTypes = new Dictionary<string, string>
    {
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".gif", "image/gif" },
        { ".bmp", "image/bmp" },
        { ".tiff", "image/tiff" },
        { ".svg", "image/svg+xml" },
        // Add more as needed
    };

    // Method to get content type based on file extension
    public static string GetContentType(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "application/octet-stream"; // Default to binary if no file name is given
        }

        // Extract the file extension
        var fileExtension = System.IO.Path.GetExtension(fileName)?.ToLower();

        // Return the corresponding MIME type or default to "application/octet-stream"
        if (_contentTypes.TryGetValue(fileExtension, out var contentType))
        {
            return contentType;
        }

        return "application/octet-stream"; // Default for unknown types
    }
}
