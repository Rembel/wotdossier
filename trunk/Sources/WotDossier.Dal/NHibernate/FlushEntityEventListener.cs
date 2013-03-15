using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;

namespace Croc.Aws.DataAccess.NHibernate
{
    /// <summary>
    /// Event listener for flush events. It has been using when HHibernate session per request has been using for external services.
    /// At this case an entity is not stored in any intermediate storage when it is transfered to a service and back
    /// </summary>
    public class FlushEntityEventListener : DefaultFlushEntityEventListener
    {
        public override void OnFlushEntity(FlushEntityEvent @event)
        {
            EntityEntry entry = @event.EntityEntry;

            // It makes sense to check concurrency access via custom flush entity event listener if entity already exists in the DB
            if (entry.ExistsInDatabase)
            {
                IEventSource session = @event.Session;
                object entity = @event.Entity;
                IEntityPersister persister = entry.Persister;

                if (persister.IsVersioned)
                {
                    object version = persister.GetVersion(entity, session.EntityMode);
                    {
                        if (!persister.VersionType.IsEqual(version, entry.Version))
                        {
                            throw new ConcurrencyException(persister.EntityName, entry.Id);
                        }
                    }
                }
            }
            base.OnFlushEntity(@event);
        }
    }
}
