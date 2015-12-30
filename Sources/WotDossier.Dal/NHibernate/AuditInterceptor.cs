using System;
using NHibernate;
using NHibernate.Type;
using WotDossier.Domain.Entities;

namespace WotDossier.Dal.NHibernate
{
    public class AuditInterceptor : EmptyInterceptor
    {
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames,
            IType[] types)
        {
            IRevised revised = entity as IRevised;

            if (revised != null)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == nameof(IRevised.Rev))
                    {
                        currentState[i] = RevisionProvider.GetRev();
                    }
                }
                return true;
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            IRevised revised = entity as IRevised;

            if (revised != null)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == nameof(IRevised.Rev))
                    {
                        state[i] = RevisionProvider.GetRev();
                    }
                }
                return true;
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        
    }

    public static class RevisionProvider
    {
        private static int _rev = 0;

        public static void SetParentContext(IRevised context)
        {
            if (_rev == 0)
            {
                _rev = Int32.Parse(DateTime.Now.ToString("yyyyMMdd")) * 100;
            }
            if (_rev > context.Rev)
            {
                context.Rev = _rev;
            }
            context.Rev++;
            _rev = context.Rev;
        }

        public static int GetRev()
        {
            return _rev;
        }
    }
}
