using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executors
{
    public enum StatusProcessExecutor
    {
        OK = 0,
        TLE = 1, // Time limit error
        MemoryLimit = 2, // quá ram
        RE = 3, // Runtime error
    }
}
