using System.Collections.Generic;
using System.Collections.Specialized;

namespace MVVQueues
{
    public class MVVQueueSharedObjectsBase
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        #region public 

        public object GetSharedObjectForKey(string key) 
        {
            object result = null;

            dict.TryGetValue(key, out result);

            return null; 
        }

        public void SetShareObjectForKey(string key, object obj) 
        {
            dict.Add(key, obj); 
        }

        #endregion

         
    }
}