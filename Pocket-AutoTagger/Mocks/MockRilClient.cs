using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SuddenElfilio.RilSharp;

namespace Pocket_AutoTagger.Mocks
{
    class MockRilClient : IRilClient
    {
        public Credentials RilCredentials { get; set; }
        public Limits ApiLimits { get; private set; }
        
        public MockRilClient(Credentials credentials)
        {
            RilCredentials = credentials;
        }


        public bool Add(string url, string title, bool autoTitle)
        {
            throw new NotImplementedException();
        }

        

        public Limits ApiStatistics()
        {
            throw new NotImplementedException();
        }

        public bool Authenticate()
        {
            throw new NotImplementedException();
        }

        public RilList Get(ReadState state, DateTime? since, int? count, int? page, bool myAppOnly, bool tags)
        {
            var list = new RilList();
            list.Items = new Dictionary<string, RilListItem>();
            list.Items.Add("12345", new RilListItem() { Title = "Website 1", Url = "www.web1.com" });
            list.Items.Add("23456", new RilListItem() { Title = "Website 2", Url = "www.web2.com" });
            list.Items.Add("34567", new RilListItem() { Title = "Website 3", Url = "www.web3.com", Tags = "existing-tag" });
            return list;
        }

        public bool RegisterClient()
        {
            throw new NotImplementedException();
        }

        

        public bool Send(SendType type, List<RilListItem> items)
        {
            return true;
        }

        public Statistics Stats()
        {
            throw new NotImplementedException();
        }
    }
}
