using Foundation;
using NSUtils.Interfaces;
using System;

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
            _prefs.SetInt((nint)value, key);
            _prefs.Synchronize();
        }

        public void SetLong(string key, long value)
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
            _prefs.SetBool(value, key);
            _prefs.Synchronize();
        }

        public int GetInt(string key)
        {
            return (int)_prefs.IntForKey(key);
        }

        public long GetLong(string key)
        {
            string val = _prefs.StringForKey(key);
            return Convert.ToInt64(val);
        }

        public string GetString(string key)
        {
            return _prefs.StringForKey(key);
        }

        public bool GetBool(string key)
        {
            return _prefs.BoolForKey(key);
        }

        public bool GetBoolTrue(string key)
        {
            try
            {
                return _prefs.BoolForKey(key);
            }
            catch
            {
                return true;
            }
        }

        public double GetDouble(string key)
        {
            return _prefs.DoubleForKey(key);
        }

        public void SetDouble(string key, double value)
        {
            _prefs.SetDouble(value, key);
            _prefs.Synchronize();
        }


        public void CleanData()
        {
            _prefs.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
        }

    }
}
