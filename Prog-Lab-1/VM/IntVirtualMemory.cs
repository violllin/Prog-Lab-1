namespace VM.Domain;

public class IntVirtualMemory(IPageBuffer pageBuffer, int size) : IVirtualMemory<int>
{
    private const int ElementsPerPage = 128;
    
    public void Set(int index, int value)
    {
        if (index < 0 || index >= size)
        {
            throw new IndexOutOfRangeException("Индекс вне границ массива");
        }

        var pageIndex = index / ElementsPerPage;
        var localIndex = index % ElementsPerPage;
        var page = pageBuffer.GetPage(pageIndex);

        if (localIndex >= page.BitMap.Length)
        {
            throw new IndexOutOfRangeException("Локальный индекс выходит за границы битовой карты страницы");
        }

        page.Data[localIndex] = value;
        page.BitMap[localIndex] = true;
        pageBuffer.MarkPageModified(pageIndex);
    }
    
    public int Get(int index)
    {
        if (index < 0)
        {
            throw new IndexOutOfRangeException("Индекс вне границ массива");
        }

        var pageIndex = index / ElementsPerPage;
        var localIndex = index % ElementsPerPage;
        var page = pageBuffer.GetPage(pageIndex);

        if (localIndex >= page.BitMap.Length)
        {
            throw new IndexOutOfRangeException("Локальный индекс выходит за границы битовой карты страницы");
        }

        if (!page.BitMap[localIndex])
        {
            throw new InvalidOperationException("Элемент не инициализирован");
        }

        return page.Data[localIndex];
    }
    
    public void Dispose()
    {
        pageBuffer.Dispose();
        GC.SuppressFinalize(this);
    }
}