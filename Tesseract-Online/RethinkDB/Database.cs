using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    class Database
    {
        public static RethinkDB R = RethinkDB.R;
        private static Connection c;
        private static readonly string DB = "tesseract";
        public Database()
        {
            c = R.Connection()
             .Hostname("localhost")
             .Port(RethinkDBConstants.DefaultPort)
             .Timeout(60)
             .Connect();
        }

        public static bool TryAuthentificate(string username, string password, out UserDTO user, bool isIRC = false)
        {
            
            Cursor<UserDTO> userCursor = R.Db(DB).Table("users").Filter(doc => doc["username"] == username).Run<UserDTO>(c);
            user = userCursor.FirstOrDefault();
            if (user != null && user.password == password.ToUpper())
                return true;
            return false;
        }

        public static bool TryGetUser(string username, out UserDTO user)
        {
            Cursor<UserDTO> userCursor = R.Db(DB).Table("users").Filter(doc => doc["username"] == username).Run<UserDTO>(c);
            user = userCursor.FirstOrDefault();
            return user != null;
        }
    }
}
