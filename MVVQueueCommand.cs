using System;

namespace MVVQueues
{
    public abstract class MVVQueueCommand
    {

        #region private members

        MVVQueue _queue = null;

        #endregion

        public MVVQueueCommand(MVVQueue queue) {
            _queue = queue;
        }

        #region properties

        /// <summary>
        /// Gets the parent queue this coommand is placed to.
        /// </summary>
        /// <value>The parent queue.</value>
        public MVVQueue ParentQueue 
        {
            get { return _queue; }
        }

        internal MVVQueueWorker worker; 
        public object WorkerSharedObject
        {
            get { return worker.sharedObject; }
            set { worker.sharedObject = value; }
        }

        #endregion

        /// <summary>
        /// Executes the command.


        public abstract void ExecuteCommand();
        public virtual void OnError(Exception ex) { }
        public virtual void OnComplete() { }
    }
}