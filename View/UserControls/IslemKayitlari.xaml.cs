using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
namespace MyWpfApp.View.UserControls
{
    public partial class IslemKayitlari : UserControl
    {

        public IslemKayitlari()
        {

            InitializeComponent();
            Task.Run(() =>
            {
                Dispatcher.Invoke(() => {
                    if (Loading_Gif != null)
                    {
                        Loading_Gif.Visibility = Visibility.Visible;
                        System.Diagnostics.Trace.WriteLine(Loading_Gif.GetType());

                    }
                    else {
                        System.Diagnostics.Trace.WriteLine("değer null döner");

                    }
                });
            });
            Task.Run(() => {
                string connstr = @"server=localhost;port=3306;database=bitki_tara;user=root;password=root;";

                Dispatcher.Invoke(() => {
                    Loading_Gif.Visibility = Visibility.Visible;
                });

                try
                {
                    using (var conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        string sql = "select * from islemler";
                        var dataTable = new DataTable();
                        var adapter = new MySqlDataAdapter(sql, conn);

                        adapter.Fill(dataTable);
                   
                   

                        Dispatcher.Invoke(() => {
                            dataGrid.ItemsSource = dataTable.DefaultView;

                            Loading_Gif.Visibility = Visibility.Hidden;

                        });
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => {
                    });
                }
            });
        }

        private async void SilButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                int islemId = Convert.ToInt32(row["islem_id"]);

                string connstr = @"server=localhost;port=3306;database=bitki_tara;user=root;password=root;";

                try
                {
                    using (var conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        string deleteSql = $"DELETE FROM islemler WHERE islem_id = {islemId}";
                        var deleteCommand = new MySqlCommand(deleteSql, conn);
                        await deleteCommand.ExecuteNonQueryAsync();
                    }

                    // Veritabanından silme işlemi başarılı oldu, şimdi datagrid'i yeniden doldur.

                    DataTable dataTable = new DataTable();
                    string selectSql = "select * from islemler";
                    var adapter = new MySqlDataAdapter(selectSql, connstr);
                    adapter.Fill(dataTable);

                    dataGrid.ItemsSource = dataTable.DefaultView;
                    MessageBox.Show("Silme işlemi başarılı ");

                }
                catch (Exception ex)
                {
                    // Hata durumunda buraya düşer
                    MessageBox.Show("Silme işlemi başarısız oldu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }

    }
}
