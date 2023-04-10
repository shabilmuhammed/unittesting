using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adf_unit_testing
{
    public class databasehelper
    {
        public int RowCount(string tableName)
        {
            using (var conn = new SqlConnection("CONNECTION_STRING"))
            {
                conn.Open();
                using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }
    }
}
