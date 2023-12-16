using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    /// Логика взаимодействия для ContentTasks.xaml
    /// </summary>
    public partial class ContentTasks : Page
    {
   
            public ContentTasks()
        {
            InitializeComponent();
            lvBindingTasks();
            cbBindingStatus();
            ApplyFilterForm();


            if (Connection.users != null)
            {
                LogIn.Content = Connection.clUsers.FullName;
            }
        }

        public void ApplyFilterForm()
        {

            ICollectionView view = CollectionViewSource.GetDefaultView(lvTasks.ItemsSource);
            if (view == null) { return; }

            if (view.CanFilter == true)
            {
                view.Filter = new Predicate<object>(o =>
                {
                    ClTasks form = o as ClTasks;
                    if (form == null) return false;

                    return form.User == Connection.clUsers.ID ;
                });
            }
        }
        public void lvBindingTasks()
        {
            Binding binding = new Binding
            { Source = Connection.tasks };
            lvTasks.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            Connection.SelectTableTasks();

        }

        public void cbBindingStatus()
        {
            Binding binding = new Binding
            { Source = Connection.status };
            tbStatus.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            Connection.SelectTableStatus();

        }
        public void BindingcbStatusSort()
        {
            Binding binding = new Binding
            { Source = Connection.status };
            cbStatusSort.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            Connection.SelectTableStatus();

        }

        private void Filter()
        {
            string searchString = tbSearch.Text.Trim();

            var view = CollectionViewSource.GetDefaultView(lvTasks.ItemsSource);
            view.Filter = new Predicate<object>(o =>
            {
                ClTasks product = o as ClTasks;
                if (product == null) { return false; }

                bool isVisible = true;
                if (searchString.Length > 0)
                {
                    isVisible = product.Heading.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 ||
                        product.Description.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1;
                }
                return isVisible;
            });
        }

        public void ClearingInformationElements()
        {
            lvTasks.SelectedItem = null;
            tbHeading.Clear();
            tbDescription.Clear();
            tbStatus.Text = null;
            dpDateCreate.Text = null;


        }

        private void lvTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvTasks.SelectedItem != null)
            {
                tbHeading.Text = (lvTasks.SelectedItem as ClTasks).Heading.ToString();
                tbDescription.Text = (lvTasks.SelectedItem as ClTasks).Description.ToString();
                tbStatus.Text = (lvTasks.SelectedItem as ClTasks).Status.ToString();
                dpDateCreate.Text = (lvTasks.SelectedItem as ClTasks).Datecreate.ToString();

            }
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var heading = tbHeading.Text.Trim();
            var description = tbDescription.Text.Trim();
            var status = tbStatus.Text.Trim();
            var datecreate = dpDateCreate.SelectedDate;

            ClStatus namestatus = tbStatus.SelectedItem as ClStatus;
            

            NpgsqlCommand cmd = Connection.GetCommand("insert into \"Tasks\" (\"Heading\", \"Description\", \"DateCreate\", \"Status\", \"User\" )" +
                 "values (@heading, @descripthion, @datecreate, @status, @user) returning \"ID\"");
            cmd.Parameters.AddWithValue("@heading", NpgsqlDbType.Varchar, heading);
            cmd.Parameters.AddWithValue("@descripthion", NpgsqlDbType.Varchar, description);
            cmd.Parameters.AddWithValue("@datecreate", NpgsqlDbType.Date, datecreate);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, namestatus.NameStatus);
            cmd.Parameters.AddWithValue("@user", NpgsqlDbType.Integer, Connection.clUsers.ID);

            var result = cmd.ExecuteScalar();

            if (result == null)
            {
                MessageBox.Show("Данные не добавлены");
            }
            else
            {
                MessageBox.Show("Данные добавлены");
                Connection.tasks.Clear();
                lvBindingTasks();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearingInformationElements();
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            var heading = tbHeading.Text.Trim();
            var description = tbDescription.Text.Trim();
            var status = tbStatus.Text.Trim();
            var datecreate = dpDateCreate.SelectedDate;

            ClStatus namestatus = tbStatus.SelectedItem as ClStatus;
            ClTasks tasks = lvTasks.SelectedItem as ClTasks;


            NpgsqlCommand cmd = Connection.GetCommand("UPDATE \"Tasks\" SET \"Heading\" = @heading, \"Description\" = @descripthion, \"DateCreate\" = @datecreate, \"Status\" = @Status WHERE \"ID\" = @id");

            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, tasks.ID);
            cmd.Parameters.AddWithValue("@heading", NpgsqlDbType.Varchar, heading);
            cmd.Parameters.AddWithValue("@descripthion", NpgsqlDbType.Varchar, description);
            cmd.Parameters.AddWithValue("@datecreate", NpgsqlDbType.Date, datecreate);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, namestatus.NameStatus);
            cmd.Parameters.AddWithValue("@user", NpgsqlDbType.Integer, Connection.clUsers.ID);

            

            var result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                MessageBox.Show("Данные не изменены");
            }
            else
            {
                MessageBox.Show("Данные изменены");
                Connection.tasks.Clear();
                lvBindingTasks();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            
            ClTasks tasks = lvTasks.SelectedItem as ClTasks;


            NpgsqlCommand cmd = Connection.GetCommand("Delete FROM \"Tasks\" WHERE \"ID\" = @id");
            cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, tasks.ID);
            cmd.Parameters.AddWithValue("@heading", NpgsqlDbType.Varchar, tasks.Heading);
            cmd.Parameters.AddWithValue("@descripthion", NpgsqlDbType.Varchar, tasks.Description);
            cmd.Parameters.AddWithValue("@datecreate", NpgsqlDbType.Date, tasks.Datecreate);
            cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, tasks.Status);
            cmd.Parameters.AddWithValue("@user", NpgsqlDbType.Integer, tasks.User);

            var result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                MessageBox.Show("Данные не удалены");
            }
            else
            {
                MessageBox.Show("Данные удалены");
                Connection.tasks.Clear();
                lvBindingTasks();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void BtnSortDate_Click(object sender, RoutedEventArgs e)
        {

            Connection.tasks.Clear();
            var datecreate = dpSortStatusDate.SelectedDate;
            NpgsqlCommand cmd = Connection.GetCommand("Select \"ID\", \"Heading\", \"Description\", \"DateCreate\",  \"Status\",  \"User\" from \"Tasks\" Where \"DateCreate\" = @datecreate");
            cmd.Parameters.AddWithValue("@datecreate", NpgsqlDbType.Date, datecreate);

            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    Connection.tasks.Add(new ClTasks(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetString(4), reader.GetInt32(5)));
                }
            }


        }

        private void BtnSortStatus_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
