using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace ChessIT.Financeiro.View
{
    class MeioPagtoView
    {
        Form Form;

        bool Loaded;

        bool bloqueado;

        List<Model.ChequeModel> m_ChequeList = new List<Model.ChequeModel>();

        List<Model.CartaoModel> m_CartaoList = new List<Model.CartaoModel>();

        Model.DinheiroModel m_DinheiroModel = new Model.DinheiroModel();

        Model.TransferenciaModel m_TransferenciaModel = new Model.TransferenciaModel();

        Model.BoletoModel m_BoletoModel = new Model.BoletoModel();

        List<string> m_Menus = new List<string>();

        int m_LastRowCheque = 0;

        public MeioPagtoView(Form form)
        {
            this.Form = form;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);

            Controller.MainController.Application.ItemEvent += HandleItemEvent;
            Controller.MainController.Application.MenuEvent += HandleMenuEvent;
            Controller.MainController.Application.RightClickEvent += HandleRightClickEvent;

            
        }

        private void HandleMenuEvent(ref MenuEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (Controller.MainController.Application.Forms.ActiveForm.UniqueID == Form.UniqueID)
            {
                if (pVal.MenuUID.StartsWith("MRCHEQUE") && !pVal.BeforeAction)
                {
                    int row = Convert.ToInt32(pVal.MenuUID.Replace("MRCHEQUE", ""));

                    Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                    if (m_ChequeList.Count > row)
                    {
                        m_ChequeList.RemoveAt(row - 1);
                    }

                    matrix.DeleteRow(row);

                    if (matrix.RowCount == 0)
                        matrix.AddRow();
                }

                if (pVal.MenuUID == "5915")
                {
                    if (Form.ActiveItem.Equals("etCarValor"))
                    {
                        ((EditText)Form.Items.Item("etCarValor").Specific).String =
                            (Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTotalPagar() + Controller.MainController.ConvertDouble(Form.DataSources.UserDataSources.Item("encargo").Value)).ToString();
                    }

                    if (Form.ActiveItem.Equals("etBolTotal"))
                    {
                        ((EditText)Form.Items.Item("etBolTotal").Specific).String =
                            (Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTotalPagar() + Controller.MainController.ConvertDouble(Form.DataSources.UserDataSources.Item("encargo").Value)).ToString();
                    }

                    if (Form.ActiveItem.Equals("etTotalTr"))
                    {
                        ((EditText)Form.Items.Item("etTotalTr").Specific).String =
                            (Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTotalPagar() + Controller.MainController.ConvertDouble(Form.DataSources.UserDataSources.Item("encargo").Value)).ToString();
                    }

                    if (Form.ActiveItem.Equals("etTotalDin"))
                    {
                        ((EditText)Form.Items.Item("etTotalDin").Specific).String =
                            (Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTotalPagar() + Controller.MainController.ConvertDouble(Form.DataSources.UserDataSources.Item("encargo").Value)).ToString();
                    }

                    CalcularTotais();
                }
            }
        }

        private void HandleRightClickEvent(ref ContextMenuInfo pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            foreach (string menu in m_Menus)
            {
                if (Controller.MainController.Application.Menus.Exists(menu))
                    Controller.MainController.Application.Menus.RemoveEx(menu);
            }

            if (pVal.ItemUID.Equals("mtCheque"))
            {
                string menuID = string.Format("MRCHEQUE{0}", pVal.Row);

                m_Menus.Add(menuID);                    

                SAPbouiCOM.Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                string colTitle = matrix.Columns.Item(pVal.ColUID).Title;
                string firstCol = matrix.Columns.Item(0).UniqueID;

                if (pVal.Row > 0 && pVal.Row <= matrix.RowCount && (colTitle != "#" || colTitle != ""))
                {
                    MenuItem menuItem = null;
                    Menus menu = null;
                    MenuCreationParams creationPackage = null;

                    creationPackage = ((MenuCreationParams)(Controller.MainController.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams)));

                    creationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    creationPackage.UniqueID = menuID;
                    creationPackage.String = "Eliminar linha";
                    creationPackage.Enabled = true;
                    creationPackage.Position = 1;

                    menuItem = Controller.MainController.Application.Menus.Item("1280");
                    menu = menuItem.SubMenus;

                    try
                    {
                        menu.AddEx(creationPackage);
                    }
                    catch { }
                }
            }
        }

        private void HandleItemEvent(string formUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            if (bloqueado)
                return;

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
                                    if (pVal.ItemUID == "btOK")
                                    {
                                        if (m_BoletoModel.valor > 0)
                                        {
                                            if (m_BoletoModel.contaC == "" || m_BoletoModel.vcto == DateTime.MinValue || m_BoletoModel.pais == "" || m_BoletoModel.banco == "" || m_BoletoModel.conta == "" || m_BoletoModel.formaPagto == "")
                                            {
                                                Controller.MainController.Application.StatusBar.SetText("Todos os dados do boleto devem ser preenchidos");
                                                bubbleEvent = false;
                                            }
                                        }
                                    }
                                    else if (pVal.ItemUID == "mtCheque")
                                    {
                                        if (pVal.Row != m_LastRowCheque)
                                        {
                                            m_LastRowCheque = pVal.Row;

                                            try
                                            {
                                                Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                                if (((ComboBox)matrix.Columns.Item("banco").Cells.Item(pVal.Row).Specific).Selected != null)
                                                {
                                                    for (int i = matrix.Columns.Item("conta").ValidValues.Count - 1; i >= 0; i--)
                                                    {
                                                        matrix.Columns.Item("conta").ValidValues.Remove(i, BoSearchKey.psk_Index);
                                                    }

                                                    string query = @"select ""AbsEntry"", ""Account"" from DSC1 where ""BankCode"" = '" + m_ChequeList[pVal.Row - 1].banco + "'";

                                                    SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                                    recordSet.DoQuery(query);

                                                    while (!recordSet.EoF)
                                                    {
                                                        matrix.Columns.Item("conta").ValidValues.Add(recordSet.Fields.Item(1).Value.ToString(), "");

                                                        recordSet.MoveNext();
                                                    }
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                else
                                {
                                    if (pVal.ItemUID == "btOK")
                                    {
                                        if (m_ChequeList.Count > 0)
                                            m_ChequeList.Remove(m_ChequeList.Last());

                                        Controller.MainController.GetMeioPagtoParent(Form.UniqueID).SetChequeList(m_ChequeList);

                                        Controller.MainController.GetMeioPagtoParent(Form.UniqueID).SetCartaoList(m_CartaoList);

                                        Controller.MainController.GetMeioPagtoParent(Form.UniqueID).SetBoletoModel(m_BoletoModel);

                                        Controller.MainController.GetMeioPagtoParent(Form.UniqueID).SetTransferenciaModel(m_TransferenciaModel);

                                        Controller.MainController.GetMeioPagtoParent(Form.UniqueID).SetDinheiroModel(m_DinheiroModel);

                                        Controller.MainController.GetMeioPagtoParent(Form.UniqueID).SetEncargo(Controller.MainController.ConvertDouble(Form.DataSources.UserDataSources.Item("encargo").Value));

                                        Controller.MainController.GetMeioPagtoParent(Form.UniqueID).SetDividirTransacoesCartao(((CheckBox)Form.Items.Item("ckCarDiv").Specific).Checked);

                                        Form.Close();
                                    }

                                    if (pVal.ItemUID == "flCheque")
                                    {
                                        Form.PaneLevel = 1;
                                    }

                                    if (pVal.ItemUID == "flTransf")
                                    {
                                        Form.PaneLevel = 2;
                                    }

                                    if (pVal.ItemUID == "flCartao")
                                    {
                                        Form.PaneLevel = 3;
                                    }

                                    if (pVal.ItemUID == "flDinheiro")
                                    {
                                        Form.PaneLevel = 4;
                                    }

                                    if (pVal.ItemUID == "flBoleto")
                                    {
                                        Form.PaneLevel = 5;
                                    }

                                    if (pVal.ItemUID.Equals("mtCheque"))
                                    {
                                        if (pVal.ColUID == "manual")
                                        {
                                            if (m_ChequeList.Count < pVal.Row)
                                                m_ChequeList.Add(new Model.ChequeModel());

                                            Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                            m_ChequeList[pVal.Row - 1].manual = ((CheckBox)matrix.Columns.Item("manual").Cells.Item(pVal.Row).Specific).Checked;
                                        }                                                                            
                                    }

                                    if (pVal.ItemUID.Equals("mtComprov"))
                                    {
                                        Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                        if (matrixComprovante.GetNextSelectedRow() == matrixComprovante.RowCount)
                                        {
                                            m_CartaoList.Add(new Model.CartaoModel());

                                            Form.Freeze(true);
                                            bloqueado = true;
                                            try
                                            {
                                                ((ComboBox)Form.Items.Item("cbCarCred").Specific).Select("", BoSearchKey.psk_ByValue);
                                                ((EditText)Form.Items.Item("etCarConta").Specific).String = "";
                                                ((EditText)Form.Items.Item("etCarValor").Specific).String = "";
                                                ((EditText)Form.Items.Item("etCarPgtos").Specific).String = "1";
                                                ((EditText)Form.Items.Item("etCarPriVl").Specific).String = "";
                                                ((EditText)Form.Items.Item("etCarPriDt").Specific).String = "";
                                                ((EditText)Form.Items.Item("etCarPgAd").Specific).String = "";
                                                ((ComboBox)Form.Items.Item("cbCarDiv").Specific).Select(((CheckBox)Form.Items.Item("ckCarDiv").Specific).Checked ? "Y" : "N", BoSearchKey.psk_ByValue);
                                            }
                                            finally
                                            {
                                                Form.Freeze(false);
                                                bloqueado = false;
                                            }
                                        }
                                        else
                                        {
                                            Form.Freeze(true);
                                            bloqueado = true;
                                            try
                                            {
                                                Model.CartaoModel cartaoModel = m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1];

                                                ((ComboBox)Form.Items.Item("cbCarCred").Specific).Select(cartaoModel.nome.ToString(), BoSearchKey.psk_ByValue);
                                                ((EditText)Form.Items.Item("etCarConta").Specific).String = cartaoModel.contaC;
                                                ((EditText)Form.Items.Item("etCarValor").Specific).String = cartaoModel.valor.ToString();
                                                ((EditText)Form.Items.Item("etCarPgtos").Specific).String = cartaoModel.pgtos.ToString();
                                                ((EditText)Form.Items.Item("etCarPriVl").Specific).String = cartaoModel.primPagVal.ToString();
                                                ((EditText)Form.Items.Item("etCarPriDt").Specific).String = cartaoModel.primPagEm.ToString("dd/MM/yyyy");
                                                ((EditText)Form.Items.Item("etCarPgAd").Specific).String = cartaoModel.adcVal.ToString();
                                                ((ComboBox)Form.Items.Item("cbCarDiv").Specific).Select(cartaoModel.divComp ? "Y" : "N", BoSearchKey.psk_ByValue);
                                            }
                                            finally
                                            {
                                                Form.Freeze(false);
                                                bloqueado = false;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_VALIDATE:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID.Equals("mtCheque"))
                                {
                                    if (pVal.ColUID == "filial")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].filial = ((EditText)matrix.Columns.Item("filial").Cells.Item(pVal.Row).Specific).String;
                                    }

                                    if (pVal.ColUID == "vcto")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;
                                                                                
                                        m_ChequeList[pVal.Row - 1].vcto = Controller.MainController.ConvertDate(((EditText)matrix.Columns.Item("vcto").Cells.Item(pVal.Row).Specific).String);
                                    }

                                    if (pVal.ColUID == "valor")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].valor = Convert.ToDouble(((EditText)matrix.Columns.Item("valor").Cells.Item(pVal.Row).Specific).String);

                                        if (matrix.RowCount == pVal.Row && m_ChequeList[pVal.Row - 1].valor > 0)
                                        {
                                            matrix.AddRow();
                                            ((ComboBox)matrix.Columns.Item("pais").Cells.Item(1).Specific).Select("BR");
                                            ((ComboBox)matrix.Columns.Item("endosso").Cells.Item(1).Specific).Select("N");
                                        }

                                        CalcularTotais();
                                    }

                                    if (pVal.ColUID == "numero")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].numero = Convert.ToInt32(((EditText)matrix.Columns.Item("numero").Cells.Item(pVal.Row).Specific).String);
                                    }

                                    if (pVal.ColUID == "contaC")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].contaC = ((EditText)matrix.Columns.Item("contaC").Cells.Item(pVal.Row).Specific).String;
                                    }
                                }

                                if (pVal.ItemUID.Equals("etCarConta"))
                                {
                                    Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                    m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].contaC = ((EditText)Form.Items.Item("etCarConta").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etCarValor"))
                                {
                                    if (pVal.ItemChanged)
                                    {
                                        Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                        Form.Freeze(true);
                                        bloqueado = true;
                                        try
                                        {
                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].valor = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etCarValor").Specific).String);

                                            if (((EditText)Form.Items.Item("etCarPriDt").Specific).String == "")
                                            {
                                                ((EditText)Form.Items.Item("etCarPriDt").Specific).String = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).ToString("dd/MM/yyyy");
                                            }

                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos = Convert.ToInt32(((EditText)Form.Items.Item("etCarPgtos").Specific).String);

                                            double adcValor = Convert.ToInt32(m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].valor) / m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos;

                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].adcVal = adcValor;

                                            double priPgto = adcValor + (m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].valor - (adcValor * m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos));

                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].primPagVal = priPgto;

                                            ((EditText)Form.Items.Item("etCarPriVl").Specific).String = priPgto.ToString();

                                            ((EditText)Form.Items.Item("etCarPgAd").Specific).String = adcValor.ToString();
                                        }
                                        finally
                                        {
                                            Form.Freeze(false);
                                            bloqueado = false;
                                        }

                                        CalcularTotais();
                                    }
                                }

                                if (pVal.ItemUID.Equals("etCarPgtos"))
                                {
                                    if (pVal.ItemChanged)
                                    {
                                        Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                        Form.Freeze(true);
                                        bloqueado = true;
                                        try
                                        {
                                            if (((EditText)Form.Items.Item("etCarPgtos").Specific).String == "" || Convert.ToInt32(((EditText)Form.Items.Item("etCarPgtos").Specific).String) == 0)
                                            {
                                                ((EditText)Form.Items.Item("etCarPgtos").Specific).String = "1";
                                            }

                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos = Convert.ToInt32(((EditText)Form.Items.Item("etCarPgtos").Specific).String);

                                            double adcValor = Convert.ToInt32(m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].valor) / m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos;

                                            double priPgto = adcValor + (m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].valor - (adcValor * m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos));

                                            if (m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos == 1)
                                                adcValor = 0;

                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].primPagVal = priPgto;

                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].adcVal = adcValor;

                                            ((EditText)Form.Items.Item("etCarPriVl").Specific).String = priPgto.ToString();

                                            ((EditText)Form.Items.Item("etCarPgAd").Specific).String = adcValor.ToString();
                                        }
                                        finally
                                        {
                                            Form.Freeze(false);
                                            bloqueado = false;
                                        }
                                    }
                                }

                                if (pVal.ItemUID.Equals("etCarPriVl"))
                                {
                                    if (pVal.ItemChanged)
                                    {
                                        Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                        double priPgto = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etCarPriVl").Specific).String);

                                        if (priPgto >= m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].valor)
                                        {
                                            ((EditText)Form.Items.Item("etCarPgtos").Specific).String = "1";
                                            return;
                                        }

                                        Form.Freeze(true);
                                        bloqueado = true;
                                        try
                                        {
                                            double adcValor = (m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].valor - priPgto) / (m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].pgtos - 1);

                                            m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].adcVal = adcValor;

                                            ((EditText)Form.Items.Item("etCarPgAd").Specific).String = adcValor.ToString();
                                        }
                                        finally
                                        {
                                            Form.Freeze(false);
                                            bloqueado = false;
                                        }
                                    }
                                }

                                if (pVal.ItemUID.Equals("etCarCprv"))
                                {
                                    if (pVal.ItemChanged)
                                    {
                                        Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                        m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].comprov = ((EditText)Form.Items.Item("etCarCprv").Specific).String;
                                    }
                                }

                                if (pVal.ItemUID.Equals("etContaBol"))
                                {
                                    m_BoletoModel.contaC = ((EditText)Form.Items.Item("etContaBol").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etBolNum"))
                                {
                                    m_BoletoModel.numero = Controller.MainController.ConvertInt(((EditText)Form.Items.Item("etBolNum").Specific).String);
                                }

                                if (pVal.ItemUID.Equals("etBolVcto"))
                                {
                                    m_BoletoModel.vcto = Controller.MainController.ConvertDate(((EditText)Form.Items.Item("etBolVcto").Specific).String);
                                }

                                if (pVal.ItemUID.Equals("etBolRef"))
                                {
                                    m_BoletoModel.ref1 = ((EditText)Form.Items.Item("etBolRef").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etBolRef2"))
                                {
                                    m_BoletoModel.ref2 = ((EditText)Form.Items.Item("etBolRef2").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etBolObs"))
                                {
                                    m_BoletoModel.obs = ((EditText)Form.Items.Item("etBolObs").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etBolCodB"))
                                {
                                    m_BoletoModel.codBarras = ((EditText)Form.Items.Item("etBolCodB").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etBolTotal"))
                                {
                                    m_BoletoModel.valor = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etBolTotal").Specific).String);

                                    CalcularTotais();
                                }

                                if (pVal.ItemUID.Equals("etBolConta"))
                                {
                                    m_BoletoModel.conta = ((EditText)Form.Items.Item("etBolConta").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etContaTr"))
                                {
                                    m_TransferenciaModel.contaC = ((EditText)Form.Items.Item("etContaTr").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etDataTr"))
                                {
                                    m_TransferenciaModel.data = Controller.MainController.ConvertDate(((EditText)Form.Items.Item("etDataTr").Specific).String);
                                }

                                if (pVal.ItemUID.Equals("etRefTr"))
                                {
                                    m_TransferenciaModel.ref1 = ((EditText)Form.Items.Item("etRefTr").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etTotalTr"))
                                {
                                    m_TransferenciaModel.valor = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etTotalTr").Specific).String);
                                }

                                if (pVal.ItemUID.Equals("etContaDin"))
                                {
                                    m_DinheiroModel.contaC = ((EditText)Form.Items.Item("etContaDin").Specific).String;
                                }

                                if (pVal.ItemUID.Equals("etTotalDin"))
                                {
                                    m_DinheiroModel.valor = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etTotalDin").Specific).String);

                                    CalcularTotais();
                                }

                                if (pVal.ItemUID.Equals("etEncargo"))
                                {                                    
                                    CalcularTotais();
                                }
                            }
                            break;
                        case BoEventTypes.et_COMBO_SELECT:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID.Equals("mtCheque"))
                                {
                                    if (pVal.ColUID == "pais")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].pais = ((ComboBox)matrix.Columns.Item("pais").Cells.Item(pVal.Row).Specific).Selected.Value;
                                    }

                                    if (pVal.ColUID == "banco")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].banco = ((ComboBox)matrix.Columns.Item("banco").Cells.Item(pVal.Row).Specific).Selected.Value;

                                        if (pVal.ItemChanged)
                                        {
                                            for (int i = matrix.Columns.Item("conta").ValidValues.Count - 1; i >= 0; i--)
                                            {
                                                matrix.Columns.Item("conta").ValidValues.Remove(i, BoSearchKey.psk_Index);
                                            }

                                            string query = @"select ""AbsEntry"", ""Account"" from DSC1 where ""BankCode"" = '" + m_ChequeList[pVal.Row - 1].banco + "'";

                                            SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                            recordSet.DoQuery(query);

                                            while (!recordSet.EoF)
                                            {
                                                matrix.Columns.Item("conta").ValidValues.Add(recordSet.Fields.Item(1).Value.ToString(), "");

                                                recordSet.MoveNext();
                                            }
                                        }
                                    }

                                    if (pVal.ColUID == "conta")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].conta = ((ComboBox)matrix.Columns.Item("conta").Cells.Item(pVal.Row).Specific).Selected.Value;

                                        if (pVal.ItemChanged)
                                        {
                                            string query = @"select ""Branch"", ""GLAccount"", COALESCE(""NextCheck"", 0) from DSC1 where ""BankCode"" = '" + m_ChequeList[pVal.Row - 1].banco + @"' and ""Account"" = '" + m_ChequeList[pVal.Row - 1].conta + "'";

                                            SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                            recordSet.DoQuery(query);

                                            if (!recordSet.EoF)
                                            {
                                                m_ChequeList[pVal.Row - 1].filial = recordSet.Fields.Item(0).Value.ToString();
                                                m_ChequeList[pVal.Row - 1].contaC = recordSet.Fields.Item(1).Value.ToString();

                                                ((EditText)matrix.Columns.Item("filial").Cells.Item(pVal.Row).Specific).String = recordSet.Fields.Item(0).Value.ToString();
                                                ((EditText)matrix.Columns.Item("contaC").Cells.Item(pVal.Row).Specific).String = recordSet.Fields.Item(1).Value.ToString();
                                                matrix.Columns.Item("numero").Editable = true;
                                                try
                                                {
                                                    ((EditText)matrix.Columns.Item("numero").Cells.Item(pVal.Row).Specific).String = recordSet.Fields.Item(2).Value.ToString();

                                                    Controller.MainController.Application.SendKeys("{TAB}");
                                                }
                                                finally
                                                {
                                                    matrix.Columns.Item("numero").Editable = false;
                                                }
                                            }
                                        }
                                    }

                                    if (pVal.ColUID == "endosso")
                                    {
                                        if (m_ChequeList.Count < pVal.Row)
                                            m_ChequeList.Add(new Model.ChequeModel());

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        m_ChequeList[pVal.Row - 1].endosso = ((ComboBox)matrix.Columns.Item("endosso").Cells.Item(pVal.Row).Specific).Selected.Value == "Y";
                                    }
                                }

                                if (pVal.ItemUID.Equals("cbCarCred"))
                                {
                                    Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                    if (m_CartaoList.Count < matrixComprovante.GetNextSelectedRow())
                                    {
                                        m_CartaoList.Add(new Model.CartaoModel());

                                        bloqueado = true;
                                        ((EditText)Form.Items.Item("etCarPgtos").Specific).String = "1";
                                        ((ComboBox)Form.Items.Item("cbCarDiv").Specific).Select(((CheckBox)Form.Items.Item("ckCarDiv").Specific).Checked ? "Y" : "N", BoSearchKey.psk_ByValue);
                                        bloqueado = false;
                                    }

                                    m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].nome = Convert.ToInt32(((ComboBox)Form.Items.Item("cbCarCred").Specific).Selected.Value);

                                    CarregarMatrixComprovante();
                                }

                                if (pVal.ItemUID.Equals("cbCarDiv"))
                                {
                                    Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                    m_CartaoList[matrixComprovante.GetNextSelectedRow() - 1].divComp = ((ComboBox)Form.Items.Item("cbCarDiv").Specific).Selected.Value == "Y";
                                }

                                if (pVal.ItemUID.Equals("cbBolForp"))
                                {
                                    m_BoletoModel.formaPagto = Controller.MainController.ConvertStringCombo((ComboBox)Form.Items.Item("cbBolForp").Specific);
                                }

                                if (pVal.ItemUID.Equals("cbBolSta"))
                                {
                                    m_BoletoModel.status = Controller.MainController.ConvertStringCombo((ComboBox)Form.Items.Item("cbBolSta").Specific);
                                }

                                if (pVal.ItemUID.Equals("cbBolPais"))
                                {
                                    m_BoletoModel.pais = Controller.MainController.ConvertStringCombo((ComboBox)Form.Items.Item("cbBolPais").Specific);
                                }

                                if (pVal.ItemUID.Equals("cbBolBanco"))
                                {
                                    m_BoletoModel.banco = Controller.MainController.ConvertStringCombo((ComboBox)Form.Items.Item("cbBolBanco").Specific);
                                }
                            }
                            break;
                        case BoEventTypes.et_CHOOSE_FROM_LIST:
                            {
                                if (!pVal.BeforeAction)
                                {
                                    IChooseFromListEvent chooseFromListEvent = ((IChooseFromListEvent)(pVal));

                                    if (chooseFromListEvent.SelectedObjects != null)
                                    {
                                        if (pVal.ItemUID.Equals("etContaTr"))
                                        {
                                            Form.DataSources.DataTables.Item("dtTransf").SetValue("contaC", 0, chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString());
                                            ((StaticText)Form.Items.Item("stContaTr").Specific).Caption = chooseFromListEvent.SelectedObjects.GetValue("AcctName", 0).ToString();
                                        }

                                        if (pVal.ItemUID.Equals("etCarConta"))
                                        {
                                            Form.DataSources.DataTables.Item("dtCartao").SetValue("contaC", 0, chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString());                                            
                                        }

                                        if (pVal.ItemUID.Equals("etContaDin"))
                                        {
                                            Form.DataSources.DataTables.Item("dtDin").SetValue("contaC", 0, chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString());
                                            ((StaticText)Form.Items.Item("stContaDin").Specific).Caption = chooseFromListEvent.SelectedObjects.GetValue("AcctName", 0).ToString();
                                        }

                                        if (pVal.ItemUID.Equals("etContaBol"))
                                        {
                                            Form.DataSources.DataTables.Item("dtBoleto").SetValue("contaC", 0, chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString());
                                            ((StaticText)Form.Items.Item("stContaBol").Specific).Caption = chooseFromListEvent.SelectedObjects.GetValue("AcctName", 0).ToString();
                                        }

                                        if (pVal.ItemUID.Equals("mtCheque"))
                                        {
                                            if (pVal.ColUID == "contaC")
                                            {
                                                Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                                try
                                                {
                                                    ((EditText)matrix.Columns.Item("contaC").Cells.Item(pVal.Row).Specific).String = chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString();
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_FORM_CLOSE:
                            {
                                if (!pVal.BeforeAction)
                                {
                                    Controller.MainController.Application.ItemEvent -= HandleItemEvent;
                                    Controller.MainController.Application.MenuEvent -= HandleMenuEvent;
                                    Controller.MainController.Application.RightClickEvent -= HandleRightClickEvent;
                                }
                            }
                            break;
                        case BoEventTypes.et_GOT_FOCUS:
                            if (!pVal.BeforeAction)
                            {
                                if (!Loaded)
                                {
                                    try
                                    {

                                        m_DinheiroModel = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetDinheiroModel();

                                        Controller.MainController.ModelToDataTable<Model.DinheiroModel>(Form.DataSources.DataTables.Item("dtDin"),
                                            Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetDinheiroModel());

                                        m_TransferenciaModel = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTransferenciaModel();

                                        Controller.MainController.ModelToDataTable<Model.TransferenciaModel>(Form.DataSources.DataTables.Item("dtTransf"),
                                            Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTransferenciaModel());

                                        m_BoletoModel = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetBoletoModel();

                                        Controller.MainController.ModelToDataTable<Model.BoletoModel>(Form.DataSources.DataTables.Item("dtBoleto"),
                                            Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetBoletoModel());

                                        m_ChequeList = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetChequeList();

                                        Form.DataSources.DataTables.Item("dtCartao").Rows.Add();

                                        m_CartaoList = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetCartaoList();

                                        Controller.MainController.ListToMatrix<Model.ChequeModel>(Form.DataSources.DataTables.Item("dtCheque"), (Matrix)Form.Items.Item("mtCheque").Specific, Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetChequeList(), true);

                                        Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

                                        if (matrix.RowCount == 0)
                                            matrix.AddRow();

                                        string query = @"select ""Code"", ""Name"" from OCRY where exists (select * from ODSC where ""CountryCod"" = ""Code"")";

                                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(query);

                                        while (!recordSet.EoF)
                                        {
                                            matrix.Columns.Item("pais").ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            ((ComboBox)Form.Items.Item("cbBolPais").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        query = @"select ""BankCode"", ""BankName"" from ODSC";

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(query);

                                        while (!recordSet.EoF)
                                        {
                                            matrix.Columns.Item("banco").ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            ((ComboBox)Form.Items.Item("cbBolBanco").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        query = @"SELECT ""PayMethCod"", ""Descript"" FROM OPYM where ""Type"" = 'O'";

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(query);

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbBolForp").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        matrix.Columns.Item("endosso").ValidValues.Add("Y", "Sim");
                                        matrix.Columns.Item("endosso").ValidValues.Add("N", "Não");

                                        ((ComboBox)Form.Items.Item("cbCarCred").Specific).ValidValues.Add("", "");

                                        query = @"select ""CreditCard"", ""CardName"" from OCRC";

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(query);

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbCarCred").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        ((ComboBox)Form.Items.Item("cbCarDiv").Specific).ValidValues.Add("Y", "Sim");
                                        ((ComboBox)Form.Items.Item("cbCarDiv").Specific).ValidValues.Add("N", "Não");

                                        CarregarMatrixComprovante();

                                        ((ComboBox)Form.Items.Item("cbBolSta").Specific).ValidValues.Add("", "");
                                        ((ComboBox)Form.Items.Item("cbBolSta").Specific).ValidValues.Add("G", "Gerado");

                                        Form.DataSources.DataTables.Item("dtBoleto").SetValue("contaC", 0, "2.1.1.01.001");
                                        ((StaticText)Form.Items.Item("stContaBol").Specific).Caption = "Fornecedores Nacionais";

                                        Form.Items.Item("flTransf").Click();

                                        Form.DataSources.UserDataSources.Item("moeda").Value = "R$";

                                        Form.DataSources.UserDataSources.Item("total").Value = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTotalPagar().ToString();

                                        Form.DataSources.UserDataSources.Item("encargo").Value = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetEncargo().ToString();

                                        ((CheckBox)Form.Items.Item("ckCarDiv").Specific).Checked = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetDividirTransacoesCartao();

                                        CalcularTotais();
                                    }
                                    catch (Exception exception)
                                    {
                                        //Controller.MainController.Application.StatusBar.SetText(exception.Message);
                                    }
                                    finally
                                    {
                                        Loaded = true;
                                    }
                                }

                                if (pVal.ItemUID.Equals("etCarPriVl"))
                                {
                                    if (((EditText)Form.Items.Item("etCarPgtos").Specific).String == "1")
                                        Controller.MainController.Application.SendKeys("{TAB}");
                                }

                                if (pVal.ItemUID.Equals("etCarPgAd"))
                                {
                                    Controller.MainController.Application.SendKeys("{TAB}");
                                }

                                if (pVal.ItemUID.Equals("cbCarCred"))
                                {
                                    Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

                                    if (matrixComprovante.GetNextSelectedRow() <= 0)
                                        matrixComprovante.Columns.Item("linha").Cells.Item(matrixComprovante.RowCount).Click();
                                }

                                if (pVal.ItemUID.Equals("etCarConta") || pVal.ItemUID.Equals("etCarValor") ||
                                    pVal.ItemUID.Equals("etCarPgtos") || pVal.ItemUID.Equals("etCarPriDt") ||
                                    pVal.ItemUID.Equals("etCarCprv") || pVal.ItemUID.Equals("cbCarDiv"))
                                {
                                    if (((ComboBox)Form.Items.Item("cbCarCred").Specific).Selected == null || ((ComboBox)Form.Items.Item("cbCarCred").Specific).Selected.Value == "")
                                    {
                                        Form.ActiveItem = "cbCarCred";
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Controller.MainController.Application.StatusBar.SetText(exception.Message);
            }
        }

        private void CarregarMatrixCheque()
        {
            string query = "";

            int linha = 1;

            foreach (Model.ChequeModel chequeModel in m_ChequeList)
            {
                SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                query += @" select to_date('" + chequeModel.vcto.ToString("yyyy-MM-dd") + @"', 'yyyy-MM-dd') as ""vcto"", " +
                                    chequeModel.valor.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + @" as ""valor"", " +
                                    "'" + chequeModel.pais + @"' as ""pais"", " +
                                    "'" + chequeModel.banco + @"' as ""banco"", " +
                                    "'" + chequeModel.filial + @"' as ""filial"", " +
                                    "'" + chequeModel.conta + @"' as ""conta"", " +
                                    (chequeModel.manual ? "'Y'" : "'N'") + @" as ""manual"", " +
                                    chequeModel.numero + @" as ""numero"", " +
                                    "'" + chequeModel.contaC + @"' as ""contaC"", " +
                                    (chequeModel.endosso ? "'Y'" : "'N'") + @" as ""endosso"" ";



                query += " from dummy ";

                query += "union all";

                linha++;
            }

            if (m_ChequeList.Count == 0)
            {
                query += @" select      cast(null as date) as ""vcto"", " +
                                        @"cast(0.0 as numeric(10,2)) as ""valor"", " +
                                        @"cast('BR' as varchar(2)) as ""pais"", " +
                                        @"cast('' as varchar(10)) as ""banco"", " +
                                        @"cast('' as varchar(30)) as ""filial"", " +
                                        @"cast('' as varchar(50)) as ""conta"", " +
                                        @"'N' as ""manual"", " +
                                        @"0 as ""numero"", " +
                                        @"cast('' as varchar(15)) as ""contaC"", " +
                                        @"'N' as ""endosso"" ";



                query += " from dummy ";

            }
            else
            {
                query = query.Substring(0, query.Length - 10);
            }

            System.IO.File.WriteAllText("query.sql", query);

            Form.DataSources.DataTables.Item("dtCheque").ExecuteQuery(query);

            Matrix matrix = (Matrix)Form.Items.Item("mtCheque").Specific;

            matrix.LoadFromDataSource();

            matrix.AutoResizeColumns();
        }

        private void CarregarMatrixComprovante()
        {
            string query = "";

            int linha = 1;

            foreach (Model.CartaoModel cartaoModel in m_CartaoList)
            {
                SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSet.DoQuery(@"select ""CardName"" from OCRC where ""CreditCard"" = " + cartaoModel.nome.ToString());

                query += @" select " + linha.ToString() + @" as ""linha"", '" +
                    recordSet.Fields.Item(0).Value.ToString() + @"' as ""cartao"" ";



                                            query += " from dummy ";

                query += "union all";

                linha++;
            }

            query += @" select " + linha.ToString() + @" as ""linha"", 'Definir novo' as ""cartao"" ";



                                            query += " from dummy ";

            Form.DataSources.DataTables.Item("dtComprov").ExecuteQuery(query);

            Matrix matrixComprovante = (Matrix)Form.Items.Item("mtComprov").Specific;

            int selectedRow = matrixComprovante.GetNextSelectedRow();

            if (selectedRow <= 0)
                selectedRow = linha;

            matrixComprovante.LoadFromDataSource();

            matrixComprovante.SelectRow(selectedRow, true, false);

            matrixComprovante.AutoResizeColumns();
        }

        private void CalcularTotais()
        {            
            Form.DataSources.UserDataSources.Item("carTotal").Value = m_CartaoList.Sum(r => r.valor).ToString();

            double totalPagamentos = m_ChequeList.Sum(r => r.valor) + m_TransferenciaModel.valor + m_DinheiroModel.valor + m_CartaoList.Sum(r => r.valor) + m_BoletoModel.valor;

            double totalComEncargo = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTotalPagar() +
                Controller.MainController.ConvertDouble(Form.DataSources.UserDataSources.Item("encargo").Value);

            double saldo = totalComEncargo - totalPagamentos;

            double pago = Controller.MainController.GetMeioPagtoParent(Form.UniqueID).GetTotalPagar() - saldo;

            Form.DataSources.UserDataSources.Item("saldo").Value = saldo.ToString();

            Form.DataSources.UserDataSources.Item("pago").Value = pago.ToString();
        }

    }
}
