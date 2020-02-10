using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F4ST.MultiLang
{
    public interface IResourceModel
    {
        string Key { get; set; }
        string Value { get; set; }
    }
}
