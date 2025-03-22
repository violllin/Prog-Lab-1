
namespace VM.Domain;

using System;
using System.Collections;

public class Page: IPage
{
    public int PageIndex { get; }
    public DateTime AccessTime { get; set; }
    public bool IsModified { get; set; }
    public BitArray BitMap { get; }
    public int[] Data { get; }

    public Page(int pageIndex, BitArray bitMap, int[] data)
    {
        if (bitMap is not { Length: 128 })
            throw new ArgumentException("BitMap должен содержать 128 бит", nameof(bitMap));
        if (data is not { Length: 128 })
            throw new ArgumentException("Data должен содержать 128 элементов", nameof(data));

        PageIndex = pageIndex;
        AccessTime = DateTime.Now;
        IsModified = false;
        BitMap = bitMap;
        Data = data;
    }
}