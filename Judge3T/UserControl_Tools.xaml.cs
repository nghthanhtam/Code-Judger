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
using System.Windows.Media;
using System.Windows.Threading;
using System.IO; //Folder, Directory
using System.Diagnostics; //Debug,Writeline
using Winforms = System.Windows.Forms; //FolderDialog
using System.Threading;
using System.Media;

namespace Judge3T
{
    /// <summary>
    /// Interaction logic for UserControl_Tools.xaml
    /// </summary>
    public partial class UserControl_Tools : UserControl
    {
        public UserControl_Tools()
        {
            InitializeComponent();
             
        }


        string dirGoc = String.Empty;
        string dirLuu = String.Empty;

        private void btn_ChonBoTestGoc_Click(object sender, RoutedEventArgs e)
        { 
            Winforms.FolderBrowserDialog FBD_ChonFile = new Winforms.FolderBrowserDialog();

            Winforms.DialogResult Dialogkq = FBD_ChonFile.ShowDialog();
            if (Dialogkq == Winforms.DialogResult.Cancel)
            {
                return;
            }

            txt_DuongDanBoTestGoc.Text = FBD_ChonFile.SelectedPath;
            btn_ChonNoiLuu.IsEnabled = true;
            txt_DuongDanNoiLuu.IsEnabled = true;
        }

        private void txt_DuongDanBoTestGoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_DuongDanBoTestGoc.Text == String.Empty)
            { 
                btn_ChonNoiLuu.IsEnabled = false;
                txt_DuongDanNoiLuu.IsEnabled = false;
                btn_ChuyenDoi.IsEnabled = false;
            }
            else
            { 
                btn_ChonNoiLuu.IsEnabled = true;
                txt_DuongDanNoiLuu.IsEnabled = true;
                txt_DuongDanNoiLuu_TextChanged(sender, e);
            }
        }

        private void btn_ChonNoiLuu_Click(object sender, RoutedEventArgs e)
        {
            Winforms.FolderBrowserDialog FBD_NoiLuu = new Winforms.FolderBrowserDialog();
            Winforms.DialogResult Dialogkq = FBD_NoiLuu.ShowDialog();
            if (Dialogkq == Winforms.DialogResult.Cancel)
            {
                return;
            }
            txt_DuongDanNoiLuu.Text = FBD_NoiLuu.SelectedPath;
             btn_ChuyenDoi.IsEnabled = true;
        }

        private void btn_ChuyenDoi_Click(object sender, RoutedEventArgs e)
        {

            dirGoc = txt_DuongDanBoTestGoc.Text;
            dirLuu = txt_DuongDanNoiLuu.Text;
            bool isError = false;
            try
            {
                string[] arr = Directory.GetFiles(dirGoc); // Lấy list chứa path của từng thư mục con của thư mục dirCode
                int x = arr.Length;
                int i = 0;
                string[] arr1 = new string[x];

                foreach (string z in arr)
                {
                    arr1[i] = System.IO.Path.GetFileNameWithoutExtension(z);// List chứa tên của từng thư mục con của thư mục dirCode
                    i += 1;
                }
                Directory.CreateDirectory(dirLuu + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc));
                foreach (string ts in arr1)
                {
                    string dirThuMucBoTestGoc = dirLuu + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc);
                    Directory.CreateDirectory(dirThuMucBoTestGoc + "\\" + ts);
                    if (!File.Exists(dirThuMucBoTestGoc + "\\" + ts + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc) + ".inp"))
                    {
                        File.Copy(dirGoc + "\\" + ts + ".in", dirThuMucBoTestGoc + "\\" + ts + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc) + ".inp");
                    }
                    if (!File.Exists(dirLuu + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc) + "\\" + ts + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc) + ".out"))
                    {
                        File.Copy(dirGoc + "\\" + ts + ".out", dirThuMucBoTestGoc + "\\" + ts + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc) + ".out");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Có lỗi xảy ra!\n" + ex.Message, "Xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                try
                {
                    Directory.Delete(dirLuu + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirGoc), true);//Xóa file đã tạo ra

                }
                catch
                {

                }
                isError = true;
            }
            if (!isError)
            {
                ThreadPool.QueueUserWorkItem( // Tiến trình quản lí thông báo 
            (obj1) =>
          {






              SoundPlayer audio = new SoundPlayer(Properties.Resources.cham_xong); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
              audio.Play();


              this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
              {
                  Snackbar_ChuyenDoiBoTest.IsActive = true;

              }));




              Thread.Sleep(3000);

              this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
              {
                  Snackbar_ChuyenDoiBoTest.IsActive = false;

              }));

          });
                Process.Start(dirLuu, "-p");
            }
        }
         
         
        private void txt_DuongDanNoiLuu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_DuongDanNoiLuu.Text == String.Empty)
            {
                btn_ChuyenDoi.IsEnabled = false;
            }
            else
            {
                btn_ChuyenDoi.IsEnabled = true;
            }
        }
    }
}
