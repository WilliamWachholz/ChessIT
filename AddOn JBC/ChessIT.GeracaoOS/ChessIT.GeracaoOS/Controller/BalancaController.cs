using ChessIT.GeracaoOS.Helper;
using ChessIT.GeracaoOS.Model;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace ChessIT.GeracaoOS.Controller
{
    class BalancaController
    {
        public Balanca OBalanca;
        private Form pForm;
        public BalancaController(Form pForm)
        {
            this.OBalanca = CarregaBalanca();
            this.pForm = pForm;
            CarregaArquivo();
        }

        private void CarregaArquivo()
        {
            
            string caminho = System.IO.Path.Combine(this.OBalanca.diretorio, this.OBalanca.nomeArquivo);

            this.OBalanca.iTentativas = 50;
            //int iTentativas = this.OBalanca.iTentativas ;

            //fazer algumas tentativas de encontrar a linha...
            for (int i =1; i <= this.OBalanca.iTentativas; i++)
            {
                bool linhaEncontrada = false;
                

                string hora = DateTime.Now.ToString("HH:mm:ss");
                string arquivo = this.GetArquivo(this.OBalanca.ip, this.OBalanca.porta, caminho);

                if (arquivo == "ARQUIVO NÃO ENCONTRADO")
                {
                    LogHelper.InfoError(arquivo);
                    //throw new Exception("Falha na comunicaçao com a balança capital: arquivo não encontrado");
                }
                else if (arquivo.Contains( "O caminho da rede não foi encontrado"))
                {
                    LogHelper.InfoError(arquivo);
                }
                else
                {
                    //LogHelper.InfoWarning(string.Format("Processando arquivo tentativa {0}/{1} para o horário {2}", i, this.OBalanca.iTentativas, hora));

                    StringBuilder sb = new StringBuilder(arquivo);                   

                    string[] lines = arquivo.Split(
                        new[] { @"\r\n\" },
                        StringSplitOptions.RemoveEmptyEntries
                    );

                    foreach (string line in lines)
                    {
                        string clearedLine = line.Replace("\\", "").Replace(" ", "").Replace("\"", "").Trim();

                        //if (clearedLine.Substring(6, 8) == hora)
                        //{
                            linhaEncontrada = true;

                            OBalanca.peso = clearedLine.Substring(0, 6);
                            LogHelper.MostraBalanca(OBalanca.peso, hora, this.pForm);

                            if (OBalanca.peso.Equals(string.Empty))
                                LogHelper.InfoError("Não há peso tara definido para OS em questão");                     

                            break;

                        //}
                        //else
                        //{
                        //    OBalanca.pesoHora = clearedLine.Substring(0, 6);
                        //    LogHelper.MostraBalanca(OBalanca.pesoHora, hora, this.pForm);
                        //}
                    }
                    if (!linhaEncontrada)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        LogHelper.InfoSuccess(string.Format("Leitura na Balança realizada com sucesso!!"));
                        break;
                    }
                }

                if (!linhaEncontrada)
                    LogHelper.InfoError("Não há peso registrado nas linhas do arquivo " );
            }

        }

        private string GetArquivo(string ip, string porta, string caminho,int iTentativas=0)
        {

            string result = string.Empty;
            try
            {
                Task<string> t = HttpGetResponse(ip, porta, caminho);
                t.Wait();
                result = t.Result;
            }
            //catch (AggregateException ex)
            //{
            //    throw ex.InnerExceptions.First().InnerException;
            //}
            catch (Exception ex)
            {
                LogHelper.InfoError(ex.Message);
            }

            return result;
        }

        private async Task<string> HttpGetResponse(string ip, string porta, string caminho)
        {
            string url = string.Format("http://{0}:{1}/arquivo?caminho={2}", ip, porta, caminho.Replace(@"\", "%5C"));

            WebRequest request = WebRequest.Create(url);

            string responseData;
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }


            return responseData;
        }

        private Balanca CarregaBalanca()
        {
            Balanca OBalanca= new Balanca();
            SAPbobsCOM.Recordset recordSet = null;
            try
            {
                string query = @"SELECT * FROM ""@INTCFG"" ";

                recordSet = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recordSet.DoQuery(query);

                if (!recordSet.EoF)
                {
                    OBalanca.ip = recordSet.Fields.Item("U_IP").Value.ToString();
                    OBalanca.porta = recordSet.Fields.Item("U_Porta").Value.ToString();
                    OBalanca.diretorio = recordSet.Fields.Item("U_Diretorio").Value.ToString();
                    OBalanca.nomeArquivo = recordSet.Fields.Item("U_NomeArquivo").Value.ToString();
                }
                else
                {
                    LogHelper.InfoError("Configuracoes não encontradas!");
                }
            }
            catch (Exception ex)
            {
                LogHelper.InfoError("Erro ao buscar dados de conexão: " + ex.Message);
            }
            finally
            {
                LimparObjeto(recordSet);
            }

            return OBalanca;
        }


        public static void LimparObjeto(Object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);

            }
            catch { }
            try
            {
                obj = null;
            }
            catch { }
            GC.Collect();
            GC.WaitForFullGCComplete();

        }
    }
}
