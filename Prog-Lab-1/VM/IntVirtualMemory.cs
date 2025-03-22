namespace VM.Domain;

/// <summary>
/// Represents a virtual memory implementation for integers.
/// </summary>
/// <param name="pageBuffer">The page buffer used to manage memory pages.</param>
public class IntVirtualMemory(IPageBuffer pageBuffer, int size) : IVirtualMemory<int>
{
    private const int ElementsPerPage = 128;

    /// <summary>
    /// Sets the value at the specified index in the virtual memory.
    /// </summary>
    /// <param name="index">The index at which to set the value.</param>
    /// <param name="value">The value to set.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
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

    /// <summary>
    /// Gets the value at the specified index in the virtual memory.
    /// </summary>
    /// <param name="index">The index from which to get the value.</param>
    /// <returns>The value at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the element is not initialized.</exception>
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

    /// <summary>
    /// Disposes the virtual memory and releases all resources.
    /// </summary>
    public void Dispose()
    {
        pageBuffer.Dispose();
        GC.SuppressFinalize(this);
    }
}