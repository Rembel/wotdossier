using System;

namespace WotDossier.Domain.Replay
{
    public class ChatMessage
    {
        public string Player { get; set; }
        public string PlayerColor { get; set; }
        public string Text { get; set; }
        public string TextColor { get; set; }
        public TimeSpan Time { get; set; }

        public override string ToString()
        {
            return string.Format("{0:mm\\:ss}:{1}:\t{2}", Time, Player, Text);
        }
    }
}
