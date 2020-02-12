using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

//using Microsoft.AspNetCore.Hosting;

namespace F4ST.MultiLang
{
    public class ResourcesProcessor 
    {
        public static IJsonFileProcessor Get(string path)
        {
            return new JsonFileProcessor(path);
        }

    }
}