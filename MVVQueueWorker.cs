using System;
using System.Threading;

namespace MVVQueues
{
    public class MVVQueueWorker: MVVQueueSharedObjectsBase
    {
        private MVVQueue _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:queues.QueueWorker"/> class.
        /// </summary>
        /// <param name="queue">Queue to work on</param>
        internal MVVQueueWorker(MVVQueue queue):base()
        {
            _queue = queue;

            new Thread(this.Run).Start();
        }

        private bool _isRequestedToStop = false;

        private bool _isBusy;

        /// <summary>
        /// indicates  whenever current worker is executing something
        /// </summary>
        /// <value><c>true</c> if is busy; otherwise, <c>false</c>.</value>
        public bool isBusy 
        {
            get { return _isBusy; }
        }

       

        public object sharedObject 
        { get; set; }

        internal void Run() 
        {
            _isRequestedToStop = false;

            while (!_isRequestedToStop) 
            {
                MVVQueueCommand cmd =_queue.Pop();

                if (cmd != null) 
                {
                    _isBusy = true;

                    try 
                    {
                        cmd.worker = this;

                        cmd.ExecuteCommand();
                        cmd.OnComplete();
                    }
                    catch(Exception ex) 
                    {
                        cmd.OnError(ex);
                        _isBusy = false;

                        Console.WriteLine(ex.ToString());

                        throw ex;
                    }
                    _isBusy = false;
                }

                Thread.Sleep(1);
            }
        }

        internal void Stop() 
        {
            _isRequestedToStop = true;
        }
    }
}
