using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SemanticProxy.Mocks
{
    public class MockSemanticProxyProvider: ISemanticProxyProvider
    {

        public string GetResponse(string url)
        {
            return File.ReadAllText("sample_response.xml");
        }

        
    }
}
