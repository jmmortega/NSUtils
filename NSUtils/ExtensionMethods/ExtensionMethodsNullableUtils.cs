namespace NSUtils
{
    public static class ExtensionMethodsNullableUtils
    {
        public static int TryValue(this int? nullableValue)
        {
            if (nullableValue.HasValue)
            {
                return nullableValue.Value;
            }
            return 0;
        }

        public static float TryValue(this float? nullableValue)
        {
            if (nullableValue.HasValue)
            {
                return nullableValue.Value;
            }
            return 0f;
        }

        public static double TryValue(this double? nullableValue)
        {
            if (nullableValue.HasValue)
            {
                return nullableValue.Value;
            }
            return 0d;
        }

        public static uint TryValue(this uint? nullableValue)
        {
            if (nullableValue.HasValue)
            {
                return nullableValue.Value;
            }
            return 0u;
        }

        public static string TryToString(this object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        public static int? ParseInt(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                return null;
            }
        }

        public static uint? ParseUInt(string value)
        {
            try
            {
                return uint.Parse(value);
            }
            catch
            {
                return null;
            }
        }

        public static double? ParseDouble(string value)
        {
            try
            {
                return double.Parse(value);
            }
            catch
            {
                return null;
            }
        }

        public static float? ParseFloat(string value)
        {
            try
            {
                return float.Parse(value);
            }
            catch
            {
                return null;
            }
        }    
    }
}
