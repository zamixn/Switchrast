public interface ISaving
{
    string GetString(string key, string defaultValue = "");
    void SetString(string key, string value);

    int GetInt(string key, int defaultValue = 0);
    void SetInt(string key, int value);

    float GetFloat(string key, float defaultValue = 0);
    void SetFloat(string key, float value);

    bool GetBool(string key, bool defaultValue = false);
    void SetBool(string key, bool value);

    void Save();
}
