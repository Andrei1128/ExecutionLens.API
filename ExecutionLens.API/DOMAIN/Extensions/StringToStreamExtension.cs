using System.Text;

namespace ExecutionLens.API.DOMAIN.Extensions;

public static class StringToStreamExtension
{
    public static Stream GetStream(this string content, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var byteArray = encoding.GetBytes(content);
        var memoryStream = new MemoryStream(byteArray);
        return memoryStream;
    }
}
