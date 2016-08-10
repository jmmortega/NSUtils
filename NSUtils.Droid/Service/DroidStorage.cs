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

        public int GetInt(string key)
        {
            return prefs.GetInt(key, 0);
        }

        public long GetLong(string key)
        {
            return prefs.GetLong(key, 0);
        }

        public string GetString(string key)
        {
            return prefs.GetString(key, "");
        }

        public bool GetBool(string key)
        {
            return prefs.GetBoolean(key, false);
        }

        public bool GetBoolTrue(string key)
        {
            return prefs.GetBoolean(key, true);
        }

        public double GetDouble(string key)
        {
            return prefs.GetFloat(key, 0);
        }

        public void SetDouble(string key, double value)
        {
            editor.PutFloat(key, (float)value);
            editor.Commit();
        }

        public void CleanData()
        {
            editor.Clear().Commit();
        }
    }
}