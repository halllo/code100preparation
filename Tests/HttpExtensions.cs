using System.Text;
using System.Text.Json;

public static class HttpExtensions 
{
    public static StringContent AsJsonBody<T>(this T t)
    {
        return new StringContent(JsonSerializer.Serialize(t, typeof(T)), Encoding.UTF8, "application/json");
    }
}