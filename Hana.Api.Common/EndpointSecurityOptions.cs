namespace Hana;

public static class EndpointSecurityOptions
{
    public static readonly string AdminRoleName = "Admin";
    public static readonly string ReaderRoleName = "Reader";
    public static readonly string WriteRoleName = "Writer";
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static bool SecurityIsEnabled = true;
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public static void DisableSecurity() => SecurityIsEnabled = false;
}