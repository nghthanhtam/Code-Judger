using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Judge3T
{
    public enum StatusResult
    {
        TLE = 1,
        MemoryLimit = 2,
        RE = 3, // chạy sinh lỗi
        WA = 4, //wrong answer
        AC = 5, //accept
        CE = 6, // Compiler Error
        BAN = 7, // Bị cấm bởi từ khóa

    }
}
