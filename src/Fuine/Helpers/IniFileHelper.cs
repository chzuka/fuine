namespace Fuine.Helpers;
public class IniFileHelper
{
    public string path;

    public IniFileHelper(string path)
    {
        this.path = path;
    }

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

    public void IniWrite(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, path);
    }

    public string IniRead(string section, string key)
    {
        StringBuilder temp = new(1024);
        GetPrivateProfileString(section, key, "", temp, 1024, path);
        return temp.ToString();
    }
}
