using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Activation;
using Net.SuddenElfilio.RilSharp;
using System.Configuration;

namespace Pocket_AutoTagger.Ioc
{
    class CredentialProvider : IProvider
    {
        protected Credentials CreateInstance(IContext context)
        {
            var creds = new Credentials()
            {
                UserName = ConfigurationManager.AppSettings["pocket_username"],
                Password = ConfigurationManager.AppSettings["pocket_password"],
                ApiKey = ConfigurationManager.AppSettings["pocket_api_key"]
            };
            return creds;
        }

        public object Create(IContext context)
        {
            return CreateInstance(context);
        }

        public Type Type
        {
            get { return typeof(Credentials); }
        }
    }
}
