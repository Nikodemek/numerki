using System.Globalization;

namespace Zadanie2.Utils;

public static class Parser
{
    public static double ToDouble(string s)
    {
        return Double.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
    }
}
