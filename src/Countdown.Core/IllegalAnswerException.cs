namespace Countdown.Core
{
    public class IllegalAnswerException : Exception
    {
        public IllegalAnswerException(string message) : base(message)
        {
        }
    }
}