namespace Prog_Lab_1.VM;

public interface IVirtualMemoryService
{
    public ArrayType.Types? ActiveArrayType { get; }
    
    public bool IsActive { get; }
    
    void CreateCharVirtualMemory(string fileName, int size, int fixedStringLength);

    void CreateVarcharVirtualMemory(string fileName, int size, int maxStringLength);

    void CreateIntVirtualMemory(string fileName, int size);

    string GetItemByIndex(int index);

    void CloseVirtualMemory();

    void InsertValueToActiveVirtualMemory(int index, int value);
    
    void InsertValueToActiveVirtualMemory(int index, string value);
}