# Page Content Explorer
## Project Goals
1. Fetch and display all images from the specified target URL in a user-friendly carousel.
2. Calculate the total word count of the content and present it to the user.
3. Provide a list of the top 10 most frequently occurring words along with their respective counts.
4. Visualize this word frequency data in chart format for enhanced comprehension.
## Technologies and Framework used
1. .Net 6.0
2. Bootstrap
3. JQuery
4. Canvas JS
## Prerequisites
1. Visual Studio 2022
1. .Net 6.0
1. 
## Project Overview

In this project, the user inputs a URL into a text box. When they click the "Analyze" button, it triggers the "OnPostAnalyse" method with the provided URL as a parameter. The "LoadHtmlDocument" method utilizes the HttpClient to fetch the web page and loads its HTML content as an "HtmlDocument."

- To retrieve all the images from the page, we use the "GetImagesfromPage" method. This method takes the downloaded page, represented as an "HtmlDocument," as an input. It scans for "img" tags and compiles their "src" values into an "ImageReportModel" list. Before adding the URL to the list, we validate whether it's an absolute or relative URL and convert it to an absolute URL using the "ValidatedUri" method, which accepts a URI parameter. The "ImageReportModel" is then passed to the "_Carousel.cshtml" as an "IndexModel" and displayed to the user as a carousel.

- To extract words from the HTML document, we utilize the "GetWordsfromPage" method. This method also takes the downloaded page, represented as an "HtmlDocument," as an input. To extract words, we use XPath, which captures text from within the body tag and excludes words with a parent element of "Script." After extracting the words, we compile them into a character array, remove any empty entries, and exclude special or Unicode characters. This process allows us to calculate the word count. If the word count is greater than zero, we then compare each word with the others. If a word appears more than once, its count is increased. This is how we determine the top 10 most frequently occurring words. These top 10 words are sorted in descending order and converted into "Datapoint" items. We then serialize this data using Newtonsoft. This data is consumed in CanvasJS in the "_Words.cshtml" and displayed to the user, showing the word count and a graph of the top 10 most occurring words.

- As we make requests to an external site from our localhost, we may encounter Cross-Site Request Forgery (CSRF) issues. To address this, we generate an Anti-forgery Token using the "CreateConsentCookie" method and pass it as a header name to ensure secure communication.

- We utilize the "IPageAnalyse" interface and have implemented it within the "PageAnalyseService." This service is registered and configured for dependency injection with a scoped lifetime.