using SQLite;

namespace Vikings
{
    public interface ISQLiteDb //This is an interface which will act as a connector to the SQLite database
    {
        SQLiteAsyncConnection GetConnection();//get a connection
    }
}
