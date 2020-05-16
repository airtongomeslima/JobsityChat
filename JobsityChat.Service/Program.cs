using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Service
{
    static class Program
    {
        static void Main()
        {
            System.IO.File.WriteAllText(@"C:\JobsityChat\servicebegin.txt", "step1");
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
