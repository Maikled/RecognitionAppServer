namespace Server.Models.Exceptions
{
    public class SpeechException : Exception
    {
        public int ErrorCode { get; set; }

        public SpeechException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}