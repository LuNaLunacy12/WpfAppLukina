
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Net.Sockets;
using System.Windows.Media;

namespace WpfAppLukina
{
    public class Connection
    {
        public static NpgsqlConnection connection;
        public static ClUsers clUsers;
        public static ClTasks clTasks;
        public static ClStatus clStatus;

        public static void Connect(string host, string port, string user, string pass, string database)
        {
            string cs = string.Format("Server = {0}; Port = {1}; User Id = {2}; Password = {3}; Database = {4}", host, port, user, pass, database);

            connection = new NpgsqlConnection(cs);
            connection.Open();
        }

        public static ObservableCollection<ClUsers> users { get; set; } = new ObservableCollection<ClUsers> { };
        public static ObservableCollection<ClStatus> status { get; set; } = new ObservableCollection<ClStatus> { };

        public static ObservableCollection<ClTasks> tasks { get; set; } = new ObservableCollection<ClTasks> { };

        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = sql;
            return command;
        }

        public static void SelectTableUsers()
        {
            NpgsqlCommand cmd = GetCommand("Select \"ID\", \"Login\", \"Password\", \"FullName\",  \"Email\" from \"User\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    users.Add(new ClUsers(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetString(3), result.GetString(4)));
                }

            }
            result.Close();
        }

        public static void SelectTableStatus()
        {
            NpgsqlCommand cmd = GetCommand("Select \"NameStatus\" from \"Status\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    status.Add(new ClStatus(result.GetString(0)));
                }

            }
            result.Close();
        }

        public static void SelectTableTasks()
        {
            tasks.Clear();
            NpgsqlCommand cmd = GetCommand("Select \"ID\", \"Heading\", \"Description\", \"DateCreate\",  \"Status\",  \"User\" from \"Tasks\"");
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    tasks.Add(new ClTasks(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetDateTime(3), result.GetString(4), result.GetInt32(5)));
                }

            }
            result.Close();
        }

    }
}
