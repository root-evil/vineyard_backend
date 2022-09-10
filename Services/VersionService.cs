using vineyard_backend.Models;

namespace vineyard_backend.Services;

public class VersionService
{
    private readonly VersionInfo version;
    public VersionService(VersionInfo version)
    {
        this.version = version;
    }

    public VersionInfo GetVersion()
    {
        return version;
    }
}