using System;
using System.Data;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Motorka
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CashierPage : Page
    {
        string connectionString = @"Data Source=DESKTOP-UA4MNST\BUBOCHKA;Initial Catalog=Motorka_Base;User ID=sa;Password=1056vQOAKZMtalvgty";
        StorageFile file;
        int totalCost = 0;

        DataTable checkTable = new DataTable("Check");
        public CashierPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region Загрузка первого пивота
            DataBase.LoadComboBox(connectionString, "select ID_Warehouse, Name from Warehouse", tovarComboBox);

            checkTable.Columns.Add("Товар", Type.GetType("System.String"));
            checkTable.Columns.Add("Количество товара", Type.GetType("System.Int32"));
            checkTable.Columns.Add("Цена в сумме", Type.GetType("System.Int32"));

            Filling.FillDataGrid(checkTable, dataGrid);
            #endregion

            #region Загрузка второго пивота
            DataBase.LoadComboBox(connectionString, "select ID_Warehouse, Name from Warehouse", returnComboBox);
            #endregion
        }

        #region Первый пивот с кассой
        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            if (tovarComboBox.SelectedIndex != -1)
            {
                string check = allCheck();

                if (check == "ОК")
                {
                    int amount = (int)DataBase.GetTable(connectionString, $"select Quantity from Warehouse where ID_Warehouse = {tovarComboBox.SelectedValue}").Rows[0][0];
                    int cost = (int)DataBase.GetTable(connectionString, $"select Cost from Warehouse where ID_Warehouse = {tovarComboBox.SelectedValue}").Rows[0][0];
                    checkTable.Rows.Add(new Object[] { tovarComboBox.SelectedItem.ToString().Substring(1, tovarComboBox.SelectedItem.ToString().IndexOf(',')), Convert.ToInt32(amountTextBox.Text), Convert.ToInt32(amountTextBox.Text) * cost });
                    totalCost += Convert.ToInt32(amountTextBox.Text) * cost;
                    totalTextBox.Text = totalCost.ToString();
                    DataBase.CDUBase(connectionString, $"update Warehouse set Quantity = {amount - Convert.ToInt32(amountTextBox.Text)} where ID_Warehouse = {tovarComboBox.SelectedValue}");
                    DataBase.CDUBase(connectionString, $"insert into Statistic([Product_Name], [Amount], [Date_Add]) values ('{tovarComboBox.SelectedItem.ToString().Substring(1, tovarComboBox.SelectedItem.ToString().IndexOf(','))}', {amountTextBox.Text}, GETDATE())");
                    Filling.FillDataGrid(checkTable, dataGrid);
                }
                else
                {
                    var message = new MessageDialog(check).ShowAsync();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                int amount = (int)DataBase.GetTable(connectionString, $"select Quantity from Warehouse where ID_Warehouse = {tovarComboBox.SelectedValue}").Rows[0][0];
                DataBase.CDUBase(connectionString, $"insert into Statistic([Product_Name], [Amount], [Date_Add]) values ('{tovarComboBox.SelectedItem.ToString().Substring(1, tovarComboBox.SelectedItem.ToString().IndexOf(','))}', -{(dataGrid.SelectedItem as object[])[1]}, GETDATE())");
                DataBase.CDUBase(connectionString, $"update Warehouse set Quantity = {amount + (int)(dataGrid.SelectedItem as object[])[1]} where ID_Warehouse = {tovarComboBox.SelectedValue}");
                totalCost -= (int)(dataGrid.SelectedItem as object[])[2];
                totalTextBox.Text = totalCost.ToString();
                checkTable.Rows.RemoveAt(dataGrid.SelectedIndex);
                Filling.FillDataGrid(checkTable, dataGrid);
            }
        }

        private string allCheck()
        {
            int amount = (int)DataBase.GetTable(connectionString, $"select Quantity from Warehouse where ID_Warehouse = {tovarComboBox.SelectedValue}").Rows[0][0];
            if (amountTextBox.Text == "")
                return "Заполните все поля";
            if (tovarComboBox.SelectedValue == null)
                return "Такого товара нет";
            if (Convert.ToInt32(amountTextBox.Text) > amount)
                return "Столько товара на складе нет";
            if (Convert.ToInt32(amountTextBox.Text) > 999999999 || Convert.ToInt32(amountTextBox.Text) <= 0)
                return "Значение поля количество должно быть в пределе от 0 и до 999999999";
            if (amountTextBox.Text[0] == '0')
                return "Цена товара и его количество не могут начинаться на ноль";
            return "ОК";
        }
        #endregion

        #region Второй пивот с возвратом
        private async void InsertPicture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker dialog = new FileOpenPicker();
                dialog.FileTypeFilter.Add(".jpg");
                dialog.FileTypeFilter.Add(".png");
                file = await dialog.PickSingleFileAsync();
                if (file != null)
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(await file.GetThumbnailAsync(ThumbnailMode.PicturesView, 300));
                    imageCheck.Source = bitmapImage;
                }
            }
            catch (Exception exception)
            {
                var message = new MessageDialog(exception.Message).ShowAsync();
            }
        }

        private async void InsertReturn_Click(object sender, RoutedEventArgs e)
        {
            if (returnComboBox.SelectedIndex != -1 && imageCheck.Source != null)
            {
                DataBase.CDUBase(connectionString, $"insert into [Return] (ID_Warehouse, Amount, [Case], [Check]) values " +
                    $"({returnComboBox.SelectedValue}, {amountReturn.Text}, '{reasonReturn.Text}', @img)", "@img", await ImageConventer.ImageToByte(file));
                var message = new MessageDialog("Возврат успешно запрошен!").ShowAsync();
            }
        }
        #endregion
    }
}
