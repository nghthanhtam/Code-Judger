using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Judge3T
{
    static public class DuongDan
    {
        /// <summary>
        /// Đường dẫn thư mục Judge3T
        /// </summary>
        static public string dir { get; set; } = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); // Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%") + "\\Judge3T";

        /// <summary>
        /// đường dẫn Folder GCC
        /// </summary>
        static public string fgcc { get; set; } = dir + "\\GCC";
        static public string fpc { get; set; } = dir + "\\FPC";

        static public string randtemp()
        {
            return Path.GetTempPath() + "code_" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);

        }
    }
}
