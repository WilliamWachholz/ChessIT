using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using System.Diagnostics;

namespace JBC.Coletor.View
{
    class CnfIntegraBalancaView
    {
        Form Form;

        bool Loaded;

        public CnfIntegraBalancaView(Form form)
        {
            this.Form = form;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);

            Program.oApplicationS.ItemEvent += HandleItemEvent;
        }

        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string name, string domain, string pass, int logType, int logpv, out IntPtr pht);

        private void HandleItemEvent(string formUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                if (pVal.FormUID == Form.UniqueID)
                {
                    switch (pVal.EventType)
                    {
                        case BoEventTypes.et_CLICK:
                            {
                                if (pVal.BeforeAction)
                                {
                                    if (pVal.ItemUID == "btSalvar" && Form.Items.Item("btSalvar").Enabled)
                                    {
                                        SAPbobsCOM.Recordset recordSet = null;
                                        try
                                        {
                                            string usuario = ((EditText)Form.Items.Item("etUsuario").Specific).String;
                                            string senha = ((EditText)Form.Items.Item("etSenha").Specific).String;
                                            string ip = ((EditText)Form.Items.Item("etIPConex").Specific).String;
                                            string porta = ((EditText)Form.Items.Item("etPorta").Specific).String;
                                            string diretorio = ((EditText)Form.Items.Item("etDiret").Specific).String;
                                            string nomeArquivo = ((EditText)Form.Items.Item("etNomeArq").Specific).String;

                                            string query = string.Format(@"UPSERT ""@INTCFG"" (
                                                                ""Code"",
                                                                ""Name"",
                                                                ""U_IP"",
                                                                ""U_Porta"",
                                                                ""U_Usuario"",
                                                                ""U_Senha"",
                                                                ""U_Diretorio"",
                                                                ""U_NomeArquivo""
                                                            ) 
                                                            VALUES (
                                                                '1',
                                                                '1',
                                                                '{0}',
                                                                '{1}',
                                                                '{2}',
                                                                '{3}',
                                                                '{4}',
                                                                '{5}'                                                            
                                                            )", ip, porta, usuario, senha, diretorio, nomeArquivo);

                                            recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                            recordSet.DoQuery(query);

                                            Program.oApplicationS.StatusBar.SetText("Operação completada com êxito", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);

                                            Form.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            Program.oApplicationS.StatusBar.SetText("Erro ao salvar: " + ex.Message);
                                        }
                                        finally
                                        {
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                                            GC.Collect();
                                        }
                                    }
                                    else if (pVal.ItemUID == "btTestar")
                                    {
                                        bool sucesso = false;

                                        //string usuario = ((EditText) Form.Items.Item("etUsuario").Specific).String;
                                        //string senha = ((EditText)Form.Items.Item("etSenha").Specific).String;
                                        string ip = ((EditText)Form.Items.Item("etIPConex").Specific).String;
                                        string porta = ((EditText)Form.Items.Item("etPorta").Specific).String;
                                        string diretorio = ((EditText)Form.Items.Item("etDiret").Specific).String;
                                        string nomeArquivo = ((EditText)Form.Items.Item("etNomeArq").Specific).String;

                                        //if (ip.Equals(string.Empty) || usuario.Equals(string.Empty) || senha.Equals(string.Empty) || diretorio.Equals(string.Empty) || nomeArquivo.Equals(string.Empty))
                                        if (ip.Equals(string.Empty) || diretorio.Equals(string.Empty) || nomeArquivo.Equals(string.Empty))
                                        {
                                            Program.oApplicationS.StatusBar.SetText("Preencher dados para conexão");
                                        }
                                        else
                                        {
                                            try
                                            {
                                                Program.oApplicationS.SendKeys("{TAB}");

                                                //string maquina = ip + (porta == "" ? "" : ":" + porta);

                                                //var caminhoOrigem = @"\\" + Path.Combine(ip + (porta == "" ? "" : ":" + porta), diretorio);
                                                //var caminhoDestino = Path.GetTempPath();

                                                //int retorno = 0;

                                                //var command = "NET USE " + caminhoOrigem + " /delete";

                                                //retorno = ExecuteCommand(command, 5000);

                                                //command = "NET USE " + caminhoOrigem + " /user:" + usuario + " " + senha;

                                                //retorno = ExecuteCommand(command, 5000);

                                                //command = " copy \"" + Path.Combine(caminhoOrigem, nomeArquivo) + "\"  \"" + Path.Combine(caminhoDestino, nomeArquivo) + "\" /Y";

                                                //retorno = ExecuteCommand(command, 5000);

                                                //sucesso = retorno == 0;

                                                //command = "NET USE " + caminhoOrigem + " /delete";

                                                //retorno = ExecuteCommand(command, 5000);

                                                string caminho = System.IO.Path.Combine(diretorio, nomeArquivo);

                                                string arquivo = Helper.ApiHelper.GetArquivo(ip, porta, caminho);

                                                sucesso = arquivo != "ARQUIVO NÃO ENCONTRADO";

                                                Form.Items.Item("btSalvar").Enabled = sucesso;

                                                if (sucesso)
                                                {
                                                    Program.oApplicationS.StatusBar.SetText("Conexão estabelecida com êxito", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
                                                }
                                                else
                                                {
                                                    Program.oApplicationS.StatusBar.SetText("Não foi possível estabelecer conexão");
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                                Program.oApplicationS.StatusBar.SetText("Erro ao testar conexão: " + ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                            break;                        
                        case BoEventTypes.et_VALIDATE:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemChanged)
                                {
                                    Form.Items.Item("btSalvar").Enabled = false;
                                }
                            }

                            break;
                        
                        case BoEventTypes.et_FORM_CLOSE:
                            {
                                if (pVal.BeforeAction)
                                {
                                }
                                else
                                {
                                    Program.oApplicationS.ItemEvent -= HandleItemEvent;
                                }
                            }
                            break;
                        case BoEventTypes.et_GOT_FOCUS:
                            if (pVal.BeforeAction)
                            {

                            }
                            else
                            {
                                if (!Loaded)
                                {
                                    Loaded = true;

                                    try
                                    {
                                        SAPbobsCOM.Recordset recordSet = null;
                                        try
                                        {
                                            string query = @"SELECT * FROM ""@INTCFG"" ";

                                            recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                            recordSet.DoQuery(query);
                                            
                                            if (!recordSet.EoF)
                                            {
                                                ((EditText)Form.Items.Item("etUsuario").Specific).String = recordSet.Fields.Item("U_Usuario").Value.ToString();
                                                ((EditText)Form.Items.Item("etSenha").Specific).String = recordSet.Fields.Item("U_Senha").Value.ToString();
                                                ((EditText)Form.Items.Item("etIPConex").Specific).String = recordSet.Fields.Item("U_IP").Value.ToString();
                                                ((EditText)Form.Items.Item("etPorta").Specific).String = recordSet.Fields.Item("U_Porta").Value.ToString();
                                                ((EditText)Form.Items.Item("etDiret").Specific).String = recordSet.Fields.Item("U_Diretorio").Value.ToString();
                                                ((EditText)Form.Items.Item("etNomeArq").Specific).String = recordSet.Fields.Item("U_NomeArquivo").Value.ToString();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception("Erro ao buscar dados de conexão: " + ex.Message);
                                        }
                                        finally
                                        {
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                                            GC.Collect();
                                        }
                                    }
                                    catch { }
                                }
                            }
                            break;
                        case BoEventTypes.et_FORM_RESIZE:
                            if (!pVal.BeforeAction)
                            {
                                if (Loaded)
                                    Form.Items.Item("33").Height = 1;
                            }
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Program.oApplicationS.StatusBar.SetText(exception.Message);
            }
        }

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
    }
}
