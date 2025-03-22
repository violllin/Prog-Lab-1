using System.Text;

namespace VM.Domain;

/// <summary>
/// Represents a virtual memory for storing variable-length strings.
/// </summary>
public class VarcharVirtualMemory : IVirtualMemory<string>
{
    private const int ElementsPerPage = 128;

    private readonly int _maxStringSize;
    private readonly ISwapFile _swapFile;
    private readonly FileStream _dataFile;
    private readonly int _size;

    /// <summary>
    /// Initializes a new instance of the <see cref="VarcharVirtualMemory"/> class.
    /// </summary>
    /// <param name="swapFile">The swap file used to manage pages of memory.</param>
    /// <param name="dataFileName">The name of the data file used to store string values.</param>
    /// <param name="maxStringSize">The maximum size of the strings to be stored.</param>
    public VarcharVirtualMemory(ISwapFile swapFile, string dataFileName, int maxStringSize, int size)
    {
        _maxStringSize = maxStringSize;
        _swapFile = swapFile;
        _dataFile = new FileStream(dataFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        _size = size;
    }

    /// <summary>
    /// Sets the string value at the specified index in the virtual memory.
    /// </summary>
    /// <param name="index">The index at which to set the value.</param>
    /// <param name="value">The string value to set.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
    /// <exception cref="ArgumentException">Thrown when the string exceeds the maximum size.</exception>
    public void Set(int index, string value)
    {
        if (index < 0 || index >= _size)
            throw new IndexOutOfRangeException("Индекс вне границ массива");

        if (value.Length > _maxStringSize)
            throw new ArgumentException($"Строка превышает максимальный размер {_maxStringSize} символов");

        var pageIndex = index / ElementsPerPage;
        var localIndex = index % ElementsPerPage;

        var page = _swapFile.LoadPage(pageIndex);
        long position;

        using (var writer = new BinaryWriter(_dataFile, Encoding.UTF8, true))
        {
            _dataFile.Seek(0, SeekOrigin.End);
            position = _dataFile.Position;

            writer.Write(value.Length);
            writer.Write(Encoding.UTF8.GetBytes(value));
        }

        page.Data[localIndex] = (int)position;
        page.BitMap[localIndex] = true;

        _swapFile.SavePage(page);
    }

    /// <summary>
    /// Gets the string value at the specified index in the virtual memory.
    /// </summary>
    /// <param name="index">The index from which to get the value.</param>
    /// <returns>The string value at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the element is not initialized.</exception>
    public string Get(int index)
    {
        if (index < 0)
            throw new IndexOutOfRangeException("Индекс вне границ массива");

        var pageIndex = index / ElementsPerPage;
        var localIndex = index % ElementsPerPage;
        var page = _swapFile.LoadPage(pageIndex);

        if (!page.BitMap[localIndex])
            throw new InvalidOperationException("Элемент не инициализирован");

        long position = page.Data[localIndex];

        using var reader = new BinaryReader(_dataFile, Encoding.UTF8, true);
        _dataFile.Seek(position, SeekOrigin.Begin);
        var length = reader.ReadInt32();
        var bytes = reader.ReadBytes(length);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Disposes the resources used by the virtual memory.
    /// </summary>
    public void Dispose()
    {
        _swapFile.Dispose();
        _dataFile.Dispose();
        GC.SuppressFinalize(this);
    }
}