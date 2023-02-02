using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Diagnostics;

namespace ArquivoBalancaAPI.Controller
{
    public class ArquivoController : ApiController
    {
        private static int ExecuteCommand(string command, int timeout)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = "C:\\",
            };

            var process = Process.Start(processInfo);
            process.WaitForExit(timeout);
            var exitCode = process.ExitCode;
            process.Close();
            return exitCode;
        }

        [HttpGet]
        public string Get(string caminho)
        {
            if (caminho == string.Empty)
                caminho = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Peso.txt");

            string conteudo = "";

            try
            {
                conteudo = System.IO.File.ReadAllText(caminho);

                //var caminhoOrigem = caminho;
                //var caminhoDestino = Path.GetTempPath();

                //int retorno = 0;

                //var command = "NET USE " + caminhoOrigem + " /delete";

                //retorno = ExecuteCommand(command, 5000);



                //command = "NET USE " + caminhoOrigem + " /user:" + "user" + " " + "12345";

                //try
                //{
                //    retorno = ExecuteCommand(command, 5000);

                //    if (retorno != 0)
                //        throw new Exception("Falha na comunicaçao com a balança capital: [" + command + "]");
                //}
                //catch
                //{
                //    throw new Exception("Falha na comunicaçao com a balança capital: [" + command + "]");
                //}

                //command = " copy \"" + caminho + "\"  \"" + Path.Combine(caminhoDestino, "Peso.txt") + "\" /Y";

                //retorno = ExecuteCommand(command, 5000);

                //try
                //{
                //    retorno = ExecuteCommand(command, 5000);

                //    if (retorno != 0)
                //        throw new Exception("Falha na comunicaçao com a balança capital: [" + command + "]");
                //}
                //catch
                //{
                //    throw new Exception("Falha na comunicaçao com a balança capital: [" + command + "]");
                //}

                //command = "NET USE " + caminhoOrigem + " /delete";
                //retorno = ExecuteCommand(command, 5000);

                //using (FileStream fs = File.Open(Path.Combine(caminhoDestino, "Peso.txt"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                //using (BufferedStream bs = new BufferedStream(fs))
                //using (StreamReader sr = new StreamReader(bs))
                //{
                //    conteudo = sr.ReadToEnd();
                //}
            }
            catch (Exception ex)
            {
                //conteudo = "ARQUIVO NÃO ENCONTRADO";
                conteudo = caminho + " " + ex.Message;
            }

            return conteudo;
        }
    }
}
