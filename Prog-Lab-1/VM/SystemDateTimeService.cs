namespace VM.Domain;

public class SystemDateTimeService : IDateTimeService
{
    public DateTime CurrentDateTime()
    {
        return DateTime.Now;
    }
}