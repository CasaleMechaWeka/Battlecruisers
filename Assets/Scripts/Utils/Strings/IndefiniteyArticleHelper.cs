using System.Linq;

namespace BattleCruisers.Utils.Strings
{
    public static class IndefiniteyArticleHelper
    {
        public static string FindIndefiniteArticle(string noun)
        {
            switch (noun.ToUpper().FirstOrDefault())
            {
                case 'A':
                case 'E':
                case 'I':
                case 'O':
                case 'U':
                    return "an";
                default:
                    return "a";
            }
        }
    }
}
