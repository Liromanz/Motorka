using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Windows.Graphics.Printing;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Motorka
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class DocumentExpertPage : Page
    {
        string connectionString = @"Data Source=DESKTOP-UA4MNST\BUBOCHKA;Initial Catalog=Motorka_Base;User ID=sa;Password=1056vQOAKZMtalvgty";
        public DocumentExpertPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataBase.LoadDataBase(connectionString, "select * from [Return]", dataGrid, 1);
            DataBase.LoadComboBox(connectionString, "select * from Guarantee_Type", guarantieBox);
            DataBase.LoadComboBox(connectionString, "select * from Quality_Type", qualityBox);
            DataBase.LoadComboBox(connectionString, "select ID_Document, Name from [Supplier]", supplierSelect);
            dateTime.Text = DateTime.Today.Date.Date.Date.ToString();
            dateBox.Text = DateTime.Today.Date.Date.Date.ToString();
        }

        private async void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                amountText.Text = (dataGrid.SelectedItem as object[])[2].ToString();
                reasonText.Text = (dataGrid.SelectedItem as object[])[3].ToString();
                imageCheck.Source = await ImageConventer.ByteToImage((byte[])(dataGrid.SelectedItem as object[])[4]);
            }
        }

        private async void Print_Click(object sender, RoutedEventArgs e)
        {
            if (PrintManager.IsSupported())
            {
                try
                {
                    Print.SetPrint(moneyExchange);
                    await PrintManager.ShowPrintUIAsync();
                }
                catch (Exception exception)
                {
                    var message = new MessageDialog(exception.Message).ShowAsync();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
                DataBase.CDUInterface(connectionString, $"delete from [Return] where ID_Return = {(dataGrid.SelectedItem as object[])[0]}", "ОК", "select * from [Return]", dataGrid, 1);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text.Length != 0)
                checkNumber.Text = (sender as TextBox).Text;
        }

        private void InsertSupplier_Click(object sender, RoutedEventArgs e)
        {
            string check = allCheck();
            if (check == "ОК")
            {
                DataBase.CDUBase(connectionString, "insert into Supplier(Name, Country, Start_Date, End_Date, ID_Quality_Type, ID_Guarantee) values " +
                    $"('{supplierName.Text}','{country.Text}',Convert(datetime, '{startDatePicker.Date.Date.Date}', 104), CONVERT(datetime, '{endDatePicker.Date.Date.Date}', 104)," +
                    $" {qualityBox.SelectedValue}, {guarantieBox.SelectedValue})");
                var message = new MessageDialog("Документ успешно добавлен").ShowAsync();
            }
            else
            {
                var message = new MessageDialog(check).ShowAsync();
            }
        }

        private async void PrintSuppier_Click(object sender, RoutedEventArgs e)
        {
            if (PrintManager.IsSupported())
            {
                try
                {
                    Print.SetPrint(supplierDocument);
                    await PrintManager.ShowPrintUIAsync();
                }
                catch (Exception exception)
                {
                    var message = new MessageDialog(exception.Message).ShowAsync();
                }
            }
        }

        private string allCheck()
        {
            if (supplierName.Text == "" || country.Text == "" || startDatePicker.Date == null || endDatePicker.Date == null || qualityBox.SelectedIndex == -1 || guarantieBox.SelectedIndex == -1)
                return "Заполните все поля";
            if (startDatePicker.Date.Date == DateTime.Today)
                return "Документ не может быть с сегодняшней датой";
            if (endDatePicker.Date.Date <= startDatePicker.Date.Date)
                return "Конечная дата должна быть больше чем стартовая";
            return "ОК";
        }

        private void SupplierTextChanged(object sender, Run run)
        {
            if ((sender as TextBox).Text.Length != 0)
            {
                run.Text = (sender as TextBox).Text;
            }
        }

        private void SupplierDateChanged(object sender, Run run)
        {
            if ((sender as DatePicker).Date != null)
            {
                run.Text = (sender as DatePicker).Date.Date.Date.ToString();
            }
        }
        private void SupplierComboChanged(object sender, Run run)
        {
            if ((sender as ComboBox).SelectedIndex != -1)
            {
                run.Text = (sender as ComboBox).SelectedItem.ToString().Substring(1, (sender as ComboBox).SelectedItem.ToString().IndexOf(','));
            }
        }

        private void supplierName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SupplierTextChanged(sender, supplierBox);
        }

        private void country_TextChanged(object sender, TextChangedEventArgs e)
        {
            SupplierTextChanged(sender, reasonBox);
        }

        private void startDatePicker_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            SupplierDateChanged(sender, startDate);
        }

        private void endDatePicker_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            SupplierDateChanged(sender, endDate);
        }

        private void qualityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SupplierComboChanged(sender, qualityText);
        }

        private void guarantieBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SupplierComboChanged(sender, guarantieText);
        }

        private void DatePicker_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            string zero = ((sender as DatePicker).Date.Month.ToString().Length == 1) ? "0" : "";
            DataTable table = DataBase.GetTable(connectionString, $"select Name + ',', (select SUM(Amount) from Statistic where Product_Name = Name+ ',' and [Date_Add] LIKE '%{(sender as DatePicker).Date.Year}-{zero + (sender as DatePicker).Date.Month}%') from Warehouse ");
            List<ChartData> ChartInfo = new List<ChartData>();
            foreach (DataRow row in table.Rows)
            {
                if (row[1].ToString() == "")
                    row[1] = 0;
                ChartInfo.Add(new ChartData()
                {
                    DataName = row[0].ToString(),
                    DataValue = (int)row[1]
                });

            }
            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = ChartInfo;
        }

        private void country_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (args.NewText.Length > 0)
            {
                char lastSymb = Convert.ToChar(args.NewText.Substring(args.NewText.Length - 1));
                if (Char.IsDigit(lastSymb) || lastSymb == ' ')
                {
                    args.Cancel = true;
                }
            }
        }

        private void supplierSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (supplierSelect.SelectedIndex != -1)
            {
                DataTable table = DataBase.GetTable(connectionString, "SELECT dbo.Supplier.ID_Document, dbo.Supplier.Name, " +
                    "dbo.Supplier.Country, dbo.Supplier.Start_Date, dbo.Supplier.End_Date, dbo.Quality_Type.Name AS Quality," +
                    " dbo.Guarantee_Type.Name AS Guarantee FROM dbo.Guarantee_Type INNER JOIN dbo.Supplier " +
                    "ON dbo.Guarantee_Type.ID_Guarantee = dbo.Supplier.ID_Guarantee INNER JOIN dbo.Quality_Type " +
                   $"ON dbo.Supplier.ID_Quality_Type = dbo.Quality_Type.ID_Quality WHERE(dbo.Supplier.ID_Document = {supplierSelect.SelectedValue})");
                if (table != null)
                {
                    supplierBox.Text = table.Rows[0][1].ToString();
                    reasonBox.Text = table.Rows[0][2].ToString();
                    startDate.Text = table.Rows[0][3].ToString();
                    endDate.Text = table.Rows[0][4].ToString();
                    qualityText.Text = table.Rows[0][5].ToString();
                    guarantieText.Text = table.Rows[0][6].ToString();
                }
            }
        }
    }
}
