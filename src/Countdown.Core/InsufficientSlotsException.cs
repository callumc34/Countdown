namespace Countdown.Core
{
    public class InsufficientSlotsException : Exception
    {
        public InsufficientSlotsException(string message) : base(message)
        {
        }
    }
}