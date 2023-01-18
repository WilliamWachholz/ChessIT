using Castle.Core.Logging;
using Chess.IT.Services.Controller;
using Chess.IT.Services.Helper;
using Chess.IT.Services.Model;
using JBC.Framework.Attribute;
using JBC.Framework.DAO;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
//using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace Chess.IT.Services
{
    [AddIn(
        Name = "Chess.IT.Services"
        , Description = "Chess IT Services"
        , Namespace = "Chess.IT.Services"
        , InitMethod = "CheckInit"
        )]

    [ResourceBOM("Chess.IT.Services.resources.BOM.DESLOCAMENTO.xml", ResourceType.UserField)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.MOTORISTA.xml", ResourceType.UserField)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.OAT1.xml", ResourceType.UserField)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.OOAT.xml", ResourceType.UserField)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.ORDR.xml", ResourceType.UserField)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.RDR1.xml", ResourceType.UserField)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.ROTA.xml", ResourceType.UserField)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.VEICULOS.xml", ResourceType.UserField)]
    //[ResourceBOM("Chess.IT.Services.resources.BOM.UDO.xml", ResourceType.UDO)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.tabelas.xml", ResourceType.UserTable)]
    [ResourceBOM("Chess.IT.Services.resources.BOM.UDT.xml", ResourceType.UserTable)]

    [Menu(FatherUID = "CHESSIT", UniqueID = "JBCCD", String = "Certificado de Destinação", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = 1)]
    class Program
    {
        public SAPbobsCOM.Company oCompany { get; set; }
        public SAPbouiCOM.Application oApplication { get; set; }

        public static SAPbobsCOM.Company oCompanyS { get; set; }
        public static SAPbouiCOM.Application oApplicationS { get; set; }

        public BusinessOneDAO oB1DAO { get; set; }
        public ILogger Log { get; set; }

        static void Main(string[] args)
        {

        }
        private void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            //int i = 0;
            //int iAddAfter = 0;
            //string sXML = null;
            string sPath = null;

            oMenus = oApplication.Menus;

            SAPbouiCOM.MenuCreationParams oMenuCreationParams = null;
            oMenuCreationParams = (SAPbouiCOM.MenuCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams));

            oMenuItem = oApplication.Menus.Item("43520");

            sPath = Application.StartupPath;
            //sPath = sPath.Remove(sPath.Length-9,0);

            oMenuCreationParams.Type = BoMenuType.mt_POPUP;
            oMenuCreationParams.UniqueID = "CHESSIT";
            oMenuCreationParams.String = "CHESS - IT";
            oMenuCreationParams.Enabled = true;
            //oMenuCreationParams.Image = sPath + @"\\UI.bmp";
            oMenuCreationParams.Position = 0;


            oMenus = oMenuItem.SubMenus;

            try
            {
                oMenus.AddEx(oMenuCreationParams);

                oMenuItem = oApplication.Menus.Item("CHESSIT");
                oMenus = oMenuItem.SubMenus;

                oMenuCreationParams.Type = BoMenuType.mt_STRING;
                oMenuCreationParams.UniqueID = "CHESSITC";
                oMenuCreationParams.String = "Configurações";
                oMenus.AddEx(oMenuCreationParams);

                oMenuCreationParams.Type = BoMenuType.mt_STRING;
                oMenuCreationParams.UniqueID = "CHESSITG";
                oMenuCreationParams.String = "Gestão de OS";
                oMenus.AddEx(oMenuCreationParams);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public void CheckInit()
        {
            oCompanyS = oCompany;
            oApplicationS = oApplication;


            //SAPbobsCOM.Documents oOrder;
            //oOrder = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            //if (oOrder.GetByKey(1))
            //{
            //    oOrder.SaveXML(@"c:\order.xml");
            //}


                AddMenuItems();
            Form sboForm = oApplication.Forms.GetFormByTypeAndCount(169, 1);

            sboForm.Freeze(true);
            try
            {

                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "Chess.IT.Services.xml.Menu.xml";
                //string response = System.Resources.GetString(resourceName);
                using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Menu.xml"), System.IO.FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                        {
                            fileStream.WriteByte((byte)stream.ReadByte());
                        }
                        fileStream.Close();
                    }
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Menu.xml");

                string xml = xmlDoc.InnerXml.ToString();
                oApplication.LoadBatchActions(ref xml);
            }
            catch (Exception exception)
            {
                oApplication.StatusBar.SetText("Erro ao criar menu: " + exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                Environment.Exit(-1);
            }
            finally
            {
                sboForm.Freeze(false);
                sboForm.Update();
            }
            //MessageBox.Show("123");

            //ThreadStart threadDelegate = new ThreadStart(this.processo);
            //Thread newThread = new Thread(threadDelegate);
            //newThread.Start();



            oApplication.AppEvent += HandleAppEvent;
            oApplication.MenuEvent += HandleMenuEvent;
            oApplication.ItemEvent += HandleFormLoadEvent;
            oApplication.ItemEvent += HandleButtonClickEvent;
            oApplication.FormDataEvent += HandleFormDataEvent;
        }

        private void HandleFormDataEvent(ref BusinessObjectInfo pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (pVal.Type == "13" && pVal.EventType == BoEventTypes.et_FORM_DATA_ADD && !pVal.BeforeAction && pVal.ActionSuccess)
            {
                string query = @"select COALESCE(INV1.""U_OSEntry"", 0)                                       
                                from INV1
                                where ""DocEntry"" = {0}
                                and COALESCE(INV1.""U_OSEntry"", 0) > 0
                                and (select count(*) from RDR1 where RDR1.""DocEntry""= COALESCE(INV1.""U_OSEntry"", 0)) = 
                                    (
                                        select count(*) from RDR1 
                                        where RDR1.""DocEntry""= COALESCE(INV1.""U_OSEntry"", 0) 
                                        and RDR1.""Quantity"" = (   select sum(TX.""Quantity"") 
                                                                    from INV1 TX 
                                                                    inner join OINV on OINV.""DocEntry"" = TX.""DocEntry"" 
                                                                    where OINV.""CANCELED"" = 'N' 
                                                                    and TX.""U_OSEntry"" = RDR1.""DocEntry"" 
                                                                    and TX.""U_OSLine"" = RDR1.""LineNum""
                                                                )
                                    )";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pVal.ObjectKey);
                XmlNode xn = doc.SelectSingleNode("DocumentParams").SelectSingleNode("DocEntry");
                string docEntry = xn.InnerText;

                query = string.Format(query, docEntry);

                SAPbobsCOM.Recordset recordSet = null;
                try
                {
                    recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    recordSet.DoQuery(query);
                    while (!recordSet.EoF)
                    {
                        int osEntry = Convert.ToInt32(recordSet.Fields.Item(0).Value);

                        SAPbobsCOM.Documents oDocuments = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                        if (oDocuments.GetByKey(osEntry))
                        {
                            oDocuments.UserFields.Fields.Item("U_Status").Value = "F";
                            oDocuments.UserFields.Fields.Item("U_Situacao").Value = "11";

                            oDocuments.Update();

                            if (oDocuments.DocumentStatus == SAPbobsCOM.BoStatus.bost_Open)
                                oDocuments.Close();
                        }

                        recordSet.MoveNext();
                    }
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                    GC.Collect();
                }
            }
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

            if (pVal.BeforeAction == false)
            {
                if (pVal.MenuUID.Equals("CHESSITG"))
                {
                    oApplication.ActivateMenuItem("FrmGeraOS");
                }
                else if (pVal.MenuUID.Equals("CHESSITC"))
                {
                    oApplication.ActivateMenuItem("FrmCnfIntegraBalanca");
                }

            }

            if (pVal.MenuUID.Equals("FrmGeraOS") && !pVal.BeforeAction)
            {
                try
                {


                    var resourceName = "Chess.IT.Services.SrfFiles.FrmGeraOS.srf";
                    //string response = System.Resources.GetString(resourceName);
                    using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(System.Environment.CurrentDirectory, "FrmGeraOS.srf"), System.IO.FileMode.Create))
                        {
                            for (int i = 0; i < stream.Length; i++)
                            {
                                fileStream.WriteByte((byte)stream.ReadByte());
                            }
                            fileStream.Close();
                        }
                    }
                    string srfPath = System.Environment.CurrentDirectory + "\\FrmGeraOS.srf";

                    if (File.Exists(srfPath) == false)
                    {
                        throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                    }

                    string xml = File.ReadAllText(srfPath);

                    string formUID = GerarFormUID("FrmGeraOS");

                    xml = xml.Replace("uid=\"FrmGeraOS\"", string.Format("uid=\"{0}\"", formUID));

                    //#if DEBUG
                    //                    xml = xml.Replace("from dummy", "");
                    //#endif

                    oApplication.LoadBatchActions(ref xml);
                }
                catch (Exception exception)
                {
                    oApplication.StatusBar.SetText(exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }

            if (pVal.MenuUID.Equals("FrmCnfIntegraBalanca") && !pVal.BeforeAction)
            {
                try
                {
                    var resourceName = "Chess.IT.Services.SrfFiles.FrmCnfIntegraBalanca.srf";
                    //string response = System.Resources.GetString(resourceName);
                    using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(System.Environment.CurrentDirectory, "FrmCnfIntegraBalanca.srf"), System.IO.FileMode.Create))
                        {
                            for (int i = 0; i < stream.Length; i++)
                            {
                                fileStream.WriteByte((byte)stream.ReadByte());
                            }
                            fileStream.Close();
                        }
                    }

                    string srfPath = System.Environment.CurrentDirectory + "\\FrmCnfIntegraBalanca.srf";

                    if (File.Exists(srfPath) == false)
                    {
                        throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                    }

                    string xml = File.ReadAllText(srfPath);

                    string formUID = GerarFormUID("FrmCnfIntegraBalanca");

                    xml = xml.Replace("uid=\"FrmCnfIntegraBalanca\"", string.Format("uid=\"{0}\"", formUID));

#if DEBUG
                    //xml = xml.Replace("from dummy", "");
#endif

                    oApplication.LoadBatchActions(ref xml);
                }
                catch (Exception exception)
                {
                    oApplication.StatusBar.SetText(exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                }
            }
        }

        private void HandleFormLoadEvent(string formUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (pVal.EventType == BoEventTypes.et_FORM_LOAD && !pVal.BeforeAction)
            {
                if (pVal.FormTypeEx == "FrmGeraOS")
                {
                    Form form = oApplication.Forms.Item(pVal.FormUID);

                    new View.GeraOSView(form);
                }

                if (pVal.FormTypeEx == "FrmCnfIntegraBalanca")
                {
                    Form form = oApplication.Forms.Item(pVal.FormUID);

                    new View.CnfIntegraBalancaView(form);
                }

                if (pVal.FormTypeEx == "FrmNotasGeradas")
                {
                    Form form = oApplication.Forms.Item(pVal.FormUID);

                    new View.NotasGeradasView(form, m_NotasGeradas);
                }

                if (pVal.FormTypeEx == "139") //Pedido de Venda (OS)
                {
                    Form form = oApplication.Forms.Item(pVal.FormUID);

                    Item _stParPeso = form.Items.Add("stParPeso", BoFormItemTypes.it_STATIC);
                    _stParPeso.Top = form.Items.Item("1980000501").Top - 70;
                    _stParPeso.Left = form.Items.Item("2001").Left + 450;
                    _stParPeso.Width = 100;
                    _stParPeso.LinkTo = "2001";

                    StaticText stParPeso = (StaticText)_stParPeso.Specific;
                    stParPeso.Caption = "Parâmetro Pesagem";

                    Item _cbParPeso = form.Items.Add("cbParPeso", BoFormItemTypes.it_COMBO_BOX);
                    _cbParPeso.Top = _stParPeso.Top + 20;
                    _cbParPeso.Left = _stParPeso.Left;
                    _cbParPeso.Width = 100;
                    _cbParPeso.LinkTo = "2001";
                    _cbParPeso.DisplayDesc = true;

                    ComboBox cbParPeso = (ComboBox)_cbParPeso.Specific;
                    cbParPeso.ValidValues.Add("0", "Peso Bruto");
                    cbParPeso.ValidValues.Add("1", "Tara");

                    cbParPeso.Select("0");

                    Item _btCapPeso = form.Items.Add("btCapPeso", BoFormItemTypes.it_BUTTON);
                    _btCapPeso.Top = _cbParPeso.Top + 20;
                    _btCapPeso.Left = _cbParPeso.Left;
                    _btCapPeso.Width = 100;
                    _btCapPeso.LinkTo = "2001";

                    Button btEtiq = (Button)_btCapPeso.Specific;
                    btEtiq.Caption = "Capturar Peso";

                    form.Mode = BoFormMode.fm_OK_MODE;
                }
            }
        }

        private void HandleButtonClickEvent(string formUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;



            //Pedido de Venda (OS)
            if (pVal.EventType == BoEventTypes.et_CLICK && pVal.FormTypeEx == "139" && pVal.ItemUID == "btCapPeso" && !pVal.BeforeAction)
            {
                Form form = oApplication.Forms.Item(pVal.FormUID);
                oApplication.StatusBar.SetText("Lendo peso...", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning);
                BalancaController oBalancaController = new BalancaController(form);
                //oBalancaController.OBalanca
                //StaticText lblBalanca = (StaticText)form.Items.Item("lblBalanca").Specific;
                //lblBalanca.Item.Visible = true;
                form.Freeze(false);
                LogHelper.MostraBalanca("", "", form);
                //LogHelper.MostraBalanca(OBalanca.peso, hora, this.pForm);

                double dPeso = Convert.ToDouble(oBalancaController.OBalanca.peso);

              

                //    Form form = oApplication.Forms.Item(pVal.FormUID);

                    Matrix matrix = (Matrix)form.Items.Item("38").Specific;

                    string parametroPesagem = ((ComboBox)form.Items.Item("cbParPeso").Specific).Selected.Value;



                    string passo = "0";

                    try
                    {

                        string um = "";
                        try
                        {
                            um = ((EditText)matrix.Columns.Item("212").Cells.Item(1).Specific).String.ToUpper();
                        }
                        catch { }

                        passo = "1";
                        string sPesoBruto = ((EditText)form.Items.Item("U_PesoBruto").Specific).String;
                        string sTara = ((EditText)form.Items.Item("U_Tara").Specific).String;

                        passo = "2";
                        double pesoBruto = double.Parse((sPesoBruto.Contains(",") ? sPesoBruto.Replace(".", "").Replace(",", ".") : sPesoBruto), System.Globalization.CultureInfo.InvariantCulture); ;
                        double tara = double.Parse((sTara.Contains(",") ? sTara.Replace(".", "").Replace(",", ".") : sTara), System.Globalization.CultureInfo.InvariantCulture); ;

                        passo = "3";
                        if (parametroPesagem.Equals("0"))
                        {
                            passo = "4";
                            pesoBruto = double.Parse((oBalancaController.OBalanca.peso.Contains(",") ? oBalancaController.OBalanca.peso.Replace(".", "").Replace(",", ".") : oBalancaController.OBalanca.peso), System.Globalization.CultureInfo.InvariantCulture);
                            ((EditText)form.Items.Item("U_PesoBruto").Specific).String = pesoBruto.ToString();
                        }
                        else
                        {
                            passo = "5";
                            tara = double.Parse((oBalancaController.OBalanca.peso.Contains(",") ? oBalancaController.OBalanca.peso.Replace(".", "").Replace(",", ".") : oBalancaController.OBalanca.peso), System.Globalization.CultureInfo.InvariantCulture);
                            ((EditText)form.Items.Item("U_Tara").Specific).String = tara.ToString();
                        }

                        passo = "6";
                        if (tara > 0)
                        {
                            passo = "7";
                            double pesoLiquido = pesoBruto - tara;
                            ((EditText)form.Items.Item("U_PesoLiq").Specific).String = pesoLiquido.ToString();
                            if (um == "TONELADAS")
                            {
                                passo = "8";
                                ((EditText)matrix.Columns.Item("11").Cells.Item(1).Specific).String = (pesoLiquido / 1000).ToString();
                            }
                        }

                        passo = "9";
                        ((EditText)form.Items.Item("U_HoraEntradaOS").Specific).String = DateTime.Now.ToString("HH:mm");
                        ((EditText)form.Items.Item("U_DataEntradaOS").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                        ((ComboBox)form.Items.Item("U_Situacao").Specific).Select("11");
                        ((ComboBox)form.Items.Item("U_Status").Specific).Select("P");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao preencher campos em tela (etapa " + passo + ") [ " + oBalancaController.OBalanca.peso + "] :" + ex.Message);
                    }

                    //sucesso = true;

                    //break;


                //if (sucesso)
                //{
                //    oApplication.StatusBar.SetText("Operação completada com êxito", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
                //}
                //else
                //{
                //    oApplication.StatusBar.SetText(mensagem);
                //}
            }
        }

        public static List<NotaGerada> m_NotasGeradas = new List<NotaGerada>();

        public static void OpenNotasGeradasView(List<NotaGerada> notas)
        {
            try
            {
                m_NotasGeradas = notas;

                var resourceName = "Chess.IT.Services.SrfFiles.FrmNotasGeradas.srf";
                //string response = System.Resources.GetString(resourceName);
                using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(System.IO.Path.Combine(System.Environment.CurrentDirectory, "FrmNotasGeradas.srf"), System.IO.FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                        {
                            fileStream.WriteByte((byte)stream.ReadByte());
                        }
                        fileStream.Close();
                    }
                }

                string srfPath = System.Environment.CurrentDirectory + "\\FrmNotasGeradas.srf";

                if (File.Exists(srfPath) == false)
                {
                    throw new Exception("Arquivo SRF não encontrado. Verifique a instalação do addOn.");
                }

                string xml = File.ReadAllText(srfPath);

                string formUID = GerarFormUID("FrmNotasGeradas");

                xml = xml.Replace("uid=\"FrmNotasGeradas\"", string.Format("uid=\"{0}\"", formUID));

#if DEBUG
                //xml = xml.Replace("from dummy", "");
#endif

                oApplicationS.LoadBatchActions(ref xml);
            }
            catch (Exception exception)
            {
                oApplicationS.StatusBar.SetText(exception.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
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

        private  static string GerarFormUID(string formType)
        {
            string result = string.Empty;

            int count = 0;

            bool next = true;

            while (next)
            {
                count++;

                try
                {
                    oApplicationS.Forms.GetForm(formType, count);
                }
                catch
                {
                    next = false;
                }
            }

            result = string.Format("Frm{0}-{1}", count, new Random().Next(999));

            return result;
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

        //public void processo()
        //{
        //    MainController mainController = new MainController(oApplication, oCompany);
        //}
    }
}
