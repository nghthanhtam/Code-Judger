using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Judge3T
{
    /// <summary>
    /// Interaction logic for frm_CapNhatPhanMem.xaml
    /// </summary>
    public partial class frm_CapNhatPhanMem : Window
    {
        public frm_CapNhatPhanMem()
        {
            InitializeComponent();

        }

        string Url = "";

        public frm_CapNhatPhanMem(int id,string tenphienban, string url, string noidung, string date)
        {
            InitializeComponent();
            txt_NoiDung.Text = "Phiên bản mới nhất: "+ tenphienban + " ("+ date + ")\n"+noidung;
            Grid_ThanhTrangThaiTren.Children.Add(new UserControl_ThanhTrangThaiTren("Cập nhật mới", false));
            this.Url = url;
        }

        private void btn_CapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Url);
            }
            catch
            {

            }
            
            this.Close();

        }
    }
}
