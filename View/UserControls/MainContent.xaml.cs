using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;




namespace MyWpfApp.View.UserControls
{
    /// <summary>
    /// MainContent.xaml etkileşim mantığı
    /// </summary>
    public partial class MainContent : UserControl
    {
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        string etiket;
        string dosyaYolu;
        
        public MainContent()
        {
            InitializeComponent();
        



        }
        //  RESİM EKLE BUTONU
        private void AddPic_Click(object sender, RoutedEventArgs e)

        {


            dlg.Filter = "Resim Dosyaları|*.jpeg;*.jpg;*.png|Tüm Dosyalar|*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string dosyaYolu = dlg.FileName;
                string uzanti = System.IO.Path.GetExtension(dosyaYolu);
                this.dosyaYolu = dosyaYolu;

                
                

               

                if (uzanti == ".jpg" || uzanti == ".jpeg" || uzanti == ".png")
                {
                    // Image oluştur
                    Image yeniResim = new Image();
                    yeniResim.Source = new BitmapImage(new Uri(dosyaYolu));
                    DropShadowEffect shadowEffect = new DropShadowEffect();
                    shadowEffect.ShadowDepth = 5; // Gölge derinliği
                    shadowEffect.Color = Colors.Black; // Gölge rengi
                    yeniResim.Effect = shadowEffect;

                    // Image'ı FirstPic Border'ına ekle
                    FirstPic.Child = yeniResim;
                    
                    ResimEkle.Text = "RESİM EKLENDİ";
                    ResimEkle.Foreground = Brushes.Green; // Arka plan rengini mavi yapar


                    KontrolEtButton.Visibility = Visibility.Visible;

                }
                else
                {
                    MessageBox.Show("Lütfen sadece jpeg veya png formatında bir resim seçin.");
                }
            }
            Storyboard moveAnimation = (Storyboard)this.Resources["MoveAnimation"];
            moveAnimation.Begin();

        }

        private async void KontrolEtButton_Click(object sender, RoutedEventArgs e)
        {
            Loading_Gif2.Visibility = Visibility.Visible;
            KontrolEtButton.IsEnabled = false; // Butonu etkisiz hale getirir.
            await Task.Run(() =>
            {
                // Uzun süren işlemler burada yapılır.
                var imageBytes = File.ReadAllBytes(dosyaYolu);
                Apples.ModelInput sampleData = new Apples.ModelInput()
                {
                    ImageSource = imageBytes,
                };

                var predictionresult = Apples.Predict(sampleData);
                var label = predictionresult.PredictedLabel;
                etiket = label;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var probabilities = predictionresult.Score;
                    //StringBuilder all = new StringBuilder();
                    float HastalikAc = 0;
                    float SaglikAc = 0;
                    string naber = "selam";
                    // Her bir sınıfın olasılığını yazdır
                    for (int i = 0; i < probabilities.Length; i++)
                    {
                        Console.WriteLine($"Label {i}: {probabilities[i]}");
                        //all.Append(probabilities[i].ToString());
                        if (i == 0)
                        {
                            SaglikAc = probabilities[i];
                        }
                        else if (i == 1)
                        {
                            HastalikAc = probabilities[i];
                        }
                    }
                    //all.Insert(0, label);

                    // Değerleri stringe dönüştür

                    //all.Insert(0, label);


                    //string resultString = all.ToString();

                    Hastalik_Ac.Content = $"HASTALIKLI OLMA ORANI {HastalikAc.ToString("0.00")}"; 
                    Saglik_Ac.Content = $"SAĞLIKLI OLMA ORANI {SaglikAc.ToString("0.00")}";

                    OutputLabel.Content = label;
                    Loading_Gif2.Visibility = Visibility.Hidden;
                    OutputLabel.Visibility = Visibility.Visible;
                    KontrolEtButton.IsEnabled = true;
                    SaveBtn.Visibility = Visibility.Visible;
                });
          
            
            });

            Storyboard OutputAnimation = (Storyboard)this.Resources["OutputAnimation"];
            OutputAnimation.Begin();

        }
        //SAVE BUTONU ÖNCE SQL SORGUSUNU ALIYOR İŞLİYOR. SONRA ADDPİC ' DEKİ RESMİ TEMP KLASÖRÜNDE İSMİ KAYİTLAR OLAN KLASÖRE İSMİNİ İD ' NİN DEĞERİYLE DEĞİŞTİRİP EKLİYOR.
        //BU SAYEDE DAHA SONRA BU KAYİTLAR KLASÖRÜNDEN İSTENEN FOTOĞRAFLARI CEKEBİLİR İŞLEM YAPABİLİR VE YİNE DAHA SONRA KULLANICIYA GÖSTEREBİLİRİZ. 

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string zamanDamgasi = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            string kayitlarKlasoru = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "kayitlar");

            string hedefDosyaYolu = System.IO.Path.Combine(kayitlarKlasoru, $"{zamanDamgasi}.jpg");

            Task.Run(() =>
            {
                string connstr = @"server=localhost;port=3306;database=bitki_tara;user=root;password=root;";

                using (var conn = new MySqlConnection(connstr))
                {
                    try
                    {
                        conn.Open();
                        string sql = $"INSERT INTO `bitki_tara`.`islemler` (`islem_resim`, `islem_tarih`, `islem_sonuc`) VALUES ('{hedefDosyaYolu}', NOW(), '{etiket}');";
                      

                        
                      

                        var comm = new MySqlCommand(sql, conn);

                        comm.ExecuteNonQuery();

                        // Burada hedefDosyaYolu yerine yapılandırma dosyasından alınan değeri kullanıyoruz.
                       
                        if (!Directory.Exists(kayitlarKlasoru))
                        {
                            Directory.CreateDirectory(kayitlarKlasoru);
                        }

                        File.Copy(dosyaYolu, hedefDosyaYolu, true);

                     
                        MessageBox.Show("VERİTABANINA KAYDETME BAŞARILI");
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            System.Diagnostics.Trace.WriteLine(ex.Message);
                        });
                    }
                }
            });
        }




    }
}

