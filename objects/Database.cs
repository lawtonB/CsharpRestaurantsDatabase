using System.Data;
using System.Data.SqlClient;

namespace FavoriteRestaurants
{
  public class DB
  {
    public static SqlConnection Connection()
    {
      SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
      return conn;
    }
  }
}
