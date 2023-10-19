using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Page.ContentExplorer.Web.Models;
using Page.ContentExplorer.Web.Services;

namespace Page.ContentExplorer.Web.Pages
{
    public class IndexModel : PageModel
    {
        public string ReqUrl { get; set; }
        public IEnumerable<ImageReportModel> ImagesList;
        public IEnumerable<WordReportModel> WordsList;
        public bool IsEmpty { get; set; } = false;
        private readonly IPageAnalyseService _pageAnalyse;
        public IndexModel(IPageAnalyseService pageAnalyse)
        {
            _pageAnalyse = pageAnalyse;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPostAnalyse(string Url)
        {
            List<HtmlNode> lsts = new();
            try
            {
                UriBuilder builder = new(Url);
                _pageAnalyse.RequestedUrl = builder.Uri;

                //load html from Url
                var loadHtml = Task.Run(async () => await _pageAnalyse.LoadHtmlDocument());

                //load images and words from Html document
                if (loadHtml != null && loadHtml.Result != null)
                {
                    ImagesList = _pageAnalyse.GetImagesfromPage(loadHtml.Result);
                    WordsList = _pageAnalyse.GetWordsfromPage(loadHtml.Result);
                }

                if (ImagesList == null && WordsList == null)
                    IsEmpty = true;
            }
            catch (Exception ex)
            {
                return null;
            }
            return new PartialViewResult
            {
                ViewName = "_Carousel",
                ViewData = new ViewDataDictionary<IndexModel>(ViewData)
            };
        }
    }
}