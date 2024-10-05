namespace Store.Application.Utils
{
    public static class EnumValuesGetter
    {
        public static string[] GetAllValues<TEnum>() where TEnum : struct
        {
            return Enum.
                GetValues(typeof(TEnum)).
                Cast<TEnum>().
                Select(s => s.ToString()).
                ToArray();
        }
    }
}
