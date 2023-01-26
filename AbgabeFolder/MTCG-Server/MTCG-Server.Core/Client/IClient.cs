using MTCGServer.Core.Request;

namespace MTCGServer.Core.Client
{
    public interface IClient
    {
        public Request.Request? ReceiveRequest();
        public void SendResponse(Response.Response response);
        public void Close();
    }
}
