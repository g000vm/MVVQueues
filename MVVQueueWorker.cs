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

        internal void Run() 
        {
            _isRequestedToStop = false;

            while (!_isRequestedToStop) 
            {
                MVVQueueCommand cmd =_queue.Pop();

                if (cmd != null) 
                {
                    try 
                    { 
                        cmd.ExecuteCommand();
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);

                        throw ex;
                    }
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
