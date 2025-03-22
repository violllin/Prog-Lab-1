using VM.Domain;

namespace Prog_Lab_1.Factory;

public class VarcharVirtualMemoryFactory
{
    public IVirtualMemory<string> Create(string fileName, int size, int maxStringLength)
    {
        var swapFile = new SwapFile(fileName, size);
        return new VarcharVirtualMemory(swapFile, $"{fileName}.data", maxStringLength, size);
    }
}