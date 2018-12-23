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
    /// Interaction logic for frm_ChiTietChamBai.xaml
    /// </summary>
    public partial class frm_ChiTietChamBai : Window
    {

        private KetQuaChamBai kqChamBai;
        List<Object> ChiTietChamBai;




        public frm_ChiTietChamBai()
        {
            InitializeComponent();
        }

        public frm_ChiTietChamBai(List<Object> kq, string thongtin) : this()
        {
            ChiTietChamBai = kq;


            txt_ThongTin.Text = thongtin;



            #region Data Grid
            DG_ChiTietChamBai.Columns.Add(new DataGridTextColumn
            {
                Header = "STT",
                Binding = new System.Windows.Data.Binding("STT")
            });

            DG_ChiTietChamBai.Columns.Add(new DataGridTextColumn
            {
                Header = "Test",
                Binding = new System.Windows.Data.Binding("NameTest")
                
            });


            DG_ChiTietChamBai.Columns.Add(new DataGridTextColumn
            {
                Header = "Time",
                Binding = new System.Windows.Data.Binding("TimeUsed"), 
            });

            DG_ChiTietChamBai.Columns.Add(new DataGridTextColumn
            {
                Header = "MEM",
                Binding = new System.Windows.Data.Binding("MemoryUsed")
            });

            DG_ChiTietChamBai.Columns.Add(new DataGridTextColumn
            {
                Header = "Messages",
                Binding = new System.Windows.Data.Binding("Messages")
            });

            DG_ChiTietChamBai.Columns.Add(new DataGridTextColumn
            {
                Header = "Kết quả",
                Binding = new System.Windows.Data.Binding("Status")
            });


             DG_ChiTietChamBai.ItemsSource = ChiTietChamBai;
            DG_ChiTietChamBai.Items.Refresh();
            #endregion


        }
     
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DG_ChiTietChamBai.Height = Math.Max(this.Height - 100, 0);
            DG_ChiTietChamBai.Width = Math.Max(this.Width - 20, 0);


            DG_ChiTietChamBai.Columns[0].Width = 50;

            for (int i=1; i<6; i++)
            DG_ChiTietChamBai.Columns[i].Width = (Double)DG_ChiTietChamBai.Width/6;


        }
    }
}
