using Foundation;
using NSUtils.Interfaces;
using System;
using NSUtils;

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

        public void SetLong(string key, long value)
        {
            _prefs.SetString(value.ToString(), key);
            _prefs.Synchronize();
        }

        public void SetDouble(string key, double value)
        {
            _prefs.SetString(value.ToString(), key);
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

        public int GetInt(string key, int defaultValue = 0)
        {
            var val = _prefs.StringForKey(key);
            return val != null ? Convert.ToInt32(val) : defaultValue;
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

        public double GetDouble(string key, double defaultValue = 0d)
        {
            var val = _prefs.StringForKey(key);
            return val != null ? Convert.ToDouble(val) : defaultValue;
        }

        public void CleanData()
        {
            _prefs.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
        }
    }
}
