namespace NSUtils.Interfaces
{
    public interface IStorage
    {
        void SetInt(string key, int value);
        void SetLong(string key, long value);
        void SetString(string key, string value);
        void SetBool(string key, bool value);
        int GetInt(string key);
        long GetLong(string key);
        string GetString(string key);
        bool GetBool(string key);
        bool GetBoolTrue(string key);
        double GetDouble(string key);
        void SetDouble(string key, double value);

        void CleanData();
    }
}
