namespace Knowledge.DDD.Demo.Kernel.Guards;

public static class Check
{
    public static T NotNull<T>(T input) where T : notnull => input ?? throw new ArgumentNullException(nameof(input));
    
    public static void NotNullOrWhiteSpace(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentNullException(nameof(input));
        }
    }
    
    public static void Matches<T>(T input, Func<T, bool> predicate) where T : notnull
    {
        NotNull(input);

        var matched = predicate(input);
        if (!matched)
        {
            throw new ArgumentNullException(nameof(input));
        }
    }


}