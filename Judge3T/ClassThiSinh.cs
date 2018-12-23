using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Judge3T
{
    class ClassThiSinh
    {
        /// <summary>
        /// Tên thí sinh
        /// </summary>
        public string tenThiSinh { get; set; }
        /// <summary>
        /// Đường dẫn thư mục bài làm của thí sinh, Ex: ...Code/ThiSinh1
        /// </summary>
        public string dirThiSinh { get; set; }

        public List<ClassBaiLam> listBaiLam = new List<ClassBaiLam>();

        public ClassThiSinh(string ten, string dir)
        {
            tenThiSinh = ten;
            dirThiSinh = dir; 

            Dictionary<String, int> listTenBaiLamCuaThiSinh = new Dictionary<string, int>(); 
            string[] arrBaiLam = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly); // Lấy list đường dẫn file bài làm của thư mục dir 

              
            for (int i = 0; i < arrBaiLam.Count(); i++)
            {

                // Chuẩn hóa tên các bài làm thành in hoa
                if (System.IO.Path.GetFileName(arrBaiLam[i]).ToUpper() != System.IO.Path.GetFileName(arrBaiLam[i]))
                {
                    string BaiLam_Temp = System.IO.Path.GetDirectoryName(arrBaiLam[i]) + "\\" + System.IO.Path.GetFileName(arrBaiLam[i]) + "_temp";
                    string BaiLam_Chuan = System.IO.Path.GetDirectoryName(arrBaiLam[i]) + "\\" + System.IO.Path.GetFileName(arrBaiLam[i]).ToUpper();
                    File.Move(arrBaiLam[i], BaiLam_Temp);
                    File.Move(BaiLam_Temp, BaiLam_Chuan);
                    arrBaiLam[i] = BaiLam_Chuan;
                }
                // Chuẩn hóa tên các bài làm thành in hoa


                String tenBaiLam = Path.GetFileNameWithoutExtension(arrBaiLam[i]); // lấy tên bài làm, không chứa phần mở rộng
                if (!listTenBaiLamCuaThiSinh.ContainsKey(tenBaiLam)) // kiểm tra "tên bài làm, không chứa phần mở rộng" đã tồn tại trong Dictionary chưa?
                {
                    listTenBaiLamCuaThiSinh.Add(tenBaiLam, listBaiLam.Count()); // chưa có trong list thì thêm vào Khóa "Tên bài làm" có giá trị là "vị trí của bài làm" trong "ListbaiLam"
                    listBaiLam.Add(new ClassBaiLam(tenBaiLam, arrBaiLam[i])); 
                }
                else // khi trùng thì thông báo, và hủy bài làm
                {
                    if (listTenBaiLamCuaThiSinh[tenBaiLam] != -1) // tránh trường hợp nộp 1 bài nhiều hơn 2.
                    {
                        System.Windows.Forms.MessageBox.Show("Thí sinh " + ten + " đã nộp nhiều bài " + tenBaiLam + " cùng 1 lúc.\nBài làm của thí sinh này sẽ được hủy.");
                        listBaiLam.RemoveAt(listTenBaiLamCuaThiSinh[tenBaiLam]); //hủy khỏi listbailam bài đã được thêm trước đó.
                        listTenBaiLamCuaThiSinh[tenBaiLam] = -1; // đánh dấu lại, rằng bài này đã được xóa khỏi list. Tránh trường hợp nó bị xóa rồi, nhưng sau đó lại có bài như vậy nữa (ngôn ngữ khác)
                    }
                }
            } 
        }
         
         
    }
}
