using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSdk
{
    public class LoggerManager
    {
        private static LoggerManager instance;

        private static Logger logger;

        public static LoggerManager Instance
        {
            get
            {
                if (instance == null)
                    instance =new LoggerManager();
                return instance;
            }
        }
        

        public ILogger GetLogger(string dir)
        {
            if (logger == null)
                logger= new Logger(dir);
            else
            {
                if(logger.dir!= dir)
                    logger = new Logger(dir);
            }
            return logger;

        }
    }
}
