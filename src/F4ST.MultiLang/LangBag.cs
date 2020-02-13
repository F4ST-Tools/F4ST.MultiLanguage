using System.Dynamic;

namespace F4ST.MultiLang
{
    public class LangBag : DynamicObject
    {
        private readonly JsonFileProcessor _processor;
        private readonly string _resource;
        public LangBag(JsonFileProcessor processor, string resource = null)
        {
            _processor = processor;
            _resource = resource;
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();

            if (string.IsNullOrWhiteSpace(_resource))
            {
                result = new LangBag(_processor, name);
                return true;
            }

            result = _processor[_resource, name];
            return true;
        }
    }
}