using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceLogs
{
    public interface ILogger
    {
        void Info(object message);
        void Warn(object message);
        void Error(object message);
        void Fatal(object message);

    }
}
