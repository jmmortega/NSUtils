using Foundation;
using NSUtils.Interfaces;
using System;
using NSUtils;
using System.Globalization;

namespace NSUtils.Touch.Service
{
    public class TouchStorage : IStorage
    {
        NSUserDefaults _prefs;

        public TouchStorage()
        {
            _prefs = NSUserDefaults.StandardUserDefaults;
        }

        public void SetInt(string key, int value)
        {
            _prefs.SetString(value.ToString(), key);
            _prefs.Synchronize();
        }
        [Obsolete]
        public void Legacy_SetInt(string key, int value)
        {
            _prefs.SetInt((nint)value, key);
            _prefs.Synchronize();
        }

        public void SetLong(string key, long value)
        {
            _prefs.SetString(value.ToString(), key);
            _prefs.Synchronize();
        }
        
        public void SetDouble(string key, double value)
        {
            _prefs.SetString(value.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." }), key);
            _prefs.Synchronize();
        }
        [Obsolete]
        public void Legacy_SetDouble(string key, double value)
        {
            _prefs.SetDouble(value, key);
            _prefs.Synchronize();
        }
        public void SetString(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            _prefs.SetString(value, key);
            _prefs.Synchronize();
        }

        public void SetBool(string key, bool value)
        {
            _prefs.SetString(ExtensionMethodsString.ToBooleanString(value), key);
            _prefs.Synchronize();
        }
        [Obsolete]
        public void Legacy_SetBool(string key, bool value)
        {
            _prefs.SetBool(value, key);
            _prefs.Synchronize();
        }


        public int GetInt(string key, int defaultValue = 0)
        {
            var val = _prefs.StringForKey(key);
            return val != null ? Convert.ToInt32(val) : defaultValue;
        }
        [Obsolete]
        public int Legacy_GetInt(string key)
        {
            return (int)_prefs.IntForKey(key);
        }
        public long GetLong(string key, long defaultValue = 0L)
        {
            var val = _prefs.StringForKey(key);
            return val != null ? Convert.ToInt64(val) : defaultValue;
        }

        public string GetString(string key, string defaultValue = "")
        {
            return _prefs.StringForKey(key) ?? defaultValue;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            var val = _prefs.StringForKey(key);
            return val != null ? ExtensionMethodsString.StringToBoolean(val) : defaultValue;
        }
        [Obsolete]
        public bool Legacy_GetBool(string key)
        {
            return _prefs.BoolForKey(key);
        }
        public double GetDouble(string key, double defaultValue = 0d)
        {
            var val = _prefs.StringForKey(key);
            return val != null ? Convert.ToDouble(val, new NumberFormatInfo() { NumberDecimalSeparator = "."}) : defaultValue;
        }
        [Obsolete]
        public double Legacy_GetDouble(string key)
        {
            return _prefs.DoubleForKey(key);
        }
        public void CleanData()
        {
            _prefs.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
        }
    }
}
