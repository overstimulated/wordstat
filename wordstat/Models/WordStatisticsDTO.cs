using System.Collections.Generic;

namespace wordstat.Models
{
    public class WordStatisticsDTO
    {
        public int NumberOfSentence { get; set; }
        public int NumberOfWords { get; set; }
        public string MostUsedWord { get; set; }
        public List<KeyValuePair<string, string>> SentenceWithMostWords { get; set; }
        public List<string> ThirdLongestWords { get; set; }

    }
}