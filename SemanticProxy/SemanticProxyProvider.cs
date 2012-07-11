using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;

namespace SemanticProxy
{
    public class SemanticProxyProvider : SemanticProxy.ISemanticProxyProvider
    {
        //http://service.semanticproxy.com/processurl/123/json/test
        private const string ROOT_URL = "http://service.semanticproxy.com/processurl";
        public string ApiKey { get; set; }
        public SemanticProxyProvider()
        {
        }

        public string GetResponse(string url)
        {
            var requestUrl = BuildRequestUrl(url);
            var cl= new WebClient();
            
            string hashCode = String.Format("{0:X}", requestUrl.GetHashCode());

            FileInfo fi = new FileInfo(Path.Combine("sp_cache", hashCode + ".sp"));

            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }


            if (!fi.Exists)
            {
               cl.DownloadFile(requestUrl, fi.FullName);
            }

            

            return File.ReadAllText(fi.FullName);
        }

        private string BuildRequestUrl(string url)
        {
            return String.Format("{0}/{1}/xml/{2}", ROOT_URL, ApiKey, url);
        }

        
    }
}
