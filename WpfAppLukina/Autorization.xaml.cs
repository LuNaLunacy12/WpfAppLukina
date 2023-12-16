using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppLukina
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        public Autorization()
        {
            InitializeComponent();
        }

        private void bSingIn_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlCommand cmd = Connection.GetCommand("Select \"ID\", \"Login\", \"Password\", \"FullName\",  \"Email\" from \"User\"" +
                    "WHERE \"Login\" = @log AND \"Password\" = @pass");
            cmd.Parameters.AddWithValue("@log", NpgsqlDbType.Varchar, login.Text.Trim());
            cmd.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, password.Text.Trim());
            NpgsqlDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Connection.clUsers = new ClUsers(result.GetInt32(0), result.GetString(1), result.GetString(2), result.GetString(3), result.GetString(4));

                }
                result.Close();
                NavigationService.Navigate(new ContentTasks());
            }
            else
            {
                MessageBox.Show("Проверьте правильность вводимых данных!");
            }

        }
    }
}
