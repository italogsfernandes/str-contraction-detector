using System.Threading;

namespace DetectorContracao
{
    //
    // Summary:
    //     Higher methods to controls a thread a thread with a loop.
    public class ThreadHandler
    {
        
        private bool _is_alive; //Flag to indicate if the loop is running. - indicar que o loop está sendo executado
        private bool _is_running; //Flag to indicate if the loop isn't paused. - indicar que o loop não está pausado
        private ThreadRoutine _routine; //Method specified by the user to be called repeatedly. - método a ser chamada repetidamente
        private ThreadRoutine _onEndFunction; //Method specified by the user to be called repeatedly.

        //
        // Summary:
        //     Object to creates and controls a thread, sets its priority, and gets its status.
        public Thread thread; // declaração de uma thread 

        //
        // Summary:
        //     Delegate to the routine specified by the user to be called repeatedly.
        //
        public delegate void ThreadRoutine(); //declaração do delegate

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
        public ThreadHandler(ThreadRoutine threadfunction) //recebe o método a ser invocado repetidamente pela thread
        {
            if (threadfunction.Equals(null))
            {
                throw new System.ArgumentNullException("threadfunction", "The threadfunction parameter is null.");
            }
            _routine = threadfunction; //método especificado
            _onEndFunction = new ThreadRoutine(doNothing);
            thread = new Thread(DoRoutine);
        }

        private void DoRoutine() //realizar a rotina da thread
        {
            while (_is_alive) //se a thread estiver "viva" 
            {
                if (_is_running) //e não estiver pausada
                {
                    _routine(); //realizar a rotina especificada
                }
            }
            _onEndFunction(); //função executada pela thread quando essa acaba o while
        }

        public void setOnEndFunction(ThreadRoutine onEndFunction) //setar o método de onEndFunction
        {
            _onEndFunction = onEndFunction;
        }

        private void doNothing() //não realiza nada 
        {

        }

        public void Start() //começar o funcionamento da thread
        {
            _is_alive = true; //thread está viva
            _is_running = true; //thread não está pausada
            thread.Start(); //começar a thread
        }

        public void Stop() //"apagar a thread"
        {
            _is_alive = false; //thread não está viva
            _is_running = false; //thread não está rodando
            thread = new Thread(DoRoutine);
        }

        public void Pause()//pausar a thread
        {
            _is_running = false;
        }

        public void Resume() //"despausar" a thread
        {
            _is_running = true;
        }


    }
}
