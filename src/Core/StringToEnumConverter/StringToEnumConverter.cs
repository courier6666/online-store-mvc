namespace Store.Domain.StringToEnumConverter
{
    /// <summary>
    /// Static class for converting string to enum value.
    /// </summary>
    public static class StringToEnumConverter
    {
        /// <summary>
        /// Receives string value and converts it into enum value. In case string could not be converted, returns null.
        /// </summary>
        /// <typeparam name="TEnum">Enum value to convert to</typeparam>
        /// <param name="str">Provided string to convert.</param>
        /// <returns>Enum value of <typeparamref name="TEnum"/> or null</returns>
        public static TEnum? ConvertStringToEnumValue<TEnum>(string str) where TEnum : struct
        {
            var statuses = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            foreach (var status in statuses)
            {
                if (status.ToString() == str)
                    return status;
            }
            return null;
        }
        /// <summary>
        /// Wrapper for method <see cref="ConvertStringToEnumValue{TEnum}(string)"/>
        /// Takes in a value of string and formats it with editString method delegate and then passes it to <see cref="ConvertStringToEnumValue{TEnum}(string)"/>
        /// </summary>
        /// <typeparam name="TEnum">Enum value to convert to</typeparam>
        /// <param name="str">Provided string to convert.</param>
        /// <param name="editString">Delegate method for formatting string</param>
        /// <returns>Enum value of <typeparamref name="TEnum"/> or null</returns>
        public static TEnum? ConvertStringToEnumValue<TEnum>(string str, Func<string, string> editString) where TEnum : struct
        {
            return ConvertStringToEnumValue<TEnum>(editString(str));
        }
    }
}
