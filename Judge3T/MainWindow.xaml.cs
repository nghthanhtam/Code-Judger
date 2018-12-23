using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Judge3T
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        internal List<ClassBoTest> listBoTest = new List<ClassBoTest>();
        internal List<ClassThiSinh> listThiSinh = new List<ClassThiSinh>();
        public bool DaLoadDuLieu { get; set; } = false; 

        internal List<string[]> BangDiem = new List<string[]>();

        public int nBangDiem = 0, mBangDiem = 0;

 
        public MainWindow()
        {
            InitializeComponent();

            Grid_ThanhTrangThaiTren.Children.Add(new UserControl_ThanhTrangThaiTren());
             

            usc_TrangChinh = new UserControl_TrangChinh(this);
            usc = usc_TrangChinh;
            GridMain.Children.Add(usc);

 

            #region Cập nhật lượt sử dụng và người dùng mới




           ThreadPool.QueueUserWorkItem(
          (obj) =>
          {


              try
              {
                  int id = int.Parse(Properties.Resources.idphienban);
                  string tenphienban = "";
                  string url = "";
                  string noidung = "";
                  string date = "";

                  using (WebClient wc = new WebClient())
                  {
                      wc.Encoding = Encoding.UTF8;
                      var json = wc.DownloadString(Properties.Resources.DomainJudge3T + "/update/get-update.php");
 
                      JsonTextReader reader = new JsonTextReader(new StringReader(json));
                       while (reader.Read())
                      {
                          if (reader.Value != null)
                          {
                              string ThuocTinh = reader.Value.ToString();
                              switch (ThuocTinh)
                              {
                                  case "id":

                                      while (reader.Read())
                                      {
                                          if (reader.Value != null)
                                          {
                                              string strValue = reader.Value.ToString();

                                              id = int.Parse(strValue);

                                              break;
                                          }
                                      }

                                      break;
                                       
                                          case "tenphienban":

                                              while (reader.Read())
                                              {
                                                  if (reader.Value != null)
                                                  {
                                                      string strValue = reader.Value.ToString();

                                                      tenphienban = strValue;

                                                      break;
                                                  }
                                              }

                                              break;

                                          case "url":

                                              while (reader.Read())
                                              {
                                                  if (reader.Value != null)
                                                  {
                                                      string strValue = reader.Value.ToString();

                                                      url = strValue;
                                                      break;
                                                  }
                                              }

                                              break;

                                          case "noidung":


                                              while (reader.Read())
                                              {
                                                  if (reader.Value != null)
                                                  {
                                                      string strValue = reader.Value.ToString();

                                                      noidung = strValue;
                                                      break;
                                                  }
                                              }



                                              break;

                                          case "date":


                                              while (reader.Read())
                                              {
                                                  if (reader.Value != null)
                                                  {
                                                      string strValue = reader.Value.ToString();

                                                      date = strValue;
                                                      break;
                                                  }
                                              }



                                              break;
 
                              }
                          }
                      }
                  }

                  

                  //Debug.WriteLine(id + " " + tenphienban + " " + url + " " + noidung + " " + date);

                  if (int.Parse(Properties.Resources.idphienban )!= id)
                  {
                      this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                      {
                          Window frmCapNhat = new frm_CapNhatPhanMem(id, tenphienban, url, noidung, date);
                          frmCapNhat.ShowDialog();
                      }));
                  }


                  

              }
              catch (Exception ex)
              {
                  Debug.WriteLine("Lỗi khi lấy thông tin update: " + ex.Message);
              }


              try
              {
                  string SerialNumberHDD = "";
                  ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                  foreach (ManagementObject wmi_HD in searcher.Get())
                  {
                      SerialNumberHDD = wmi_HD["SerialNumber"].ToString();
                      break;
                  }

                  string urlAddress = Properties.Resources.DomainJudge3T + "/analytics/send.php?LuotSuDung=1&SerialNumberHDD=" + SerialNumberHDD;
                  HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                  request.Credentials = CredentialCache.DefaultCredentials;
                  WebResponse response = request.GetResponse();
              }
              catch (Exception ex)
              {
                  Debug.WriteLine("Lỗi gửi dữ liệu thống kê về server : " + ex.Message);
              }


          });
            #endregion


             


        }


        private void btn_OpenMenu_Click(object sender, RoutedEventArgs e)
        {
            btn_OpenMenu.Visibility = Visibility.Collapsed;
            btn_CloseMenu.Visibility = Visibility.Visible;
         }

        private void btn_CloseMenu_Click(object sender, RoutedEventArgs e)
        {
            btn_OpenMenu.Visibility = Visibility.Visible;
            btn_CloseMenu.Visibility = Visibility.Collapsed;
         }

        private void Part_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }
         
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             
            Reload_Size_GUI();


        }

        /// <summary>
        /// Điều chỉnh lại kích thước các thành phần
        /// + Thanh bên trên
        /// + GridMain
        /// </summary>
        private void Reload_Size_GUI()
        { 
            Grid_ThanhTrangThaiTren.Width = this.Width;
            Reload_Size_GridMain();
        }

        /// <summary>
        /// re-Size lại kích thước GridMain
        /// </summary>
        private void Reload_Size_GridMain()
        {
            GridMain.Width = Math.Max(this.Width - GridMenu.Width - 10, 0);
            GridMain.Height = Math.Max(this.Height - Grid_ThanhTrangThaiTren.Height - 5, 0);
            GridMain.Margin = new Thickness(GridMenu.Width, 50, 0, 0);
        }





 #region Load UserControl
        UserControl usc = null;
         
        UserControl usc_BangDiem = null;
        UserControl usc_Tools = null;
        UserControl usc_HuongDan = null;
        UserControl usc_TrangChinh = null;
 


        private void GridMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Reload_Size_GridMain();
        }

      

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "lvi_MenuBangDiem":
                    if (usc_BangDiem == null)
                    {
                        usc_BangDiem = new UserControl_BangDiem(this);
                    }
                    usc = usc_BangDiem;
                    GridMain.Children.Add(usc);
                    break;

                case "lvi_MenuCongCu":
                    //      if (usc_CauHinhPhanMem == null || listBoTest.Count==0)
                    //   {
                    usc_Tools = new UserControl_Tools();
                  //  }
                    usc = usc_Tools; 
                    GridMain.Children.Add(usc);
                    break;
                case "lvi_MenuHuongDan":
                    if (usc_HuongDan == null)
                    {
                        usc_HuongDan = new UserControl_HuongDan(this);
                    }
                    usc = usc_HuongDan;
                    GridMain.Children.Add(usc);
                    break;

                case "lvi_MenuTrangChinh":
                       // if (usc_TrangChinh == null)
                        {
                            usc_TrangChinh = new UserControl_TrangChinh(this);
                        }
                        usc = usc_TrangChinh;
                        GridMain.Children.Add(usc);
                        break;




                case "lvi_MenuTacGia": 
                        usc = new UserControl_ThongTinTacGia(); 
                        GridMain.Children.Add(usc);
                        break;
            }




        }


#endregion

         
          
    }
}
