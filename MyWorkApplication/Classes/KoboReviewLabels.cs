using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;

namespace MyWorkApplication.Classes
{
    public static class KoboReviewLabels
    {
        private static readonly Lazy<JObject> forms = new Lazy<JObject>(Load);
        private static JObject Load()
        {
            using(Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("MyWorkApplication.KoboMonitoringFieldLabels.json"))
            {
                if(stream==null) return new JObject();
                using(StreamReader reader=new StreamReader(stream)) return JObject.Parse(reader.ReadToEnd());
            }
        }
        private static JObject Field(string uid,string pointer)
        {
            JObject form=forms.Value[uid] as JObject;
            if(form==null) return null;
            string path=pointer.TrimStart('/').Replace("~1","/").Replace("~0","~");
            JObject exact=form[path] as JObject;
            if(exact!=null) return exact;
            string leaf=path.Substring(path.LastIndexOf('/')+1);
            JObject found=null;
            foreach(JProperty property in form.Properties())
            {
                if(property.Name!=leaf && !property.Name.EndsWith("/"+leaf,StringComparison.Ordinal)) continue;
                if(found!=null) return null;
                found=property.Value as JObject;
            }
            return found;
        }
        public static string Label(string uid,string pointer)
        {
            JObject field=Field(uid,pointer);
            return field==null?pointer:field["label"].ToString();
        }
        public static string Value(string uid,string pointer,string value)
        {
            if(value=="[missing]") return "غير موجود";
            JObject field=Field(uid,pointer);
            if(field==null) return value;
            JToken token;
            try{token=JToken.Parse(value);}catch{return value;}
            if(token.Type!=JTokenType.String) return value;
            JObject choices=field["choices"] as JObject;
            string code=token.ToString();
            if(choices!=null && choices[code]!=null) return choices[code] + " ("+code+")";
            return code;
        }
    }
}
