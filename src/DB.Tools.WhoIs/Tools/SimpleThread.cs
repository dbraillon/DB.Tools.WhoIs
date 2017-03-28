using System.Threading;

namespace DB.Tools.WhoIs.Threading
{
    /// <summary>
    /// Quick implementation of a Thread.
    /// </summary>
    public abstract class SimpleThread
    {
        public Thread Thread { get; set; }
        public bool IsRunning { get; set; }
        public abstract int SleepTime { get; set; }

        public SimpleThread()
        {
            Thread = new Thread(Loop);
            IsRunning = false;
        }

        public void Loop()
        {
            while (IsRunning)
            {
                Execute();
                Thread.Sleep(SleepTime);
            }
        }
        public abstract void Execute();

        public void Start()
        {
            IsRunning = true;
            Thread.Start();
        }
        public void Stop()
        {
            IsRunning = false;
            Thread.Join();
        }
    }
}
