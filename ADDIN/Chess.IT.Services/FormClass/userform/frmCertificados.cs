using Castle.Core.Logging;
using Chess.IT.Services.services;
using JBC.Framework.Attribute;
using JBC.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.IT.Services.FormClass.userform
{
    [MenuEvent(UniqueUID = "JBCCD")]
    [FormAttribute("JBC_CD", "Chess.IT.Services.SrfFiles.frmCertificados.srf")]
    public class frmCertificados : JBCUserFormBase
    {
        public SAPbobsCOM.Company oCompany { get; set; }
        public SAPbouiCOM.Application oApplication { get; set; }
        public JBC.Framework.DAO.BusinessOneDAO B1DAO { get; set; }
        public ILogger Log { get; set; }

        private SAPbouiCOM.IForm _oForm;
        
        private SAPbouiCOM.EditText _EditTextCertificado;
        //private SAPbouiCOM.EditText _EditTextDataDe;
        //private SAPbouiCOM.EditText _EditTextDataAte;

        private SAPbouiCOM.EditText _EditTextCardCodeDe;
        //private SAPbouiCOM.EditText _EditTextCardCodeAte;
        private SAPbouiCOM.EditText _EditTextNFSDe;
        //private SAPbouiCOM.EditText _EditTextNFSAte;
        private SAPbouiCOM.EditText _EditTextOSDe;
        //private SAPbouiCOM.EditText _EditTextOSAte;
        private SAPbouiCOM.Button _ButtonCertificar;
        private SAPbouiCOM.Button _ButtonCancelar;
        private SAPbouiCOM.Button _ButtonImprimir;
        private SAPbouiCOM.Button _ButtonConsultar;
        private SAPbouiCOM.Button _ButtonMarcarTudo;
        private SAPbouiCOM.Button _ButtonDesmarcarTudo;        
        private SAPbouiCOM.Matrix _oMatrix1;
        private DataTable dtMatrix1 { get; set; }
        private bool bConfiguraTela = true;

        JBCKURICAService _JBCKURICAService;

        private JBCKURICAService oJBCKURICAService
        {
            get
            {
                if (_JBCKURICAService == null)
                {
                    _JBCKURICAService = new JBCKURICAService(this.B1DAO, this.Log, this.oCompany);
                }
                return _JBCKURICAService;
            }
        }

        public override void OnInitializeComponent()
        {
            try
            {
                GetUIControls();
                SetEventHandlers();
                SetMascaraDeData();

                this._oForm.DataSources.UserDataSources.Add("PNini", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 40);
                this._EditTextCardCodeDe.DataBind.SetBound(true, "", "PNini");

                this._oForm.DataSources.UserDataSources.Add("PNfim", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 40);
                //this._EditTextCardCodeAte.DataBind.SetBound(true, "", "PNfim");



                this._oForm.DataSources.UserDataSources.Add("NFSini", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 40);
                this._EditTextNFSDe.DataBind.SetBound(true, "", "NFSini");

                this._oForm.DataSources.UserDataSources.Add("NFSfim", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 40);
                //this._EditTextNFSAte.DataBind.SetBound(true, "", "NFSfim");

                this._oForm.DataSources.UserDataSources.Add("OSini", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 40);
                this._EditTextOSDe.DataBind.SetBound(true, "", "OSini");

                this._oForm.DataSources.UserDataSources.Add("OSfim", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 40);
                //this._EditTextOSAte.DataBind.SetBound(true, "", "OSfim");


                this.dtMatrix1 = _oForm.DataSources.DataTables.Add("DT_01");
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void ConfiguraTela()
        {
            try
            {
                if (this.bConfiguraTela)
                {
                    this._oForm.Freeze(true);

                    AddChooseFromListPN();
                    //AddChooseFromListNFS();
                    AddChooseFromListOS();
                    //_EditTextDataDe.Value = oJBCKURICAService.PrimeiroDiaDoMes();
                    //_EditTextDataAte.Value = oJBCKURICAService.UltimoDiaDoMes();
                    ConfiguraMatrix1();

                    this.bConfiguraTela = false;
                    if (oJBCKURICAService.UserCancelaCertificado())
                    {
                        _ButtonCancelar.Item.Enabled = true;
                    }
                    else
                    {
                        _ButtonCancelar.Item.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                this._oForm.Freeze(false);
            }
        }

        private void ConfiguraMatrix1()
        {
            try
            {

                //_oForm.Freeze(true);
                Consultar(true);
                ConfigurarColunas();
                this._oMatrix1.AutoResizeColumns();
                this._oMatrix1.LoadFromDataSource();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                //_oForm.Freeze(false);
            }
        }

        private void Consultar(bool bIncialConfig = false)
        {
            try
            {
                //_oForm.Freeze(true);
                Log.InfoFormat(string.Format("Aguarde {0}, \\O/ consultando dados...", oJBCKURICAService.UserName()));
                string select = string.Empty;
                if (bIncialConfig)
                {
                    select = oJBCKURICAService.CertificadoConsulta(
                        this._EditTextCertificado.Value,
                       // this._EditTextDataDe.Value,
                        //this._EditTextDataAte.Value,
                        this._EditTextCardCodeDe.Value,
                        //this._EditTextCardCodeAte.Value,
                        this._EditTextNFSDe.Value,
                        //this._EditTextNFSAte.Value,
                        this._EditTextOSDe.Value
                       // this._EditTextOSAte.Value
                        );
                }
                else
                {
                    select = oJBCKURICAService.CertificadoConsulta(
                        this._EditTextCertificado.Value,
                        //this._EditTextDataDe.Value,
                        //this._EditTextDataAte.Value,
                        this._EditTextCardCodeDe.Value,
                        //this._EditTextCardCodeAte.Value,
                        this._EditTextNFSDe.Value,
                        //this._EditTextNFSAte.Value,
                        this._EditTextOSDe.Value
                        //this._EditTextOSAte.Value
                        );

                }
                dtMatrix1.ExecuteQuery(select);

                this._oMatrix1.Clear();
                this._oMatrix1.AutoResizeColumns();
                this._oMatrix1.LoadFromDataSource();



                Log.InfoFormat("Consulta de Dados Finalizada.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                //_oForm.Freeze(false);
                //Application.ActivateMenuItem("1304");
            }
        }

        private void ConfigurarColunas()
        {
            try
            {
                //_oForm.Freeze(true);
                int iCountCol = 1;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_CHECK_BOX);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "SEL");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "SEL";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = true;
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "CardCode");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Código";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 50;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                ((LinkedButton)this._oMatrix1.Columns.Item(iCountCol).ExtendedObject).LinkedObject = BoLinkedObject.lf_BusinessPartner;
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "CardName");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Razão Social";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "Carrier");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Cod. Transp.";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 50;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                ((LinkedButton)this._oMatrix1.Columns.Item(iCountCol).ExtendedObject).LinkedObject = BoLinkedObject.lf_BusinessPartner;
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "CardNameT");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Transportadora";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                iCountCol++;


                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "OS");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 10;
                this._oMatrix1.Columns.Item(iCountCol).Visible = false;
                                
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "DocNumOS");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "PV";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                ((LinkedButton)this._oMatrix1.Columns.Item(iCountCol).ExtendedObject).LinkedObject = BoLinkedObject.lf_Order;


                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "DocDateOS");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Data PV";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                iCountCol++;


                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "NFS");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 10;
                this._oMatrix1.Columns.Item(iCountCol).Visible = false;
                
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "DocNumNFS");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "NF";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                ((LinkedButton)this._oMatrix1.Columns.Item(iCountCol).ExtendedObject).LinkedObject = BoLinkedObject.lf_Invoice;
                iCountCol++;


                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "NNF");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Nº";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "CertificadoOS");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Certificado";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "DataCertificadoOS");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Data Certificado";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                iCountCol++;



                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "AgrNo");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Contrato";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 50;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                //((LinkedButton)this._oMatrix1.Columns.Item(iCountCol).ExtendedObject).LinkedObject = BoLinkedObject.lf_Invoice;
                iCountCol++;

                this._oMatrix1.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                this._oMatrix1.Columns.Item(iCountCol).DataBind.Bind("DT_01", "Descript");
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Caption = "Descrição";
                this._oMatrix1.Columns.Item(iCountCol).TitleObject.Sortable = false;
                this._oMatrix1.Columns.Item(iCountCol).Width = 100;
                this._oMatrix1.Columns.Item(iCountCol).Editable = false;
                iCountCol++;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                //_oForm.Freeze(false);
            }
        }

        private void AddChooseFromListOS()
        {
            try
            {
                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                SAPbouiCOM.Conditions oCons = null;
                SAPbouiCOM.Condition oCon = null;

                oCFLs = _oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                oCFLCreationParams.MultiSelection = true;
                oCFLCreationParams.ObjectType = "17";
                oCFLCreationParams.UniqueID = "CLF_OS";
                oCFL = oCFLs.Add(oCFLCreationParams);

                _EditTextOSDe.ChooseFromListUID = "CLF_OS";
                _EditTextOSDe.ChooseFromListAlias = "DocEntry";

               // oCFLs = _oForm.ChooseFromLists;

               // oCFL = null;
               // oCFLCreationParams = null;
               // oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));


               // oCFLCreationParams.MultiSelection = false;
               // oCFLCreationParams.ObjectType = "17";
               // oCFLCreationParams.UniqueID = "CLF_OS1";
               // oCFL = oCFLs.Add(oCFLCreationParams);

               // //_EditTextOSAte.ChooseFromListUID = "CLF_OS1";
               //// _EditTextOSAte.ChooseFromListAlias = "DocEntry";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        private void AddChooseFromListNFS()
        {
            try
            {
                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                SAPbouiCOM.Conditions oCons = null;
                SAPbouiCOM.Condition oCon = null;

                oCFLs = _oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                oCFLCreationParams.MultiSelection = true;
                oCFLCreationParams.ObjectType = "13";
                oCFLCreationParams.UniqueID = "CLF_NFS";
                oCFL = oCFLs.Add(oCFLCreationParams);

                _EditTextNFSDe.ChooseFromListUID = "CLF_NFS";
                _EditTextNFSDe.ChooseFromListAlias = "DocEntry";

                //oCFLs = _oForm.ChooseFromLists;

                //oCFL = null;
                //oCFLCreationParams = null;
                //oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));


                //oCFLCreationParams.MultiSelection = true;
                //oCFLCreationParams.ObjectType = "13";
                //oCFLCreationParams.UniqueID = "CLF_NFS1";
                //oCFL = oCFLs.Add(oCFLCreationParams);

                //_EditTextNFSAte.ChooseFromListUID = "CLF_NFS1";
                //_EditTextNFSAte.ChooseFromListAlias = "DocEntry";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        private void AddChooseFromListPN()
        {
            try
            {
                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                SAPbouiCOM.Conditions oCons = null;
                SAPbouiCOM.Condition oCon = null;

                oCFLs = _oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                oCFLCreationParams.MultiSelection = false;
                oCFLCreationParams.ObjectType = "2";
                oCFLCreationParams.UniqueID = "CLF_CLIENTE";
                oCFL = oCFLs.Add(oCFLCreationParams);
                //Adding Conditions to CFL1
                oCons = oCFL.GetConditions();
                oCon = oCons.Add();
                oCon.Alias = "CardType";
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCon.CondVal = "C";
                oCFL.SetConditions(oCons);

                _EditTextCardCodeDe.ChooseFromListUID = "CLF_CLIENTE";
                _EditTextCardCodeDe.ChooseFromListAlias = "CardCode";

                oCFLs = _oForm.ChooseFromLists;

                oCFL = null;
                oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));


                oCFLCreationParams.MultiSelection = false;
                oCFLCreationParams.ObjectType = "2";
                oCFLCreationParams.UniqueID = "CLF_CLIENT1";
                oCFL = oCFLs.Add(oCFLCreationParams);
                //Adding Conditions to CFL1
                oCons = oCFL.GetConditions();
                oCon = oCons.Add();
                oCon.Alias = "CardType";
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCon.CondVal = "C";
                oCFL.SetConditions(oCons);

                //_EditTextCardCodeAte.ChooseFromListUID = "CLF_CLIENT1";
                //_EditTextCardCodeAte.ChooseFromListAlias = "CardCode";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        protected override void OnFormVisibleAfter(SBOItemEventArg pVal)
        {
            try
            {

                ConfiguraTela();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {

            }
        }

        private void SetMascaraDeData()
        {
            //this._oForm.DataSources.UserDataSources.Add("DtDe", SAPbouiCOM.BoDataType.dt_DATE, 0);
            //this._EditTextDataDe.DataBind.SetBound(true, "", "DtDe");

            //this._oForm.DataSources.UserDataSources.Add("DtAte", SAPbouiCOM.BoDataType.dt_DATE, 0);
            //this._EditTextDataAte.DataBind.SetBound(true, "", "DtAte");
        }

        private void GetUIControls()
        {
            this._oForm = ((JBC.Framework.Form.JBCFormBase)(this)).UIAPIRawForm;
            this._EditTextCertificado = (EditText)this.GetItem("Item_5").Specific;

            //this._EditTextDataDe = (EditText)this.GetItem("Item_1").Specific;
            //this._EditTextDataAte = (EditText)this.GetItem("Item_3").Specific;

            this._EditTextCardCodeDe = (EditText)this.GetItem("Item_11").Specific;
            //this._EditTextCardCodeAte = (EditText)this.GetItem("Item_13").Specific;

            this._EditTextNFSDe = (EditText)this.GetItem("Item_15").Specific;
            //this._EditTextNFSAte = (EditText)this.GetItem("Item_17").Specific;

            this._EditTextOSDe = (EditText)this.GetItem("Item_19").Specific;
            //this._EditTextOSAte = (EditText)this.GetItem("Item_21").Specific;

            this._ButtonCertificar = (Button)this.GetItem("Item_6").Specific;
            this._ButtonCancelar = (Button)this.GetItem("Item_7").Specific;
            this._ButtonImprimir = (Button)this.GetItem("Item_23").Specific;
            //this._ButtonImprimir.Item.Visible = false;

            this._ButtonConsultar = (Button)this.GetItem("Item_8").Specific;
            this._ButtonMarcarTudo = (Button)this.GetItem("Item_22").Specific;
            this._ButtonDesmarcarTudo = (Button)this.GetItem("Item_24").Specific;            
            this._oMatrix1 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_9")).Specific);
        }

        protected override void OnFormCloseAfter(SBOItemEventArg pVal)
        {
            try
            {
                SetEventHandlers(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {

            }
        }

        private void SetEventHandlers(bool exit = false)
        {
            if (!exit)
            {
                this._EditTextCardCodeDe.ChooseFromListAfter += _EditTextCardCodeAte_ChooseFromListAfter;
                //this._EditTextCardCodeAte.ChooseFromListAfter += _EditTextCardCodeAte_ChooseFromListAfter;

                this._EditTextNFSDe.ChooseFromListAfter += _EditTextNFSDe_ChooseFromListAfter;
                //this._EditTextNFSAte.ChooseFromListAfter += _EditTextNFSDe_ChooseFromListAfter;

                this._EditTextOSDe.ChooseFromListAfter += _EditTextOSDe_ChooseFromListAfter;
                //this._EditTextOSAte.ChooseFromListAfter += _EditTextOSDe_ChooseFromListAfter;

                this._ButtonMarcarTudo.ClickAfter += _ButtonMarcarTudo_ClickAfter;
                this._ButtonDesmarcarTudo.ClickAfter += _ButtonDesmarcarTudo_ClickAfter;

                this._ButtonConsultar.ClickAfter += _ButtonConsultar_ClickAfter;

                this._ButtonCertificar.PressedAfter += _ButtonCertificar_PressedAfter;
                this._ButtonCancelar.PressedAfter += _ButtonCancelar_PressedAfter;
                this._ButtonImprimir.PressedAfter += _ButtonImprimir_PressedAfter;

                this._oMatrix1.LinkPressedBefore += _oMatrix1_LinkPressedBefore;
            }
            else
            {
                this._EditTextCardCodeDe.ChooseFromListAfter -= _EditTextCardCodeAte_ChooseFromListAfter;
                //this._EditTextCardCodeAte.ChooseFromListAfter -= _EditTextCardCodeAte_ChooseFromListAfter;

                this._EditTextNFSDe.ChooseFromListAfter -= _EditTextNFSDe_ChooseFromListAfter;
                //this._EditTextNFSAte.ChooseFromListAfter -= _EditTextNFSDe_ChooseFromListAfter;

                this._EditTextOSDe.ChooseFromListAfter -= _EditTextOSDe_ChooseFromListAfter;
                //this._EditTextOSAte.ChooseFromListAfter -= _EditTextOSDe_ChooseFromListAfter;

                this._ButtonMarcarTudo.ClickAfter -= _ButtonMarcarTudo_ClickAfter;
                this._ButtonDesmarcarTudo.ClickAfter -= _ButtonDesmarcarTudo_ClickAfter;

                this._ButtonConsultar.ClickAfter -= _ButtonConsultar_ClickAfter;

                this._ButtonCertificar.PressedAfter -= _ButtonCertificar_PressedAfter;
                this._ButtonCancelar.PressedAfter -= _ButtonCancelar_PressedAfter;
                this._ButtonImprimir.PressedAfter -= _ButtonImprimir_PressedAfter;

                this._oMatrix1.LinkPressedBefore -= _oMatrix1_LinkPressedBefore;
            }
        }

        private void _oMatrix1_LinkPressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "C7")
            {
                BubbleEvent = false;

                string docEntry = ((EditText)_oMatrix1.Columns.Item("C6").Cells.Item(pVal.Row).Specific).String;

                app.OpenForm(BoFormObjectEnum.fo_Order, "17", docEntry);
            }
            else if(pVal.ColUID == "C10")
            {
                BubbleEvent = false;

                string docEntry = ((EditText)_oMatrix1.Columns.Item("C9").Cells.Item(pVal.Row).Specific).String;

                app.OpenForm(BoFormObjectEnum.fo_Invoice, "13", docEntry);
            }
        }

        protected internal virtual void _ButtonImprimir_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                this._oMatrix1.FlushToDataSource();

                string certificado = "";

                bool bSelecionouAlgo = false;
                bool bSelecionouVariosClientes = false;
                for (int i = 0; i < this.dtMatrix1.Rows.Count; i++)
                {
                    if (this.dtMatrix1.GetValue("SEL", i).ToString().Equals("Y"))
                    {
                        bSelecionouAlgo = true;

                        if (certificado != string.Empty && certificado != this.dtMatrix1.GetValue("CertificadoOS", i).ToString())
                        {
                            bSelecionouVariosClientes = true;
                            break;
                        }

                        certificado = this.dtMatrix1.GetValue("CertificadoOS", i).ToString();
                    }
                }
                if (!bSelecionouAlgo)
                {
                    Log.ErrorFormat("Por favor selecione registros para imprimir o certificado!!");
                }
                else if (bSelecionouVariosClientes)
                {
                    Log.ErrorFormat("Não é permitido imprimir vários certificados.");
                }
                else
                {
                    oApplication.ActivateMenuItem("9623aca5034543ebb5d84076e357dd56");
                    //oApplication.ActivateMenuItem("794607f5742c487289b6f15ec790feb4");

                    Form oForm = oApplication.Forms.GetFormByTypeAndCount(410000100, 1);

                    ((EditText)oForm.Items.Item("1000003").Specific).String = certificado;

                    oForm.Items.Item("1").Click();
                }
                //SAPbobsCOM.ReportLayoutsService oReportLayoutService = (SAPbobsCOM.ReportLayoutsService)oCompany.GetCompanyService().GetBusinessService(SAPbobsCOM.ServiceTypes.ReportLayoutsService);
                //SAPbobsCOM.ReportLayoutPrintParams oReportPrintParams = (SAPbobsCOM.ReportLayoutPrintParams)oReportLayoutService.GetDataInterface(SAPbobsCOM.ReportLayoutsServiceDataInterfaces.rlsdiReportLayoutPrintParams);

                //oReportPrintParams.LayoutCode = "RCRI0027";

                //oReportPrintParams.DocEntry = 1;

                //oReportLayoutService.Print(oReportPrintParams);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
            }
        }

        protected internal virtual void _ButtonCancelar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (oApplication.MessageBox(string.Format("Você confirma o Cancelamento da numeração de Certificado?"), 1, "Sim", "Não") == 1)
                {
                    //if (string.IsNullOrEmpty(this._EditTextCertificado.Value))
                    //{
                    //    Log.ErrorFormat("Por favor informe o certificado!!");
                    //}
                    //else
                    //{
                        this._oMatrix1.FlushToDataSource();
                        bool bSelecionouAlgo = false;
                        for (int i = 0; i < this.dtMatrix1.Rows.Count; i++)
                        {
                            if (this.dtMatrix1.GetValue("SEL", i).ToString().Equals("Y"))
                            {
                                bSelecionouAlgo = true;
                                break;
                            }
                        }
                        if (!bSelecionouAlgo)
                        {
                            Log.ErrorFormat("Por favor selecione registros para cancelar o certificado!!");
                        }
                        else
                        {
                            DateTime date = DateTime.Now;
                            for (int i = 0; i < this.dtMatrix1.Rows.Count; i++)
                            {
                                if (this.dtMatrix1.GetValue("SEL", i).ToString().Equals("Y"))
                                {
                                    oJBCKURICAService.AtualizaCertificadoOS("", this.dtMatrix1.GetValue("OS", i).ToString());
                                    //SAPbobsCOM.Documents oInvoices = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    ////Log.InfoFormat((this.dtMatrix1.GetValue("NFS", i).ToString()));

                                    //if (oInvoices.GetByKey(Convert.ToInt32(this.dtMatrix1.GetValue("NFS", i).ToString())))
                                    //{
                                    //    if (oInvoices.UserFields.Fields.Item("U_JBC_CERTI").Value.Equals(this._EditTextCertificado.Value))
                                    //    {
                                    //        oInvoices.UserFields.Fields.Item("U_JBC_CERTI").Value = "";
                                    //        oInvoices.UserFields.Fields.Item("U_JBC_DTCERTI").Value = date;
                                    //        if (oInvoices.Update() == 0)
                                    //        {
                                    //            Log.InfoFormat("NFS {0} atualizado Certificado com Sucesso!!!", this.dtMatrix1.GetValue("DocNumNFS", i).ToString());
                                    //        }
                                    //        else
                                    //        {
                                    //            int lErrCode;
                                    //            string sErrMsg;
                                    //            oCompany.GetLastError(out lErrCode, out sErrMsg);
                                    //            Log.Error(string.Format("Erro Ao Salvar NFS {0} atualizando Certificado. {1}, {2}"
                                    //                , this.dtMatrix1.GetValue("DocNumNFS", i).ToString()
                                    //                , lErrCode.ToString()
                                    //                , sErrMsg
                                    //                ));
                                    //        }
                                    //    }

                                    //}
                                    //LimparObjeto(oInvoices);


                                    //SAPbobsCOM.Documents ooOrders = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                    //if (ooOrders.GetByKey(Convert.ToInt32(this.dtMatrix1.GetValue("OS", i).ToString())))
                                    //{
                                    //    if (ooOrders.UserFields.Fields.Item("U_JBC_CERTI").Value.Equals(this._EditTextCertificado.Value))
                                    //    {
                                    //        ooOrders.UserFields.Fields.Item("U_JBC_CERTI").Value = "";
                                    //        ooOrders.UserFields.Fields.Item("U_JBC_DTCERTI").Value = date;
                                    //        if (ooOrders.Update() == 0)
                                    //        {
                                    //            Log.InfoFormat("OS {0} atualizado Certificado com Sucesso!!!", this.dtMatrix1.GetValue("OS", i).ToString());
                                    //        }
                                    //        else
                                    //        {
                                    //            int lErrCode;
                                    //            string sErrMsg;
                                    //            oCompany.GetLastError(out lErrCode, out sErrMsg);
                                    //            Log.Error(string.Format("Erro Ao Salvar OS {0} atualizando Certificado. {1}, {2}"
                                    //                , this.dtMatrix1.GetValue("OS", i).ToString()
                                    //                , lErrCode.ToString()
                                    //                , sErrMsg
                                    //                ));
                                    //        }
                                    //    }
                                    //}
                                    //LimparObjeto(ooOrders);
                                }
                            }
                            Log.InfoFormat("Processo Finalizado!!!");
                            Consultar();
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
            }
        }

        protected internal virtual void _ButtonCertificar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (oApplication.MessageBox(string.Format("{0}, você confirma a Geração de uma nova numeração de Certificado?", oJBCKURICAService.UserName()), 1, "Sim", "Não") == 1)
                {
                    string cardCode = "";
                    Dictionary<string, int> totalOSPorNF = new Dictionary<string, int>();
                    Dictionary<string, int> marcadasOSPorNF = new Dictionary<string, int>();

                    this._oMatrix1.FlushToDataSource();
                    bool bSelecionouAlgo = false;
                    bool bSelecionouVariosClientes = false;
                    bool bSelecionouJaCertificado = false;
                    for (int i = 0; i < this.dtMatrix1.Rows.Count; i++)
                    {
                        string nota = this.dtMatrix1.GetValue("NFS", i).ToString();

                        if (totalOSPorNF.ContainsKey(nota))
                            totalOSPorNF[nota] = totalOSPorNF[nota] + 1;
                        else
                            totalOSPorNF[nota] = 1;

                        if (this.dtMatrix1.GetValue("SEL", i).ToString().Equals("Y"))
                        {
                            bSelecionouAlgo = true;

                            if (cardCode != string.Empty && cardCode != this.dtMatrix1.GetValue("CardCode", i).ToString())
                            {
                                bSelecionouVariosClientes = true;
                                break;
                            }

                            if (this.dtMatrix1.GetValue("CertificadoOS", i).ToString() != "")
                            {
                                bSelecionouJaCertificado = true;
                                break;
                            }

                            cardCode = this.dtMatrix1.GetValue("CardCode", i).ToString();

                            if (marcadasOSPorNF.ContainsKey(nota))
                                marcadasOSPorNF[nota] = marcadasOSPorNF[nota] + 1;
                            else
                                marcadasOSPorNF[nota] = 1;
                        }
                    }
                    if (!bSelecionouAlgo)
                    {
                        Log.ErrorFormat("Por favor selecione registros para gerar o certificado.");
                    }
                    else if (bSelecionouJaCertificado)
                    {
                        Log.ErrorFormat("Não é permitida a geração de certificado para documentos já certificados.");
                    }
                    else if (bSelecionouVariosClientes)
                    {
                        Log.ErrorFormat("Não é permitida a geração de certificado para clientes diferentes na mesma requisição.");
                    }
                    else if (marcadasOSPorNF.Where(r => r.Value != totalOSPorNF[r.Key]).Count() > 0)
                    {
                        Log.ErrorFormat("Selecionar todas as OS da nota para gerar um único certificado para a mesma.");
                    }
                    else
                    {
                        //if (oCompany.InTransaction)
                        //{
                        //    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        //}
                        //oCompany.StartTransaction();
                        DateTime date = DateTime.Now;
                        string sCertificadoNovoNumero = oJBCKURICAService.CertificadoNovoNumero();
                        for (int i = 0; i < this.dtMatrix1.Rows.Count; i++)
                        {
                            if (this.dtMatrix1.GetValue("SEL", i).ToString().Equals("Y"))
                            {
                                //SAPbobsCOM.Documents oInvoices = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                //if (oInvoices.GetByKey(Convert.ToInt32(this.dtMatrix1.GetValue("NFS", i).ToString())))
                                //{
                                //    oInvoices.UserFields.Fields.Item("U_JBC_CERTI").Value = sCertificadoNovoNumero;
                                //    oInvoices.UserFields.Fields.Item("U_JBC_DTCERTI").Value = date;
                                //    if (oInvoices.Update() == 0)
                                //    {
                                //        Log.InfoFormat("NFS {0} atualizado Certificado com Sucesso!!!", this.dtMatrix1.GetValue("DocNumNFS", i).ToString());
                                //    }
                                //    else
                                //    {
                                //        int lErrCode;
                                //        string sErrMsg;
                                //        oCompany.GetLastError(out lErrCode, out sErrMsg);
                                //        Log.Error(string.Format("Erro Ao Salvar NFS {0} atualizando Certificado. {1}, {2}"
                                //            , this.dtMatrix1.GetValue("DocNumNFS", i).ToString()
                                //            , lErrCode.ToString()
                                //            , sErrMsg
                                //            ));
                                //        //if (oCompany.InTransaction)
                                //        //{
                                //        //    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                //        //}
                                //        //break;
                                //    }
                                //}
                                //LimparObjeto(oInvoices);


                                //SAPbobsCOM.Documents ooOrders = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                //if (ooOrders.GetByKey(Convert.ToInt32(this.dtMatrix1.GetValue("OS", i).ToString())))
                                //{
                                //    ooOrders.UserFields.Fields.Item("U_JBC_CERTI").Value = sCertificadoNovoNumero;
                                //    ooOrders.UserFields.Fields.Item("U_JBC_DTCERTI").Value = date;
                                //    if (ooOrders.Update() == 0)
                                //    {
                                //        Log.InfoFormat("OS {0} atualizado Certificado com Sucesso!!!", this.dtMatrix1.GetValue("OS", i).ToString());
                                //    }
                                //    else
                                //    {
                                //        int lErrCode;
                                //        string sErrMsg;
                                //        oCompany.GetLastError(out lErrCode, out sErrMsg);
                                //        Log.Error(string.Format("Erro Ao Salvar OS {0} atualizando Certificado. {1}, {2}"
                                //            , this.dtMatrix1.GetValue("OS", i).ToString()
                                //            , lErrCode.ToString()
                                //            , sErrMsg
                                //            ));
                                //        //if (oCompany.InTransaction)
                                //        //{
                                //        //    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                //        //}
                                //        //break;
                                //    }
                                //}
                                //LimparObjeto(ooOrders);

                                oJBCKURICAService.AtualizaCertificadoOS(sCertificadoNovoNumero, this.dtMatrix1.GetValue("OS", i).ToString());
                            }
                        }
                        //if (oCompany.InTransaction)
                        //{
                        //    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        //}
                        Log.InfoFormat("{0}, Processo Finalizado!!!  ｡◕‿◕｡", oJBCKURICAService.UserName());
                        Consultar();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                if (oCompany.InTransaction)
                {
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
            }
        }

        private void TratarChooseFromListOS(string sCardCode)
        {
            try
            {

                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                SAPbouiCOM.Conditions oCons = null;
                SAPbouiCOM.Condition oCon = null;

                oCFLs = _oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                oCFL = oCFLs.Item("CLF_OS");
                oCFL.SetConditions(null);
                oCons = oCFL.GetConditions();
                oCon = oCons.Add();
                oCon.Alias = "CardCode";
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCon.CondVal = sCardCode;
                oCFL.SetConditions(oCons);


                //oCFL = oCFLs.Item("CLF_OS1");
                //oCFL.SetConditions(null);
                //oCons = oCFL.GetConditions();
                //oCon = oCons.Add();
                //oCon.Alias = "CardCode";
                //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                //oCon.CondVal = sCardCode;
                //oCFL.SetConditions(oCons);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }
        private void TratarChooseFromListSFS(string sCardCode)
        {
            try
            {

                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                SAPbouiCOM.Conditions oCons = null;
                SAPbouiCOM.Condition oCon = null;

                oCFLs = _oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                oCFL = oCFLs.Item("CLF_NFS");
                oCFL.SetConditions(null);
                oCons = oCFL.GetConditions();
                oCon = oCons.Add();
                oCon.Alias = "CardCode";
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCon.CondVal = sCardCode;
                oCFL.SetConditions(oCons);


                //oCFL = oCFLs.Item("CLF_NFS1");
                //oCFL.SetConditions(null);
                //oCons = oCFL.GetConditions();
                //oCon = oCons.Add();
                //oCon.Alias = "CardCode";
                //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                //oCon.CondVal = sCardCode;
                //oCFL.SetConditions(oCons);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }
        protected internal virtual void _ButtonConsultar_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                Consultar();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }
        protected internal virtual void _EditTextOSDe_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (_oForm.Mode == BoFormMode.fm_FIND_MODE)
            {
                return;
            }
            SBOChooseFromListEventArg cflArg = (SBOChooseFromListEventArg)pVal;

            var oDataTable = cflArg.SelectedObjects;
            string sCode = "";
            string sName = "";
            try
            {
                sCode = oDataTable.GetValue(0, 0).ToString();
                sName = oDataTable.GetValue(1, 0).ToString();
            }
            catch
            {
                sCode = "";
                sName = "";
            }
            if (string.IsNullOrEmpty(sCode))
            {
                return;
            }
            if (pVal.ItemUID.Equals(_EditTextOSDe.Item.UniqueID))
            {
                try
                {
                    _oForm.DataSources.UserDataSources.Item("OSini").ValueEx = sCode;
                    //_oForm.DataSources.UserDataSources.Item("PNiniN").ValueEx = sName;
                }
                catch
                {
                }
            }
            //else if (pVal.ItemUID.Equals(_EditTextOSAte.Item.UniqueID))
            //{
            //    try
            //    {
            //        _oForm.DataSources.UserDataSources.Item("OSfim").ValueEx = sCode;
            //        //_oForm.DataSources.UserDataSources.Item("PNfimN").ValueEx = sName;
            //    }
            //    catch
            //    {
            //    }
            //}

        }
        protected internal virtual void _EditTextNFSDe_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (_oForm.Mode == BoFormMode.fm_FIND_MODE)
            {
                return;
            }
            SBOChooseFromListEventArg cflArg = (SBOChooseFromListEventArg)pVal;

            var oDataTable = cflArg.SelectedObjects;
            string sCode = "";
            string sName = "";
            try
            {
                sCode = oDataTable.GetValue(0, 0).ToString();
                sName = oDataTable.GetValue(1, 0).ToString();
            }
            catch
            {
                sCode = "";
                sName = "";
            }
            if (string.IsNullOrEmpty(sCode))
            {
                return;
            }
            if (pVal.ItemUID.Equals(_EditTextNFSDe.Item.UniqueID))
            {
                try
                {
                    _oForm.DataSources.UserDataSources.Item("NFSini").ValueEx = sCode;
                    //_oForm.DataSources.UserDataSources.Item("PNiniN").ValueEx = sName;
                }
                catch
                {
                }
            }
            //else if (pVal.ItemUID.Equals(_EditTextNFSAte.Item.UniqueID))
            //{
            //    try
            //    {
            //        _oForm.DataSources.UserDataSources.Item("NFSfim").ValueEx = sCode;
            //        //_oForm.DataSources.UserDataSources.Item("PNfimN").ValueEx = sName;
            //    }
            //    catch
            //    {
            //    }
            //}

        }
        protected internal virtual void _EditTextCardCodeAte_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (_oForm.Mode == BoFormMode.fm_FIND_MODE)
            {
                return;
            }
            SBOChooseFromListEventArg cflArg = (SBOChooseFromListEventArg)pVal;

            var oDataTable = cflArg.SelectedObjects;
            string sCode = "";
            string sName = "";
            try
            {
                sCode = oDataTable.GetValue(0, 0).ToString();
                sName = oDataTable.GetValue(1, 0).ToString();
            }
            catch
            {
                sCode = "";
                sName = "";
            }
            if (string.IsNullOrEmpty(sCode))
            {
                return;
            }
            if (pVal.ItemUID.Equals(_EditTextCardCodeDe.Item.UniqueID))
            {
                try
                {
                    _oForm.DataSources.UserDataSources.Item("PNini").ValueEx = sCode;
                    TratarChooseFromListOS(sCode);
                    //TratarChooseFromListSFS(sCode);
                    //_oForm.DataSources.UserDataSources.Item("PNiniN").ValueEx = sName;
                }
                catch
                {
                }
            }
            //else if (pVal.ItemUID.Equals(_EditTextCardCodeAte.Item.UniqueID))
            //{
            //    try
            //    {
            //        _oForm.DataSources.UserDataSources.Item("PNfim").ValueEx = sCode;
            //        //_oForm.DataSources.UserDataSources.Item("PNfimN").ValueEx = sName;
            //    }
            //    catch
            //    {
            //    }
            //}

        }
        protected internal virtual void _ButtonDesmarcarTudo_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                for (int i = 0; i < this._oMatrix1.RowCount; i++)
                {
                    dtMatrix1.SetValue(0, i, "N");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                this._oMatrix1.LoadFromDataSource();
            }
        }
        protected internal virtual void _ButtonMarcarTudo_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                for (int i = 0; i < this._oMatrix1.RowCount; i++)
                {
                    dtMatrix1.SetValue(0, i, "Y");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            finally
            {
                this._oMatrix1.LoadFromDataSource();
            }
        }

        private static void LimparObjeto(Object obj)
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
