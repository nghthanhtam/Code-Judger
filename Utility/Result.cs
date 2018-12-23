using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Result
    {
        public bool Flag { get; }
        public string ApplicationMessage { get; set; }
        public string SystemMessage { get; set; }

        public Result()
        {
            Flag = true;
        }
        public Result(bool Fl)
        {
            this.Flag = Fl;
        }

        /// <summary>
        /// thông báo trạng thái 
        /// </summary>
        /// <param name="Fl"></param>
        /// <param name="appMessage"></param>
        public Result(bool Fl, string appMessage)
        {
            this.Flag = Fl;
            this.ApplicationMessage = appMessage;
        }

        public Result(bool Fl, string appMessage, string sysMessage)
        {
            this.Flag = Fl;
            this.SystemMessage = sysMessage;
            this.ApplicationMessage = appMessage;
        }
    }
}
