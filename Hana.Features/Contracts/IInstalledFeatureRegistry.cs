using Hana.Features.Models;

namespace Hana.Features.Contracts;

public interface IInstalledFeatureRegistry
{
   
    void Add(FeatureDescriptor descriptor);
    
     IEnumerable<FeatureDescriptor> List();
    
    FeatureDescriptor? Find(string fullName);
}