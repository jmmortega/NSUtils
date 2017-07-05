namespace NSUtils.Interfaces
{
    public interface IStorage
    {
        void SetInt(string key, int value);
        void SetLong(string key, long value);
        void SetDouble(string key, double value);
        void SetString(string key, string value);
        void SetBool(string key, bool value);

        int GetInt(string key, int defaultValue);
        long GetLong(string key, long defaultValue);
        string GetString(string key, string defaultValue);
        bool GetBool(string key, bool defaultValue);
        double GetDouble(string key, double defaultValue);
        
        void CleanData();
    }
}
