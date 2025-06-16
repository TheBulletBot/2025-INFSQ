using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ScooterBackend{
    public static class DatabaseFunctions{

        public static void Query(){
            using(var connection = new SqliteConnection("Data Source=db/db/INFSQScooterBackend.db")){
                throw new NotImplementedException();
            }
        }
    }
}
