using Feed.UI.MVCApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Feed.UI.MVCApp.Services
{
    public class FeedService
    {
        public IEnumerable<SyndicationItemWrapper> GetNews(string url)
        {
            return SyndicationFeed.Load(XmlReader.Create(url)).Items.Select(x => new SyndicationItemWrapper(x));
        }
    }
}