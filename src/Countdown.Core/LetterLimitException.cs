namespace Countdown.Core
{
    public class LetterLimitException : Exception
    {
        public LetterLimitException(int limit) : base($"Cannot add more than {limit} letters.")
        {
        }
    }
}