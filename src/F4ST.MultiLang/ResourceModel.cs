using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F4ST.MultiLang
{
    public class ResourceModel
    {
        /// <summary>
        /// Name of resource
        /// </summary>
        public string Key { get; set; }

        internal string Path { get; set; }

        /// <summary>
        /// List of cultures for this resource
        /// If culture is rtl, value set true
        /// </summary>
        public Dictionary<string, bool> Cultures { get; set; }

        /// <summary>
        /// Resources
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Resources { get; set; }
    }
}
