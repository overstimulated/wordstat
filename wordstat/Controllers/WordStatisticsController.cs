using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using wordstat.Models;
using WordStatisticsProcessors;

namespace wordstat.Controllers
{
    public class WordStatisticsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetValues(string paragraph) {

            var WordProcessor = new WordStatisticsProcessor();

            var sentenceWithTheMostWord = WordProcessor.SentenceWithTheMostWords(paragraph).First();

            var thirdLongestWord = WordProcessor.FindTheThirdLongestWord(paragraph);

            var myParagraphStatistics = new WordStatisticsDTO() {
                NumberOfSentence = WordProcessor.CountTheSentence(paragraph),
                NumberOfWords = WordProcessor.WordCount(paragraph),
                MostUsedWord = WordProcessor.MostUsedWord(paragraph),
                SentenceWithMostWords = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string> (WordProcessor.Formatter(sentenceWithTheMostWord.Key), sentenceWithTheMostWord.Value)
                },
                ThirdLongestWords = thirdLongestWord
            };

            return Request.CreateResponse(HttpStatusCode.OK, myParagraphStatistics);
        }
    }
}
