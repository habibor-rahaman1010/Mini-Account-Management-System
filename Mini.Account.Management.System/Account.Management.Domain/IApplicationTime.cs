using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain
{
    public interface IApplicationTime
    {
        public DateTime GetCurrentTime();
        public DateTime GetUtcNowTime();
    }
}
