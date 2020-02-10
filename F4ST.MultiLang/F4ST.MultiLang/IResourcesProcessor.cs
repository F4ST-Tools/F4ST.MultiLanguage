using System.Collections.Generic;
using F4ST.Common.Containers;

namespace F4ST.MultiLang
{
    public interface IResourcesProcessor:ISingleton
    {
        Dictionary<string, Dictionary<string, string>> ResourcesProcess();
        List<CultureModel> GetCultures();

        /// <summary>
        ///  Get Resource by key name and current culture
        /// </summary>
        /// <param name="name">resource name</param>
        /// <returns>return resource value</returns>
        string GetResource(string name);
    }
}