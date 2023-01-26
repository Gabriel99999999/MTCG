using System.Runtime.Serialization;

namespace MTCGServer.API.RouteCommands.Game
{
    [Serializable]
    internal class DeckNotCorrectConfiguredException : Exception
    {
        public DeckNotCorrectConfiguredException()
        {
        }

        public DeckNotCorrectConfiguredException(string? message) : base(message)
        {
        }

        public DeckNotCorrectConfiguredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DeckNotCorrectConfiguredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}