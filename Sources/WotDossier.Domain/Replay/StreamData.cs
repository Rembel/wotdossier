using System.Collections.Generic;

namespace WotDossier.Domain.Replay
{
    public class StreamData
    {
        private List<ChatMessage> _messages = new List<ChatMessage>();
        public List<ChatMessage> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        private List<Slot> _slots = new List<Slot>();
        public List<Slot> Slots
        {
            get { return _slots; }
            set { _slots = value; }
        }
    }
}