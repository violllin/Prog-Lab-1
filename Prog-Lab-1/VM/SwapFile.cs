using System.Collections;

namespace VM.Domain;

/// <summary>
/// Represents a swap file used for managing virtual memory pages.
/// </summary>
public class SwapFile : ISwapFile
{
    private readonly FileStream _fs;
    private readonly BinaryWriter _writer;
    private readonly BinaryReader _reader;

    private const int PageSize = 512;
    private const int ElementsPerPage = 128;
    private const int BitMapSize = ElementsPerPage / 8;
    private const int StartOffset = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="SwapFile"/> class.
    /// </summary>
    /// <param name="fileName">The name of the swap file.</param>
    /// <param name="size">The size of the swap file.</param>
    public SwapFile(string fileName, long size)
    {
        _fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        _writer = new BinaryWriter(_fs);
        _reader = new BinaryReader(_fs);

        InitializeFile(size);
    }

    /// <summary>
    /// Initializes the swap file with the specified size.
    /// </summary>
    /// <param name="size">The size of the swap file.</param>
    private void InitializeFile(long size)
    {
        _writer.Write("VM"u8.ToArray());

        var pageCount = (size * sizeof(int) + PageSize - 1) / PageSize;
        for (int i = 0; i < pageCount; i++)
        {
            _writer.Write(new byte[BitMapSize]);
            _writer.Write(new byte[PageSize]);
        }

        _writer.Flush();
    }

    /// <summary>
    /// Loads the page at the specified index.
    /// </summary>
    /// <param name="pageIndex">The index of the page to load.</param>
    /// <returns>The loaded page.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the requested page is out of file bounds.</exception>
    public IPage LoadPage(int pageIndex)
    {
        if (_fs.Length < StartOffset + (pageIndex + 1) * PageSize)
            throw new InvalidOperationException("Запрашиваемая страница выходит за границы файла.");

        _fs.Seek(StartOffset + pageIndex * PageSize, SeekOrigin.Begin);
        var bitMapBytes = _reader.ReadBytes(BitMapSize);
        var bitmap = new BitArray(bitMapBytes);
        var data = new int[ElementsPerPage];

        for (int i = 0; i < ElementsPerPage; i++)
        {
            data[i] = _reader.ReadInt32();
        }

        return new Page(pageIndex, bitmap, data);
    }

    /// <summary>
    /// Saves the specified page to the swap file.
    /// </summary>
    /// <param name="page">The page to save.</param>
    public void SavePage(IPage page)
    {
        _fs.Seek(StartOffset + page.PageIndex * PageSize, SeekOrigin.Begin);
        var bitMapBytes = new byte[BitMapSize];
        page.BitMap.CopyTo(bitMapBytes, 0);
        _writer.Write(bitMapBytes);

        for (var i = 0; i < ElementsPerPage; i++)
        {
            _writer.Write(page.Data[i]);
        }

        _writer.Flush();
    }

    /// <summary>
    /// Disposes the resources used by the swap file.
    /// </summary>
    public void Dispose()
    {
        _writer.Dispose();
        _reader.Dispose();
        _fs.Dispose();
        GC.SuppressFinalize(this);
    }
}