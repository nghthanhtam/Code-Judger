using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MaterialDesignThemes.Wpf;
using Utility;

namespace Judge3T
{
    /// <summary>
    /// Interaction logic for UserControl_BangDiem.xaml
    /// </summary>
    public partial class UserControl_BangDiem : System.Windows.Controls.UserControl
    {
        MainWindow MainWin;
        int soLanXoaHet;
        public UserControl_BangDiem()
        {
            InitializeComponent();
        }

        public UserControl_BangDiem(MainWindow x)
        {
            InitializeComponent();
            MainWin = x;

            btn_ChonThiSinh.IsEnabled = false;
            btn_ChamBai.IsEnabled = false;
            btn_XuatPDF.IsEnabled = false;

            Win_BangDiem.Width = Double.NaN;
            Win_BangDiem.Height = Double.NaN;
        }



        private void btn_ChonBoTest_Click(object sender, RoutedEventArgs e)
        {
            if (DangChamBai == true)
                return;
            FolderBrowserDialog FBD_ChonBoTest = new FolderBrowserDialog();

            if (Properties.Resources.isDebug == "True")
            {
                FBD_ChonBoTest.SelectedPath = Properties.Resources.PathBoTest;
            }
            else
            {
                DialogResult Dialogkq = FBD_ChonBoTest.ShowDialog();
                if (Dialogkq == DialogResult.Cancel)
                {
                    return;
                }
            }

            MainWin.listBoTest.Clear();
            DG_BangDiem.Columns.Clear();
            ClassBoTest.thongBao = "";

            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                DG_BangDiem.Items.Refresh();
            }));


            string dirBoTest = FBD_ChonBoTest.SelectedPath;


            MainWin.listBoTest.Clear(); // làm tươi list bài tập

            var arr = Directory.GetDirectories(dirBoTest); // lấy danh sách thư mục Problem trong thư mục bộ 


            for (int j = 0; j < arr.Count(); j++)
            {

                // Chuẩn hóa lại tên thư mục bộ test thành chữ in
                if (System.IO.Path.GetFileName(arr[j]).ToUpper()!= System.IO.Path.GetFileName(arr[j]))
                {
                    string dirTest_Temp = System.IO.Path.GetDirectoryName(arr[j]) + "\\" + System.IO.Path.GetFileName(arr[j]) + "_temp";
                    string dirTest_Chuan = System.IO.Path.GetDirectoryName(arr[j]) + "\\" + System.IO.Path.GetFileName(arr[j]).ToUpper();
                    Directory.Move(arr[j], dirTest_Temp);
                    Directory.Move(dirTest_Temp, dirTest_Chuan);
                    arr[j] = dirTest_Chuan;
                }
                // Chuẩn hóa lại tên thư mục bộ test thành chữ in

                if (ClassBoTest.kiemTraBoTest(arr[j], arr.Count()) == true)
                {
                    MainWin.listBoTest.Add(new ClassBoTest(System.IO.Path.GetFileNameWithoutExtension(arr[j]), dirBoTest + "\\" + System.IO.Path.GetFileNameWithoutExtension(arr[j]))); // Tạo đối tượng BaiTap và đẩy vào listBoTest 
                }
            }


            MainWin.nBangDiem = 0;

            DG_BangDiem.Columns.Add(new DataGridTextColumn
            {
                Header = "STT",
                Binding = new System.Windows.Data.Binding(string.Format("[{0}]", MainWin.nBangDiem++))
            });

            DG_BangDiem.Columns.Add(new DataGridTextColumn
            {
                Header = "Họ và tên",
                Binding = new System.Windows.Data.Binding(string.Format("[{0}]", MainWin.nBangDiem++))
            });

            //danhSachBaiTap = new string[MainWin.listBoTest.Count];
            //int i = 0;
            foreach (ClassBoTest BT in MainWin.listBoTest) // sau khi chọn xong bộ test thì đẩy tên bài lên lưới
            {
                DG_BangDiem.Columns.Add(new DataGridTextColumn
                {
                    Header = BT.tenBoTest,
                    Binding = new System.Windows.Data.Binding(string.Format("[{0}]", MainWin.nBangDiem++))
                });

                //danhSachBaiTap[i] = BT.tenBoTest;
                //i++;
            }


            DG_BangDiem.Columns.Add(new DataGridTextColumn
            {
                Header = "Tổng điểm",
                Binding = new System.Windows.Data.Binding(string.Format("[{0}]", MainWin.nBangDiem++))
            });


            DG_BangDiem.ItemsSource = MainWin.BangDiem;
            btn_ChonThiSinh.IsEnabled = true;
            btn_CauHinhBoTest.Visibility = Visibility.Visible;

        }

        private void btn_ChonThiSinh_Click(object sender, RoutedEventArgs e)
        {
            if (DangChamBai == true)
                return;
            FolderBrowserDialog FBD_ChonThuMucThiSinh = new FolderBrowserDialog();


            if (Properties.Resources.isDebug == "True")
            {
                FBD_ChonThuMucThiSinh.SelectedPath = Properties.Resources.PathCode;
            }
            else
            {
                DialogResult Dialogkq = FBD_ChonThuMucThiSinh.ShowDialog();

                if (DialogResult.Cancel == Dialogkq)
                {
                    return;
                }
            }

            MainWin.listThiSinh.Clear();
            for (int i = MainWin.mBangDiem - 1; i >= 0; i--)
            {
                MainWin.BangDiem.RemoveAt(i);
            }

            MainWin.mBangDiem = 0;

            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                DG_BangDiem.Items.Refresh();
            }));

            string dirCode = FBD_ChonThuMucThiSinh.SelectedPath;

            string[] arrThiSinh = Directory.GetDirectories(dirCode); // Lấy list chưa tên thư mục của thư mục dirdirCode
            for (int i = 0; i < arrThiSinh.Count(); i++)
            {
                // Chuẩn hóa lại tên thư mục bộ thí sinh thành chữ in
                if (System.IO.Path.GetFileName(arrThiSinh[i]).ToUpper() != System.IO.Path.GetFileName(arrThiSinh[i]))
                {
                    string dirThiSinh_Temp = System.IO.Path.GetDirectoryName(arrThiSinh[i]) + "\\" + System.IO.Path.GetFileName(arrThiSinh[i]) + "_temp";
                    string dirThiSinh_Chuan = System.IO.Path.GetDirectoryName(arrThiSinh[i]) + "\\" + System.IO.Path.GetFileName(arrThiSinh[i]).ToUpper();
                    Directory.Move(arrThiSinh[i], dirThiSinh_Temp);
                    Directory.Move(dirThiSinh_Temp, dirThiSinh_Chuan);
                    arrThiSinh[i] = dirThiSinh_Chuan;
                }
                // Chuẩn hóa lại tên thư mục bộ thí sinh thành chữ in

                MainWin.listThiSinh.Add(new ClassThiSinh(System.IO.Path.GetFileNameWithoutExtension(arrThiSinh[i]), dirCode + "\\" + System.IO.Path.GetFileNameWithoutExtension(arrThiSinh[i])));
            }

            foreach (ClassThiSinh TS in MainWin.listThiSinh) // ghi thông tin thí sinh vào mảng BangDiem
            {
                MainWin.BangDiem.Add(new string[100]);
                MainWin.BangDiem[MainWin.mBangDiem][0] = (MainWin.mBangDiem + 1).ToString();
                MainWin.BangDiem[MainWin.mBangDiem][1] = TS.tenThiSinh;
                MainWin.mBangDiem++;
            }
            //MainWin.BangDiem[MainWin.mBangDiem][1] = "abc";
            DG_BangDiem.Items.Refresh();
            btn_ChamBai.IsEnabled = true;
        }


        #region Xử lí chấm bài 
        private bool DangChamBai = false;

        private void btn_ChamBai_Click(object sender, RoutedEventArgs e)
        {
            soLanXoaHet = 0;
            if (DangChamBai == false)
            {
                DangChamBai = true;
            }
            else
                return;

            int SoLuongBaiCham = 0;

            ProgressBar_BtnChamBai.Visibility = Visibility.Visible; // Hiện trạng thái đang chấm bài
            #region Tạo tiến trình để chấm bài

            TaoTienTrinhChamBai(0, MainWin.listThiSinh.Count() - 1, 0, int.MaxValue);


            #endregion
        }
        #endregion



        /// <summary>
        /// Chấm bài và cập nhật lên DGV.
        /// Sẽ chấm từ listThiSinh[vtdau_ThiSinh] -> listThiSinh[vtcuoi_ThisSinh]
        /// Với mỗi thí sinh như trên, sẽ chấm từ bài listThiSinh[].listbaiLam[vtdau_BaiLam] -> listThiSinh[].listbaiLam[vtcuoi_BaiLam]
        /// </summary>
        /// <param name="vtdau_ThiSinh">chỉ số vị trí đầu của thí sinh trong (listThiSinh) cần chấm</param>
        /// <param name="vtcuoi_ThiSinh">chỉ số vị trí cuối của thí sinh trong (listThiSinh) cần chấm</param>
        /// <param name="vtdau_BaiLam">chỉ số vị trí đầu của bài làm của thí sinh trong (listThiSinh[].listbaiLam[vtdau_BaiLam]) cần chấm</param>
        /// <param name="vtcuoi_BaiLam">chỉ số vị trí đầu của bài làm của thí sinh trong (listThiSinh[].listbaiLam[vtcuoi_BaiLam]) cần chấm</param>
        private void TaoTienTrinhChamBai(int vtdau_ThiSinh, int vtcuoi_ThiSinh, int vtdau_BaiLam, int vtcuoi_BaiLam )
        {
            ThreadPool.QueueUserWorkItem( // việc chấm bài sử dụng trên luồng khác, không cùng luồng với GUI
            (obj) =>
            {
                int demts, SoLuongBaiCham = 0;
                ClassThiSinh thisinh;
                ClassBaiLam bailam;
                for (int i = vtdau_ThiSinh; i <= vtcuoi_ThiSinh; i++) // Duyệt qua từng thí sinh, 
                {

                    demts = i;

                    thisinh = MainWin.listThiSinh[i];

                    float tongDiem = 0;
                    for (int j = vtdau_BaiLam; j <= Math.Min(vtcuoi_BaiLam , MainWin.listThiSinh[i].listBaiLam.Count()-1); j++) // Với mỗi bài làm của thí sinh đó thì:
                    {
                        bailam = thisinh.listBaiLam[j];

                        int dembt = 0;
                        string dirtemp = DuongDan.randtemp(); // đường dẫn bộ nhớ tạm 

                        #region Xác định bộ test
                        bool TonTaiBoTest = false;
                        ClassBoTest BoTest = new ClassBoTest();
                        foreach (ClassBoTest x in MainWin.listBoTest)
                        {
                            dembt++;
                            if (x.tenBoTest == bailam.tenBaiLam.ToUpper())
                            {
                                TonTaiBoTest = true;
                                BoTest = x;
                                break;
                            }
                        }
                        #endregion

                        if (!TonTaiBoTest) // ko tồn tại bộ test - do đặt sai tên -> 0đ
                        {
                            continue; // bỏ qua việc chấm bài
                        }

                        SoLuongBaiCham++;

                        MainWin.BangDiem[demts][dembt + 1] = "...";
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                        {
                            DG_BangDiem.Items.Refresh();
                        }));

                        #region Kiểm tra từ cấm
                        if (XuLi.KiemTraTuKhoa(bailam.dirBaiLam, BoTest.tuKhoaCam)) // nếu tồn tại từ khóa cấm thì
                        {
                            MainWin.BangDiem[demts][dembt + 1] = "Bị cấm";
                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                DG_BangDiem.Items.Refresh();
                            }));


                            bailam.ketQua = new KetQuaChamBai(StatusResult.BAN, 0, BoTest.kieuCham);
                            continue; //  bỏ qua bài hiện tại
                        }
                        #endregion

                        #region Biên dịch
                        Result ResBienDich;
                        if (System.IO.Path.GetExtension((bailam.dirBaiLam).ToUpper()) == ".PAS")
                        {
                            ResBienDich = XuLi.biendichpas(bailam.dirBaiLam, dirtemp + ".exe"); // biên dịch file pas ra exe
                        }
                        else
                        {
                            if (System.IO.Path.GetExtension((bailam.dirBaiLam).ToUpper()) == ".CPP")
                                ResBienDich = XuLi.biendichcpp(bailam.dirBaiLam, dirtemp + ".exe"); // biên dịch file cpp ra exe   
                            else
                                ResBienDich = XuLi.biendichc(bailam.dirBaiLam, dirtemp + ".exe"); // bien dich file c ra exe
                        }


                        if (ResBienDich.Flag == false) // biên dịch thất bại
                        {
                            MainWin.BangDiem[demts][dembt + 1] = "Lỗi biên dịch";
                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                DG_BangDiem.Items.Refresh();
                            }));

                            bailam.ketQua = new KetQuaChamBai(StatusResult.CE, 0, BoTest.kieuCham);
                            continue; //  bỏ qua bài hiện tại
                        }
                        #endregion

                        #region Chọn kiểu chấm
                        switch (BoTest.kieuCham)
                        {
                            case KieuChamBai.OI:
                                bailam.ketQua = XuLi.ChamDiemOI(dirtemp, BoTest);
                                break;
                            case KieuChamBai.ACM:
                                bailam.ketQua = XuLi.ChamDiemACM(dirtemp, BoTest);
                                break;
                            case KieuChamBai.Challenge:
                                bailam.ketQua = XuLi.ChamDiemChallenge(dirtemp, BoTest, (int)(new FileInfo(bailam.dirBaiLam)).Length);  // diem == -1 -> WA
                                break;
                        }
                        #endregion

                        #region Cập nhật điểm lên lưới
                        string DiemToDataGrid_STR = "";
                        switch (bailam.ketQua.Type)
                        {
                            case KieuChamBai.OI:
                                DiemToDataGrid_STR = bailam.ketQua.Scores.ToString();
                                break;
                            case KieuChamBai.ACM:
                                switch (bailam.ketQua.Status)
                                {
                                    case StatusResult.AC:
                                        DiemToDataGrid_STR = bailam.ketQua.Scores.ToString();
                                        break;
                                    case StatusResult.RE:
                                        DiemToDataGrid_STR = "RE";
                                        break;
                                    case StatusResult.WA:
                                        DiemToDataGrid_STR = "WA";
                                        break;
                                    case StatusResult.TLE:
                                        DiemToDataGrid_STR = "TLE";
                                        break;
                                }
                                break;
                            case KieuChamBai.Challenge:
                                switch (bailam.ketQua.Status)
                                {
                                    case StatusResult.AC:
                                        DiemToDataGrid_STR = bailam.ketQua.Scores.ToString();
                                        break;
                                    case StatusResult.RE:
                                        DiemToDataGrid_STR = "RE";
                                        break;
                                    case StatusResult.WA:
                                        DiemToDataGrid_STR = "WA";
                                        break;
                                    case StatusResult.TLE:
                                        DiemToDataGrid_STR = "TLE";
                                        break;
                                }
                                break;
                        }
                        MainWin.BangDiem[demts][dembt+1] = DiemToDataGrid_STR;

                        tongDiem += bailam.ketQua.Scores;

                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                        {
                            DG_BangDiem.Items.Refresh();
                        }));
                        #endregion


                        if (File.Exists(dirtemp)) // nếu tồn tại file rác exe thì xóa nó 
                            File.Delete(dirtemp);

                    }
 

                    #region Cập nhật tổng điểm
                    if (vtdau_BaiLam == vtcuoi_BaiLam || vtcuoi_BaiLam == -1) // tức là đang chấm lại 1 bài duy nhất hoặc trường hợp ko chấm lại bài nào cả
                    {
                        tongDiem = 0;
                        for (int j=2; j< MainWin.listBoTest.Count()+2; j++)
                        {
                            float diem = 0;
                            if (float.TryParse(MainWin.BangDiem[i][j], out diem) == true)
                                tongDiem += diem;
                        }
                    }
                    
                    MainWin.BangDiem[demts][MainWin.nBangDiem - 1] = tongDiem.ToString();
                    #endregion

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        DG_BangDiem.Items.Refresh();
                    }));

                    demts++;
                }


                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    btn_XuatPDF.IsEnabled = true;
                    ProgressBar_BtnChamBai.Visibility = Visibility.Collapsed;
                    DangChamBai = false; // Cho phép bấm button chấm bài!
                    //DataGridContextMenu.Visibility = Visibility.Visible;
                }));


                ThreadPool.QueueUserWorkItem( // Tiến trình quản lí thông báo 
                    (obj1) =>
                    { 
                        SoundPlayer audio = new SoundPlayer(Properties.Resources.cham_xong); // âm thanh báo hiệu chấm xong
                        audio.Play();


                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                        {
                            Snackbar_ThongBao.IsActive = true;

                        }));




                        Thread.Sleep(3000);

                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                        {
                            Snackbar_ThongBao.IsActive = false;

                        }));


                        #region Gui du lieu thong ke ve server
                        try
                        {
                            string urlAddress = Properties.Resources.DomainJudge3T + "/analytics/send.php?luotcham=1&sobaicham=" + SoLuongBaiCham;
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                            request.Credentials = CredentialCache.DefaultCredentials;
                            WebResponse response = request.GetResponse();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Lỗi gửi dữ liệu thống kê về server : " + ex.Message);
                        }
                        #endregion


                    });


            }); 
        }


        private void btn_XuatPDF_Click(object sender, RoutedEventArgs e)
        {
            if (DangChamBai == true)
                return;
            SaveFileDialog SFD_ChonThuMucLuu = new SaveFileDialog();
            SFD_ChonThuMucLuu.FileName = "KetQuaChamBai_Judge3T_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            SFD_ChonThuMucLuu.Filter = "PDF (*.pdf)|*.pdf";


            DialogResult Dialogkq = SFD_ChonThuMucLuu.ShowDialog();

            if (DialogResult.Cancel == Dialogkq)
            {

                return;
            }

            string duongDanPDF = SFD_ChonThuMucLuu.FileName;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(duongDanPDF, FileMode.Create));
            doc.Open();// Mở Document để viết

            string ARIALUNI_TFF = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

            BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            iTextSharp.text.Font boldLess10Column = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font boldMoreThan10Column = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font boldMoreThan15Column = new iTextSharp.text.Font(bf, 6, iTextSharp.text.Font.BOLD);

            iTextSharp.text.Font bold = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLD);

            iTextSharp.text.Font normalLess10Column = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font normalMoreThan10Column = new iTextSharp.text.Font(bf, 7, iTextSharp.text.Font.NORMAL);

            iTextSharp.text.Font normalMoreThan15Column = new iTextSharp.text.Font(bf, 4, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Paragraph par = new iTextSharp.text.Paragraph("KẾT QUẢ CHẤM BÀI", bold);
            iTextSharp.text.Paragraph par1 = new iTextSharp.text.Paragraph("     ");
            par1.Alignment = Element.ALIGN_CENTER;
            par.Alignment = Element.ALIGN_CENTER;
            doc.Add(par);
            doc.Add(par1);

            PdfPTable table = new PdfPTable(DG_BangDiem.Columns.Count);//TÍnh số cột của DataGridView và tạo bảng theo số cột đó

            //Thêm header vào từ DataGridView vào table
            for (int j = 0; j < DG_BangDiem.Columns.Count; j++)
            {
                if (DG_BangDiem.Columns.Count > 15)
                {
                    table.AddCell(new Phrase(DG_BangDiem.Columns[j].Header.ToString(), boldMoreThan15Column));
                }
                else if (DG_BangDiem.Columns.Count > 10)
                {
                    table.AddCell(new Phrase(DG_BangDiem.Columns[j].Header.ToString(), boldMoreThan10Column));
                }
                else
                {
                    table.AddCell(new Phrase(DG_BangDiem.Columns[j].Header.ToString(), boldLess10Column));

                }
            }

            //Đánh dấu dòng 1 là header
            table.HeaderRows = 1;

            //Chuyển dữ liệu từ DataGridView vào PDF
            for (int i = 0; i < MainWin.mBangDiem; i++)
            {
                for (int j = 0; j < MainWin.nBangDiem; j++)
                {
                    if (DG_BangDiem.Columns.Count > 15)
                    {

                        table.AddCell(new Phrase(MainWin.BangDiem[i][j], normalMoreThan15Column));
                    }
                    else if (DG_BangDiem.Columns.Count > 10)
                    {
                        table.AddCell(new Phrase(MainWin.BangDiem[i][j], normalMoreThan10Column));
                    }
                    else
                    {

                        table.AddCell(new Phrase(MainWin.BangDiem[i][j], normalLess10Column));
                    }
                }
            }


            doc.Add(table);


            doc.Close();
            System.Diagnostics.Process.Start(@duongDanPDF);
        }


        private void btn_CauHinhBoTest_Click(object sender, RoutedEventArgs e)
        {
            var winForm = new frm_CauHinhBoTest(MainWin);
            winForm.ShowDialog();
        }


        private void MenuItem_XoaKetQua_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (btn_XuatPDF.IsEnabled == true)
                {
                    if (dangChamLaiHoacXoa == false)
                    {
                        dangChamLaiHoacXoa = true;

                        //if (DangChamBai = true)
                        //{
                        //    return;
                        //}
                        if (colindex == -2)
                        {
                            //for (int i = 0; i < MainWin.listThiSinh.Count; i++)
                            //{
                            //    MainWin.listThiSinh.RemoveAt(i);
                            //}
                            for (int z = 0; z < MainWin.listThiSinh.Count; z++)
                            {

                                for (int i = 0; i < DG_BangDiem.Columns.Count; i++)
                                {
                                    MainWin.BangDiem[z][i] = "";
                                }
                            }
                            int zx = MainWin.listThiSinh.Count;
                            for (int i = 0; i < zx; i++)
                            {
                                MainWin.listThiSinh.RemoveAt(0);
                            }
                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                DG_BangDiem.Items.Refresh();
                            }));
                            ThreadPool.QueueUserWorkItem( // Tiến trình quản lí thông báo 
                         (obj1) =>
                         {


                             SoundPlayer audio = new SoundPlayer(Properties.Resources.cham_xong); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
                             audio.Play();


                             this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                             {
                                 Snackbar_ThongBaoXoaBai.IsActive = true;

                             }));




                             Thread.Sleep(3000);

                             this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                             {
                                 Snackbar_ThongBaoXoaBai.IsActive = false;

                             }));





                         });

                            dangChamLaiHoacXoa = false;
                            //do nothing
                        }
                        else if (colindex == 0 || colindex == 1 || colindex == (DG_BangDiem.Columns.Count - 1))
                        {

                            soLanXoaHet++;

                            MainWin.listThiSinh.RemoveAt(rowindex);
                            for (int i = 1; i < DG_BangDiem.Columns.Count; i++)
                            {
                                float y = 0;
                                float x = float.Parse(MainWin.BangDiem[rowindex][DG_BangDiem.Columns.Count - 1]);
                                float n;
                                bool isNumeric = float.TryParse(MainWin.BangDiem[rowindex][i], out n);
                                // kiểm tra cell có chứa string mà biến đổi thành int hay không?
                                // Để sau khi xóa xong thi ổ Tổng thay đổi giá trị vì bị xóa 1 cell
                                // Tránh trường hợp ô cần xóa chứa string VD:"Lệnh cấm" mà không biển đổi thành int để trừ vào cột Tổng
                                if (isNumeric == true)
                                {
                                    y = float.Parse(MainWin.BangDiem[rowindex][i]);
                                }
                                MainWin.BangDiem[rowindex][i] = "";//Cell muốn xóa đã bị xóa
                                for (int k = rowindex; k < DG_BangDiem.Items.Count - 1; k++)
                                {

                                    MainWin.BangDiem[k][i] = MainWin.BangDiem[k + 1][i];


                                }
                                MainWin.BangDiem[DG_BangDiem.Items.Count - 1 - soLanXoaHet + 1][0] = "";
                                MainWin.BangDiem[DG_BangDiem.Items.Count - 1 - soLanXoaHet + 1][i] = "";
                            }
                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                DG_BangDiem.Items.Refresh();
                            }));
                            ThreadPool.QueueUserWorkItem( // Tiến trình quản lí thông báo 
                                (obj1) =>
                                {


                                    SoundPlayer audio = new SoundPlayer(Properties.Resources.cham_xong); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
                                    audio.Play();


                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                    {
                                        Snackbar_ThongBaoXoaBai.IsActive = true;

                                    }));




                                    Thread.Sleep(3000);

                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                    {
                                        Snackbar_ThongBaoXoaBai.IsActive = false;

                                    }));





                                });
                            dangChamLaiHoacXoa = false;
                        }
                        else
                        {
                            float y = 0;
                            float x = float.Parse(MainWin.BangDiem[rowindex][DG_BangDiem.Columns.Count - 1]);
                            float n;
                            bool isNumeric = float.TryParse(MainWin.BangDiem[rowindex][colindex], out n);
                            // kiểm tra cell có chứa string mà biến đổi thành int hay không?
                            // Để sau khi xóa xong thi ổ Tổng thay đổi giá trị vì bị xóa 1 cell
                            // Tránh trường hợp ô cần xóa chứa string VD:"Lệnh cấm" mà không biển đổi thành int để trừ vào cột Tổng
                            if (isNumeric == true)
                            {


                                y = float.Parse(MainWin.BangDiem[rowindex][colindex]);
                            }
                            MainWin.BangDiem[rowindex][DG_BangDiem.Columns.Count - 1] = (x - y).ToString();//Cell tổng =tổng- cell đang muốn xóa
                            MainWin.BangDiem[rowindex][colindex] = "";//Cell muốn xóa đã bị xóa 
                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                            {
                                DG_BangDiem.Items.Refresh();
                            }));
                            ThreadPool.QueueUserWorkItem( // Tiến trình quản lí thông báo 
                                (obj1) =>
                                {


                                    SoundPlayer audio = new SoundPlayer(Properties.Resources.cham_xong); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
                                    audio.Play();


                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                    {
                                        Snackbar_ThongBaoXoaBai.IsActive = true;

                                    }));




                                    Thread.Sleep(3000);

                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                    {
                                        Snackbar_ThongBaoXoaBai.IsActive = false;

                                    }));





                                });
                            dangChamLaiHoacXoa = false;

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("Chưa có bảng để xóa. Vui lòng thực hiện thao tác 'Chấm bài'");

            }
        }

        bool dangChamLaiHoacXoa = false;


        #region Xử lí chấm lại bài
        private void MenuItem_ChamLaiBai_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btn_XuatPDF.IsEnabled == true)
                {
                    if (DangChamBai == false)
                    {
                        DangChamBai = true;

                        int col, row;

                        if (MainWin.listThiSinh.Count > rowindex)
                        {
                            int SoLuongBaiCham = 0;
                            if (colindex == -2)
                            {
                                DangChamBai = false;
                                //do nothing
                            }
                            else
                            {
                                if (colindex >=2 && colindex< DG_BangDiem.Columns.Count - 1) // chấm 1 bài
                                {
                                    #region TÌm vị trí bài làm này của thí sinh trong listbailam
                                    int vtBaiLamTrongListBaiLam = -1; 
                                    for (int i = 0; i <MainWin.listThiSinh[rowindex].listBaiLam.Count(); i++) // duyệt qua các bài làm của thí sinh này
                                    {
                                        if (MainWin.listBoTest[colindex - 2].tenBoTest == MainWin.listThiSinh[rowindex].listBaiLam[i].tenBaiLam)
                                        {
                                            vtBaiLamTrongListBaiLam = i;
                                            break;
                                        }
                                    }
                                    #endregion

                                    if (vtBaiLamTrongListBaiLam != -1)
                                        TaoTienTrinhChamBai(rowindex, rowindex, vtBaiLamTrongListBaiLam, vtBaiLamTrongListBaiLam);
                                    else 
                                        TaoTienTrinhChamBai(rowindex, rowindex, 0, -1); // để 0, -1 mục đích cho vòng lập duyệt qua bài làm của thí sinh không chạy.

                                }
                                else // chấm nhiều bài
                                {
                                    TaoTienTrinhChamBai(rowindex, rowindex, 0, int.MaxValue);
                                }

                                #region code cũ đã hủy

                                    /*

                                    if (colindex == 0 || colindex == 1 || colindex == (DG_BangDiem.Columns.Count - 1))
                                    {




                                        #region Chấm lại tất cả bài của thí sinh
                                        row = rowindex;
                                        col = colindex;
                                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                        {
                                            ProgressBar_BtnChamBai.Visibility = Visibility.Visible;
                                        }));
                                        ClassThiSinh thiSinhChamLai = MainWin.listThiSinh[rowindex];
                                        ClassBaiLam baiLamChamLai;

                                        MainWin.BangDiem[row][DG_BangDiem.Columns.Count - 1] = "0";//Cell tổng =tổng- cell đang muốn chấm lại
                                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                        {
                                            DG_BangDiem.Items.Refresh();
                                        }));
                                        //for (int i = 0; i < MainWin.listThiSinh.Count(); i++) // Duyệt qua từng thí sinh
                                        {
                                            //thisinh = MainWin.listThiSinh[i];
                                            float tongDiem = 0;
                                            for (int j = 0; j < thiSinhChamLai.listBaiLam.Count(); j++) // Với mỗi bài làm của thí sinh đó thì:
                                            {
                                                baiLamChamLai = thiSinhChamLai.listBaiLam[j];
                                                int dembt = 0;
                                                string dirtemp = DuongDan.randtemp(); // đường dẫn bộ nhớ tạm 


                                                #region Xác định bộ test
                                                bool TonTaiBoTest = false;
                                                ClassBoTest boTestBaiChamLai = new ClassBoTest();
                                                foreach (ClassBoTest boTest in MainWin.listBoTest)
                                                {

                                                    dembt++;
                                                    if (boTest.tenBoTest.ToUpper() == baiLamChamLai.tenBaiLam.ToUpper())
                                                    {
                                                        TonTaiBoTest = true;
                                                        boTestBaiChamLai = boTest;
                                                        break;
                                                    }
                                                }
                                                #endregion

                                                if (!TonTaiBoTest) // ko tồn tại bộ test - do đặt sai tên -> 0đ
                                                {
                                                    continue; // bỏ qua việc chấm bài
                                                }

                                                SoLuongBaiCham++;

                                                MainWin.BangDiem[row][dembt + 1] = "...";
                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    DG_BangDiem.Items.Refresh();
                                                }));

                                                #region Kiểm tra từ cấm
                                                if (XuLi.KiemTraTuKhoa(baiLamChamLai.dirBaiLam, boTestBaiChamLai.tuKhoaCam)) // nếu tồn tại từ khóa cấm thì
                                                {
                                                    MainWin.BangDiem[row][dembt + 1] = "Bị cấm";
                                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                    {
                                                        DG_BangDiem.Items.Refresh();
                                                    }));


                                                    baiLamChamLai.ketQua = new KetQuaChamBai(StatusResult.BAN, 0, boTestBaiChamLai.kieuCham);
                                                    continue; //  bỏ qua bài hiện tại
                                                }
                                                #endregion
                                                Result ResBienDich;

                                                #region Biên dịch
                                                if (System.IO.Path.GetExtension((baiLamChamLai.dirBaiLam).ToUpper()) == ".PAS")
                                                {
                                                    //System.Windows.MessageBox.Show(bailam.dirBaiLam);
                                                    ResBienDich = XuLi.biendichpas(baiLamChamLai.dirBaiLam, dirtemp + ".exe"); // biên dịch file pas ra exe
                                                }
                                                else
                                                {
                                                    if (System.IO.Path.GetExtension((baiLamChamLai.dirBaiLam).ToUpper()) == ".CPP")
                                                    {
                                                        ResBienDich = XuLi.biendichcpp(baiLamChamLai.dirBaiLam, dirtemp + ".exe"); // biên dịch file cpp ra exe   
                                                    }
                                                    else
                                                    {
                                                        ResBienDich = XuLi.biendichc(baiLamChamLai.dirBaiLam, dirtemp + ".exe"); // bien dich file c ra exe
                                                    }
                                                }

                                                if (ResBienDich.Flag == false) // biên dịch thất bại
                                                {
                                                    MainWin.BangDiem[row][dembt + 1] = "Lỗi biên dịch";
                                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                    {
                                                        DG_BangDiem.Items.Refresh();
                                                    }));

                                                    baiLamChamLai.ketQua = new KetQuaChamBai(StatusResult.CE, 0, boTestBaiChamLai.kieuCham);
                                                    continue; //  bỏ qua bài hiện tại
                                                }
                                                #endregion

                                                switch (boTestBaiChamLai.kieuCham)
                                                {
                                                    case KieuChamBai.OI:
                                                        baiLamChamLai.ketQua = XuLi.ChamDiemOI(dirtemp, boTestBaiChamLai);
                                                        break;

                                                    case KieuChamBai.ACM:
                                                        baiLamChamLai.ketQua = XuLi.ChamDiemACM(dirtemp, boTestBaiChamLai);  // diem == -1 -> WA

                                                        break;
                                                    case KieuChamBai.Challenge:
                                                        baiLamChamLai.ketQua = XuLi.ChamDiemChallenge(dirtemp, boTestBaiChamLai, (int)(new FileInfo(baiLamChamLai.dirBaiLam)).Length);  // diem == -1 -> WA
                                                        break;
                                                }


                                                #region Cập nhật điểm lên lưới
                                                string DiemToDataGrid_STR = "";
                                                switch (baiLamChamLai.ketQua.Type)
                                                {
                                                    case KieuChamBai.OI:
                                                        DiemToDataGrid_STR = baiLamChamLai.ketQua.Scores.ToString();
                                                        break;
                                                    case KieuChamBai.ACM:
                                                        switch (baiLamChamLai.ketQua.Status)
                                                        {
                                                            case StatusResult.AC:
                                                                DiemToDataGrid_STR = baiLamChamLai.ketQua.Scores.ToString();
                                                                break;
                                                            case StatusResult.RE:
                                                                DiemToDataGrid_STR = "RE";
                                                                break;
                                                            case StatusResult.WA:
                                                                DiemToDataGrid_STR = "WA";
                                                                break;
                                                            case StatusResult.TLE:
                                                                DiemToDataGrid_STR = "TLE";
                                                                break;
                                                        }
                                                        break;
                                                    case KieuChamBai.Challenge:
                                                        switch (baiLamChamLai.ketQua.Status)
                                                        {
                                                            case StatusResult.AC:
                                                                DiemToDataGrid_STR = baiLamChamLai.ketQua.Scores.ToString();
                                                                break;
                                                            case StatusResult.RE:
                                                                DiemToDataGrid_STR = "RE";
                                                                break;
                                                            case StatusResult.WA:
                                                                DiemToDataGrid_STR = "WA";
                                                                break;
                                                            case StatusResult.TLE:
                                                                DiemToDataGrid_STR = "TLE";
                                                                break;
                                                        }
                                                        break;
                                                }
                                                MainWin.BangDiem[row][dembt + 1] = DiemToDataGrid_STR;
                                                #endregion

                                                tongDiem += baiLamChamLai.ketQua.Scores;

                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    DG_BangDiem.Items.Refresh();
                                                }));

                                                if (File.Exists(dirtemp)) // nếu tồn tại file rác exe thì xóa nó 
                                                    File.Delete(dirtemp);
                                            }

                                            MainWin.BangDiem[row][MainWin.nBangDiem - 1] = tongDiem.ToString();

                                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                            {
                                                DG_BangDiem.Items.Refresh();
                                            }));



                                        }
                                        //System.Windows.Forms.MessageBox.Show("Đã chấm lại bài làm " + baiLamChamLai.tenBaiLam + " của thí sinh " + thiSinhChamLai.tenBoTest, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);



                                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                        {
                                            btn_XuatPDF.IsEnabled = true;
                                            ProgressBar_BtnChamBai.Visibility = Visibility.Collapsed;

                                            DangChamBai = false; // Cho phép bấm button chấm bài!

                                          }));


                                        ThreadPool.QueueUserWorkItem( // Tiến trình quản lí thông báo 
                                            (obj1) =>
                                            {


                                                SoundPlayer audio = new SoundPlayer(Properties.Resources.cham_xong); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
                                                  audio.Play();


                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    Snackbar_ThongBaoChamLai.IsActive = true;

                                                }));

                                                Thread.Sleep(3000);

                                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                {
                                                    Snackbar_ThongBaoChamLai.IsActive = false;

                                                }));


                                                  #region Gui du lieu thong ke ve server
                                                  try
                                                {
                                                    string urlAddress = Properties.Resources.DomainJudge3T + "/analytics/send.php?luotcham=1&sobaicham=" + SoLuongBaiCham;
                                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                                                    request.Credentials = CredentialCache.DefaultCredentials;
                                                      // Get the response.  
                                                      WebResponse response = request.GetResponse();
                                                }
                                                catch (Exception ex)
                                                {
                                                    Debug.WriteLine("Lỗi gửi dữ liệu thống kê về server : " + ex.Message);
                                                }
                                                  #endregion


                                              });



                                        dangChamLaiHoacXoa = false;

                                        #endregion
                                    }
                                    else
                                    {

                                        #region Chấm lại 1 bài

                                        row = rowindex;
                                        col = colindex;
                                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                        {
                                            ProgressBar_BtnChamBai.Visibility = Visibility.Visible;
                                        }));
                                        ClassThiSinh thiSinhChamLai = MainWin.listThiSinh[row];
                                        ClassBaiLam baiLamChamLai = new ClassBaiLam();
                                        foreach (ClassBaiLam baiLam in MainWin.listThiSinh[row].listBaiLam)
                                        {
                                            if (danhSachBaiTap[col - 2] == baiLam.tenBaiLam.ToUpper())
                                            {
                                                baiLamChamLai = baiLam;
                                            }

                                        }
                                        ClassBoTest boTestBaiChamLai = MainWin.listBoTest[col - 2];
                                        //for (int i = 0; i < MainWin.listThiSinh.Count(); i++) // Duyệt qua từng thí sinh, 
                                        float n;
                                        float y = 0;
                                        bool isNumeric = float.TryParse(MainWin.BangDiem[row][col], out n);
                                        float x = float.Parse(MainWin.BangDiem[row][DG_BangDiem.Columns.Count - 1]);
                                        if (isNumeric == true)
                                        {
                                            y = float.Parse(MainWin.BangDiem[row][col]);
                                        }
                                        MainWin.BangDiem[row][DG_BangDiem.Columns.Count - 1] = (x - y).ToString();//Cell tổng =tổng- cell đang muốn chấm lại
                                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                        {
                                            DG_BangDiem.Items.Refresh();
                                        }));

                                        {
                                            //for (int j = 0; j < thisinh.listBaiLam.Count(); j++) // Với mỗi bài làm của thí sinh đó thì:
                                            {

                                                string dirtemp = DuongDan.randtemp(); // đường dẫn bộ nhớ tạm 


                                                #region Xác định bộ test
                                                bool TonTaiBoTest = false;

                                                if (boTestBaiChamLai.tenBoTest.ToUpper() == baiLamChamLai.tenBaiLam.ToUpper())
                                                {
                                                    TonTaiBoTest = true;

                                                }
                                                #endregion

                                                if (!TonTaiBoTest) // ko tồn tại bộ test - do đặt sai tên -> 0đ
                                                {
                                                    MainWin.BangDiem[row][col] = "";// bỏ qua việc chấm bài
                                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                    {
                                                        DG_BangDiem.Items.Refresh();
                                                    }));
                                                }
                                                else
                                                {
                                                    SoLuongBaiCham++;

                                                    MainWin.BangDiem[row][col] = "...";
                                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                    {
                                                        DG_BangDiem.Items.Refresh();
                                                    }));

                                                    #region Kiểm tra từ cấm
                                                    if (XuLi.KiemTraTuKhoa(baiLamChamLai.dirBaiLam, boTestBaiChamLai.tuKhoaCam)) // nếu tồn tại từ khóa cấm thì
                                                    {
                                                        MainWin.BangDiem[row][col] = "Bị cấm";
                                                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                        {
                                                            DG_BangDiem.Items.Refresh();
                                                        }));


                                                        baiLamChamLai.ketQua = new KetQuaChamBai(StatusResult.BAN, 0, boTestBaiChamLai.kieuCham);
                                                        //continue; //  bỏ qua bài hiện tại
                                                    }
                                                    #endregion
                                                    else
                                                    {

                                                        Result ResBienDich = new Result(false);

                                                        #region Biên dịch
                                                        if (System.IO.Path.GetExtension((baiLamChamLai.dirBaiLam).ToUpper()) == ".PAS")
                                                        {
                                                            //System.Windows.MessageBox.Show(bailam.dirBaiLam);
                                                            ResBienDich = XuLi.biendichpas(baiLamChamLai.dirBaiLam, dirtemp + ".exe"); // biên dịch file pas ra exe
                                                        }
                                                        else if (System.IO.Path.GetExtension((baiLamChamLai.dirBaiLam).ToUpper()) == ".CPP")
                                                        {
                                                            ResBienDich = XuLi.biendichcpp(baiLamChamLai.dirBaiLam, dirtemp + ".exe"); // biên dịch file cpp ra exe   
                                                        }
                                                        else
                                                        {
                                                            ResBienDich = XuLi.biendichc(baiLamChamLai.dirBaiLam, dirtemp + ".exe"); // bien dich file c ra exe
                                                        }
                                                        if (ResBienDich.Flag == false) // biên dịch thất bại
                                                        {
                                                            MainWin.BangDiem[row][col] = "Lỗi biên dịch";
                                                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                            {
                                                                DG_BangDiem.Items.Refresh();
                                                            }));

                                                            baiLamChamLai.ketQua = new KetQuaChamBai(StatusResult.CE, 0, boTestBaiChamLai.kieuCham);
                                                        }
                                                        #endregion




                                                        else
                                                        {
                                                            switch (boTestBaiChamLai.kieuCham)
                                                            {
                                                                case KieuChamBai.OI:
                                                                    baiLamChamLai.ketQua = XuLi.ChamDiemOI(dirtemp, boTestBaiChamLai);
                                                                    break;

                                                                case KieuChamBai.ACM:
                                                                    baiLamChamLai.ketQua = XuLi.ChamDiemACM(dirtemp, boTestBaiChamLai);  // diem == -1 -> WA

                                                                    break;
                                                                case KieuChamBai.Challenge:
                                                                    baiLamChamLai.ketQua = XuLi.ChamDiemChallenge(dirtemp, boTestBaiChamLai, (int)(new FileInfo(baiLamChamLai.dirBaiLam)).Length);  // diem == -1 -> WA
                                                                    break;
                                                            }
                                                            #region Cập nhật điểm lên lưới
                                                            string DiemToDataGrid_STR = "";
                                                            switch (baiLamChamLai.ketQua.Type)
                                                            {
                                                                case KieuChamBai.OI:
                                                                    DiemToDataGrid_STR = baiLamChamLai.ketQua.Scores.ToString();
                                                                    break;
                                                                case KieuChamBai.ACM:
                                                                    switch (baiLamChamLai.ketQua.Status)
                                                                    {
                                                                        case StatusResult.AC:
                                                                            DiemToDataGrid_STR = baiLamChamLai.ketQua.Scores.ToString();
                                                                            break;
                                                                        case StatusResult.RE:
                                                                            DiemToDataGrid_STR = "RE";
                                                                            break;
                                                                        case StatusResult.WA:
                                                                            DiemToDataGrid_STR = "WA";
                                                                            break;
                                                                        case StatusResult.TLE:
                                                                            DiemToDataGrid_STR = "TLE";
                                                                            break;
                                                                    }
                                                                    break;
                                                                case KieuChamBai.Challenge:
                                                                    switch (baiLamChamLai.ketQua.Status)
                                                                    {
                                                                        case StatusResult.AC:
                                                                            DiemToDataGrid_STR = baiLamChamLai.ketQua.Scores.ToString();
                                                                            break;
                                                                        case StatusResult.RE:
                                                                            DiemToDataGrid_STR = "RE";
                                                                            break;
                                                                        case StatusResult.WA:
                                                                            DiemToDataGrid_STR = "WA";
                                                                            break;
                                                                        case StatusResult.TLE:
                                                                            DiemToDataGrid_STR = "TLE";
                                                                            break;
                                                                    }
                                                                    break;
                                                            }
                                                            MainWin.BangDiem[row][col] = DiemToDataGrid_STR;
                                                            #endregion


                                                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                            {
                                                                DG_BangDiem.Items.Refresh();
                                                            }));

                                                            if (File.Exists(dirtemp)) // nếu tồn tại file rác exe thì xóa nó 
                                                                File.Delete(dirtemp);

                                                        }
                                                    }



                                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                                    {
                                                        DG_BangDiem.Items.Refresh();
                                                    }));

                                                    if (File.Exists(dirtemp)) // nếu tồn tại file rác exe thì xóa nó 
                                                        File.Delete(dirtemp);
                                                }
                                            }




                                        }
                                        isNumeric = float.TryParse(MainWin.BangDiem[row][col], out n);
                                        x = float.Parse(MainWin.BangDiem[row][DG_BangDiem.Columns.Count - 1]);
                                        if (isNumeric == true)
                                        {
                                            y = float.Parse(MainWin.BangDiem[row][col]);
                                        }
                                        MainWin.BangDiem[row][DG_BangDiem.Columns.Count - 1] = (x + y).ToString();//Cell tổng =tổng- cell đang muốn chấm lại
                                                                                                                  //System.Windows.Forms.MessageBox.Show("Đã chấm lại bài làm " + baiLamChamLai.tenBaiLam + " của thí sinh " + thiSinhChamLai.tenBoTest, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);



                                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                        {
                                            btn_XuatPDF.IsEnabled = true;
                                            ProgressBar_BtnChamBai.Visibility = Visibility.Collapsed;



                                            DangChamBai = false; // Cho phép bấm button chấm bài!

                                          }));


                                        ThreadPool.QueueUserWorkItem( // Tiến trình quản lí thông báo 
                                        (obj1) =>
                                        {


                                            SoundPlayer audio = new SoundPlayer(Properties.Resources.cham_xong); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
                                              audio.Play();


                                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                            {
                                                Snackbar_ThongBaoChamLai.IsActive = true;

                                            }));




                                            Thread.Sleep(3000);

                                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                                            {
                                                Snackbar_ThongBaoChamLai.IsActive = false;

                                            }));


                                              #region Gui du lieu thong ke ve server
                                              try
                                            {


                                                string urlAddress = Properties.Resources.DomainJudge3T + "/analytics/send.php?luotcham=1&sobaicham=" + SoLuongBaiCham;
                                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                                                request.Credentials = CredentialCache.DefaultCredentials;
                                                  // Get the response.  
                                                  WebResponse response = request.GetResponse();

                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine("Lỗi gửi dữ liệu thống kê về server : " + ex.Message);
                                            }
                                              #endregion


                                          });


                                        dangChamLaiHoacXoa = false;



                                        #endregion


                                    }


                                    */
                                    #endregion
                            }



                        }

                        

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        int colindex, rowindex;
        #endregion

        private void RightButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItem_ChamLaiBai.Visibility = Visibility.Visible;
            MenuItem_ChiTietChamBai.Visibility = Visibility.Visible;
            MenuItem_XemCode.Visibility = Visibility.Visible;
            //   if (btn_XuatPDF.IsEnabled == true) // nạp xong test và thí sinh
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;

                //Stepping through the visual tree
                while ((dep != null) && !(dep is System.Windows.Controls.DataGridCell))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                //Is the dep a cell or outside the bounds of Window1? 
                //Kiểm tra xem có bấm ngoài DataGrid không
                if (dep == null | !(dep is System.Windows.Controls.DataGridCell))
                {
                    //  System.Windows.Forms.MessageBox.Show("Ngoai vung DATAGRID");

                    colindex = -2;
                    MenuItem_ChamLaiBai.Visibility = Visibility.Collapsed;
                    MenuItem_ChiTietChamBai.Visibility = Visibility.Collapsed;
                    MenuItem_XemCode.Visibility = Visibility.Collapsed;
                    return;
                }
                else
                {
                    System.Windows.Controls.DataGridCell cell = new System.Windows.Controls.DataGridCell();
                    cell = (System.Windows.Controls.DataGridCell)dep;
                    while ((dep != null) && !(dep is DataGridRow))
                    {
                        dep = VisualTreeHelper.GetParent(dep);
                    }

                    if (dep == null)
                    {
                        return;
                    }
                    colindex = cell.Column.DisplayIndex;

                    DataGridRow row = dep as DataGridRow;
                    rowindex = DG_BangDiem.ItemContainerGenerator.IndexFromContainer(row);
                    //System.Windows.Forms.MessageBox.Show("Count" + DG_BangDiem.Items.Count);
                    //System.Windows.Forms.MessageBox.Show(DG_BangDiem.Columns.Count.ToString());
                    //Kiểm tra xem có bấm vào column STT, column Tên hay Tổng điểm không?
                    //Colindex=0 là cột 1 => STT
                    //Colindex=1 là cột 2 => Tên thí sinh
                    //Colindex=(DG-1) là cột cuối => Tổng điểm
                    //if (colindex == 0 || colindex == 1 || colindex == (DG_BangDiem.Columns.Count - 1))
                    //{
                    //    // System.Windows.Forms.MessageBox.Show("Ngoai vung DATAGRID");
                    //    colindex = -3;
                    //    return;
                    //}
                }

            }


        }

        private void MenuItem_XemCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (colindex == -3 || colindex == -2 || DangChamBai == true)
                {

                    return;
                }
                ClassBaiLam bailam = null;

                foreach (ClassBaiLam x in MainWin.listThiSinh[rowindex].listBaiLam) // Duyệt qua các bài làm của thí sinh đó
                {
                    if (x.tenBaiLam.ToUpper() == MainWin.listBoTest[colindex - 2].tenBoTest.ToUpper()) // nếu bài đang chọn trùng tên với bài đang duyệt qua
                    {
                        bailam = x;
                    }
                }

                if (bailam == null)
                {
                    System.Windows.Forms.MessageBox.Show("thí sinh này chưa nộp bài này!");
                    // thí sinh này chưa nộp bài này
                    return;
                }
                string code = File.ReadAllText(bailam.dirBaiLam);
                string thongtin = "Thí sinh: " + MainWin.listThiSinh[rowindex].tenThiSinh + " Bài: " + MainWin.listBoTest[colindex - 2].tenBoTest;

                Window frm = new frm_CodeThiSinh(code, thongtin);
                frm.ShowDialog();


            }

            catch
            {

            }
        }


        private void MenuItem_ChiTietChamBai_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (colindex == -3 || colindex == -2 || DangChamBai == true)
                {
                    return;
                }

                ClassBaiLam bailam = null;
                foreach (ClassBaiLam x in MainWin.listThiSinh[rowindex].listBaiLam) // Duyệt qua các bài làm của thí sinh đó
                {
                    if (x.tenBaiLam == MainWin.listBoTest[colindex - 2].tenBoTest) // nếu bài đang chọn trùng tên với bài đang duyệt qua
                    {
                        bailam = x;
                    }
                }

                if (bailam == null)
                {
                    System.Windows.Forms.MessageBox.Show("thí sinh này chưa nộp bài này!");
                    // thí sinh này chưa nộp bài này
                    return;
                }

                List<Object> listChiTietChamBai = new List<object>();
                listChiTietChamBai.Clear();
                int demChiTiet = 0;
                if (bailam.ketQua.Details != null)
                {
                    foreach (KetQuaChamTest x in bailam.ketQua.Details)
                    {
                        demChiTiet++;
                        Object kqchitiet = new
                        {
                            STT = demChiTiet,
                            NameTest = x.NameTest,
                            Status = x.Status,
                            TimeUsed = Math.Round(x.TimeUsed.TotalSeconds, 5),
                            MemoryUsed = (double)Math.Round((double)x.MemoryUsed / Math.Pow(2, 20), 2),
                            Messages = x.Messages
                        };
                        listChiTietChamBai.Add(kqchitiet);
                    }
                }

                string thongtin = "Thí sinh: " + MainWin.listThiSinh[rowindex].tenThiSinh + " - " + MainWin.listBoTest[colindex - 2].tenBoTest + " - " + bailam.ketQua.Status.ToString();
                Window frm = new frm_ChiTietChamBai(listChiTietChamBai, thongtin);
                frm.ShowDialog();


            }
            catch
            {

            }



        }


    }

}