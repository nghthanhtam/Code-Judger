using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executors
{
    /// <summary>
    /// Kết quả trả về của việc thực thi exe
    /// Bao gồm:
    /// - dung lượng ram sử dụng
    /// - output
    /// - time xử lí
    /// - Trạng thái: TLE, RE, OK??
    /// ....
    /// </summary>
    public class ResultProcessExecutor
    {
        public ResultProcessExecutor()
        {
            this.Output = string.Empty;
            this.Error = string.Empty;
            this.ExitCode = 0;
            this.TrangThai = StatusProcessExecutor.OK;
            this.TimeWorked = default(TimeSpan);
            this.MemoryUsed = 0;
        }
        public string Output { get; set; }

        public string Error { get; set; }

        public int ExitCode { get; set; }

        public StatusProcessExecutor TrangThai { get; set; }

        public TimeSpan TimeWorked { get; set; }
        
        public long MemoryUsed { get; set; }

        public TimeSpan PrivilegedProcessorTime { get; set; }

        public TimeSpan UserProcessorTime { get; set; }

        public TimeSpan TotalProcessorTime => this.PrivilegedProcessorTime + this.UserProcessorTime;
    }
}
