namespace ImdbClone.Api.Utils;

public static class StringHelper
{
    public static string ToSnakeCase(string input)
    {
        return string.Concat(
                input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())
            )
            .ToLower();
    }

    public static string ToPascalCase(string input)
    {
        return string.Concat(
            input.Split('_').Select(word => char.ToUpper(word[0]) + word.Substring(1))
        );
    }
}
