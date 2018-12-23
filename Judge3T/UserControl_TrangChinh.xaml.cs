using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    /// Interaction logic for UserControl_TrangChinh.xaml
    /// </summary>
    public partial class UserControl_TrangChinh : UserControl
    {
        Window MainWin;
        public UserControl_TrangChinh()
        {
            InitializeComponent();
        }

        public UserControl_TrangChinh(MainWindow x)
        {
            InitializeComponent();
            MainWin = x; 

            

            ThreadPool.QueueUserWorkItem( 
            (obj) =>
            {
                 
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        var json = wc.DownloadString(Properties.Resources.DomainJudge3T + "/analytics/get-analytics.php");
                         
                        JsonTextReader reader = new JsonTextReader(new StringReader(json));
                        while (reader.Read())
                        {
                            if (reader.Value != null)
                            {
                                string ThuocTinh = reader.Value.ToString();
                                switch (ThuocTinh)
                                {
                                    case "luotchambai":

                                       while( reader.Read())
                                        {
                                            if (reader.Value != null)
                                            {
                                                string strValue = reader.Value.ToString();


                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    txt_SoLuotChamBai.Text = strValue.ToString();
                                                }));
 
                                                break;
                                            }
                                        }
                                        
                                        break;
                                    case "baidacham":

                                        while (reader.Read())
                                        {
                                            if (reader.Value != null)
                                            {
                                                string strValue = reader.Value.ToString();
                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    txt_SoBaiCham.Text = strValue;
                                                }));
                                                break;
                                            }
                                        }
                                         
                                        break;

                                    case "luotsudung":


                                        while (reader.Read())
                                        {
                                            if (reader.Value != null)
                                            {
                                                string strValue = reader.Value.ToString();

                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    txt_LuotSuDung.Text = strValue;
                                                }));
                                                break;
                                            }
                                        }



                                        break;

                                    case "songuoidung":


                                        while (reader.Read())
                                        {
                                            if (reader.Value != null)
                                            {
                                                string strValue = reader.Value.ToString();

                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    txt_SoNguoiDung.Text = strValue;
                                                }));
                                                break;
                                            }
                                        }



                                        break;

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                 {
                    Debug.WriteLine("Lỗi khi lấy thống kê từ server: " + ex.Message);
                    MessageBox.Show("Lỗi khi lấy thống kê từ server: " + ex.Message);
                } 
            });
        }

    }
}
