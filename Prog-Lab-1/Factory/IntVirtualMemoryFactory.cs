using VM.Domain;

namespace Prog_Lab_1.Factory;

public class IntVirtualMemoryFactory(IDateTimeService dateTimeService)
{
    public IVirtualMemory<int> Create(string fileName, int size)
    {
        var pageBuffer = CreatePageBuffer(fileName, size);
        return new IntVirtualMemory(pageBuffer, size);
    }


    private IPageBuffer CreatePageBuffer(string fileName, int size)
    {
        var swapfile = new SwapFile(fileName, size);
        return new PageBuffer(swapfile, dateTimeService);
    }
}