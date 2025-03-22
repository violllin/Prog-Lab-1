using System.Collections;

namespace VM.Domain;

/// <summary>
/// Represents a page in the virtual memory domain.
/// </summary>
public interface IPage
{
    /// <summary>
    /// Gets the index of the page.
    /// </summary>
    int PageIndex { get; }

    /// <summary>
    /// Gets or sets the last access time of the page.
    /// </summary>
    DateTime AccessTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the page has been modified.
    /// </summary>
    bool IsModified { get; set; }

    /// <summary>
    /// Gets the bitmap representing the page.
    /// </summary>
    BitArray BitMap { get; }

    /// <summary>
    /// Gets the data contained in the page.
    /// </summary>
    int[] Data { get; }
}