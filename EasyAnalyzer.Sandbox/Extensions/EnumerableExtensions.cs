namespace EasyAnalyzer.Sandbox.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> FilterNull<T>(this IEnumerable<T> source)
    {
        return source.Where(x => x != null);
    }
}