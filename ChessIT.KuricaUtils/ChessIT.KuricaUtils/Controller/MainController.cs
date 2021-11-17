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

        private int m_UltimaProposta = 0;

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

                    if (m_ResultadoAprovacaoList.Count() > 0)
                        new View.ResultadoView(form, m_ResultadoAprovacaoList);
                    else
                       new View.ResultadoView(form, m_ResultadoPropostaModel);
                }

                if (pVal.FormTypeEx == "FrmPropostaContrato")
                {
                    Form form = Application.Forms.Item(pVal.FormUID);

                    Model.PropostaModel propostaModel = new Model.PropostaModel();
                    propostaModel.Proposta = m_UltimaProposta;

                    new View.PropostaContratoView(form, propostaModel);
                }

                //proposta (cotação de vendas)
                if (pVal.FormTypeEx == "149")
                {
                    Form form = Application.Forms.Item(pVal.FormUID);

                    form.Freeze(true);
                    try
                    {
                        Item _buttonGerarContrato = form.Items.Add("BTCTR", BoFormItemTypes.it_BUTTON);
                        _buttonGerarContrato.Top = form.Items.Item("10000329").Top - 20;
                        _buttonGerarContrato.Left = form.Items.Item("10000329").Left;
                        _buttonGerarContrato.Width = 90;                        

                        Button buttonGerarContrato = (Button)_buttonGerarContrato.Specific;
                        buttonGerarContrato.Caption = "Gerar Contrato";
                    }
                    finally
                    {
                        form.Freeze(false);
                    }
                }
            }
            else if (pVal.EventType == BoEventTypes.et_CLICK && !pVal.BeforeAction)
            {
                //proposta (cotação de vendas)
                if (pVal.FormTypeEx == "149")
                {
                    if (pVal.ItemUID == "BTCTR")
                    {
                        try
                        {
                            string srfPath = System.Environment.CurrentDirectory + "\\SrfFiles\\FrmPropostaContrato.srf";

                            if (File.Exists(srfPath) == false)
                            {
                                throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                            }

                            string xml = File.ReadAllText(srfPath);

                            string aprovacaoUID = GerarFormUID("FrmPropostaContrato");

                            xml = xml.Replace("uid=\"FrmPropostaContrato\"", string.Format("uid=\"{0}\"", aprovacaoUID));

#if DEBUG
                        xml = xml.Replace("from dummy", "");
#endif
                            Form form = Application.Forms.Item(pVal.FormUID);

                            m_UltimaProposta = Convert.ToInt32(form.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0));

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

        public static List<Model.AprovacaoModel> m_ResultadoAprovacaoList = new List<Model.AprovacaoModel>();

        public static Model.PropostaModel m_ResultadoPropostaModel = new Model.PropostaModel();

        public static void OpenResultadoView(List<Model.AprovacaoModel> aprovacaoList)
        {
            try
            {
                m_ResultadoAprovacaoList = aprovacaoList;
                m_ResultadoPropostaModel = new Model.PropostaModel();

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

        public static void OpenResultadoView(Model.PropostaModel propostaModel)
        {
            try
            {
                m_ResultadoAprovacaoList = new List<Model.AprovacaoModel>();
                m_ResultadoPropostaModel = propostaModel;

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

