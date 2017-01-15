using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class AuthDatabase
    {
        readonly SQLiteAsyncConnection database;

        public AuthDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<User>().Wait();
        }

        public async Task<User> GetUserAsync()
        {
            return await database.Table<User>().FirstOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            return await database.InsertAsync(user);
        }

        public async Task<int> DeleteUserAsync(User user)
        {
            return await database.DeleteAsync(user);
        }
    }
}
