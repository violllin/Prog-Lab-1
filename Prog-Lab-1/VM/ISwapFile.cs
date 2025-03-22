namespace VM.Domain;

/// <summary>
/// Represents an interface for a swap file used in virtual memory management.
/// </summary>
public interface ISwapFile : IDisposable
{
    /// <summary>
    /// Loads the page at the specified index.
    /// </summary>
    /// <param name="pageIndex">The index of the page to load.</param>
    /// <returns>The loaded page.</returns>
    public IPage LoadPage(int pageIndex);

    /// <summary>
    /// Saves the specified page to the swap file.
    /// </summary>
    /// <param name="page">The page to save.</param>
    public void SavePage(IPage page);
}