namespace VM.Domain;

using System;
using System.Text;

/// <summary>
/// Represents a virtual memory for storing fixed-length strings.
/// </summary>
public class CharVirtualMemory : IVirtualMemory<string>
{
    private const int ElementsPerPage = 128;
    private readonly IPageBuffer _pageBuffer;
    private readonly int _alignedFixedStringLength;
    private readonly int _realFixedStringLength;
    private readonly int _intPerString;
    private readonly int _size;

    /// <summary>
    /// Initializes a new instance of the <see cref="CharVirtualMemory"/> class.
    /// </summary>
    /// <param name="pageBuffer">The page buffer used to manage pages of memory.</param>
    /// <param name="fixedStringLength">The fixed length of the strings to be stored.</param>
    /// <param name="size">The length of the array</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the fixed string length is negative or zero.</exception>
    public CharVirtualMemory(IPageBuffer pageBuffer, int fixedStringLength, int size)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fixedStringLength, "Длина строки");

        _pageBuffer = pageBuffer;
        _alignedFixedStringLength = (fixedStringLength + 3) / 4 * 4;
        _realFixedStringLength = fixedStringLength;
        _intPerString = _alignedFixedStringLength / 4;
        _size = size;
    }

    /// <summary>
    /// Sets the string value at the specified index in the virtual memory.
    /// </summary>
    /// <param name="index">The index at which to set the value.</param>
    /// <param name="value">The string value to set.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
    /// <exception cref="ArgumentException">Thrown when the string is too long.</exception>
    public void Set(int index, string value)
    {
        if (index < 0 || index >= _size)
            throw new IndexOutOfRangeException("Индекс вне границ массива");

        if (Encoding.UTF8.GetByteCount(value) > _realFixedStringLength)
            throw new ArgumentException("Строка слишком длинная");

        byte[] bytes = new byte[_alignedFixedStringLength];
        Encoding.UTF8.GetBytes(value, 0, value.Length, bytes, 0);

        int startElementIndex = index * _intPerString;
        int remainingInts = _intPerString;

        while (remainingInts > 0)
        {
            int pageIndex = startElementIndex / ElementsPerPage;
            int localIndex = startElementIndex % ElementsPerPage;

            var page = _pageBuffer.GetPage(pageIndex);

            int spaceInPage = ElementsPerPage - localIndex;
            int toWrite = Math.Min(spaceInPage, remainingInts);

            for (int i = 0; i < toWrite; i++)
            {
                page.Data[localIndex + i] = BitConverter.ToInt32(bytes, (i + (_intPerString - remainingInts)) * 4);
                page.BitMap[localIndex + i] = true;
            }

            _pageBuffer.MarkPageModified(pageIndex);

            remainingInts -= toWrite;
            startElementIndex += toWrite;
        }
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

        byte[] bytes = new byte[_alignedFixedStringLength];
        int startElementIndex = index * _intPerString;
        int remainingInts = _intPerString;

        while (remainingInts > 0)
        {
            int pageIndex = startElementIndex / ElementsPerPage;
            int localIndex = startElementIndex % ElementsPerPage;

            var page = _pageBuffer.GetPage(pageIndex);

            int spaceInPage = ElementsPerPage - localIndex;
            int toRead = Math.Min(spaceInPage, remainingInts);

            for (int i = 0; i < toRead; i++)
            {
                if (!page.BitMap[localIndex + i])
                    throw new InvalidOperationException("Элемент не инициализирован");

                Array.Copy(BitConverter.GetBytes(page.Data[localIndex + i]), 0, bytes,
                    (i + (_intPerString - remainingInts)) * 4, 4);
            }

            remainingInts -= toRead;
            startElementIndex += toRead;
        }

        return Encoding.UTF8.GetString(bytes).TrimEnd('\0');
    }

    /// <summary>
    /// Disposes the resources used by the virtual memory.
    /// </summary>
    public void Dispose()
    {
        _pageBuffer.Dispose();
        GC.SuppressFinalize(this);
    }
}