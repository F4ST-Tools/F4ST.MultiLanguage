using System.Collections.Generic;
using System.Threading;

//using Microsoft.AspNetCore.Hosting;

namespace F4ST.MultiLang
{
    public class ResourcesProcessor : IResourcesProcessor
    {
        private readonly IJsonFileProcessor _jsonFileProcessor;
        public Dictionary<string, Dictionary<string, string>> ResourceInfos { get; set; }
        public List<CultureModel> CultureModels { get; set; }
        public ResourcesProcessor(IJsonFileProcessor jsonFileProcessor)
        {
            _jsonFileProcessor = jsonFileProcessor;
        }

        /// <summary>
        /// Get all resource from directory \Resources
        /// </summary> 
        /// <returns>return resources info</returns>
        public Dictionary<string, Dictionary<string, string>> ResourcesProcess()
        { 
            var path=System.Environment.CurrentDirectory + "\\Resources";
            ResourceInfos= _jsonFileProcessor.FilesProcessDirectory(path);
            return ResourceInfos;
        }

        /// <summary>
        /// Get all cultures from directory \Resources
        /// </summary> 
        /// <returns>return cultures info</returns>
        public List<CultureModel> GetCultures()
        {  
            var path=System.Environment.CurrentDirectory + "\\Resources";
            CultureModels = _jsonFileProcessor.CultureProcessDirectory(path);
            return CultureModels;
        }

        /// <summary>
        ///  Get Resource by key name and current culture
        /// </summary>
        /// <param name="name">resource name</param>
        /// <returns>return resource value</returns>
        public string GetResource(string name)
        {
            if (ResourceInfos.TryGetValue(name, out var values))
            {
                var culture = Thread.CurrentThread.CurrentCulture.Name;
                if (values.TryGetValue(culture, out var value))
                    return value;
            }
            return name;
        }
    }
}