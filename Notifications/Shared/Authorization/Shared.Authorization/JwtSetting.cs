namespace Shared.Authorization;

public class JwtSetting
{
    public const string SettingSection = "JwtSetting";
    
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiredMinute { get; set; }
}