using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace EpohUI.Lib
{
    public class SQLiteHelper
    {

        public static string Execute(string reqBody)
        {
            var req = JsonConvert.DeserializeObject<Dictionary<string, object>>(reqBody);
            req.TryGetValue("sql", out var sql);
            req.TryGetValue("args", out var args);
            req.TryGetValue("file", out var file);

            var connString = new SqliteConnectionStringBuilder()
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                DataSource = Path.Combine(Directory.GetCurrentDirectory(), file.ToString())
            }.ToString();

            using (var conn = new SqliteConnection(connString))
            {
                conn.Open();
                using (var command = new SqliteCommand(sql.ToString(), conn))
                {
                    if (args != null)
                    {
                        var jarray = (JArray)args;
                        for (var i = 0; i < jarray.Count; i++)
                        {
                            var jvalue = (JValue)jarray[i];
                            command.Parameters.AddWithValue($"#p{i + 1}", jvalue.Value ?? DBNull.Value);
                        }
                    }
                    var list = new List<Dictionary<string, object>>();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var ele = new Dictionary<string, object>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            ele[reader.GetName(i)] = reader.GetValue(i);
                        }
                        list.Add(ele);
                    }
                    return JsonConvert.SerializeObject(list);
                }
            }
        }

        public static string GetMethodIdMap()
        {
            return $"lib/sqlite-execute={typeof(SQLiteHelper).FullName}#Execute";
        }

    }
}
