using Npgsql;

namespace MTCGServer.DAL
{
    internal interface IDB
    {
       protected NpgsqlConnection GetConnection();
    }
}
