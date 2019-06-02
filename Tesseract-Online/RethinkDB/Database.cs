using Newtonsoft.Json;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract_Online
{
    public class Score
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long time { get; set; }
        public long when { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public int seed { get; set; }
    }

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

        public static void SetOnline(string username, bool online)
        {
            R.Db(DB).Table("users").Filter(doc => doc["username"] == username).Update(new { online }).Run<UserDTO>(c);
        }

        public static void SetScore(long time, long when, string u1, string u2, string u3, string u4, int seed)
        {
            var arr = new[]
            {
                new Score{time = time, when = when, u1 = u1, u2 = u2, u3 = u3, u4 = u4, seed = seed}
            };
            R.Db(DB).Table("scores").Insert(arr).Run<UserDTO>(c);
        }
    }
}
