namespace Server.Models
{
    public class SpeakerData
    {
        public string SpeakerName { get; }
        public TimeSpan Start {  get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan Duration { get { return End - Start; } }

        public SpeakerData(string speakerName, TimeSpan start, TimeSpan end)
        {
            SpeakerName = speakerName;
            Start = start;
            End = end;
        }
    }
}
