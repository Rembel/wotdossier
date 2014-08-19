namespace WotDossier.Domain.Replay
{
    public class ChatMessage
    {
        public string Player { get; set; }
        public string PlayerColor { get; set; }
        public string Text { get; set; }
        public string TextColor { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:\t{1}", Player, Text);
        }
    }
}
