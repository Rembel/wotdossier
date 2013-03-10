using System;

namespace WotDossier.Framework.EventAggregator
{
    public interface IDelegateReference
    {
        Delegate Target
        {
            get;
        }
    }
}