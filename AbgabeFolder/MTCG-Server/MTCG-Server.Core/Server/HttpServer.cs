using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using HttpClient = MTCGServer.Core.Client.HttpClient;

namespace MTCGServer.Core.Server
{
    public class HttpServer : IServer
    {
        private bool _listening;

        private readonly TcpListener _listener;
        private readonly IRouter _router;

        public HttpServer(IPAddress address, int port, IRouter router)
        {
            _listener = new TcpListener(address, port);
            _router = router;
        }

        public void Start()
        {
            _listener.Start();
            _listening = true;

            while (_listening)
            {
                var connection = _listener.AcceptTcpClient();

                //in den () können parameter stehen und in {} steht die funktion selbst
                ThreadStart myLamda = () => { HandleConnection(connection); };
                Thread ConnectionHandler = new Thread(myLamda); //funktion handleConnection wird als funktion als parameter für den ThreadKonstruktor
                ConnectionHandler.Start();
            }
        }

        private void HandleConnection(object obj)
        {
            var client = new HttpClient((TcpClient)obj);

            var request = client.ReceiveRequest();
            Response.Response response = new Response.Response()
            {
                StatusCode = StatusCode.BadRequest
            };

            if (request != null)
            {
                try
                {
                    var command = _router.Resolve(request);
                    if (command != null)
                    {
                        // found a command for this request, now execute it
                        response = command.Execute();
                    }
                    else
                    {
                        Console.WriteLine("Im Switch existiert kein passender Command für den request");
                        response = new Response.Response()
                        {
                            StatusCode = StatusCode.BadRequest
                        };
                        // could not find a matching command for the request
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NotFoundException)
                    {
                        response.StatusCode = StatusCode.NotFound;
                    }
                    else if (ex is InvalidOperationException)
                    {
                        response.StatusCode = StatusCode.InternalServerError;
                    }
                    else if (ex is RouteNotAuthenticatedException)
                    {
                        response.StatusCode = StatusCode.Unauthorized;
                    }
                    else if (ex is NotImplementedException)
                    {
                        response.StatusCode = StatusCode.NotImplemented;
                    }
                    else if (ex is InvalidDataException)
                    {
                        response.StatusCode = StatusCode.BadRequest;
                    }
                }
            }

            client.SendResponse(response);
            client.Close();
        }

        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }

       
    }
}
