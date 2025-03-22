using Prog_Lab_1.Factory;
using VM.Domain;

namespace Prog_Lab_1.VM;

public class VirtualMemoryService(
    IntVirtualMemoryFactory intVirtualMemoryFactory,
    CharVirtualMemoryFactory charVirtualMemoryFactory,
    VarcharVirtualMemoryFactory varcharVirtualMemoryFactory)
    : IVirtualMemoryService
{
    private IVirtualMemory? _virtualMemory;
    private ArrayType.Types? _arrayType;

    public ArrayType.Types? ActiveArrayType => _arrayType;

    public bool IsActive => _virtualMemory != null;

    public void CreateCharVirtualMemory(string fileName, int size, int fixedStringLength)
    {
        if (_arrayType != null) throw new NullReferenceException();
        _arrayType = ArrayType.Types.Char;
        _virtualMemory = charVirtualMemoryFactory.Create(fileName, size, fixedStringLength);
    }

    public void CreateVarcharVirtualMemory(string fileName, int size, int maxStringLength)
    {
        if (_arrayType != null) throw new NullReferenceException();
        _arrayType = ArrayType.Types.Varchar;
        _virtualMemory = varcharVirtualMemoryFactory.Create(fileName, size, maxStringLength);
    }

    public void CreateIntVirtualMemory(string fileName, int size)
    {
        if (_arrayType != null) throw new NullReferenceException();
        _arrayType = ArrayType.Types.Int;
        _virtualMemory = intVirtualMemoryFactory.Create(fileName, size);
    }

    public string GetItemByIndex(int index)
    {
        var memory = RequireVirtualMemory();
        var value = memory.Get(index);
        return value.ToString();
    }

    public void CloseVirtualMemory()
    {
        RequireVirtualMemory().Dispose();
        _virtualMemory = null;
        _arrayType = null;
    }

    public void InsertValueToActiveVirtualMemory(int index, int value)
    {
        RequireVirtualMemory().Set(index, value);
    }

    public void InsertValueToActiveVirtualMemory(int index, string value)
    {
        RequireVirtualMemory().Set(index, value);
    }

    private IVirtualMemory RequireVirtualMemory()
    {
        if (_virtualMemory == null) throw new NullReferenceException();
        return _virtualMemory;
    }
}