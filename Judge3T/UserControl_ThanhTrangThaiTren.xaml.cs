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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Judge3T
{
    /// <summary>
    /// Interaction logic for UserControl_ThanhTrangThaiTren.xaml
    /// </summary>
    public partial class UserControl_ThanhTrangThaiTren : UserControl
    {
        public UserControl_ThanhTrangThaiTren()
        {
            InitializeComponent();
        }

        public UserControl_ThanhTrangThaiTren(string title)
        {
            InitializeComponent();
            txt_Title.Text = title;
        }

        /// <summary>
        /// Tạo Thanh trạng thái trên
        /// </summary>
        /// <param name="title">Tiêu đề form</param>
        /// <param name="Bat_Normal_Max">false nếu không muốn hiện nút max - normal</param>
        public UserControl_ThanhTrangThaiTren(string title, bool Bat_Normal_Max)
        {
            InitializeComponent();
            txt_Title.Text = title;
            if (Bat_Normal_Max == false)
            {
                btn_Normal_MaxWin.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
            // Application.Current.Shutdown();
        }

        private void Part_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }

        private void btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            //this.WindowState = WindowState.Minimized;

            Window parentWindow = Window.GetWindow(this);
            parentWindow.WindowState = WindowState.Minimized;
        }

        private void btn_Normal_MaxWin_Click(object sender, RoutedEventArgs e)
        {
            if(Window.GetWindow(this).WindowState == WindowState.Normal)
            {
                Window.GetWindow(this).WindowState = WindowState.Maximized;
                icon_btn_Normal_MaxWin.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                StackPanel_NutChucNang.Margin = new Thickness(5);
            }
            else
            {
                Window.GetWindow(this).WindowState = WindowState.Normal;
                icon_btn_Normal_MaxWin.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                StackPanel_NutChucNang.Margin = new Thickness(0);

            }

        }




    }
}
