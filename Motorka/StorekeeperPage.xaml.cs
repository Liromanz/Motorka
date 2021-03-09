using System;
using System.Collections.Generic;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Motorka
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class StorekeeperPage : Page
    {
        string connectionString = @"Data Source=DESKTOP-UA4MNST\BUBOCHKA;Initial Catalog=Motorka_Base;User ID=sa;Password=1056vQOAKZMtalvgty";
        string dataQuery = "SELECT  dbo.Warehouse.ID_Warehouse, dbo.Warehouse.ID_Document, " +
                "dbo.Warehouse.Name AS Товар, dbo.Warehouse.Cost AS Цена, dbo.Warehouse.Quantity AS Количество, " +
                "dbo.Supplier.Name AS Поставщик FROM  dbo.Warehouse " +
                "INNER JOIN dbo.Supplier ON dbo.Warehouse.ID_Document = dbo.Supplier.ID_Document";
        public StorekeeperPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataBase.LoadDataBase(connectionString, dataQuery, dataGrid, 2);
            DataBase.LoadComboBox(connectionString, "select ID_Document, Name from Supplier", SupplierComboBox);
        }
        private string allCheck()
        {
            if (TovarName.Text == "" || Cost.Text == "" || Amount.Text == "" || SupplierComboBox.SelectedIndex == -1)
                return "Заполните все поля";
            if (Cost.Text[0] == '0' || Amount.Text[0] == '0')
                return "Цена товара и его количество не могут начинаться на ноль";
            return "ОК";
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            if (TovarName.Text != "" )
                DataBase.CDUInterface(connectionString, "insert into Warehouse(Name, Cost, Quantity, ID_Document, ID_User) values " +
                $"('{Char.ToUpper(TovarName.Text[0]) + TovarName.Text.ToLower().Substring(1)}', " +
                $"{Cost.Text}, {Amount.Text}, {SupplierComboBox.SelectedValue}, {MainPage.userId})",
                allCheck(), dataQuery, dataGrid, 2);
            else
            {
                var message = new MessageDialog("Заполните все поля").ShowAsync();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
                DataBase.CDUInterface(connectionString, $"update Warehouse set " +
                    $"Name = '{Char.ToUpper(TovarName.Text[0]) + TovarName.Text.ToLower().Substring(1)}', " +
                    $"Cost = {Cost.Text}, Quantity = {Amount.Text}, ID_Document = {SupplierComboBox.SelectedValue} " +
                    $"where ID_Warehouse = {(int)(dataGrid.SelectedItem as object[])[0]}",
                   allCheck(), dataQuery, dataGrid, 2);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
                DataBase.CDUInterface(connectionString, "delete from Warehouse " +
                    $"where ID_Warehouse = {(int)(dataGrid.SelectedItem as object[])[0]}",
                   allCheck(), dataQuery, dataGrid, 2);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                TovarName.Text = (dataGrid.SelectedItem as object[])[2].ToString();
                Cost.Text = (dataGrid.SelectedItem as object[])[3].ToString();
                Amount.Text = (dataGrid.SelectedItem as object[])[4].ToString();
                SupplierComboBox.SelectedValue = (dataGrid.SelectedItem as object[])[1].ToString();
            }

        }
    }
}
