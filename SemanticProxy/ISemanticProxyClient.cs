using System;
using System.Collections.Generic;
namespace SemanticProxy
{
    public interface ISemanticProxyClient
    {
        string QueryUrl(string p);

        IEnumerable<string> GetCategory(string p);
    }
}
