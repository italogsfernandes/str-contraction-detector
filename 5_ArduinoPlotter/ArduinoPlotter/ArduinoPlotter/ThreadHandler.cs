using System.Threading;

namespace ArduinoPlotter
{
    //
    // Summary:
    //     Higher methods to controls a thread a thread with a loop.
    public class ThreadHandler
    {
        
        private bool _is_alive; //Flag to indicate if the loop is running.
        private bool _is_running; //Flag to indicate if the loop isn't paused.
        private ThreadRoutine _routine; //Method specified by the user to be called repeatedly.

        //
        // Summary:
        //     Object to creates and controls a thread, sets its priority, and gets its status.
        public Thread thead;

        //
        // Summary:
        //     Delegate to the routine specified by the user to be called repeatedly.
        //
        public delegate void ThreadRoutine();

        //
        // Summary:
        //     Initializes a new instance of the ThreadHandler class.
        //
        // Parameters:
        //   threadfunction:
        //     A ThreadRoutine delegate that represents the methods to be invoked
        //    repeatedly when this thread is running.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     The start parameter is null.
        public ThreadHandler(ThreadRoutine threadfunction)
        {
            if (threadfunction.Equals(null))
            {
                throw new System.ArgumentNullException("threadfunction", "The threadfunction parameter is null.");
            }
            _routine = threadfunction;
            thead = new Thread(DoRoutine);
        }

        private void DoRoutine()
        {
            while (_is_alive)
            {
                if (_is_running)
                {
                    _routine();
                }
            }
        }

        public void Start()
        {
            _is_alive = true;
            _is_running = true;
            thead.Start();
        }

        public void Stop()
        {
            _is_alive = false;
            _is_running = false;
        }

        public void Pause()
        {
            _is_running = false;
        }

        public void Resume()
        {
            _is_running = true;
        }


    }
}
