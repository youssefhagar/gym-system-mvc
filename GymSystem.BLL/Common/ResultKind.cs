using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public enum ResultKind
    {
        Ok = 0,
        Conflict = 1,
        NotFound = 2,
        ValidationFailed = 3
    }
}
