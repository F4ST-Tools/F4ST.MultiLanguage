using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace F4ST.MultiLang
{
    public class JsonFileProcessor : IJsonFileProcessor
    {
        private readonly List<ResourceModel> _resources = new List<ResourceModel>();
        private readonly string _path;
        internal JsonFileProcessor(string path)
        {
            _path = path;
            ProcessDirectory();
            _r = new LangBag(this);
        }

        private readonly LangBag _r;
        public dynamic R => _r;

        public string this[string key] => GetResource(key);

        public string this[string resource, string key] => GetResource(resource, key);

        public string this[string resource, string key, string culture] => GetResource(resource, key, culture);

        /// <inheritdoc/>
        public string GetResource(string resource, string key, string culture)
        {
            resource = resource.ToLower();
            key = key.ToLower();
            culture = culture.ToLower();

            var res = _resources.FirstOrDefault(r => r.Key == resource);

            if(res?.Resources == null)
                return string.Empty;
            
            if (!res.Resources.ContainsKey(key))
                return string.Empty;

            var item = res.Resources[key];

            return !res.Cultures?.ContainsKey(culture)??true
                ? item.FirstOrDefault().Value
                : item[culture];
        }

        /// <inheritdoc/>
        public string GetResource(string resource, string key)
        {
            var culture = Thread.CurrentThread.CurrentCulture.Name;
            return GetResource(resource, key, culture);
        }

        /// <inheritdoc/>
        public string GetResource(string key)
        {
            var r = key.Split('.');
            return r.Length < 2
                ? string.Empty
                : GetResource(r[0], r[1]);
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> GetCultures(string resource)
        {
            resource = resource.ToLower();

            var res = _resources.FirstOrDefault(r => r.Key == resource);
            if (res == null)
                return new List<string>();

            return res
                .Cultures?
                .Select(k => k.Key)
                .ToList()
                .AsReadOnly();
        }

        public void AddResourceLanguage(string resource, string culture, bool isRtl, Dictionary<string, string> data)
        {
            resource = resource.ToLower();
            culture = culture.ToLower();

            var fileName = GetFileName(resource, culture, isRtl ? "rtl" : "ltr");

            var resItem = _resources.FirstOrDefault(r => r.Key == resource);
            if (resItem == null)
                return;

            var filePath = Path.Combine(resItem.Path, fileName);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
            Reload();
        }

        public void Reload()
        {
            ProcessDirectory();
        }

        #region Private methods

        /// <summary>
        /// Process directory in path
        /// </summary>
        private void ProcessDirectory()
        {
            foreach (var path in ProcessDirectory(_path))
            {
                ProcessFiles(path);
            }
        }

        /// <summary>
        /// Process all files in the directory passed in and process the files they contain
        /// </summary>
        /// <param name="path">Path for process</param>
        private void ProcessFiles(string path)
        {
            var name = Path.GetFileName(path);
            if (string.IsNullOrWhiteSpace(name))
                return;

            var res = new ResourceModel
            {
                Key = name.ToLower(),
                Path = path,
                Cultures = new Dictionary<string, bool>(),
                Resources = new Dictionary<string, Dictionary<string, string>>()
            };

            foreach (var file in GetFiles(path))
            {
                var exp = Path
                    .GetFileName(file)?
                    .Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                if (exp == null || exp.Length != 4)
                {
                    continue;
                }

                var culture = exp[1].ToLower();
                var align = exp[2].ToLower();

                if (!res.Cultures.ContainsKey(culture))
                {
                    res.Cultures.Add(culture, align == "rtl");
                }

                ProcessFileContent(file, culture, res.Resources);
            }

            _resources.Add(res);
        }

        /// <summary>
        /// process file content in path and convert to resource model
        /// </summary>
        /// <param name="path">path of file</param>
        /// <param name="culture">culture</param>
        /// <param name="resource">current resources</param>
        private void ProcessFileContent(string path, string culture,
            IDictionary<string, Dictionary<string, string>> resource)
        {
            var jsonData = File.ReadAllText(path);
            var items = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

            foreach (var item in items)
            {
                var key = item.Key.ToLower();
                if (resource.ContainsKey(key))
                {
                    var resItems = resource[key];
                    if (resItems.ContainsKey(culture))
                    {
                        continue;
                    }

                    resItems.Add(culture, item.Value);
                }
                else
                {
                    resource.Add(key,
                        new Dictionary<string, string>()
                        {
                            {culture, item.Value}
                        });
                }
            }
        }

        /// <summary>
        /// return all files in the directory passed in
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        private IEnumerable<string> GetFiles(string targetDirectory)
        {
            return Directory.GetFiles(targetDirectory);
        }

        /// <summary>
        /// Process all directory passed in, recurse on any directories  
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        private IEnumerable<string> ProcessDirectory(string targetDirectory)
        {
            // Recurse into subdirectories of this directory.
            if (!Directory.Exists(targetDirectory))
                return new List<string>();

            return Directory.GetDirectories(targetDirectory);
        }

        /// <summary>
        /// create file name 
        /// </summary>
        /// <param name="resourceName">resource name</param>
        /// <param name="culture">culture</param>
        /// <param name="alignment">ltr/rtl</param>
        /// <returns>file name</returns>
        private string GetFileName(string resourceName, string culture, string alignment)
        {
            return $"{resourceName}.{culture}.{alignment}.json";
        }

        #endregion


    }
}