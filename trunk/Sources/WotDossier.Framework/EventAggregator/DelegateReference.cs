using System;
using System.Reflection;

namespace WotDossier.Framework.EventAggregator
{
    /// <summary>
    /// 
    /// </summary>
    public class DelegateReference : IDelegateReference
    {
        private readonly Delegate _delegate;
        private readonly WeakReference _weakReference;
        private readonly MethodInfo _method;
        private readonly Type _delegateType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateReference"/> class.
        /// </summary>
        /// <param name="delegate">The @delegate.</param>
        /// <param name="keepReferenceAlive">if set to <c>true</c> [keep reference alive].</param>
        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            if (keepReferenceAlive)
            {
                _delegate = @delegate;
            }
            else
            {
                _weakReference = new WeakReference(@delegate.Target);
                _method = @delegate.Method;
                _delegateType = @delegate.GetType();
            }
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public Delegate Target
        {
            get
            {
                return _delegate ?? TryGetDelegate();
            }
        }

        private Delegate TryGetDelegate()
        {
            if (_method.IsStatic)
            {
                return Delegate.CreateDelegate(_delegateType, null, _method);
            }

            object target = _weakReference.Target;

            return (target != null) ? Delegate.CreateDelegate(_delegateType, target, _method) : null;
        }
    }
}