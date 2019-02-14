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
        #endregion

        /// <summary>
        /// Executes the command.
        /// </summary>
        public abstract void ExecuteCommand();
    }
}