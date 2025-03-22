namespace Prog_Lab_1.Presentation;

public class ApplicationLifecycle : IApplicationLifecycle
{
    public void Finish()
    {
        IsRunning = false;
    }
    
    public bool IsRunning { get; private set; } = true;
}