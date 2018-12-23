using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Judge3T
{
    /// <summary>
    /// Interaction logic for frm_CauHinhBoTest.xaml
    /// </summary>
    public partial class frm_CauHinhBoTest : Window
    {

        MainWindow MainWin;


        public frm_CauHinhBoTest()
        {
            InitializeComponent();
            Grid_ThanhTrangThaiTren.Children.Add(new UserControl_ThanhTrangThaiTren("Cấu hình bộ test", false));
        }


        public frm_CauHinhBoTest(MainWindow x)
        {
            InitializeComponent();
            MainWin = x;

            cbb_ChonBaiTap.ItemsSource = MainWin.listBoTest;
            cbb_ChonBaiTap.DisplayMemberPath = "tenBoTest";
            Grid_ThanhTrangThaiTren.Children.Add(new UserControl_ThanhTrangThaiTren("Cấu hình bộ test", false));
 
        }

        public void ReLoadCombox_ChonBai()
        {
            cbb_ChonBaiTap.Items.Refresh();
        }

        private void cbb_ChonBaiTap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rb_DangBaiOI.IsChecked = false;
            rb_DangBaiACM.IsChecked = false;
            rb_DangBaiChallenge.IsChecked = false;
            switch (MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].kieuCham)
            {
                case KieuChamBai.OI:
                    rb_DangBaiOI.IsChecked = true;
                    break;
                case KieuChamBai.ACM:
                    rb_DangBaiACM.IsChecked = true;
                    break;
                case KieuChamBai.Challenge:
                    rb_DangBaiChallenge.IsChecked = true;
                    break;
            }
            txt_timeLimit.Text = MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].timeLimit.ToString();
            txt_tuKhoaCam.Text = MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].tuKhoaCam;
            txt_MemoryLimit.Text = MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].memoryLimit.ToString();
        }

        private void btn_SaveDangBai_Click(object sender, RoutedEventArgs e)
        {
            if (cbb_ChonBaiTap.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn chưa chọn bài cần cấu hình!");
                return;
            }

            int tl;
            if (rb_DangBaiOI.IsChecked == true)
                tl = 1;
            else
            {
                if (rb_DangBaiACM.IsChecked == true)
                    tl = 2;
                else
                    tl = 3;
            }

            int tmpTime;
            if (!int.TryParse(txt_timeLimit.Text, out tmpTime))
            {
                MessageBox.Show("Giới hạn thời gian sai định dạng! Vui lòng thử lại!");
                return;
            }


            int tmpRam;
            if (!int.TryParse(txt_MemoryLimit.Text, out tmpRam))
            {
                MessageBox.Show("Giới hạn RAM sai định dạng! \nVui lòng thử lại trong khoảng [1-1024MB]");
                return;
            }

            MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].kieuCham = (KieuChamBai)tl;
            MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].timeLimit = tmpTime;
            MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].tuKhoaCam = txt_tuKhoaCam.Text;
            MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].memoryLimit = tmpRam;

            

            //ghi ra file xml
            try
            {
                string duongdanconfig = MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].dirBoTest;
                XmlTextWriter objXmlTextWriter = new XmlTextWriter(duongdanconfig + @"\config.xml", null);

                //MessageBox.Show(duongdanconfig + @"\config.xml");

                objXmlTextWriter.Formatting = Formatting.Indented;
                objXmlTextWriter.WriteStartDocument();
                objXmlTextWriter.WriteStartElement("CauHinhBoTest");
                objXmlTextWriter.WriteStartElement("kieuCham");

                switch (MainWin.listBoTest[cbb_ChonBaiTap.SelectedIndex].kieuCham)
                {
                    case KieuChamBai.OI: objXmlTextWriter.WriteString("1"); break;
                    case KieuChamBai.ACM: objXmlTextWriter.WriteString("2"); break;
                    case KieuChamBai.Challenge: objXmlTextWriter.WriteString("3"); break; 
                }

                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("timeLimit");
                objXmlTextWriter.WriteString(txt_timeLimit.Text);
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteStartElement("tuKhoaCam");
                objXmlTextWriter.WriteString(txt_tuKhoaCam.Text);
                objXmlTextWriter.WriteEndElement();


                objXmlTextWriter.WriteStartElement("memoryLimit");
                objXmlTextWriter.WriteString(txt_MemoryLimit.Text);
                objXmlTextWriter.WriteEndElement();

                objXmlTextWriter.WriteEndElement();
                objXmlTextWriter.WriteEndDocument();
                objXmlTextWriter.Flush();
                objXmlTextWriter.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //ghi ra file xml
            MessageBox.Show("Lưu thành công!");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
