using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SAPbouiCOM;

namespace ChessIT.KuricaUtils.Controller
{
    class MainController
    {
        public static SAPbouiCOM.Application Application;
        public static SAPbobsCOM.Company Company;

        public MainController()
           : base()
        {
            try
            {
                SboGuiApi SboGuiApi = null;
                string connectionString = null;

                SboGuiApi = new SboGuiApi();

                connectionString = Convert.ToString(Environment.GetCommandLineArgs().GetValue(1));

                SboGuiApi.Connect(connectionString);

                Application = SboGuiApi.GetApplication(-1);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show("Erro na conexão com o SAP Business One: " + exception.Message);
                Environment.Exit(-1);
            }

            try
            {
                Company = (SAPbobsCOM.Company)Application.Company.GetDICompany();

                if (Company == null || !Company.Connected)
                {
                    int CodErro = 0;
                    string MsgErro = "";

                    Company.GetLastError(out CodErro, out MsgErro);
                    throw new Exception(CodErro.ToString() + " " + MsgErro);
                }

            }
            catch (Exception exception)
            {
                Application.StatusBar.SetText("Erro na conexão da DI: " + exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                Environment.Exit(-1);
            }

            Form sboForm = Application.Forms.GetFormByTypeAndCount(169, 1);

            sboForm.Freeze(true);
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.Environment.CurrentDirectory + "\\Menu.xml");

                string xml = xmlDoc.InnerXml.ToString();
                Application.LoadBatchActions(ref xml);
            }
            catch (Exception exception)
            {
                Application.StatusBar.SetText("Erro ao criar menu: " + exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                Environment.Exit(-1);
            }
            finally
            {
                sboForm.Freeze(false);
                sboForm.Update();
            }

            Application.AppEvent += HandleAppEvent;
            Application.MenuEvent += HandleMenuEvent;
            Application.ItemEvent += HandleFormLoadEvent;
        }

        private void HandleAppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                    break;
            }
        }

        private void HandleMenuEvent(ref MenuEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (pVal.MenuUID.Equals("FrmAprovacao") && !pVal.BeforeAction)
            {
                try
                {
                    string srfPath = System.Environment.CurrentDirectory + "\\SrfFiles\\FrmAprovacao.srf";

                    if (File.Exists(srfPath) == false)
                    {
                        throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                    }

                    string xml = File.ReadAllText(srfPath);

                    string formUID = GerarFormUID("FrmAprovacao");

                    xml = xml.Replace("uid=\"FrmAprovacao\"", string.Format("uid=\"{0}\"", formUID));

#if DEBUG
                    xml = xml.Replace("from dummy", "");
#endif

                    Application.LoadBatchActions(ref xml);
                }
                catch (Exception exception)
                {
                    Application.StatusBar.SetText(exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

        }

        private void HandleFormLoadEvent(string formUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (pVal.EventType == BoEventTypes.et_FORM_LOAD && pVal.BeforeAction)
            {
                if (pVal.FormTypeEx == "FrmAprovacao")
                {
                    Form form = Application.Forms.Item(pVal.FormUID);

                    new View.AprovacaoView(form);
                }

                if (pVal.FormTypeEx == "FrmResultado")
                {
                    Form form = Application.Forms.Item(pVal.FormUID);

                    new View.ResultadoView(form, m_ResultadoList);
                }
            }

            if (pVal.EventType == BoEventTypes.et_FORM_RESIZE)
            {
                //load do formulário aprovação de documento
                if (pVal.FormTypeEx == "5013")
                {
                    string query = string.Format(@" select count(*)
                                                    from OUSR 
                                                    where (OUSR.""USER_CODE"" = 'manager'
                                                    or exists(select * from USR3 where USR3.""UserLink"" = OUSR.""USERID"" and USR3.""PermId"" = 'FrmAprovacao'))
                                                    and OUSR.""USER_CODE"" = '{0}'", Company.UserName);

                    SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    recordSet.DoQuery(query);

                    bool aprovacaoCustomizada = Convert.ToInt32(recordSet.Fields.Item(0).Value) > 0;

                    aprovacaoCustomizada = false;

                    if (aprovacaoCustomizada)
                    {
                        Application.Forms.Item(pVal.FormUID).Close();

                        try
                        {
                            string srfPath = System.Environment.CurrentDirectory + "\\SrfFiles\\FrmAprovacao.srf";

                            if (File.Exists(srfPath) == false)
                            {
                                throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                            }

                            string xml = File.ReadAllText(srfPath);

                            string aprovacaoUID = GerarFormUID("FrmAprovacao");

                            xml = xml.Replace("uid=\"FrmAprovacao\"", string.Format("uid=\"{0}\"", aprovacaoUID));

#if DEBUG
                        xml = xml.Replace("from dummy", "");
#endif

                            Application.LoadBatchActions(ref xml);
                        }
                        catch (Exception exception)
                        {
                            Application.StatusBar.SetText(exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        }
                    }
                }
            }
        }

        private static string GerarFormUID(string formType)
        {
            string result = string.Empty;

            int count = 0;

            bool next = true;

            while (next)
            {
                count++;

                try
                {
                    Application.Forms.GetForm(formType, count);
                }
                catch
                {
                    next = false;
                }
            }

            result = string.Format("Frm{0}-{1}", count, new Random().Next(999));

            return result;
        }

        public static List<Model.AprovacaoModel> m_ResultadoList = new List<Model.AprovacaoModel>();

        public static void OpenResultadoView(List<Model.AprovacaoModel> aprovacaoList)
        {
            try
            {
                m_ResultadoList = aprovacaoList;

                string srfPath = System.Environment.CurrentDirectory + "\\SrfFiles\\FrmResultado.srf";

                if (File.Exists(srfPath) == false)
                {
                    throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                }

                string xml = File.ReadAllText(srfPath);

                string formUID = GerarFormUID("FrmResultado");

                xml = xml.Replace("uid=\"FrmResultado\"", string.Format("uid=\"{0}\"", formUID));

#if DEBUG
                    xml = xml.Replace("from dummy", "");
#endif

                Application.LoadBatchActions(ref xml);
            }
            catch (Exception exception)
            {
                Application.StatusBar.SetText(exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }
    }
}

