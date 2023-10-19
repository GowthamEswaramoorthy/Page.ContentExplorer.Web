using HtmlAgilityPack;
using Page.ContentExplorer.Web.Models;

namespace Page.ContentExplorer.Web.Services
{
    public interface IPageAnalyseService
    {
        Uri RequestedUrl { get; set; }

        /// <summary>
        /// Request the URL and fetch HTML document
        /// </summary>
        /// <returns></returns>
        Task<HtmlDocument> LoadHtmlDocument();
        /// <summary>
        /// Helps to retrieve the images from the HTML document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        IEnumerable<ImageReportModel> GetImagesfromPage(HtmlDocument document);
        /// <summary>
        ///  Helps to retrieve the words found from the HTML document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        IEnumerable<WordReportModel> GetWordsfromPage(HtmlDocument document);
    }
}
