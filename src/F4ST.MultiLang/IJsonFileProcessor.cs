using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using F4ST.Common.Containers;
using Newtonsoft.Json; 

namespace F4ST.MultiLang
{
    public interface IJsonFileProcessor:ISingleton
    {
        Dictionary<string, Dictionary<string, string>> FilesProcessDirectory(string targetDirectory);

        /// <summary>
        /// Process file directory and result culture list
        /// </summary>
        /// <param name="targetDirectory">path</param>
        /// <returns>return file culture info</returns>

        List<CultureModel> CultureProcessDirectory(string targetDirectory);
        /// <summary>
        /// Process all directory passed in, recurse on any directories  
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        List<string> ProcessDirectory(string targetDirectory);

        /// <summary>
        /// Process all files in the directory passed in, recurse on any directories 
        /// that are found, and process the files they contain.
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        IEnumerable<string> ProcessDirectoryFiles(string targetDirectory);

        /// <summary>
        /// Insert logic for processing found files here.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ProcessFile(string path);

        /// <summary>
        /// return all files in the directory passed in
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        IEnumerable<string> GetFiles(string targetDirectory);

        /// <summary>
        /// process file content in directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns>return culture & filename & file content </returns>
        (string, string, string) GetContentFile(string path);

        /// <summary>
        /// create resource file in directory
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="resourceName">resourceName</param>
        /// <param name="culture">culture</param>
        /// <param name="alignment">ltr/rtl</param> 
        Task CreateFileProccess(
            string path, 
            string resourceName, 
            string culture,
            string alignment,
            List<ResourceModel> data);

        /// <summary>
        /// Returns the absolute path for the specified path string.
        /// </summary>
        /// <param name="directory">directory</param>
        /// <returns>Returns the absolute path for the specified path string.</returns>
        string FullPath(string directory);

        /// <summary>
        /// create file name 
        /// </summary>
        /// <param name="resourceName">resource name</param>
        /// <param name="culture">culture</param>
        /// <param name="alignment">ltr/rtl</param>
        /// <returns>file name</returns>
        string GetFileName(string resourceName, string culture, string alignment);
    }
}