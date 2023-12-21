using System.Linq;

namespace HubtelCommerce.Utils
{
    public static class MathOperandContants
    {
        public const string GreaterThan= "GT";
        public const string LessThan= "LT";
        public const string GreaterThanOrEqual= "GTE";
        public const string LessThanOrEqual= "lTE";
        public const string BetweenInclusive= "BI";
        public const string BetweenExclusive= "BE";
        public const string NotEqual= "NE";
        public const string Equal= "EE";

        public static List<string> All = new() 
        { 
            NotEqual,
            BetweenExclusive,
            BetweenInclusive,
            LessThanOrEqual,
            LessThan,
            GreaterThanOrEqual,
            GreaterThan 
        };

        public static string ToStringList()
        {
            return string.Join(", ",All);
        }
    }
}
