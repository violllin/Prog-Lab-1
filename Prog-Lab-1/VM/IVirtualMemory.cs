namespace VM.Domain;

/// <summary>
/// Interface representing a virtual memory that supports setting and getting values by index.
/// </summary>
public interface IVirtualMemory : IDisposable
{
    /// <summary>
    /// Sets the value at the specified index.
    /// </summary>
    /// <param name="index">The index at which to set the value.</param>
    /// <param name="value">The value to set.</param>
    void Set(int index, object value);
    
    object Get(int index);
}

public interface IVirtualMemory<T> : IVirtualMemory
{
    void Set(int index, T value);
    
    new T Get(int index);
    
    object IVirtualMemory.Get(int index)
    {
        return Get(index);
    }
    
    void IVirtualMemory.Set(int index, object value)
    {
        if (value is T tValue)
        {
            Set(index, tValue);
        }
        else
        {
            throw new ArgumentException("Invalid value type");
        }
    }
}