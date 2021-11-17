using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SAPbouiCOM;

namespace ChessIT.Financeiro.Controller
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

            if (pVal.MenuUID.Equals("FrmDocCP") && !pVal.BeforeAction)
            {
                try
                {
                    string srfPath = System.Environment.CurrentDirectory + "\\SrfFiles\\FrmDocCP.srf";

                    if (File.Exists(srfPath) == false)
                    {
                        throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                    }

                    string xml = File.ReadAllText(srfPath);

                    string formUID = GerarFormUID("FrmDocCP");

                    xml = xml.Replace("uid=\"FrmDocCP\"", string.Format("uid=\"{0}\"", formUID));

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

            if (pVal.MenuUID.Equals("FrmBaixaCP") && !pVal.BeforeAction)
            {
                try
                {
                    string srfPath = System.Environment.CurrentDirectory + "\\SrfFiles\\FrmBaixaCP.srf";

                    if (File.Exists(srfPath) == false)
                    {
                        throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                    }

                    string xml = File.ReadAllText(srfPath);

                    string formUID = GerarFormUID("FrmBaixaCP");

                    xml = xml.Replace("uid=\"FrmBaixaCP\"", string.Format("uid=\"{0}\"", formUID));

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
                if (pVal.FormTypeEx == "FrmDocCP")
                {
                    Form form = Application.Forms.Item(pVal.FormUID);

                    new View.DocCPView(form);
                }

                if (pVal.FormTypeEx == "FrmBaixaCP")
                {
                    Form form = Application.Forms.Item(pVal.FormUID);

                    new View.BaixaCPView(form);
                }

                //Contas a Receber
                if (pVal.FormTypeEx == "170")
                {
                    Form form = Application.Forms.Item(pVal.FormUID);

                    form.Freeze(true);
                    try
                    {
                        form.DataSources.UserDataSources.Add("porcJuros", BoDataType.dt_PERCENT);
                        form.DataSources.UserDataSources.Add("porcMulta", BoDataType.dt_PERCENT);

                        Item _grid = form.Items.Item("20");

                        Item _porcMulta = form.Items.Add("PORCMULTA", BoFormItemTypes.it_EDIT);
                        _porcMulta.Top = _grid.Top - 20;
                        _porcMulta.Left = _grid.Left + _grid.Width - 270;
                        _porcMulta.Width = 80;

                        EditText porcMulta = (EditText)_porcMulta.Specific;
                        porcMulta.DataBind.SetBound(true, "", "porcMulta");

                        Item _staticMulta = form.Items.Add("SPORCMULTA", BoFormItemTypes.it_STATIC);
                        _staticMulta.Top = _grid.Top - 20;
                        _staticMulta.Left = _grid.Left + _grid.Width - 330;
                        _staticMulta.Width = 60;
                        _staticMulta.LinkTo = "PORCMULTA";

                        StaticText staticMulta = (StaticText)_staticMulta.Specific;
                        staticMulta.Caption = "Multa";

                        Item _porcJuros = form.Items.Add("PORCJUROS", BoFormItemTypes.it_EDIT);
                        _porcJuros.Top = _grid.Top - 20;
                        _porcJuros.Left = _grid.Left + _grid.Width - 80;
                        _porcJuros.Width = 80;

                        EditText porcJuros = (EditText)_porcJuros.Specific;
                        porcJuros.DataBind.SetBound(true, "", "porcJuros");

                        Item _staticJuros = form.Items.Add("SPORCJUROS", BoFormItemTypes.it_STATIC);
                        _staticJuros.Top = _grid.Top - 20;
                        _staticJuros.Left = _grid.Left + _grid.Width - 170;
                        _staticJuros.Width = 60;
                        _staticJuros.LinkTo = "PORCJUROS";

                        StaticText staticJuros = (StaticText) _staticJuros.Specific;
                        staticJuros.Caption = "Juros";      

                        Item _aplicarMultaJuros = form.Items.Add("APLMJ", BoFormItemTypes.it_BUTTON);
                        _aplicarMultaJuros.Top = _grid.Top - 20;
                        _aplicarMultaJuros.Left = _grid.Left + _grid.Width - 470;
                        _aplicarMultaJuros.Width = 110;
                        _aplicarMultaJuros.LinkTo = "SPORCMULTA";

                        Button aplicarMultaJuros = (Button)_aplicarMultaJuros.Specific;
                        aplicarMultaJuros.Caption = "Aplicar Multa e Juros";

                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        recordSet.DoQuery(@"select ""U_CR_Porc_Juros"", ""U_CR_Porc_Multa"" from ""@CFG_JUROS_MULTA""");

                        if (!recordSet.EoF)
                        {
                            porcJuros.String = recordSet.Fields.Item(0).Value.ToString();
                            porcMulta.String = recordSet.Fields.Item(1).Value.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Application.StatusBar.SetText("Verifique se a tabela de Configuração de Juros e Multa está criada");
                    }
                    finally
                    {
                        form.Freeze(false);
                    }
                }
            }
            else if (pVal.EventType == BoEventTypes.et_CLICK && !pVal.BeforeAction)
            {
                //Contas a Receber
                if (pVal.FormTypeEx == "170")
                {
                    if (pVal.ItemUID == "APLMJ")
                    {
                        Form form = Application.Forms.Item(pVal.FormUID);

                        form.Freeze(true);
                        try
                        {
                            Matrix _grid = (Matrix) form.Items.Item("20").Specific;

                            int row = _grid.GetNextSelectedRow();

                            if (row > 0)
                            {
                                double porcJuros = Convert.ToDouble(form.DataSources.UserDataSources.Item("porcJuros").Value);
                                double porcMulta = Convert.ToDouble(form.DataSources.UserDataSources.Item("porcMulta").Value);

                                double total = ConvertDouble(((EditText)_grid.Columns.Item("6").Cells.Item(row).Specific).String);
                                int diasAtraso = Convert.ToInt32(((EditText)_grid.Columns.Item("350000131").Cells.Item(row).Specific).String);

                                double valorMulta = total * (porcMulta / 100);
                                double valorMora = (((porcJuros / 30) / 100) * total) * diasAtraso;

                                double totalPagar = total + valorMulta + valorMora;

                                ((EditText)_grid.Columns.Item("U_PorcentMulta").Cells.Item(row).Specific).String = porcMulta.ToString();
                                ((EditText)_grid.Columns.Item("U_ValorMulta").Cells.Item(row).Specific).String = valorMulta. ToString();

                                ((EditText)_grid.Columns.Item("U_PrcJurosMora").Cells.Item(row).Specific).String = porcJuros.ToString();
                                ((EditText)_grid.Columns.Item("U_ValorDoJurosMora").Cells.Item(row).Specific).String = valorMora.ToString();

                                ((EditText)_grid.Columns.Item("U_TotalAPagar").Cells.Item(row).Specific).String = totalPagar.ToString();
                            }
                        }
                        catch  (Exception ex)
                        {
                            Application.StatusBar.SetText("Erro ao calcular juros/multa: " + ex.Message);
                        }
                        finally
                        {
                            form.Freeze(false);
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

        public static double ConvertDouble(string doubleValue)
        {
            return double.Parse((doubleValue.Contains(",") ? doubleValue.Replace(".", "").Replace(",", ".") : doubleValue).Replace("R$", "").Replace("%", ""), System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
