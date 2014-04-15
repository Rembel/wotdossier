using System;
using System.Collections;
using System.Collections.Generic;

namespace WotDossier.Framework
{
    public class CallbackDataSource<T> : IEnumerable<T>
    {
        private readonly Func<List<T>> _func;

        public CallbackDataSource(Func<List<T>> func)
        {
            _func = func;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return _func().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}