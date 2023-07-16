namespace Conesoft.Website.Utilities;

public static class TokenGenerator
{
    static string TokenPieces(int pieces) => "0123456789abcdefghijklmnopqrstuvwxyz".Pick(pieces, distinct: true);
    static string Prettify(string token) => $"cnsft-token-{token[..5]}-{token[5..]}";
    static public string CreateToken() => Prettify(TokenPieces(10));
}


static class RandomExtensions
{
    public static T[] Pick<T>(this T[] from, int amount, bool distinct = false, Random? random = null)
    {
        random ??= Random.Shared;
        var result = new T[amount];
        if (distinct == false)
        {
            for (var i = 0; i < amount; i++)
            {
                result[i] = from[random.Next(from.Length)];
            }
            return result;
        }
        else
        {
            return from.Shuffle(amount);
        }
    }

    public static T[] Shuffle<T>(this T[] from, int amount = 0, Random? random = null)
    {
        random ??= Random.Shared;

        var copy = from[..];
        amount = amount > 0 ? amount : copy.Length;
        for (var i = 0; i < amount; ++i)
        {
            var r = random.Next(i, from.Length);
            (copy[i], copy[r]) = (copy[r], copy[i]);
        }
        return copy[..amount];
    }

    public static string Pick(this string from, int amount, bool distinct = false, Random? random = null)
    {
        return string.Concat(from.ToCharArray().Pick(amount, distinct, random));
    }

    public static string Shuffle(this string from, int amount = 0, Random? random = null)
    {
        return string.Concat(from.ToCharArray().Shuffle(amount, random));
    }
}
