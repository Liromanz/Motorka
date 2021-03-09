using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Motorka
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string connectionString = @"Data Source=DESKTOP-UA4MNST\BUBOCHKA;Initial Catalog=Motorka_Base;User ID=sa;Password=1056vQOAKZMtalvgty";
        SqlConnection connection;
        SqlDataAdapter dataAdapter;
        public static int userId;
        public MainPage()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter("select * from [User]", connection);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                bool logPassError = true;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var row = ds.Tables[0].Rows[i].ItemArray;
                    if (loginTextBox.Text == (string)row[4] && passwordBox.Password == (string)row[5])
                    {
                        userId = (int)row[0];
                        connection.Close();
                        if ((int)row[6] == 1)
                        {
                            Frame.Navigate(typeof(AdminPage));
                            logPassError = false;
                        }
                        if ((int)row[6] == 2)
                        {
                            Frame.Navigate(typeof(StorekeeperPage));
                            logPassError = false;
                        }
                        if ((int)row[6] == 3)
                        {
                            Frame.Navigate(typeof(CashierPage));
                            logPassError = false;
                        }
                        if ((int)row[6] == 4)
                        {
                            Frame.Navigate(typeof(DocumentExpertPage));
                            logPassError = false;
                        }
                    }
                }
                if (logPassError)
                {
                    var message = new MessageDialog("Неверное имя пользователя или пароль").ShowAsync();
                }
            }
            catch (Exception exception)
            {
                var message = new MessageDialog(exception.Message).ShowAsync();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
