using MTCGServer.BLL;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using System.Net;
using System.Net.Sockets;
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
//ab hier den code in einem eigenen THREAD laufen lassen
                
                // create a new disposable handler for the client connection
                var client = new HttpClient(connection);

                var request = client.ReceiveRequest();
                Response.Response response;

                if (request == null)
                {
                    // could not parse request
                    response = new Response.Response()
                    {
                        StatusCode = StatusCode.BadRequest
                    };
                }
                else
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
                            // could not find a matching command for the request
                            response = new Response.Response()
                            {
                                StatusCode = StatusCode.BadRequest
                            };
                        }
                    }
                    catch(Exception ex)
                    {
                        if (ex is UserNotFoundException)
                        {
                            response = new Response.Response()
                            {
                                StatusCode = StatusCode.NotFound
                            };
                        }
                        else if(ex is InvalidOperationException)
                        {
                            response = new Response.Response()
                            {
                                //StatusCode = StatusCode.NotFound
                            };
                        }
                        else if (ex is RouteNotAuthenticatedException)
                        {
                            response = new Response.Response()
                            {
                                //StatusCode = StatusCode.NotFound
                            };
                        }
                        /*else if (ex is AccessTokenMissingException)
                        {
                            response = new Response.Response()
                            {
                                //StatusCode = StatusCode.NotFound
                            };
                        }*/
                        else if (ex is NotImplementedException)
                        {
                            response = new Response.Response()
                            {
                                StatusCode = StatusCode.NotImplemented
                            };
                        }
                        else if (ex is InvalidDataException)
                        {
                            response = new Response.Response()
                            {
                                //StatusCode = StatusCode.NotFound
                            };
                        }

                    }

                    // TODO: handle invalid data, route not authenticated
                }

                client.SendResponse(response);
                client.Close();
            }
        }

        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }
    }
}
