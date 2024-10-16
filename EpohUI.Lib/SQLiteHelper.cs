using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EpohUI.Lib
{
    public class SQLiteHelper
    {
        public static string Execute(string reqBody)
        {
            var req = JsonConvert.DeserializeObject<SQLiteReq>(reqBody);

            var connString = new SqliteConnectionStringBuilder()
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                DataSource = Path.Combine(Directory.GetCurrentDirectory(), req.File)
            }.ToString();

            using (var conn = new SqliteConnection(connString))
            {
                conn.Open();
                using (var command = new SqliteCommand(req.Sql, conn))
                {
                    if (req.Args != null)
                    {
                        var jarray = (JArray)req.Args;
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

    public class SQLiteReq
    {
        private string _sql;
        private string _file;

        public string Sql
        {
            get => _sql;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _sql = value;
            }
        }

        public string Args { get; set; }

        public string File
        {
            get => _file;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _file = value;
            }
        }
    }
}