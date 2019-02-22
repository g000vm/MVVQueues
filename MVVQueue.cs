using System;
using System.Collections.Generic;
using System.Threading;

namespace MVVQueues
{
    public class MVVQueue: MVVQueueSharedObjectsBase
    {
        private long _totalAdded=0;
        private long _leftToGo=0;

        List<MVVQueueWorker> _workers = new List<MVVQueueWorker>();
        List<MVVQueueCommand> commands = new List<MVVQueueCommand>();
        //int _currentCommands = 0;

        //BasicQueue commands = new BasicQueue();

        public delegate void QueueStateChanged(long leftToGo, long totalAdded);

        public event QueueStateChangedHandler QueueStateChangedEvent;

        int busyWorkers =0 ;

        /// <summary>
        /// arguments for the event
        /// </summary>
        private QueueEventArgs args = new QueueEventArgs();
        QueueStateChangedHandler handler;

        private void InvokeEvents ()
        {
            // calculate busy workers
            busyWorkers = 0;
            foreach(MVVQueueWorker worker in _workers) 
            {
                if (worker.isBusy) busyWorkers++; 
            }

            handler = QueueStateChangedEvent;

            args.TotalAdded = _totalAdded;
            args.NowInQueue = _leftToGo + busyWorkers;

            if (handler != null)
            {
                handler(this, args);
            }
        }

        public MVVQueue(int workersCount) 
        {
            for (int i = 0; i < workersCount; i++)
            {
                MVVQueueWorker worker = new MVVQueueWorker(this);
                _workers.Add(worker);
            }
        }

        object lockObject = new object();

        public void StopAllWorkers() 
        {
            for(int i=0;i<_workers.Count; i++) 
            {
                _workers[i].Stop();
            }
        }

        public void PushOnStart(MVVQueueCommand cmd) 
        {
            lock (lockObject)
            {
                commands.Insert(0,cmd);
                //commands.Push(new BasicQueueItem(cmd));

                _leftToGo++;
                _totalAdded++;

                InvokeEvents();
            }
        }

        public void Push(MVVQueueCommand cmd)
        {
            lock (lockObject)
            {
                commands.Add(cmd);
                //commands.Push(new BasicQueueItem(cmd));

                _leftToGo++;
                _totalAdded++;

                InvokeEvents();
            }
        }

        internal MVVQueueCommand Pop()
        {


            lock (lockObject) 
            {

                /*
                MVVQueueCommand result = commands.Pop();

                if (result != null) 
                {
                    _leftToGo--;

                    InvokeEvents();
                    return result;
                }
                return null;
                //MVVQueueCommand result
                */
                if (commands.Count > 0)
                {


                    int index = commands.Count - 1;
                    MVVQueueCommand cmd = commands[index];

                    commands.RemoveAt(index);

                    //_currentCommands++;

                    _leftToGo--;



                    return cmd;
                }

                InvokeEvents();

                return null;

            }

            return null;
        }

        public class QueueEventArgs : EventArgs
        {
            public long TotalAdded;
            public long NowInQueue;
        }

        public delegate void QueueStateChangedHandler(Object sender, QueueEventArgs e);
    }
}
