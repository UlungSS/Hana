using Hana.Identity.Entities;

namespace Hana.Identity.Options;

public class ApplicationsOptions
{
    public ICollection<Application> Applications { get; set; } = [];
}