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


namespace Judge3T
{
    public partial class frm_CodeThiSinh : Window
    {
        public frm_CodeThiSinh()
        {
            InitializeComponent();
        }

        public frm_CodeThiSinh(string code, string thongtin) : this()
        {
            txt_ThiSinh.Text = thongtin;
            txt_Code.Text = code;
        }
    }
}
