using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Feed.UI.MVCApp.Model
{
    public class SyndicationItemWrapper
    {
        private readonly WordCount _wc;
        
        public string Title { get; private set; }
        public string Link { get; private set; }
        public string Summary { get; private set; }
        public DateTime PublishDate { get; private set; }
        public int ContentWordsCount
        {
            get { return WordsCount(); }
        }
        public IEnumerable<string> ContentWordsTop10
        {
            get { return WordsTop10(); }
        }
        

        public SyndicationItemWrapper(SyndicationItem si)
        {
            Title = si.Title.Text;
            Link = si.Links.Count > 0 ? si.Links[0].Uri.AbsoluteUri : null;
            Summary = si.Summary.Text;
            PublishDate = si.PublishDate.DateTime;

            _wc = new WordCount(Summary, Model.WordCount.Exclude.Prepositions | Model.WordCount.Exclude.Articles);
        }


        private int WordsCount()
        {
            return _wc.Count();
        }

        private IEnumerable<string> WordsTop10()
        {
            return _wc.TopWords(10);
        }
    }
}