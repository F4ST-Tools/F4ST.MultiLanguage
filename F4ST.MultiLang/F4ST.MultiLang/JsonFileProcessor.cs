using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
//using PitProject.Models;

namespace F4ST.MultiLang
{
    public class JsonFileProcessor:IJsonFileProcessor
    {
       // private readonly IHostEnvironment _hostingEnvironment;
        public JsonFileProcessor()//(IHostingEnvironment hostingEnvironment)
        {
           // _hostingEnvironment = hostingEnvironment;
        }
         
        /// <summary>
        /// Process file directory and result culture list
        /// </summary>
        /// <param name="targetDirectory">path</param>
        /// <returns>return file culture info</returns>
        public List<CultureModel> CultureProcessDirectory(string targetDirectory)
        {
            var result = new List<CultureModel>(); 
           // targetDirectory = FullPath(targetDirectory);
            
            foreach (var path in ProcessDirectory(targetDirectory))
            { 
                foreach (var file in GetFiles(path))
                {
                    var exp = Path.GetFileName(file)?.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    if (exp != null && exp.Length==3)
                    { 
                        var culture = exp[1];
                        var align = exp[2];
                        if (result.All(t => t.CultureName != culture))
                        {
                            result.Add(new CultureModel {CultureName = culture,Alignment = align});
                        }
                    }
                }
            }
            return result;
        }
         
        /// <summary>
        /// Process file directory
        /// </summary>
        /// <param name="targetDirectory">path</param>
        /// <returns>return file resource info</returns>
        public Dictionary<string, Dictionary<string, string>> FilesProcessDirectory(string targetDirectory)
        {
            var result = new Dictionary<string, Dictionary<string, string>>(); 
           // targetDirectory = FullPath(targetDirectory);
            foreach (var path in ProcessDirectory(targetDirectory))
            {
                foreach (var file in ProcessDirectoryFiles(path))
                {
                    var res = GetContentFile(file);
                    result = ConvertToResourceModel(result, res.Item1, res.Item2, res.Item3);
                }
            }
            return result;
        }

        /// <summary>
        /// Process all files in the directory passed in, recurse on any directories 
        /// that are found, and process the files they contain.
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        public IEnumerable<string> ProcessDirectoryFiles(string targetDirectory)
        {
            // Process the list of files found in the directory.
            var fileEntries = GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                yield return ProcessFile(fileName);
        }

        /// <summary>
        /// return all files in the directory passed in
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFiles(string targetDirectory)
        {
             return Directory.GetFiles(targetDirectory);
        }

        /// <summary>
        /// Process all directory passed in, recurse on any directories  
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        public List<string> ProcessDirectory(string targetDirectory)
        { 
            // Recurse into subdirectories of this directory.
            if (!Directory.Exists(targetDirectory)) return new List<string>();
            return  Directory.GetDirectories(targetDirectory).ToList(); 
        }

        /// <summary>
        /// Insert logic for processing found files here.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ProcessFile(string path)
        {
            return path;
        }

        /// <summary>
        /// process file content in directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns>return culture & filename & file content </returns>
        public (string, string, string) GetContentFile(string path)
        {
            var exp = Path.GetFileName(path)?.Split(new []{"."},StringSplitOptions.RemoveEmptyEntries);
            if (exp != null && exp.Length==3)
            {
                var key = exp[0];
                var culture = exp[1];
                var jsonData = System.IO.File.ReadAllText(path);
                return (culture, key, jsonData);
            }
            return (null, null, null);
        }

        private Dictionary<string, Dictionary<string, string>> ConvertToResourceModel(
            Dictionary<string, Dictionary<string, string>> result,
            string culture,
            string resourceName,
            string jsonData)
        {

            var resources = JsonConvert.DeserializeObject<List<ResourceModel>>(jsonData);
            foreach (var source in resources)
            {
                var cultureDic = new Dictionary<string, string>();
                cultureDic.Add(culture, source.Value);

                if (result.ContainsKey(source.Key) && !result[source.Key].ContainsKey(culture))
                    result[source.Key].Add(culture, source.Value);
                else
                    result.Add(source.Key, cultureDic);
            }
            return result;
        }

        /// <summary>
        /// create resource file in directory
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="resourceName">resourceName</param>
        /// <param name="culture">culture</param>
        /// <param name="alignment">ltr/rtl</param> 
        public async Task CreateFileProccess(string path, string resourceName, string culture,string alignment, List<ResourceModel> data)
        {
            var root = FullPath(path);
            var fileName = GetFileName(resourceName, culture,alignment);
            var directory = Path.Combine(root, fileName);
            var isExist = File.Exists(directory);
            if (isExist)
                File.WriteAllText(directory,string.Empty);
            
            using (var outputFile = new StreamWriter(directory, isExist))
            {
                var json = JsonConvert.SerializeObject(data);
                await outputFile.WriteAsync(json);
            }
        }

        /// <summary>
        /// Returns the absolute path for the specified path string.
        /// </summary>
        /// <param name="directory">directory</param>
        /// <returns>Returns the absolute path for the specified path string.</returns>
        public string FullPath(string directory)
        { 
            return Environment.CurrentDirectory+ directory; 
        }
         

        /// <summary>
        /// create file name 
        /// </summary>
        /// <param name="resourceName">resource name</param>
        /// <param name="culture">culture</param>
        /// <param name="alignment">ltr/rtl</param>
        /// <returns>file name</returns>
        public string GetFileName(string resourceName, string culture, string alignment)
        {
            return $"{resourceName}.{culture}.{alignment}.json";
        }
          
    }
}
