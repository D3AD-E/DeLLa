using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeLLaGUI.Enums
{
    [Flags]
    enum ThreadCreationType
    {
        CreateSuspended = 0x1,
        SkipThreadAttach = 0x2,
        HideFromDebugger = 0x4
    }
}
