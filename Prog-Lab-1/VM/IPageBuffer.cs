namespace VM.Domain;

/// <summary>
/// Provides an interface for managing pages in a buffer.
/// </summary>
public interface IPageBuffer : IDisposable
{
    /// <summary>
    /// Gets the page at the specified index.
    /// </summary>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <returns>The page at the specified index.</returns>
    public IPage GetPage(int pageIndex);

    /// <summary>
    /// Marks the page at the specified index as modified.
    /// </summary>
    /// <param name="pageIndex">The index of the page to mark as modified.</param>
    public void MarkPageModified(int pageIndex);
}