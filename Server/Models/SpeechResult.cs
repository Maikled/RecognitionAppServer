namespace Server.Models
{
    public class SpeechResult
    {
        public string SpeakerName { get; }
        public ICollection<SpeechSegment> Segments { get; } = new List<SpeechSegment>();

        public SpeechResult(string speakerName)
        {
            SpeakerName = speakerName;
        }

        public SpeechResult(string speakerName, ICollection<SpeechSegment> segments) : this(speakerName)
        {
            Segments = segments;
        }
    }
}
