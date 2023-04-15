namespace Knowledge.DDD.Demo.Kernel.Extensions;

public static class ObjectExtensions
{
    public static T EnsureNotNull<T>(this T? obj) => obj ?? throw new ArgumentNullException(nameof(obj));
}