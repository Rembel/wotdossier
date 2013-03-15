using System;

namespace WotDossier.Framework.EventAggregator
{
    public class SubscriptionToken : IEquatable<SubscriptionToken>
    {
        private readonly Guid _token = Guid.NewGuid();

        public bool Equals(SubscriptionToken other)
        {
            return (other != null) && Equals(_token, other._token);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as SubscriptionToken);
        }

        public override int GetHashCode()
        {
            return _token.GetHashCode();
        }

        public override string ToString()
        {
            return _token.ToString();
        }
    }
}