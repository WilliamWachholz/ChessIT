using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;

namespace ArquivoBalancaAPI
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        static void Main()
        {
            try
            {
                if (ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "ARQUIVOBALANCA") == null)
                {
                    Install();
                }
                else
                {
                    bool uninstall = Environment.GetCommandLineArgs().Count() > 1 && Environment.GetCommandLineArgs().GetValue(1).ToString() == "uninstall";

                    if (uninstall)
                    {
                        Uninstall();
                    }
                    else
                    {
                        EventLog.WriteEntry("ARQUIVOBALANCA", "Iniciado", EventLogEntryType.Information);
                        ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new Service1()
                        };

                        ServiceBase.Run(ServicesToRun);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ARQUIVOBALANCA", "General Failure: " + ex.Message, System.Diagnostics.EventLogEntryType.Information);
            }

        }


        static void Install()
        {
            IntegratedServiceInstaller integratedServiceInstaller = new IntegratedServiceInstaller();
            integratedServiceInstaller.Install("ARQUIVOBALANCA", "ARQUIVOBALANCA", "API Envio Arquivo Balanca",
                System.ServiceProcess.ServiceAccount.LocalSystem,
                System.ServiceProcess.ServiceStartMode.Automatic);
        }

        static void Uninstall()
        {
            IntegratedServiceInstaller integratedServiceInstaller = new IntegratedServiceInstaller();
            integratedServiceInstaller.Uninstall("ARQUIVOBALANCA");
        }

        class IntegratedServiceInstaller
        {
            public void Install(String ServiceName, String DisplayName, String Description,
                System.ServiceProcess.ServiceAccount Account,
                System.ServiceProcess.ServiceStartMode StartMode)
            {
                System.ServiceProcess.ServiceProcessInstaller ProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
                ProcessInstaller.Account = Account;

                System.ServiceProcess.ServiceInstaller SINST = new System.ServiceProcess.ServiceInstaller();

                System.Configuration.Install.InstallContext Context = new System.Configuration.Install.InstallContext();
                string processPath = Process.GetCurrentProcess().MainModule.FileName;
                if (processPath != null && processPath.Length > 0)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(processPath);

                    String path = String.Format("/assemblypath={0}", fi.FullName);
                    String[] cmdline = { path };
                    Context = new System.Configuration.Install.InstallContext("", cmdline);
                }

                SINST.Context = Context;
                SINST.DisplayName = DisplayName;
                SINST.Description = Description;
                SINST.ServiceName = ServiceName;
                SINST.StartType = StartMode;
                SINST.Parent = ProcessInstaller;

                System.Collections.Specialized.ListDictionary state = new System.Collections.Specialized.ListDictionary();
                SINST.Install(state);

                using (RegistryKey oKey = Registry.LocalMachine.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}", SINST.ServiceName), true))
                {
                    try
                    {
                        Object sValue = oKey.GetValue("ImagePath");
                        oKey.SetValue("ImagePath", sValue);
                    }
                    catch (Exception Ex)
                    {

                    }
                }

            }

            public void Uninstall(String ServiceName)
            {
                System.ServiceProcess.ServiceInstaller SINST = new System.ServiceProcess.ServiceInstaller();

                System.Configuration.Install.InstallContext Context = new System.Configuration.Install.InstallContext("c:\\install.log", null);
                SINST.Context = Context;
                SINST.ServiceName = ServiceName;
                SINST.Uninstall(null);
            }
        }
    }
}
