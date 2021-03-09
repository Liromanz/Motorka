using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Motorka
{
    class DataBase
    {
        #region Загрузка из базы данных в интерфейс
        ///<summary>
        ///<para>Выгружает все твои значения из таблицы в датагрид. Можешь указать сколько столбцов в самом начале ты хочешь скрыть</para>
        ///</summary>
        public static void LoadDataBase(string connectionString, string query, DataGrid dataGrid, int tablesToHide = 0)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                Filling.FillDataGrid(ds.Tables[0], dataGrid) ;

                for (int i = 0; i < tablesToHide; i++)
                {
                    dataGrid.Columns[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
        ///<summary>
        ///<para>Выгружает все твои значения из таблицы в комбобокс. Сделай табличку из двух столбцов: первое - ID, второе - название. Запрос кинь в query</para>
        ///</summary>
        public static void LoadComboBox(string connectionString, string query, ComboBox comboBox)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            try
            {
                connection.Open();
                DataSet ds = new DataSet();

                dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.Fill(ds);

                Dictionary<String, string> tableDictionary = new Dictionary<string, string>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    tableDictionary.Add(row[1].ToString(), row[0].ToString());
                }

                comboBox.ItemsSource = tableDictionary;
                comboBox.SelectedValuePath = "Value";
                comboBox.DisplayMemberPath = "Key";

                if(comboBox.Items.Count != 0)
                    comboBox.SelectedIndex = 0;
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
        #endregion

        #region Загрузка из интерфейса в базу данных
        ///<summary>
        ///<para>Сборная солянка из выполнения одного запроса и обновления интерфейса датагрида</para>
        ///</summary>
        public static void CDUInterface(string connectionString, string CDUquery, string check, string loadQuery, DataGrid dataGrid, int tablesToHide = 0)
        {
            if (check == "ОК")
            {
                CDUBase(connectionString, CDUquery);
                LoadDataBase(connectionString, loadQuery, dataGrid, tablesToHide);
            }
            else
            {
                var message = new MessageDialog(check).ShowAsync();
            }
        }

        ///<summary>
        ///<para>Выполняет один запрос в базе данных и не возвращает тебе жабок. Нужен только когда ты хочешь обновить только бд, без датагрида и прочего</para>
        ///</summary>
        public static void CDUBase(string connectionString, string query)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;
            try
            {
                connection.Open();
                command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
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
        ///<summary>
        ///<para>Выполняет один запрос в базе данных !с параметром! и не возвращает тебе жабок. Нужен для тупых картинок</para>
        ///</summary>
        public static void CDUBase(string connectionString, string query, string parameterName, byte[] imageByte)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;
            try
            {
                connection.Open();
                command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter(parameterName, imageByte));
                command.ExecuteNonQuery();
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
        #endregion

        #region Выгрузка значения из селекта
        ///<summary>
        ///<para>Забирает одну жабку из твоего селекта, чтобы ты смог крутить ее как хочешь и сделать ее переменной любого типа</para>
        ///</summary>
        public static DataTable GetTable(string connectionString, string query)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            try
            {
                connection.Open();
                dataAdapter = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                connection.Close();
                return ds.Tables[0];
            }
            catch (Exception exception)
            {
                var message = new MessageDialog(exception.Message).ShowAsync();
                connection.Close();
                return null;
            }
        }
        #endregion
    }
}
