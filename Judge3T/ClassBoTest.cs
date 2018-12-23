using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Xml;

namespace Judge3T
{
    class ClassBoTest
    {
        /// <summary>
        /// Tên bộ test, ex: ADD
        /// </summary>
        public string tenBoTest { get; set; }

        /// <summary>
        /// Chứa đường dẫn thư mục bộ test, Ex:.../Botest/ADD
        /// </summary>
        public string dirBoTest  { get; set; }

        public KieuChamBai kieuCham { get; set; } 

        public int timeLimit { get; set; } // miliseconds

        public int memoryLimit { get; set; } // MB

        public string tuKhoaCam { get; set; }

        public ClassBoTest(string ten, string dir)
        {
            tenBoTest = ten;
            dirBoTest = dir;

            if (File.Exists(dirBoTest+"\\config.xml"))
            {
                try
                { 
                    XmlTextReader objXmlTextReader = new XmlTextReader(dirBoTest + "\\config.xml");
                    string sName = "";
                    while (objXmlTextReader.Read())
                    {
                        switch (objXmlTextReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                sName = objXmlTextReader.Name;
                                break;
                            case XmlNodeType.Text:
                                switch (sName)
                                {
                                    case "kieuCham":
                                        this.kieuCham = (KieuChamBai)int.Parse(objXmlTextReader.Value);
                                        break;
                                    case "timeLimit":
                                        this.timeLimit = int.Parse(objXmlTextReader.Value);
                                        break;

                                    case "memoryLimit":
                                        this.memoryLimit = int.Parse(objXmlTextReader.Value);
                                        break;

                                    case "tuKhoaCam":
                                        this.tuKhoaCam = objXmlTextReader.Value;
                                        break;

                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    kieuCham = KieuChamBai.OI;
                    timeLimit = 1000;
                    tuKhoaCam = "system";
                    memoryLimit = 1024;

                    System.Windows.Forms.MessageBox.Show("Nạp config.xml thất bại!"+ex.ToString(),"Nạp cấu hình bộ test thất bại",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            else
            {
                kieuCham = KieuChamBai.OI;
                timeLimit = 1000;
                memoryLimit = 1024;

                tuKhoaCam = "system";
            }

          
        }
        public ClassBoTest()
        {
            tenBoTest = "";
            dirBoTest = "";
            kieuCham = KieuChamBai.OI;
            timeLimit = 1000;
            memoryLimit = 1024;
            tuKhoaCam = "system";
        }


        public static string thongBao = "";

        public static int dem = 0;
        public static bool kiemTraBoTest(string dirTest, int soLuongBoTest)
        {
            dem++;
            //láy danh sách các test trong 1 bài (Test1, Test2 trong ADD)
            var arrTest = Directory.GetDirectories(dirTest);
            for (int i = 0; i < arrTest.Count(); i++)
            {

                // Chuẩn hóa lại tên thư mục "từng test" thành chữ in
                if (System.IO.Path.GetFileName(arrTest[i]).ToUpper() != System.IO.Path.GetFileName(arrTest[i]))
                {
                    string dirTungTest_Temp = System.IO.Path.GetDirectoryName(arrTest[i]) + "\\" + System.IO.Path.GetFileName(arrTest[i]) + "_temp";
                    string dirTungTest_Chuan = System.IO.Path.GetDirectoryName(arrTest[i]) + "\\" + System.IO.Path.GetFileName(arrTest[i]).ToUpper();
                    Directory.Move(arrTest[i], dirTungTest_Temp);
                    Directory.Move(dirTungTest_Temp, dirTungTest_Chuan);
                    arrTest[i] = dirTungTest_Chuan;
                }
                // Chuẩn hóa lại tên thư mục "từng test" thành chữ in


                // Chuẩn hóa lại tên "các file INP, OUT" trong thư mục "từng test" thành chữ in
                if (System.IO.Path.GetFileName(arrTest[i]).ToUpper() != System.IO.Path.GetFileName(arrTest[i]))
                {
                    string dirTungTest_Temp = System.IO.Path.GetDirectoryName(arrTest[i]) + "\\" + System.IO.Path.GetFileName(arrTest[i]) + "_temp";
                    string dirTungTest_Chuan = System.IO.Path.GetDirectoryName(arrTest[i]) + "\\" + System.IO.Path.GetFileName(arrTest[i]).ToUpper();
                    Directory.Move(arrTest[i], dirTungTest_Temp);
                    Directory.Move(dirTungTest_Temp, dirTungTest_Chuan);
                    arrTest[i] = dirTungTest_Chuan;
                }

                var listDirectoryInput = Directory.GetFiles(arrTest[i], "*INP", SearchOption.TopDirectoryOnly);
                var listDirectoryOutput = Directory.GetFiles(arrTest[i], "*OUT", SearchOption.TopDirectoryOnly);

                #region Chuẩn hóa input
                for (int j = 0; j < listDirectoryInput.Count(); j++)
                {
                    if (System.IO.Path.GetFileName(listDirectoryInput[j]).ToUpper() != System.IO.Path.GetFileName(listDirectoryInput[j]))
                    {
                        string dirINP_Temp = System.IO.Path.GetDirectoryName(listDirectoryInput[j]) + "\\" + System.IO.Path.GetFileName(listDirectoryInput[j]) + "_temp";
                        string dirINP_Chuan = System.IO.Path.GetDirectoryName(listDirectoryInput[j]) + "\\" + System.IO.Path.GetFileName(listDirectoryInput[j]).ToUpper();
                        File.Move(listDirectoryInput[j], dirINP_Temp);
                        File.Move(dirINP_Temp, dirINP_Chuan);
                        listDirectoryInput[j] = dirINP_Chuan;
                    }
                }
                #endregion

                #region Chuẩn hóa output
                for (int j = 0; j < listDirectoryOutput.Count(); j++)
                {
                    if (System.IO.Path.GetFileName(listDirectoryOutput[j]).ToUpper() != System.IO.Path.GetFileName(listDirectoryOutput[j]))
                    {
                        string dirOUT_Temp = System.IO.Path.GetDirectoryName(listDirectoryOutput[j]) + "\\" + System.IO.Path.GetFileName(listDirectoryOutput[j]) + "_temp";
                        string dirOUT_Chuan = System.IO.Path.GetDirectoryName(listDirectoryOutput[j]) + "\\" + System.IO.Path.GetFileName(listDirectoryOutput[j]).ToUpper();
                        File.Move(listDirectoryOutput[j], dirOUT_Temp);
                        File.Move(dirOUT_Temp, dirOUT_Chuan);
                        listDirectoryOutput[j] = dirOUT_Chuan;
                    }
                }
                #endregion
                // Chuẩn hóa lại tên "các file INP, OUT" trong thư mục "từng test" thành chữ in


                int countINP = listDirectoryInput.Length; //đếm số inp            
                int countOUT = listDirectoryOutput.Length; //đếm số out

                if (countOUT + countINP != 2) //mỗi file test1, test2,... chỉ đc tồn tại 1 inp và 1 out
                {
                    //System.Windows.Forms.MessageBox.Show("Bộ test bài " + Path.GetFileNameWithoutExtension(dirTest) + " thiếu/dư input và output!");
                    thongBao += "Bộ test bài " + Path.GetFileNameWithoutExtension(dirTest) + " thiếu/dư input và output!\n";
                    if (dem == soLuongBoTest) System.Windows.Forms.MessageBox.Show(thongBao);
                    return false;
                }

                if (File.Exists(arrTest[i] + "\\" + Path.GetFileNameWithoutExtension(dirTest) + ".INP") == true) // kiểm tra bộ test này đã có file input chưa?
                {
                    if (File.Exists(arrTest[i] + "\\" + Path.GetFileNameWithoutExtension(dirTest) + ".OUT") == true) // kiểm tra bộ test này đã có file output chưa?
                        continue;
                    else
                    {
                        //System.Windows.Forms.MessageBox.Show("Bô test bài " + Path.GetFileNameWithoutExtension(dirTest) + " " + Path.GetFileNameWithoutExtension(arrTest[i]) + " thiếu/dư input hoặc sai tên mặc định");
                        thongBao += "Bộ test bài " + Path.GetFileNameWithoutExtension(dirTest) + " thiếu/dư input và output!\n";
                        if (dem == soLuongBoTest) System.Windows.Forms.MessageBox.Show(thongBao);
                        return false;
                    }
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show("Bô test bài " + Path.GetFileNameWithoutExtension(dirTest) + " " + Path.GetFileNameWithoutExtension(arrTest[i]) + " thiếu/dư output hoặc sai tên mặc định");
                    thongBao += "Bộ test bài " + Path.GetFileNameWithoutExtension(dirTest) + " thiếu/dư input và output!\n";
                    if (dem == soLuongBoTest) System.Windows.Forms.MessageBox.Show(thongBao);
                    return false;
                }
            }

            if (dem == soLuongBoTest && thongBao!="") System.Windows.Forms.MessageBox.Show(thongBao);
            return true;
        }

    }
}
