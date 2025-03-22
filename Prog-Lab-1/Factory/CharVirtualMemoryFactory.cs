
using VM.Domain;

namespace Prog_Lab_1.Factory;

public class CharVirtualMemoryFactory(IDateTimeService dateTimeService)
{
    public IVirtualMemory<string> Create(string fileName, int size, int fixedStringLength)
    {
        var charPageBuffer = CreatePageBuffer(fileName, size, fixedStringLength);
        return new CharVirtualMemory(charPageBuffer, fixedStringLength, size);
    }

    private IPageBuffer CreatePageBuffer(string fileName, int size, int fixedStringLength)
    {
        var swapfile = new SwapFile(fileName, fixedStringLength * size / sizeof(int));
        return new PageBuffer(swapfile, dateTimeService);
    }
}