﻿namespace Notifications.Domain.Configurations;

public class JwtSettings
{
    public const string SectionName = "JwtSetting";
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiredMinute { get; init; }
}