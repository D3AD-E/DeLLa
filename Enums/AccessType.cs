using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeLLaGUI.Enums
{
    [Flags]
    enum AccessType
    {
        SpecificRightsAll = 0xFFFF,
        StandardRightsAll = 0x1F0000
    }
}
