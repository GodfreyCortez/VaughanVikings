using SQLite;
using System.IO;
using Vikings.UWP;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]

namespace Vikings.UWP
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = ApplicationData.Current.LocalFolder.Path;
            var path = Path.Combine(documentsPath, "MySQLite.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}
