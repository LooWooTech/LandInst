using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.ImportUtil
{
    class Program
    {
        static IDbConnection _conn;
        static void Main(string[] args)
        {
            using (_conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            {
                _conn.Open();

                var names = GetAllNames();
                var newNames = GetFullNames();
                foreach(var newName in newNames)
                {
                    var ret = names.Where(x => newName.StartsWith(x.Value)).ToList(); ;
                    if(ret.Count>0)
                    {
                        foreach(var pair in ret)
                        {
                            //if (pair.Value != newName)
                            {
                                UpdateName(pair.Key, newName);                                
                            }
                            names.Remove(pair.Key);
                        }
                        
                    }
                    else
                    {
                        
                        CreateUser(newName);
                    }
                }
                Console.WriteLine(string.Join(",", names.Select(x => x.Key.ToString()).ToArray()));
            }  
                   
        }

        static List<string> GetFullNames()
        {
            var list = new List<string>();
            using (var reader = new StreamReader("Names.txt"))
            {
                var readline = reader.ReadLine();
                while (readline != null)
                {
                    if (string.IsNullOrEmpty(readline) == false) list.Add(readline);                    
                    readline = reader.ReadLine();
                }
            }
            return list;
        }
        static Dictionary<int, string> GetAllNames()
        {
            var dict = new Dictionary<int, string>();
            using (var reader = ExecuteReader("SELECT ID,Username FROM user WHERE role=2"))
            {
                while(reader.Read())
                {
                    dict.Add(Convert.ToInt32(reader[0]), Convert.ToString(reader[1]));
                }
            }
            return dict;
        }

        static void UpdateName(int id, string newName)
        {
            var sql = string.Format("UPDATE user SET Username='{0}' WHERE ID={1}", newName, id);
            ExecuteNonQuery(sql);
            if (ExistInst(id))
            {
                sql = string.Format("UPDATE institution SET Name='{0}' WHERE ID={1}", newName, id);
                ExecuteNonQuery(sql);
            }
            else
            {
                sql = string.Format("insert into institution(ID,Name,Status,CreateTime) values({0},'{1}',0,now())", id, newName);
                ExecuteNonQuery(sql);
            }
        }

        static bool ExistInst(int id)
        {
            using (var reader = ExecuteReader("SELECT ID FROM institution WHERE ID=" + id))
            {
                return reader.Read();
            }
        }

        static void CreateUser(string username)
        {
            var sql = string.Format("insert into user(Username, Password, RegisterTime, LastLoginTime, Role, Deleted) values('{0}', '202cb962ac59075b964b07152d234b70', now(), now(), 2, 0)", username);
            ExecuteNonQuery(sql);
            int id = -1;
            using (var reader = ExecuteReader("SELECT ID FROM user WHERE Username='" + username + "'"))
            {
                if(reader.Read())
                {
                    id = Convert.ToInt32(reader[0]);                    
                }
            }
            if (id > -1)
            {
                sql = string.Format("insert into institution(ID,Name,Status,CreateTime) values({0},'{1}',0,now())", id, username);
                ExecuteNonQuery(sql);
            }
        }

        static IDataReader ExecuteReader(string sql)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd.ExecuteReader();
        }
        

        static void ExecuteNonQuery(string sql)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
