using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordStatisticsProcessors
{
    public class WordStatisticsProcessor
    {
        #region public method
        /// <summary>
        /// Make sure that the paragraph has no typos E.G '..'
        /// Should look into using REGEX for handling 
        ///         ' .I' 
        ///         'john@solomon.co.nz'
        ///         ' I.' 
        /// Will do for now.
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static bool IsParagraphClean(string paragraph)
        {
            if (paragraph.Contains(" . ")) return false;
            return !paragraph.Contains("..");
        }

        /// <summary>
        /// Split string into whitespaces then return the count 
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public int CountTheSentence(string paragraph)
        {
            return SplitIntoSentence(paragraph).Count();
        }

        /// <summary>
        /// Count the number of words in the paragraph
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public int WordCount(string paragraph)
        {
            var source = SplitIntoWords(paragraph);
            return source.Count();
        }

        /// <summary>
        /// Find the most used word in a paragraph
        /// </summary>
        /// <param name="paragraph">paragraph string</param>
        /// <returns>the most used word</returns>
        public string MostUsedWord(string paragraph)
        {
            var mostUsedWord = SplitIntoWords(paragraph)
                .GroupBy(s => s)
                .OrderByDescending(g => g.Count()).First();

            return mostUsedWord.Key;
        }

        /// <summary>
        /// Returns a List{sentenceLine, someWordsInTheSentence}
        /// of a sentence with the most words from a given paragraph
        /// 
        /// I changed to List<KvP> due to dictionary duplicate keys
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public Dictionary<int, string> SentenceWithTheMostWords(string paragraph)
        {
            var lineAndCount = new List<KeyValuePair<int, int>>(); //use list due to dictionary key dupes   
            var wordAndLine = new List<KeyValuePair<int, string>>(); //store the word count and the sentence line
            var tableToReturn = new Dictionary<int, string>();

            int sentenceIndex = 0;

            var sentences = SplitIntoSentence(paragraph).GroupBy(s => s);

            //save each sentence stat in a wordAndLine
            foreach (var sentence in sentences)
            {
                int wordCounter = WordCount(sentence.Key);
                var wordLine = Regex.Match(sentence.Key, @"^(\w+\b.*?){5}").ToString(); //only take the first 4 words of the sentence.;
                sentenceIndex++;

                //lineAndCount.Add(sentenceIndex, wordCounter);
                lineAndCount.Add(new KeyValuePair<int, int>(sentenceIndex, wordCounter));

                //wordAndLine.Add(wordCounter, wordLine); //TODO: DEBUG an item with the same key has already been added
                wordAndLine.Add(new KeyValuePair<int, string>(wordCounter, wordLine));
            }

            var orderedLineAndCountDesc = lineAndCount.OrderByDescending(pair => pair.Value); //order the sentence lineAndCount by the most word count

            var sentenceWithTheMostWords = (from lc in orderedLineAndCountDesc
                                            join wL in wordAndLine on lc.Value equals wL.Key
                                            select new { lc.Key, wL.Value }).First(); //do a join to get the first entry

            tableToReturn.Add(sentenceWithTheMostWords.Key, sentenceWithTheMostWords.Value);

            return tableToReturn;
        }

        /// <summary>
        /// Return a list of the third longest words
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public List<string> FindTheThirdLongestWord(string paragraph)
        {
            var myList = new List<string>();

            var words = SplitIntoWords(paragraph).ToArray(); //split the text on whitespaces, trim the punctuations

            if (words.Length == 1)
            {
                myList.Add(words[0]); //add that one word and exit
                return myList;
            }
            if (words.Length > 2) //if there are many words
            {
                var lengthOfAllWords = words.Select(x => x.Length).Distinct().ToList();

                var letterCount = TakeNthHighestValue(lengthOfAllWords, 3) == 0 ? words.ElementAt(1).Count() : TakeNthHighestValue(lengthOfAllWords, 3); //take the length of the third longest word in the list

                var findTheThirdLongestWord = MyThirdLongestWords(words, letterCount, myList);

                if (findTheThirdLongestWord != null) return findTheThirdLongestWord;
            }
            else
            {
                var longestWord = words[0].Length > words[1].Length ? words[0] : words[1]; //just return the longest word between [0] and [1] elements
                myList.Add(longestWord);
            }

            return myList;
        }

        /// <summary>
        /// Just a simple number formatter
        /// </summary>
        /// <param name="toFormat"></param>
        /// <returns></returns>
        public string Formatter(int toFormat)
        {
            switch (toFormat)
            {
                case 1:
                    return String.Format("{0}st", toFormat);
                case 2:
                    return String.Format("{0}nd", toFormat);
                case 3:
                    return String.Format("{0}rd", toFormat);
                default:
                    return String.Format("{0}th", toFormat);
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Split the paragraph into sentences
        /// </summary>
        /// <param name="paragraph">string</param>
        /// <returns></returns>
        private static IEnumerable<string> SplitIntoSentence(string paragraph)
        {
            return Regex.Split(paragraph, @"(?<=[\.!\?])\s+");
        }

        /// <summary>
        ///  returns a list of the third longest words
        /// </summary>
        /// <param name="words">words</param>
        /// <param name="letterCount">the letter count of the longest word</param>
        /// <param name="myList"></param>
        /// <returns></returns>
        private static List<string> MyThirdLongestWords(IReadOnlyList<string> words, int letterCount, List<string> myList)
        {
            if (words[0].Length != letterCount && words[1].Length != letterCount) //if the first and second words' letter count in the list is not euquals to the highest lettercount number
            {
                var thirdLongestWords = words.Where(x => x.Length == letterCount).ToArray();

                if (thirdLongestWords.Length == 1) //if there is only one, just add the first word
                {
                    myList.Add(thirdLongestWords[0]);
                    return myList;
                }

                myList.AddRange(thirdLongestWords);
            }
            else
            {
                myList.AddRange(words.Where(x => x.Length == letterCount)); //just add every word equal to letterCount
            }
            return null;
        }

        /// <summary>
        /// Take the nth (indexer being the nth) highest value from a list
        /// </summary>
        /// <param name="lengthOfAllWords"></param>
        /// <param name="indexer">the nth </param>
        /// <returns></returns>
        private static int TakeNthHighestValue(IEnumerable<int> lengthOfAllWords, int indexer)
        {

            var thirdLongestWordLength = lengthOfAllWords.OrderByDescending(x => x)
                .Skip(indexer - 1) //skip to the number preceeding indexer
                .Distinct()
                .Take(1)
                .FirstOrDefault();
            return thirdLongestWordLength;
        }

        /// <summary>
        /// Convert the paragraph into an array of words
        /// </summary>
        /// <param name="paragraph">string paragraph</param>
        /// <returns></returns>
        private IEnumerable<string> SplitIntoWords(string paragraph)
        {
            //make sure to unescape string
            var words = Regex.Unescape(paragraph).Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '\n', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
        #endregion
    }
}
