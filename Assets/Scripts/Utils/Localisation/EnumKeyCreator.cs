namespace BattleCruisers.Utils.Localisation
{
    public class EnumKeyCreator
    {
        public const string ENUM_KEY_PREFIX = "Enums";

        public static string CreateKey<TEnum>(TEnum enumValue)
        {
            string enumType = typeof(TEnum).Name;
            return $"{ENUM_KEY_PREFIX}/{enumType}/{enumValue}";
        }
    }
}