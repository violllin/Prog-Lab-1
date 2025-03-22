namespace VM.Domain;

public class PageBuffer(
    ISwapFile swapFile,
    IDateTimeService dateTimeService,
    int maxPages = PageBuffer.DefaultMaxPagesInMemory)
    : IPageBuffer
{
    private readonly List<IPage> _pages = [];

    public IPage GetPage(int pageIndex)
    {
        var page = _pages.FirstOrDefault(p => p.PageIndex == pageIndex);
        if (page != null)
        {
            page.AccessTime = dateTimeService.CurrentDateTime();
            return page;
        }

        if (_pages.Count >= maxPages)
            ReplaceOldestPage();

        page = swapFile.LoadPage(pageIndex);
        page.AccessTime = dateTimeService.CurrentDateTime();
        _pages.Add(page);
        return page;
    }

    public void MarkPageModified(int pageIndex)
    {
        var page = _pages.FirstOrDefault(p => p.PageIndex == pageIndex) ?? GetPage(pageIndex);
        page.IsModified = true;
        page.AccessTime = dateTimeService.CurrentDateTime();
    }

    private void ReplaceOldestPage()
    {
        var oldestPage = _pages.MinBy(p => p.AccessTime);
        if (oldestPage == null) return;

        if (oldestPage.IsModified)
        {
            swapFile.SavePage(oldestPage);
        }

        _pages.Remove(oldestPage);
    }

    private void SaveAllPages()
    {
        foreach (var page in _pages)
        {
            swapFile.SavePage(page);
        }
        _pages.Clear();
    }

    private const int DefaultMaxPagesInMemory = 3;

    public void Dispose()
    {
        SaveAllPages();
        swapFile.Dispose();
        GC.SuppressFinalize(this);
    }
}