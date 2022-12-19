using MTCGServer.Core.Request;

namespace MTCGServer.Core.Routing
{
    public interface IRouter
    {
        IRouteCommand? Resolve(Request.Request request);
    }
}