using Account.Management.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Infrastructure
{
    public class ApplicationTime : IApplicationTime
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public DateTime GetUtcNowTime()
        {
            return DateTime.UtcNow;
        }
    }
}
