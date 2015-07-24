using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Feed.UI.MVCApp.Model
{
    public class WordCount
    {
        [Flags]
        public enum Exclude
        {
            None = 0
            , Prepositions = 1
            , Articles = 2
        }

        #region Properties
        private IEnumerable<string> _words;
        private IEnumerable<string> Words
        {
            get
            {
                if (_words == null) _words = GetWords();

                return _words;
            }
        }
        
        private readonly string _text;
        public string Text { get { return _text; } }

        private readonly Exclude _excludeWords;
        public Exclude ExcludeWords { get { return _excludeWords; } }
        #endregion


        public WordCount(string text, Exclude excludeWords)
        {
            _text = text;
            _excludeWords = excludeWords;
        }


        private IEnumerable<string> GetWords()
        {
            IEnumerable<string> ret = Enumerable.Empty<string>();

            if (!string.IsNullOrEmpty(Text))
            {
                Regex re;
                var reOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
                string text = Text.ToLower();

                // REMOVE HTML TAGS
                re = new Regex("<[^>]+>", reOptions);
                text = re.Replace(text, "");

                // REMOVE NCR
                re = new Regex(@"\&\#[a-zA-Z0-9]+;", reOptions);
                text = re.Replace(text, "");

                // GET WORDS
                re = new Regex(@"\b\w+\b", reOptions);
                ret = re.Matches(text).Cast<Match>().Select(x => x.Value);

                // EXCLUDES
                if ((ExcludeWords & Exclude.Prepositions) != 0)
                    ret = ret.Where(x => !Properties.Settings.Default.Prepositions.Contains(x));

                if ((ExcludeWords & Exclude.Articles) != 0)
                    ret = ret.Where(x => !Properties.Settings.Default.Articles.Contains(x));
            }

            return ret;
        }


        public int Count()
        {
            return Words.Count();
        }

        public IEnumerable<string> TopWords(int topAmount)
        {
            IEnumerable<string> ret = Enumerable.Empty<string>();

            IEnumerable<string> w = Words;

            if (w.Count() > 0)
                ret = w.GroupBy(x => x.ToLower())
                    .Select(g => new { Word = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .Take(topAmount)
                    .Select(x=> string.Format("{0} ({1})", x.Word, x.Count));

            return ret;
        }


        private class WordCompare : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.ToLower() == y.ToLower();
            }

            public int GetHashCode(string obj)
            {
                return 0;
            }
        }
    }
}

/*
 * TESTES:
 * 
 * 
 * PONTUAÇÕES:
 * 
 * . ... , ; : ! ? ' ´ ` "
 * 
 * PREPOSIÇÕES:
 * 
 * a, ante, após, até, com, contra, de, desde, em, entre, para, perante, por, sem, sob, sobre, trás
 * 
 * ARTIGOS:
 * 
 * o, os, a, as, um, uns, uma, umas, ao, aos, à, às, do, dos, da, das, dum, duns, duma, dumas, no, nos, na, nas, num, nuns, numa, numas, pelo, pelos, pela, pelas
 * 
*/