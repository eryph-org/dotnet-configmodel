using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class ProjectNetworksConfigConverter : DictionaryConverterBase<ProjectNetworksConfig, ProjectNetworksConfig>
    {

        public override ProjectNetworksConfig ConvertFromDictionary(
            IConverterContext<ProjectNetworksConfig> context, IDictionary<object, object> dictionary, object data = null)
        {
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            // target should be initialized
            context.Target = new ProjectNetworksConfig
            {
                Version = GetStringProperty(dictionary, nameof(ProjectNetworksConfig.Version)),
                Project = GetStringProperty(dictionary, nameof(ProjectNetworksConfig.Project)),
                Networks = context.ConvertList<NetworkConfig>(dictionary)
            };
            
            return context.Target;
        }
        
    }
    
}