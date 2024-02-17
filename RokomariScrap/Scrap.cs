using PuppeteerSharp;

namespace RokomariScrap;

public static class Scrap
{
    public static async Task<Queue<Book>> Act(uint pageNumber)
    {
        var queue = new Queue<Book>();
        await using var browser = await InitBrowser();
        var page = await browser.NewPageAsync();
        const string baseUrl = "https://www.rokomari.com/book/category/81/islamic";
        var query = $"{baseUrl}?page={pageNumber}";
        await page.GoToAsync(query);

        const string target = ".books-wrapper";
        await page.WaitForSelectorAsync(target);

        var root = await page.QuerySelectorAsync(target);
        var innerRoot = await root.QuerySelectorAllAsync(".books-wrapper__item");
        foreach (var div in innerRoot)
        {
            var authorElem = await div.QuerySelectorAsync(".book-author");
            var bookTitleElem = await div.QuerySelectorAsync(".book-title");

            var authorText = await authorElem.EvaluateFunctionAsync<string>("e => e.textContent", authorElem);
            var bookTitleText = await bookTitleElem.EvaluateFunctionAsync<string>("e => e.textContent", bookTitleElem);
            var bookData = new Book { Title = bookTitleText, Author = authorText };
            queue.Enqueue(bookData);
        }

        await browser.CloseAsync();
        return queue;
    }


    private static async Task<IBrowser> InitBrowser()
    {
        using var browserFetcher = new BrowserFetcher();
        var installedBrowser = await browserFetcher.DownloadAsync(BrowserTag.Stable);

        return await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                ExecutablePath = installedBrowser?.GetExecutablePath()
            }
        );
    }
}