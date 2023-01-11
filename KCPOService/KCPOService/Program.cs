using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KCPOService
{
    static class Program
    {
        public static ISnmpMessage reply;
        public static ReportMessage report;
        public static DateTime lastCheckTime;
        public static DateTime lastEtalonTime;
        public static string Path = /*KCPOPATH*/\ServiceDate.txt";
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new KCPOService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
