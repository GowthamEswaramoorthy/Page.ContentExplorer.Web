using HtmlAgilityPack;
using Page.ContentExplorer.Web.Models;
using System.Text;

namespace Page.ContentExplorer.Web.Services
{
    public class PageAnalyseService : IPageAnalyse
    {
        public Uri RequestedUrl { get; set; }

        /// <summary>
        /// Helps to retrieve the images from the HTML document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public IEnumerable<ImageReportModel> GetImagesfromPage(HtmlDocument document)
        {
            List<ImageReportModel> imageLists;
            try
            {
                if (document == null)
                    return Enumerable.Empty<ImageReportModel>();

                imageLists = new List<ImageReportModel>();

                //extract images using HtmlAgility
                var acquiredImages = document.DocumentNode.Descendants("img")
                                                .Select(e => e.GetAttributeValue("src", null))
                                                .Where(s => !string.IsNullOrEmpty(s));

                foreach (string image in acquiredImages)
                {
                    imageLists.Add(new ImageReportModel() { Src = ValidatedUri(image) });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return imageLists;
        }

        /// <summary>
        ///  Helps to retrieve the words found from the HTML document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public IEnumerable<WordReportModel> GetWordsfromPage(HtmlDocument document)
        {
            if (document == null || document?.DocumentNode == null)
                return Enumerable.Empty<WordReportModel>(); ;

            List<WordReportModel> wordReports;
            Dictionary<string, int> wordsPresent = new();
            List<DataPoint> dataPoints = new();
            string dataPointsJson = string.Empty;
            int totalWords = 0;
            try
            {
                var extractedWords = document.DocumentNode.SelectNodes("//body//text()[not(parent::script)]").Select(node => node.InnerText);
                char[] delimiter = new char[] { ' ' };

                foreach (string text in extractedWords)
                {
                    var words = text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                        .Where(s => Char.IsLetter(s[0])).Select(s => s.ToLower());
                    int wordCount = words.Count();

                    if (wordCount > 0)
                    {
                        foreach (var singleword in words)
                        {
                            if (wordsPresent.TryGetValue(singleword, out int a))
                                wordsPresent[singleword] = wordsPresent[singleword] + 1;
                            else
                                wordsPresent.Add(singleword, 1);
                        }
                    }
                }
                totalWords = wordsPresent.Count;
                foreach (var kvp in wordsPresent.OrderByDescending(a => a.Value).Take(10))
                {
                    dataPoints.Add(new DataPoint(kvp.Key, kvp.Value));
                }
                dataPointsJson = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return wordReports = new List<WordReportModel>() { new WordReportModel { DataPoints = dataPointsJson, TotalWords = totalWords } };
        }

        /// <summary>
        /// Request the URL and fetch HTML document
        /// </summary>
        /// <returns></returns>
        public async Task<HtmlDocument> LoadHtmlDocument()
        {
            HttpClient client = new();
            var document = new HtmlDocument();
            try
            {
                using var handler = new HttpClientHandler();
                handler.UseDefaultCredentials = true;
                client = new HttpClient(handler)
                {
                    BaseAddress = RequestedUrl,
                };
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Example");
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var response = await client.GetAsync(RequestedUrl.AbsoluteUri);
                using var content = response.Content;
                // read answer in non-blocking way  
                var result = await content.ReadAsStringAsync();
                document.LoadHtml(result);
            }
            catch (Exception)
            {
                //log the error if required for customizations
                return null;
            }
            return document;
        }

        /// <summary>
        /// Validate the given URL is absolute or Relative and make it absolute
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string ValidatedUri(string uri)
        {
            if (Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                return new Uri(RequestedUrl, uri).AbsoluteUri;
            }
            return uri;
        }
    }
}
