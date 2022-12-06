# dotnet-configmodel


Eryph configuration model libraries for following configuration models:

- Catlets
- Project networks

Serialization is implemented in a way that is compatible both for JSON and YAML. Each configuration object is de-/serializable from / to JSON and YAML and vice versa. 

To archive this compatibility deserialization will always first convert the input to a Dictionary<object,object> and then parsed in a custom parser framework that is independant of System.Json.Text or YamlDotNet. 

For serialization the model types will be directly serialized using System.Json.Text or YamlDotNet.

