using System;
namespace SemanticProxy
{
    public interface ISemanticProxyProvider
    {
        string GetResponse(string url);
    }
}
