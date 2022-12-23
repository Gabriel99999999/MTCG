using MTCGServer.Core.Request;

namespace MTCGServer.Core.Routing
{
    public interface IRouter
    {
        ICommand? Resolve(Request.Request request);
    }
}