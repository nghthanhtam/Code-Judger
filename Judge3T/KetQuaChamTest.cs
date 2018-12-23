using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Judge3T
{
    public class KetQuaChamTest
    {
        public KetQuaChamTest(string nameTest)
        {
            NameTest = nameTest;
        }

        public KetQuaChamTest(string nameTest, StatusResult status, TimeSpan timeUsed, long memoryUsed)
        {
            NameTest = nameTest;
            Status = status;
            TimeUsed = timeUsed;
            MemoryUsed = memoryUsed;
            Messages = "";
        }

        public KetQuaChamTest(string nameTest, StatusResult status, TimeSpan timeUsed, long memoryUsed, string messages) : this(nameTest, status, timeUsed, memoryUsed)
        {
            Messages = messages;
        }

        public string NameTest { get; set; }
        public StatusResult Status { get; set; }
        public TimeSpan TimeUsed { get; set; }
        public long MemoryUsed { get; set; }
        public string Messages { get; set; }


    }
}
