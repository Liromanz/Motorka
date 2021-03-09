using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Motorka
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AdminPage : Page
    {
        string connectionString = @"Data Source=DESKTOP-UA4MNST\BUBOCHKA;Initial Catalog=Motorka_Base;User ID=sa;Password=1056vQOAKZMtalvgty";
        string dataQuery = "SELECT dbo.[User].ID_User, dbo.[User].ID_Access_Level, dbo.[User].Surname AS Фамилия, dbo.[User].Name AS Имя, dbo.[User].Patronymic AS Отчество," +
                    " dbo.[User].Login AS Логин, dbo.[User].Password AS Пароль, dbo.Access_Level.Name AS Роль FROM dbo.[User] " +
                    "INNER JOIN dbo.Access_Level ON dbo.[User].ID_Access_Level = dbo.Access_Level.ID_Access_Level";
        public AdminPage()
        {
            this.InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataBase.LoadDataBase(connectionString, dataQuery, dataGrid, 2);
            DataBase.LoadComboBox(connectionString, "select * from [Access_Level]", roleComboBox);
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            if (Surname.Text != "" && NameBox.Text != "" && Patronymic.Text != "")
                    DataBase.CDUInterface(connectionString,
                        $"insert into [User](Surname, Name, Patronymic, Login, Password, ID_Access_Level) " +
                        $"values ('{Char.ToUpper(Surname.Text[0]) + Surname.Text.ToLower().Substring(1)}', " +
                        $"'{Char.ToUpper(NameBox.Text[0]) + NameBox.Text.ToLower().Substring(1)}', " +
                        $"'{Char.ToUpper(Patronymic.Text[0]) + Patronymic.Text.ToLower().Substring(1)}', " +
                        $"'{Login.Text}', '{Password.Text}', {roleComboBox.SelectedValue})",
                        allCheck(), dataQuery, dataGrid, 2);
            else
            {
                var message = new MessageDialog("Заполните все поля").ShowAsync();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
                DataBase.CDUInterface(connectionString,
                    $"update [User] set Surname = '{Char.ToUpper(Surname.Text[0]) + Surname.Text.ToLower().Substring(1)}', " +
                    $"Name = '{Char.ToUpper(NameBox.Text[0]) + NameBox.Text.ToLower().Substring(1)}', " +
                    $"Patronymic = '{Char.ToUpper(Patronymic.Text[0]) + Patronymic.Text.ToLower().Substring(1)}', " +
                    $"Login = '{Login.Text}', Password = '{Password.Text}', ID_Access_Level = {roleComboBox.SelectedValue} " +
                    $"where ID_User = {(int)(dataGrid.SelectedItem as object[])[0]}",
                    allCheck(), dataQuery, dataGrid, 2);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
                DataBase.CDUInterface(connectionString,
                    $"delete from [User] where ID_User = {(int)(dataGrid.SelectedItem as object[])[0]}",
                    allCheck(), dataQuery, dataGrid, 2);
        }

        private string allCheck()
        {
            if (Surname.Text == "" || NameBox.Text == "" || Patronymic.Text == "" || Login.Text == "" || Password.Text == "")
                return "Заполните все поля";
            if (Login.Text.Length < 4)
                return "Минимальная длина логина - 4 символа";
            if (Password.Text.Length < 8 || !Regex.IsMatch(Password.Text, @"[0-9]") || !Regex.IsMatch(Password.Text, @"[A-Z a-z А-Я а-я]"))
                return "Неверно введен пароль. Пароль должен имень минимум 8 символов, 1 букву и 1 цифру ";
            return "ОК";
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                Surname.Text = (dataGrid.SelectedItem as object[])[2].ToString();
                NameBox.Text = (dataGrid.SelectedItem as object[])[3].ToString();
                Patronymic.Text = (dataGrid.SelectedItem as object[])[4].ToString();
                Login.Text = (dataGrid.SelectedItem as object[])[5].ToString();
                Password.Text = (dataGrid.SelectedItem as object[])[6].ToString();
                roleComboBox.SelectedValue = (dataGrid.SelectedItem as object[])[1].ToString();
            }
        }
    }
}
