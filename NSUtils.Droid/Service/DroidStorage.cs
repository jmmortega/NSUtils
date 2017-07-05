using Android.Content;
using NSUtils.Interfaces;

namespace NSUtils.Droid.Service
{
    public class DroidStorage : IStorage
    {
        ISharedPreferences prefs;
        ISharedPreferencesEditor editor;

        public DroidStorage(Context context, string sharedPreferencesName)
        {
            prefs = context.GetSharedPreferences(sharedPreferencesName, 0);
            editor = prefs.Edit();
        }

        public void SetInt(string key, int value)
        {
            editor.PutInt(key, value);
            editor.Commit();
        }

        public void SetLong(string key, long value)
        {
            editor.PutLong(key, value);
            editor.Commit();
        }

        public void SetDouble(string key, double value)
        {
            editor.PutFloat(key, (float)value);
            editor.Commit();
        }

        public void SetString(string key, string value)
        {
            editor.PutString(key, value);
            editor.Commit();
        }

        public void SetBool(string key, bool value)
        {
            editor.PutBoolean(key, value);
            editor.Commit();
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return prefs.GetInt(key, defaultValue);
        }

        public long GetLong(string key, long defaultValue = 0L)
        {
            return prefs.GetLong(key, defaultValue);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return prefs.GetString(key, defaultValue);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return prefs.GetBoolean(key, defaultValue);
        }

        public double GetDouble(string key, double defaultValue = 0d)
        {
            return prefs.GetFloat(key, (float)defaultValue);
        }

        public void CleanData()
        {
            editor.Clear().Commit();
        }
    }
}