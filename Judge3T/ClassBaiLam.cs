using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Judge3T
{
    class ClassBaiLam
    {
        /// <summary>
        /// Tên bài làm, không chứa phần mở rộng, Ex: ADD
        /// </summary>
        public string tenBaiLam { get; set; }
        /// <summary>
        /// Đường dẫn file code bài làm, Ex: .../Code/THISINH1/ADD.C
        /// </summary>
        public string dirBaiLam { get; set; }
        public KetQuaChamBai ketQua { get; set; }
         

        public ClassBaiLam()
        {

        }

        /// <summary>
        /// Constructor Tên bài làm, đường dẫn code
        /// </summary>
        /// <param name="ten">Tên bài làm</param>
        /// <param name="dir">đường dẫn code</param>
        public ClassBaiLam(string ten, string dir)
        {
            tenBaiLam = ten;
            dirBaiLam = dir;
        }
        
         

       
    }
}
