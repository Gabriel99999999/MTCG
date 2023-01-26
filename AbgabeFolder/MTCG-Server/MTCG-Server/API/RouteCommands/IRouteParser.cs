using MTCGServer.Core.Request;
using HttpMethod = MTCGServer.Core.Request.HttpMethod;

namespace MTCGServer.API.RouteCommands
{
    public interface IRouteParser
    {
        bool IsMatch(string resourcePath, string routePattern);
        Dictionary<string, string> ParseParameters(string resourcePath, string routePattern);
    }
}
