namespace Server.Models
{
    public class SpeechSegment
    {
        public string Text { get; }
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public TimeSpan Duration { get { return End - Start; } }

        public SpeechSegment(string text, TimeSpan start, TimeSpan end)
        {
            Text = text;
            Start = start;
            End = end;
        }
    }
}
