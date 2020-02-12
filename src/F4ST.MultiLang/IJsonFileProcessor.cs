using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using F4ST.Common.Containers;
using Newtonsoft.Json;

namespace F4ST.MultiLang
{
    public interface IJsonFileProcessor : ISingleton
    {
        dynamic R { get; }

        string this[string key] { get; }
        string this[string resource, string key] { get; }
        string this[string resource, string key, string culture] { get; }

        /// <summary>
        ///  Get Resource by key name and current culture
        /// </summary>
        /// <param name="resource">Resource name</param>
        /// <param name="key">Key</param>
        /// <param name="culture">Culture</param>
        /// <returns>return resource value</returns>
        string GetResource(string resource, string key, string culture);

        /// <summary>
        ///  Get Resource by key name and current culture
        /// </summary>
        /// <param name="resource">Resource name</param>
        /// <param name="key">Key</param>
        /// <returns>return resource value</returns>
        string GetResource(string resource, string key);

        /// <summary>
        ///  Get Resource by key name and current culture
        /// </summary>
        /// <param name="key">Key splitting by dot, ex: Global.Menu1</param>
        /// <returns>return resource value</returns>
        string GetResource(string key);

        /// <summary>
        /// Get list of available culture for resource
        /// </summary>
        /// <param name="resource">Resource name</param>
        /// <returns></returns>
        IReadOnlyList<string> GetCultures(string resource);

        /// <summary>
        /// create resource file in directory
        /// </summary>
        /// <param name="resource">resourceName</param>
        /// <param name="culture">culture</param>
        /// <param name="isRtl">ltr/rtl</param> 
        /// <param name="data">resources</param> 
        void AddResourceLanguage(string resource, string culture, bool isRtl, Dictionary<string, string> data);

        /// <summary>
        /// Reload all resources
        /// </summary>
        void Reload();
    }
}