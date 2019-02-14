using System;
using System.Collections.Generic;
using System.Threading;

namespace MVVQueues
{
    public class MVVQueue: MVVQueueSharedObjectsBase
    {
        List<MVVQueueWorker> _workers = new List<MVVQueueWorker>();
        List<MVVQueueCommand> commands = new List<MVVQueueCommand>();


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

        public void Push(MVVQueueCommand cmd)
        {
            lock (lockObject)
            {
                commands.Add(cmd);
            }
        }

        internal MVVQueueCommand Pop()
        {
            MVVQueueCommand result = null;

            lock (lockObject) 
            { 

                if (commands.Count > 0)
                {
                    int index = commands.Count - 1;
                    MVVQueueCommand cmd = commands[index];
                    commands.RemoveAt(index);

                    return cmd;
                }

            }

            return result;
        }
    }
}
