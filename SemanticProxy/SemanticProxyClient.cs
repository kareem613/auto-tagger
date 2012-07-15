using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CerealBox;
using System.Xml.Linq;
using System.IO;
using System.Xml;


namespace SemanticProxy
{
    public class SemanticProxyClient : ISemanticProxyClient
    {
        private ISemanticProxyProvider Provider { get; set; }
        public SemanticProxyClient(ISemanticProxyProvider provider)
        {
            this.Provider = provider;
        }

        public string QueryUrl(string url)
        {
            return Provider.GetResponse(url);
        }


        public IEnumerable<string> GetCategory(string url)
        {
            var xml = QueryUrl(url);

            if (String.IsNullOrWhiteSpace(xml))
            {
                return new List<string>();
            }

            XNamespace rdfns = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            XNamespace cns = "http://s.opencalais.com/1/pred/";
            
            XElement xe = XElement.Load(new StringReader(xml));

            var cat = from e in xe.Elements()
                      where e.Name == rdfns + "Description"
                      && e.Descendants(rdfns + "type").Any(te => te.Attributes(rdfns + "resource").Any(a => a.Value == "http://s.opencalais.com/1/type/cat/DocCat")) 
                      select e;
            var categories = cat.Select(e => e.Descendants(cns + "categoryName"));
            var catStringList = categories.Select(e=>e.First().Value).ToList();
            return catStringList;
        }
    }
}
