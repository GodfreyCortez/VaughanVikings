using System;
using SQLite;
using System.IO;
using Xamarin.Forms;
using Vikings.iOS;

[assembly: Dependency(typeof(SQLiteDb))]

namespace Vikings.iOS
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}