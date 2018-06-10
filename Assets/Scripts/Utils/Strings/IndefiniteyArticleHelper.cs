using System.Linq;

namespace BattleCruisers.Utils.Strings
{
    public static class IndefiniteyArticleHelper
    {
        public static string FindIndefiniteArticle(string noun)
        {
            switch (noun.FirstOrDefault())
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    return "an";
                default:
                    return "a";
            }
        }
    }
}
