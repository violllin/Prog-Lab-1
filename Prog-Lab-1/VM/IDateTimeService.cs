namespace VM.Domain;

/// <summary>
/// Provides an interface for obtaining the current date and time.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Gets the current date and time.
    /// </summary>
    /// <returns>The current date and time.</returns>
    DateTime CurrentDateTime();
}