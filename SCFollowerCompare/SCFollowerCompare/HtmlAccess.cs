using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SCFollowerCompare
{
    public class HtmlAccess
    {
        public HtmlAccess(string prevString, string endString)
        {
            this.prevString = prevString;
            this.endString = endString;
        }

        private string prevString;
        public string endString;

        public string getAndPrintHtmlData(string url)
        {
            HttpClient client = new HttpClient();
            var responseBody = client.GetStringAsync(url).Result;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseBody);

            return findStringInHtml(responseBody, prevString, endString);
        }

        // Find the area in the html document where the string you are looking for is -> prevString, ClosingChar should be consts
        private string findStringInHtml(string responseBody, string prevString, string endString)
        {
            // Look for a distinctive string prior to the searched string to get the index
            var startIndex = responseBody.IndexOf(prevString) + prevString.Count();   // skip that string
            var endIndex = responseBody.IndexOf(endString);

            var responseBodyCharArr = responseBody.ToCharArray();
            List<char> searchedString = new List<char>();

            // Add all chars (numbers) to an char array. Stop at given char
            for (int i = startIndex; i < endIndex; i++)
            {
                searchedString.Add(responseBodyCharArr[i]);
            }

            string result = new string(searchedString.ToArray());
            return result;
        }
    }
}
