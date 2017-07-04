using Newtonsoft.Json.Linq;
using System;


namespace NSUtils
{
    public static class JsonExtensionMethodsNullableUtils
    {
        public static JToken TrySelectToken(this JToken token, string selectedValue)
        {
            try
            {
                //return token.SelectToken(selectedValue);
                var tokenSelected = token.SelectToken(selectedValue);

                if (tokenSelected == null)
                {
                    return JToken.Parse(string.Format("{ {0} }", selectedValue));
                }
                return tokenSelected;
            }
            catch
            {
                return JToken.Parse(string.Format("{ {0} }", selectedValue));
            }
        }

        public static Nullable<T> TryValue<T>(this JToken token) where T : struct
        {
            try
            {
                return token.Value<T>();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
