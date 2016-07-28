using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WordStatisticsProcessors;

namespace wordstat.Tests
{
    [TestClass]
    public class WordStatisticsProcessorTest
    {
        private string _paragraph;
        private WordStatisticsProcessor _wordStat;
        private WordStatisticsProcessor wordProcessor => _wordStat ?? (_wordStat = new WordStatisticsProcessor());

        public string Paragraph
        {
            get { return _paragraph; }
        }

        [TestInitialize]
        public void Initialize()
        {
            //based on test case given by them
            _paragraph =
                @"Intergen is New Zealand's most experienced provider of Microsoft based business solutions. We focus on delivering business value in our solutions and work closely with Microsoft to ensure we have the best possible understanding of their technologies and directions. Intergen is a Microsoft Gold Certified Partner with this status recognising us as an “elite business partner” for implementing solutions based on our capabilities and experience with Microsoft products.";
        }

        [TestMethod]
        public void WordProcessor_Is_Initialized()
        {
            Assert.IsNotNull(Paragraph);
        }

        [TestMethod]
        public void Paragraph_IsNot_Clean()
        {
            Assert.IsFalse(WordStatisticsProcessor.IsParagraphClean(Paragraph + ".."));
            var newParagraph = "This is an example paragraph.. With a typo";
            Assert.IsFalse(WordStatisticsProcessor.IsParagraphClean(newParagraph));
        }

        [TestMethod]
        public void Paragraph_Is_Clean()
        {
            Assert.IsTrue(WordStatisticsProcessor.IsParagraphClean(Paragraph));
        }

        [TestMethod]
        public void Paragraph_Has_3_Sentence()
        {
            var numberOfwords = wordProcessor.CountTheSentence(Paragraph);
            Assert.AreEqual(numberOfwords, 3);
        }

        [TestMethod]
        public void Paragraph_Has_NSentences()
        {
            const string myParagraph = @"One Sentence. Two Sentence. Three Sentences.";
            var numberOfSentences = wordProcessor.CountTheSentence(myParagraph);

            Assert.AreEqual(numberOfSentences, 3);
        }

        [TestMethod]
        public void Most_Used_Words()
        {
            var mostUsedword = wordProcessor.MostUsedWord(Paragraph);
            Assert.AreEqual(mostUsedword.ToLowerInvariant(), "microsoft");

            const string mywords = @"Confusing confusions confuses Confucious. Most most most most words used is most";
            Assert.AreEqual("most", wordProcessor.MostUsedWord(mywords));
        }

        [TestMethod]
        public void Word_Count_IsCorrect()
        {
            Assert.AreEqual(wordProcessor.WordCount(Paragraph), 68);
            const string mysentence = @"there are three more than three words in this sentence.";
            Assert.AreEqual(wordProcessor.WordCount(mysentence), 10);
            Assert.AreEqual(wordProcessor.WordCount(@""), 0);
        }

        [TestMethod]
        public void Sentence_With_The_Most_Words()
        {
            var sentenceWithTheMostWords = wordProcessor.SentenceWithTheMostWords(Paragraph).First();
            Assert.AreEqual(sentenceWithTheMostWords.Key, 3);
            Assert.IsTrue(sentenceWithTheMostWords.Value.Contains("Intergen is a Microsoft"));

        }

        [TestMethod]
        public void Formatter_Correctly_Formats_Number()
        {
            var first = wordProcessor.Formatter(1);
            var second = wordProcessor.Formatter(2);
            var third = wordProcessor.Formatter(3);
            var fourth = wordProcessor.Formatter(4);

            Assert.AreEqual("1st", first);
            Assert.AreEqual("2nd", second);
            Assert.AreEqual("3rd", third);
            Assert.AreEqual("4th", fourth);
        }

        [TestMethod]
        public void Find_The_ThirdLongest_Word()
        {
            var thirdLongestWords = wordProcessor.FindTheThirdLongestWord(Paragraph);

            Assert.IsNotNull(thirdLongestWords);
            Assert.AreEqual(thirdLongestWords.First(), "experienced");
            Assert.AreEqual(thirdLongestWords.ElementAt(1), "recognising");
        }

        [TestMethod]
        public void Validate_ThirdLongestWord_Different_Length()
        {
            const string mywords = @"hi its me jme but you can call me Jmeeee not Jmeeeee or Jmeeeeee";

            var thirdLongestWords = wordProcessor.FindTheThirdLongestWord(mywords);

            Assert.IsTrue(thirdLongestWords.Count == 1);
            Assert.AreEqual(thirdLongestWords.ElementAt(0), "Jmeeee");

            const string anotherWord = @"first Seconds";

            var longest = wordProcessor.FindTheThirdLongestWord(anotherWord);
            Assert.IsTrue(longest.Count == 1);
            Assert.AreEqual(longest.ElementAt(0), "Seconds");

            const string sameWords = @"one one one one";
            var same = wordProcessor.FindTheThirdLongestWord(sameWords);
            Assert.IsTrue(same.Count == 4);
            Assert.AreEqual(same.ElementAt(0), "one");

            const string sameLength = @"one two six";
            var lenght = wordProcessor.FindTheThirdLongestWord(sameLength);
            Assert.IsTrue(lenght.Count == 3);
            Assert.AreEqual(lenght.ElementAt(0), "one");
        }

        [TestMethod]
        public void FindTheThirdLongestWord_Special_Case()
        {
            const string notsameLength = @"one two six fiiive four";
            var notSameLength = wordProcessor.FindTheThirdLongestWord(notsameLength);
            Assert.IsTrue(notSameLength.Count == 3);
            Assert.AreEqual(notSameLength.ElementAt(0), "one");
        }

        [TestCleanup()]
        public void Cleanup() { }
    }
}
