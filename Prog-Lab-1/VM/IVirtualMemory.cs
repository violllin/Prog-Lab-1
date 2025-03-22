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

    /// <summary>
    /// Gets the value at the specified index.
    /// </summary>
    /// <param name="index">The index from which to get the value.</param>
    /// <returns>The value at the specified index.</returns>
    object Get(int index);
}

/// <summary>
/// Generic interface representing a virtual memory that supports setting and getting values of a specific type by index.
/// </summary>
/// <typeparam name="T">The type of values stored in the virtual memory.</typeparam>
public interface IVirtualMemory<T> : IVirtualMemory
{
    /// <summary>
    /// Sets the value of type <typeparamref name="T"/> at the specified index.
    /// </summary>
    /// <param name="index">The index at which to set the value.</param>
    /// <param name="value">The value of type <typeparamref name="T"/> to set.</param>
    void Set(int index, T value);

    /// <summary>
    /// Gets the value of type <typeparamref name="T"/> at the specified index.
    /// </summary>
    /// <param name="index">The index from which to get the value.</param>
    /// <returns>The value of type <typeparamref name="T"/> at the specified index.</returns>
    new T Get(int index);

    /// <summary>
    /// Gets the value at the specified index as an object.
    /// </summary>
    /// <param name="index">The index from which to get the value.</param>
    /// <returns>The value at the specified index as an object.</returns>
    object IVirtualMemory.Get(int index)
    {
        return Get(index);
    }

    /// <summary>
    /// Sets the value at the specified index as an object.
    /// </summary>
    /// <param name="index">The index at which to set the value.</param>
    /// <param name="value">The value to set as an object.</param>
    /// <exception cref="ArgumentException">Thrown when the value is not of type <typeparamref name="T"/>.</exception>
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