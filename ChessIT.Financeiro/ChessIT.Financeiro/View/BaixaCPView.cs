using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace ChessIT.Financeiro.View
{
    class BaixaCPView
    {
        Form Form;

        bool Loaded;

        bool bloqueado = false;

        private List<Model.ChequeModel> m_ChequeList = new List<Model.ChequeModel>();
        private List<Model.CartaoModel> m_CartaoList = new List<Model.CartaoModel>();
        private Model.TransferenciaModel m_TransferenciaModel = new Model.TransferenciaModel();
        private Model.DinheiroModel m_DinheiroModel = new Model.DinheiroModel();
        private Model.BoletoModel m_BoletoModel = new Model.BoletoModel();

        private double m_Encargo = 0;

        private bool m_DividirTransacoesCartao = false;

        private Dictionary<int, double> m_TotaisParcela = new Dictionary<int, double>();
        private Dictionary<int, double> m_TotaisDesconto = new Dictionary<int, double>();
        private Dictionary<int, double> m_TotaisJuros = new Dictionary<int, double>();
        private Dictionary<int, double> m_TotaisPagar = new Dictionary<int, double>();

        public BaixaCPView(Form form)
        {
            this.Form = form;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);

            Controller.MainController.Application.ItemEvent += HandleItemEvent;
        }

        public List<Model.ChequeModel> GetChequeList()
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            return m_ChequeList;
        }

        public void SetChequeList(List<Model.ChequeModel> chequeList)
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            m_ChequeList = chequeList;
        }

        public List<Model.CartaoModel> GetCartaoList()
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            return m_CartaoList;
        }

        public void SetCartaoList(List<Model.CartaoModel> cartaoList)
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            m_CartaoList = cartaoList;
        }

        public Model.TransferenciaModel GetTransferenciaModel()
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            return m_TransferenciaModel;
        }

        public void SetTransferenciaModel(Model.TransferenciaModel transferenciaModel)
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            m_TransferenciaModel = transferenciaModel;
        }

        public Model.BoletoModel GetBoletoModel()
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            return m_BoletoModel;
        }

        public void SetBoletoModel(Model.BoletoModel boletoModel)
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            m_BoletoModel = boletoModel;
        }

        public Model.DinheiroModel GetDinheiroModel()
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            return m_DinheiroModel;
        }

        public void SetDinheiroModel(Model.DinheiroModel dinheiroModel)
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            m_DinheiroModel = dinheiroModel;
        }

        public double GetTotalPagar()
        {
            return Controller.MainController.ConvertDouble(Form.DataSources.UserDataSources.Item("totalPagar").Value);
        }

        public string GetFornecedor()
        {
            string fornecedor = ((EditText)Form.Items.Item("etForCod").Specific).String;

            return fornecedor;
        }

        public double GetEncargo()
        {
            return m_Encargo;
        }

        public void SetEncargo(double valor)
        {
            m_Encargo = valor;
        }

        public bool GetDividirTransacoesCartao()
        {
            return m_DividirTransacoesCartao;
        }

        public void SetDividirTransacoesCartao(bool valor)
        {
            m_DividirTransacoesCartao = valor;
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
                        case BoEventTypes.et_FORM_RESIZE:
                            if (!pVal.BeforeAction)
                            {
                                if (!Loaded)
                                    return;

                                PosicionarTotais();
                            }
                            break;
                        case BoEventTypes.et_CLICK:
                            {
                                if (pVal.BeforeAction)
                                {
                                    if (pVal.ItemUID == "btPesquisa")
                                    {
                                        Pesquisar();
                                    }

                                    if (pVal.ItemUID == "btLimpar")
                                    {
                                        Limpar();
                                    }

                                    if (pVal.ItemUID == "btMeioPgto")
                                    {
                                        Controller.MainController.OpenMeioPagtoView(Form.UniqueID);
                                    }

                                    if (pVal.ItemUID == "btAplJM")
                                    {
                                        AplicarMultaJuros();
                                    }

                                    if (pVal.ItemUID == "btPagar")
                                    {
                                        Pagar();
                                    }

                                    if (pVal.ItemUID == "gridTitulo")
                                    {
                                        if (pVal.ColUID == "Check")
                                        {
                                            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

                                            if (!((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(pVal.Row))
                                            {
                                                double valorParcela = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Parcela")).GetText(pVal.Row));
                                                double valorDesc = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Desc.")).GetText(pVal.Row));
                                                double valorJuros = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Juros")).GetText(pVal.Row));
                                                double valorPagar = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).GetText(pVal.Row));

                                                if (!m_TotaisParcela.ContainsKey(pVal.Row))
                                                    m_TotaisParcela.Add(pVal.Row, 0);

                                                m_TotaisParcela[pVal.Row] = valorParcela;

                                                if (!m_TotaisJuros.ContainsKey(pVal.Row))
                                                    m_TotaisJuros.Add(pVal.Row, 0);

                                                m_TotaisJuros[pVal.Row] = valorJuros;

                                                if (!m_TotaisDesconto.ContainsKey(pVal.Row))
                                                    m_TotaisDesconto.Add(pVal.Row, 0);

                                                m_TotaisDesconto[pVal.Row] = valorDesc;

                                                if (!m_TotaisPagar.ContainsKey(pVal.Row))
                                                    m_TotaisPagar.Add(pVal.Row, 0);

                                                m_TotaisPagar[pVal.Row] = valorPagar;
                                            }
                                            else
                                            {
                                                if (m_TotaisParcela.ContainsKey(pVal.Row))
                                                    m_TotaisParcela.Remove(pVal.Row);

                                                if (m_TotaisJuros.ContainsKey(pVal.Row))
                                                    m_TotaisJuros.Remove(pVal.Row);

                                                if (m_TotaisDesconto.ContainsKey(pVal.Row))
                                                    m_TotaisDesconto.Remove(pVal.Row);

                                                if (m_TotaisPagar.ContainsKey(pVal.Row))
                                                    m_TotaisPagar.Remove(pVal.Row);
                                            }

                                            Totalizar();
                                        }       
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_MATRIX_LINK_PRESSED:
                            if (pVal.BeforeAction)
                            {
                                if (pVal.ItemUID == "gridTitulo")
                                {
                                    if (pVal.ColUID == "Nº SAP")
                                    {
                                        string numeroInterno = ((EditTextColumn)((Grid)Form.Items.Item("gridTitulo").Specific).Columns.Item("Nº Interno")).GetText(pVal.Row);

                                        switch (((EditTextColumn)((Grid)Form.Items.Item("gridTitulo").Specific).Columns.Item("Tipo Doc")).GetText(pVal.Row))
                                        {
                                            case "NE":
                                                Controller.MainController.Application.OpenForm(BoFormObjectEnum.fo_PurchaseInvoice, "18", numeroInterno);
                                                break;
                                            case "ADT":
                                                Controller.MainController.Application.OpenForm((BoFormObjectEnum)204, "204", numeroInterno);
                                                break;
                                            case "LC":
                                                Controller.MainController.Application.OpenForm(BoFormObjectEnum.fo_JournalPosting, "30", numeroInterno);
                                                break;
                                        }

                                        bubbleEvent = false;
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_ITEM_PRESSED:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID == "ckTodos")
                                {
                                    ((CheckBox)Form.Items.Item("ckBaixa").Specific).Checked = ((CheckBox)Form.Items.Item("ckTodos").Specific).Checked;
                                    ((CheckBox)Form.Items.Item("ckPendente").Specific).Checked = ((CheckBox)Form.Items.Item("ckTodos").Specific).Checked;

                                    Pesquisar();
                                }
                                else if (pVal.ItemUID == "gridTitulo" && pVal.ColUID == "Check")
                                {
                                    if (!pVal.BeforeAction)
                                    {
                                        if (((CheckBoxColumn)((Grid)Form.Items.Item("gridTitulo").Specific).Columns.Item("Check")).IsChecked(pVal.Row))
                                            ((Grid)Form.Items.Item("gridTitulo").Specific).Rows.SelectedRows.Add(pVal.Row);
                                        else
                                            ((Grid)Form.Items.Item("gridTitulo").Specific).Rows.SelectedRows.Remove(pVal.Row);
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_VALIDATE:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID.Equals("etForCod"))
                                {
                                    if (((EditText)Form.Items.Item("etForCod").Specific).String.Trim() == "")
                                    {                                        
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("forNome", 0, "");
                                    }
                                }

                                if (pVal.ItemUID.Equals("gridTitulo"))
                                {
                                    if (pVal.ColUID == "Valor Desc." || pVal.ColUID == "Valor Multa" || pVal.ColUID == "Valor Juros")
                                    {
                                        Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

                                        double valorParcela = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Parcela")).GetText(pVal.Row));

                                        double valorDesc = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Desc.")).GetText(pVal.Row));

                                        double valorMulta = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Multa")).GetText(pVal.Row));

                                        double valorJuros = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Juros")).GetText(pVal.Row));

                                        double valorTotal = valorParcela + valorMulta + valorJuros - valorDesc;

                                        ((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).SetText(pVal.Row, valorTotal.ToString());

                                        if (((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(pVal.Row))
                                        {
                                            if (!m_TotaisJuros.ContainsKey(pVal.Row))
                                                m_TotaisJuros.Add(pVal.Row, 0);

                                            m_TotaisJuros[pVal.Row] = valorJuros;

                                            if (!m_TotaisDesconto.ContainsKey(pVal.Row))
                                                m_TotaisDesconto.Add(pVal.Row, 0);

                                            m_TotaisDesconto[pVal.Row] = valorDesc;

                                            if (!m_TotaisPagar.ContainsKey(pVal.Row))
                                                m_TotaisPagar.Add(pVal.Row, 0);

                                            m_TotaisPagar[pVal.Row] = valorTotal;
                                        }

                                        Totalizar();
                                    }

                                    if (pVal.ColUID == "Total a Pagar")
                                    {
                                        Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

                                        double valorTotal = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).GetText(pVal.Row));

                                        if (((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(pVal.Row))
                                        {                                            
                                            if (!m_TotaisPagar.ContainsKey(pVal.Row))
                                                m_TotaisPagar.Add(pVal.Row, 0);

                                            m_TotaisPagar[pVal.Row] = valorTotal;
                                        }

                                        Totalizar();
                                    }
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
                                        if (pVal.ItemUID.Equals("etForCod"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("forCod", 0, chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString());
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("forNome", 0, chooseFromListEvent.SelectedObjects.GetValue("CardName", 0).ToString());
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
                                        Form.DataSources.DataTables.Item("dtFiltro").Rows.Add();

                                        ChooseFromList choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_2");

                                        Conditions conditions = new Conditions();

                                        Condition condition = conditions.Add();

                                        condition.Alias = "CardType";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "S";

                                        choose.SetConditions(conditions);

                                        ((ComboBox)Form.Items.Item("cbForPgto").Specific).ValidValues.Add("", "[Selecionar]");

                                        string query = @"select ""PayMethCod"", ""Descript"" from OPYM where ""Type"" = 'O'";

                                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(query);

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbForPgto").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        ((ComboBox)Form.Items.Item("cbEmpresa").Specific).ValidValues.Add("0", "[Selecionar]");

                                        query = @"select ""BPLId"", ""BPLName"" from OBPL";

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(query);

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbEmpresa").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        ((ComboBox)Form.Items.Item("cbEmpresa").Specific).Select("0");

                                        Form.DataSources.DataTables.Item("dtTitulo").Rows.Clear();

                                        Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

                                        gridTitulos.Columns.Item("Check").Type = BoGridColumnType.gct_CheckBox;
                                        gridTitulos.Columns.Item("Check").TitleObject.Caption = "#";

                                        ((EditText)Form.Items.Item("etDtPagto").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                    }
                                    catch { }
                                    finally
                                    {
                                        Loaded = true;
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

        private void Pesquisar()
        {
            string query = @"select '{14}' AS ""Check"",
                                    --OPCH.""BPLName"" AS ""Filial"",
                                    CASE OPCH.""BPLId"" WHEN 1 THEN 'M' ELSE 'F' END AS ""Filial"",
		                            'NE' AS ""Tipo Doc"",
                                    OPCH.""BPLId"" AS ""Nº Empresa"",
                                    OPCH.""CardCode"" AS ""Nº Fornecedor"",
		                            OPCH.""DocEntry"" AS ""Nº Interno"",
                                    OPCH.""DocNum"" AS ""Nº SAP"",
		                            OPCH.""Serial"" AS ""Nº NF"",
		                            OPCH.""CardName"" AS ""Fornecedor"",
                                    PCH6.""DueDate"" AS ""Data Vcto."",
                                    (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") as ""Data Baixa"",
		                            cast(PCH6.""InstlmntID"" as nvarchar) || '/' || cast((select count(*) from PCH6 aux where aux.""DocEntry"" = OPCH.""DocEntry"") as nvarchar) AS ""Parcela"",
		                            PCH6.""InsTotal"" AS ""Valor Parcela"",
		                            COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) as ""Valor Desc."",
		                            COALESCE((select sum(""U_ValorMulta"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) as ""Valor Multa"",
		                            COALESCE((select sum(""U_ValorDoJurosMora"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) as ""Valor Juros"",
		                            COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) + PCH6.""InsTotal"" - PCH6.""PaidToDate"" AS ""Total a Pagar"",                                    
                                    PCH6.""PaidToDate"" -
                                    COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) +
		                            COALESCE((select sum(""U_ValorMulta"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) +
		                            COALESCE((select sum(""U_ValorDoJurosMora"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Pago"",
                                    COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) + PCH6.""InsTotal"" - PCH6.""PaidToDate"" AS ""Valor Saldo"",
                                    (SELECT ""AcctName"" FROM OACT WHERE ""AcctCode"" =
                                    (select max(case 
		                                    when ""CashSum"" > 0 then ""CashAcct""
		                                    when ""CreditSum"" > 0 then (SELECT MAX(VPM3.""CreditAcct"") FROM VPM3 WHERE VPM3.""DocNum"" = OVPM.""DocEntry"")
                                            when ""TrsfrSum"" > 0 then ""TrsfrAcct""
		                                    when ""BoeSum"" > 0 then ""BoeAcc""
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N')) AS ""Conta"",
                                    (select max(case 
		                                    when ""CashSum"" > 0 then 'Dinheiro' 
		                                    when ""CreditSum"" > 0 then 'Cartão' 
		                                    when ""TrsfrSum"" > 0 then 'Transferência'
		                                    when ""BoeSum"" > 0 then 'Boleto'
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = OPCH.""DocEntry""
                                    and VPM2.""InvType"" = 18
                                    and VPM2.""InstId"" = PCH6.""InstlmntID""
                                    and ""Canceled"" = 'N') AS ""Carteira""
                            from OPCH
                            inner join PCH6 on PCH6.""DocEntry"" = OPCH.""DocEntry"" 
                            inner join JDT1 on JDT1.""TransId"" = OPCH.""TransId"" and JDT1.""SourceLine"" = PCH6.""InstlmntID""							                            
                            where OPCH.""CANCELED"" = 'N'                            
                            and ('{0}' = '' or OPCH.""Serial"" = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(PCH6.""DueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(PCH6.""DueDate"" as date) <= cast('{4}' as date))
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") >= cast('{5}' as date))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") <= cast('{6}' as date))
                            and ({7} = 0 or PCH6.""InsTotal"" >= {7})
                            and ({8} = 0 or PCH6.""InsTotal"" <= {8})
                            and ({9} = 0 or {9} = OPCH.""BPLId"")
                            and ('{10}' = '' or '{10}' = OPCH.""CardCode"")
                            and ('{11}' = '' or '{11}' = OPCH.""PeyMethod"")
                            and ('{12}' = 'N' or '{14}' = 'Y' or ('{12}' = 'Y' and exists (select OITR.""ReconDate"" from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"")))
                            and ('{13}' = 'N' or '{14}' = 'Y' or ('{13}' = 'Y' and not exists (select OITR.""ReconDate"" from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"")))
                            union
                            select  '{14}' AS ""Check"",
                                    CASE ODPO.""BPLId"" WHEN 1 THEN 'M' ELSE 'F' END AS ""Filial"",
		                            'ADT' AS ""Tipo Doc"",
                                    ODPO.""BPLId"" AS ""Nº Empresa"",
                                    ODPO.""CardCode"" AS ""Nº Fornecedor"",
		                            ODPO.""DocEntry"" AS ""Nº Interno"",
                                    ODPO.""DocNum"" AS ""Nº SAP"",
		                            0 AS ""Nº NF"",
		                            ODPO.""CardName"" AS ""Fornecedor"",
                                    DPO6.""DueDate"" AS ""Data Vcto."",
                                    (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") as ""Data Baixa"",
		                            cast(DPO6.""InstlmntID"" as nvarchar) || '/' || cast((select count(*) from DPO6 aux where aux.""DocEntry"" = ODPO.""DocEntry"") as nvarchar) AS ""Parcela"",
		                            DPO6.""InsTotal"" AS ""Valor Parcela"",
                                    COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Desc."",
		                            COALESCE((select sum(""U_ValorMulta"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Multa"",
		                            COALESCE((select sum(""U_ValorDoJurosMora"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) as ""Valor Juros"",
		                            COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) + DPO6.""InsTotal"" - DPO6.""PaidToDate"" AS ""Total a Pagar"",                                    
                                    DPO6.""PaidToDate"" -
                                    COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) +
		                            COALESCE((select sum(""U_ValorMulta"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) +
		                            COALESCE((select sum(""U_ValorDoJurosMora"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Pago"",
                                    COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N'), 0) + DPO6.""InsTotal"" - DPO6.""PaidToDate"" AS ""Valor Saldo"",
                                    (SELECT ""AcctName"" FROM OACT WHERE ""AcctCode"" =
                                    (select max(case 
		                                    when ""CashSum"" > 0 then ""CashAcct""
		                                    when ""CreditSum"" > 0 then (SELECT MAX(VPM3.""CreditAcct"") FROM VPM3 WHERE VPM3.""DocNum"" = OVPM.""DocEntry"")
                                            when ""TrsfrSum"" > 0 then ""TrsfrAcct""
		                                    when ""BoeSum"" > 0 then ""BoeAcc""
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N')) AS ""Conta"",
                                     (select max(case 
		                                    when ""CashSum"" > 0 then 'Dinheiro' 
		                                    when ""CreditSum"" > 0 then 'Cartão' 
		                                    when ""TrsfrSum"" > 0 then 'Transferência'
		                                    when ""BoeSum"" > 0 then 'Boleto'
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = ODPO.""DocEntry""
                                    and VPM2.""InvType"" = 204
                                    and VPM2.""InstId"" = DPO6.""InstlmntID""
                                    and ""Canceled"" = 'N') AS ""Carteira""                                   
                            from ODPO
                            inner join DPO6 on DPO6.""DocEntry"" = ODPO.""DocEntry""
                            inner join JDT1 on JDT1.""TransId"" = ODPO.""TransId"" and JDT1.""SourceLine"" = DPO6.""InstlmntID""                            
                            where ODPO.""CANCELED"" = 'N'
                            and ('{0}' = '')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(DPO6.""DueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(DPO6.""DueDate"" as date) <= cast('{4}' as date))
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") >= cast('{5}' as date))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") <= cast('{6}' as date))
                            and ({7} = 0 or DPO6.""InsTotal"" >= {7})
                            and ({8} = 0 or DPO6.""InsTotal"" <= {8})
                            and ({9} = 0 or {9} = ODPO.""BPLId"")
                            and ('{10}' = '' or '{10}' = ODPO.""CardCode"")
                            and ('{11}' = '' or '{11}' = ODPO.""PeyMethod"")
                            and ('{12}' = 'N' or '{14}' = 'Y' or ('{12}' = 'Y' and exists (select OITR.""ReconDate"" from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"")))
                            and ('{13}' = 'N' or '{14}' = 'Y' or ('{13}' = 'Y' and not exists (select OITR.""ReconDate"" from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"")))
                            union
                            select  '{14}' AS ""Check"",
                                    CASE JDT1.""BPLId"" WHEN 1 THEN 'M' ELSE 'F' END AS ""Filial"",
		                            'LC' AS ""Tipo Doc"",
		                            JDT1.""BPLId"" AS ""Nº Empresa"",
                                    OCRD.""CardCode"" AS ""Nº Fornecedor"",
                                    OJDT.""TransId"" AS ""Nº Interno"",
                                    OJDT.""TransId"" AS ""Nº SAP"",
		                            OJDT.""U_NDocFin"" AS ""Nº NF"",
		                            OCRD.""CardName"" AS ""Fornecedor"",
                                    JDT1.""DueDate"" AS ""Data Vcto."",
                                    (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") as ""Data Baixa"",
		                            cast((JDT1.""Line_ID"" + 1) as varchar(10)) || '/' || cast((select count(*) from JDT1 TX where TX.""TransId"" = JDT1.""TransId"" and TX.""ShortName"" LIKE 'FOR%') as varchar(10)) as ""Parcela"",
		                            JDT1.""Credit"" AS ""Valor Parcela"",
		                            COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = JDT1.""TransId""
                                    and VPM2.""InvType"" = 30
                                    and VPM2.""DocLine"" = JDT1.""Line_ID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Desc."",
		                            COALESCE((select sum(""U_ValorMulta"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = JDT1.""TransId""
                                    and VPM2.""InvType"" = 30
                                    and VPM2.""DocLine"" = JDT1.""Line_ID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Multa"",
		                            COALESCE((select sum(""U_ValorDoJurosMora"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = JDT1.""TransId""
                                    and VPM2.""InvType"" = 30
                                    and VPM2.""DocLine"" = JDT1.""Line_ID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Juros"",
		                            JDT1.""BalDueCred"" AS ""Total A Pagar"",
                                    (JDT1.""Credit"" - JDT1.""BalDueCred"") -
                                    COALESCE((select sum(""U_ValorDoDesconto"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = JDT1.""TransId""
                                    and VPM2.""InvType"" = 30
                                    and VPM2.""DocLine"" = JDT1.""Line_ID""
                                    and ""Canceled"" = 'N'), 0) -
		                            COALESCE((select sum(""U_ValorMulta"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = JDT1.""TransId""
                                    and VPM2.""InvType"" = 30
                                    and VPM2.""DocLine"" = JDT1.""Line_ID""
                                    and ""Canceled"" = 'N'), 0) +
		                            COALESCE((select sum(""U_ValorDoJurosMora"")
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = JDT1.""TransId""
                                    and VPM2.""InvType"" = 30
                                    and VPM2.""DocLine"" = JDT1.""Line_ID""
                                    and ""Canceled"" = 'N'), 0) AS ""Valor Pago"",
                                    JDT1.""BalDueCred"" AS ""Valor Saldo"",
                                    (SELECT ""AcctName"" FROM OACT WHERE ""AcctCode"" =
                                    (select max(case 
		                                    when ""CashSum"" > 0 then ""CashAcct""
		                                    when ""CreditSum"" > 0 then (SELECT MAX(VPM3.""CreditAcct"") FROM VPM3 WHERE VPM3.""DocNum"" = OVPM.""DocEntry"")
                                            when ""TrsfrSum"" > 0 then ""TrsfrAcct""
		                                    when ""BoeSum"" > 0 then ""BoeAcc""
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                    where VPM2.""DocEntry"" = JDT1.""TransId""
                                    and VPM2.""InvType"" = 30
                                    and VPM2.""DocLine"" = JDT1.""Line_ID""
                                    and ""Canceled"" = 'N')) AS ""Conta"",
                                    (select max(case 
		                                when ""CashSum"" > 0 then 'Dinheiro' 
		                                when ""CreditSum"" > 0 then 'Cartão' 
		                                when ""TrsfrSum"" > 0 then 'Transferência'
		                                when ""BoeSum"" > 0 then 'Boleto'
		                                end) 
                                   from VPM2 
                                   left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" 
                                   where VPM2.""DocEntry"" = JDT1.""TransId""
                                   and VPM2.""InvType"" = 30
                                   and VPM2.""DocLine"" = JDT1.""Line_ID""
                                   and ""Canceled"" = 'N') AS ""Carteira""
                            from JDT1
                            inner join OJDT on OJDT.""TransId"" = JDT1.""TransId""
                            inner join OCRD on OCRD.""CardCode"" = JDT1.""ShortName""
							--left join OPYM on OPYM.""PayMethCod"" = OJDT.""U_FPagFin""
                            where OJDT.""TransType"" <> 18
                            and OJDT.""TransType"" <> 204
                            and JDT1.""Credit"" > 0
                            --and ""MthDate"" is null
                            and JDT1.""ShortName"" LIKE 'FOR%'
                            and OJDT.""StornoToTr"" IS NULL
                            and not exists (select * from OJDT aux where aux.""StornoToTr"" = OJDT.""TransId"")
                            and ('{0}' = '' or OJDT.""U_NDocFin"" = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(JDT1.""DueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(JDT1.""DueDate"" as date) <= cast('{4}' as date))
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") >= cast('{5}' as date))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or (select max(OITR.""ReconDate"") from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"") <= cast('{6}' as date))
                            and ({7} = 0 or JDT1.""Credit"" >= {7})
                            and ({8} = 0 or JDT1.""Credit"" <= {8})
                            and ({9} = 0 or {9} = JDT1.""BPLId"")
                            and ('{10}' = '' or '{10}' = JDT1.""ShortName"")
                            and ('{11}' = '' or '{11}' = OJDT.""U_FPagFin"")
                            and ('{12}' = 'N' or '{14}' = 'Y' or ('{12}' = 'Y' and exists (select OITR.""ReconDate"" from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"")))
                            and ('{13}' = 'N' or '{14}' = 'Y' or ('{13}' = 'Y' and not exists (select OITR.""ReconDate"" from ITR1 inner join OITR on OITR.""ReconNum"" = ITR1.""ReconNum"" where OITR.""Canceled"" = 'N' and ITR1.""TransId"" = JDT1.""TransId"" and ITR1.""TransRowId"" = JDT1.""Line_ID"")))";


            string numNF = ((EditText)Form.Items.Item("etNumNF").Specific).String;
            string dataEmissaoDe = ((EditText)Form.Items.Item("etDtEmiIni").Specific).String;
            string dataEmissaoAte = ((EditText)Form.Items.Item("etDtEmiFim").Specific).String;
            string dataVencimentoDe = ((EditText)Form.Items.Item("etDtVctIni").Specific).String;
            string dataVencimentoAte = ((EditText)Form.Items.Item("etDtVctFim").Specific).String;
            string dataBaixaDe = ((EditText)Form.Items.Item("etDtBaiDe").Specific).String;
            string dataBaixaAte = ((EditText)Form.Items.Item("etDtBaiAte").Specific).String;
            string valorDocDe = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etValDocDe").Specific).String).ToString(System.Globalization.CultureInfo.InvariantCulture);
            string valorDocAte = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etValDocAt").Specific).String).ToString(System.Globalization.CultureInfo.InvariantCulture);
            string empresa = Controller.MainController.ConvertStringCombo(((ComboBox)Form.Items.Item("cbEmpresa").Specific));
            string fornecedor = ((EditText)Form.Items.Item("etForCod").Specific).String;
            string formaPagto = ((ComboBox)Form.Items.Item("cbForPgto").Specific).Selected.Value;
            string baixados = ((CheckBox)Form.Items.Item("ckBaixa").Specific).Checked ? "Y" : "N";
            string pendentes = ((CheckBox)Form.Items.Item("ckPendente").Specific).Checked ? "Y" : "N";
            string todos = ((CheckBox)Form.Items.Item("ckTodos").Specific).Checked ? "Y" : "N";

            query = string.Format(query,
                        numNF,
                        dataEmissaoDe == "" ? "1990-01-01" : Convert.ToDateTime(dataEmissaoDe).ToString("yyyy-MM-dd"),
                        dataEmissaoAte == "" ? "1990-01-01" : Convert.ToDateTime(dataEmissaoAte).ToString("yyyy-MM-dd"),
                        dataVencimentoDe == "" ? "1990-01-01" : Convert.ToDateTime(dataVencimentoDe).ToString("yyyy-MM-dd"),
                        dataVencimentoAte == "" ? "1990-01-01" : Convert.ToDateTime(dataVencimentoAte).ToString("yyyy-MM-dd"),
                        dataBaixaDe == "" ? "1990-01-01" : Convert.ToDateTime(dataBaixaDe).ToString("yyyy-MM-dd"),
                        dataBaixaAte == "" ? "1990-01-01" : Convert.ToDateTime(dataBaixaAte).ToString("yyyy-MM-dd"),
                        valorDocDe,
                        valorDocAte,
                        empresa,
                        fornecedor,
                        formaPagto,
                        baixados,
                        pendentes,
                        todos);

            Form.Freeze(true);
            try
            {
                Form.DataSources.DataTables.Item("dtTitulo").ExecuteQuery(query);

                Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

                gridTitulos.Columns.Item("Check").Type = BoGridColumnType.gct_CheckBox;
                gridTitulos.Columns.Item("Check").TitleObject.Caption = "#";

                ((EditTextColumn)gridTitulos.Columns.Item("Nº SAP")).LinkedObjectType = "30";

                gridTitulos.Columns.Item("Nº Empresa").Visible = false;
                gridTitulos.Columns.Item("Nº Fornecedor").Visible = false;
                gridTitulos.Columns.Item("Nº Interno").Visible = false;

                gridTitulos.Columns.Item("Filial").Editable = false;
                gridTitulos.Columns.Item("Tipo Doc").Editable = false;
                gridTitulos.Columns.Item("Nº SAP").Editable = false;
                gridTitulos.Columns.Item("Nº NF").Editable = false;
                gridTitulos.Columns.Item("Fornecedor").Editable = false;
                gridTitulos.Columns.Item("Data Vcto.").Editable = false;
                gridTitulos.Columns.Item("Data Baixa").Editable = false;
                gridTitulos.Columns.Item("Parcela").Editable = false;
                gridTitulos.Columns.Item("Valor Parcela").Editable = false;
                //gridTitulos.Columns.Item("Total a Pagar").Editable = false;
                gridTitulos.Columns.Item("Valor Pago").Editable = false;
                gridTitulos.Columns.Item("Valor Saldo").Editable = false;
                gridTitulos.Columns.Item("Conta").Editable = false;
                gridTitulos.Columns.Item("Carteira").Editable = false;

                gridTitulos.Columns.Item("Filial").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Tipo Doc").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Nº SAP").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Nº NF").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Fornecedor").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Data Vcto.").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Data Baixa").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Parcela").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Valor Parcela").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Total a Pagar").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Valor Pago").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Valor Saldo").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Conta").TitleObject.Sortable = true;
                gridTitulos.Columns.Item("Carteira").TitleObject.Sortable = true;

                PosicionarTotais();

                m_BoletoModel = new Model.BoletoModel();
                m_DinheiroModel = new Model.DinheiroModel();
                m_TransferenciaModel = new Model.TransferenciaModel();
                m_CartaoList = new List<Model.CartaoModel>();
                m_ChequeList = new List<Model.ChequeModel>();

                m_DividirTransacoesCartao = false;
                m_Encargo = 0;

                m_TotaisParcela = new Dictionary<int, double>();
                m_TotaisDesconto = new Dictionary<int, double>();
                m_TotaisJuros = new Dictionary<int, double>();
                m_TotaisPagar = new Dictionary<int, double>();

                if (todos == "Y")
                {
                    SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset) Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    recordSet.DoQuery(query);

                    int row = 0;

                    while(!recordSet.EoF)
                    {
                        m_TotaisParcela.Add(row, Convert.ToDouble(recordSet.Fields.Item(12).Value));
                        m_TotaisDesconto.Add(row, Convert.ToDouble(recordSet.Fields.Item(13).Value));
                        m_TotaisJuros.Add(row, Convert.ToDouble(recordSet.Fields.Item(15).Value));
                        m_TotaisPagar.Add(row, Convert.ToDouble(recordSet.Fields.Item(16).Value));

                        row++;
                        recordSet.MoveNext();
                    }
                }

                Totalizar();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("Sql.sql", query);

                throw ex;
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        private void Limpar()
        {
            Form.DataSources.DataTables.Item("dtFiltro").Rows.Clear();
            Form.DataSources.DataTables.Item("dtFiltro").Rows.Add();

            Form.DataSources.DataTables.Item("dtTitulo").Rows.Clear();

            ((ComboBox)Form.Items.Item("cbEmpresa").Specific).Select("0");

            Form.DataSources.UserDataSources.Item("percMulta").Value = "";
            Form.DataSources.UserDataSources.Item("percJuros").Value = "";

            ((EditText)Form.Items.Item("etDtPagto").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");

            m_BoletoModel = new Model.BoletoModel();
            m_DinheiroModel = new Model.DinheiroModel();
            m_TransferenciaModel = new Model.TransferenciaModel();
            m_CartaoList = new List<Model.CartaoModel>();
            m_ChequeList = new List<Model.ChequeModel>();

            m_DividirTransacoesCartao = false;
            m_Encargo = 0;

            m_TotaisParcela = new Dictionary<int, double>();
            m_TotaisDesconto = new Dictionary<int, double>();
            m_TotaisJuros = new Dictionary<int, double>();
            m_TotaisPagar = new Dictionary<int, double>();

            Totalizar();
        }

        private void AplicarMultaJuros()
        {
            Form.Freeze(true);
            bloqueado = true;
            try
            {
                Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

                int linha = -1;

                try
                {
                    linha = gridTitulos.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
                }
                catch { }

                if (linha == -1)
                {
                    Controller.MainController.Application.StatusBar.SetText("Nenhuma linha selecionada");
                    return;
                }

                double percMulta = Convert.ToDouble(Form.DataSources.UserDataSources.Item("percMulta").Value);

                double percJuros = Convert.ToDouble(Form.DataSources.UserDataSources.Item("percJuros").Value);

                double valorParcela = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Parcela")).GetText(linha));

                double valorDesc = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Desc.")).GetText(linha));

                int diasAtraso = (DateTime.Now - Controller.MainController.ConvertDate(((EditTextColumn)gridTitulos.Columns.Item("Data Vcto.")).GetText(linha))).Days;

                double valorMulta = valorParcela * (percMulta / 100);

                double valorJuros = (((percJuros / 30) / 100) * valorParcela) * diasAtraso;

                double valorTotal = valorParcela + valorMulta + valorJuros - valorDesc;

                ((EditTextColumn)gridTitulos.Columns.Item("Valor Multa")).SetText(linha, valorMulta.ToString());

                ((EditTextColumn)gridTitulos.Columns.Item("Valor Juros")).SetText(linha, valorJuros.ToString());

                ((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).SetText(linha, valorTotal.ToString());

                if (((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(linha))
                {
                    if (!m_TotaisJuros.ContainsKey(linha))
                        m_TotaisJuros.Add(linha, 0);

                    m_TotaisJuros[linha] = valorJuros;

                    if (!m_TotaisPagar.ContainsKey(linha))
                        m_TotaisPagar.Add(linha, 0);

                    m_TotaisPagar[linha] = valorTotal;
                }

                Totalizar();
            }
            finally
            {
                bloqueado = false;
                Form.Freeze(false);
            }
        }

        private void Pagar()
        {
            Controller.MainController.Application.StatusBar.SetText("Criando contas a pagar", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);

            bool erroLog = false;

            List<Model.BaixaCPModel> baixaCPList = new List<Model.BaixaCPModel>();

            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            for (int linha = 0; linha < gridTitulos.Rows.Count; linha++)
            {
                if (((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(linha))
                {
                    string fornecedor = ((EditTextColumn)gridTitulos.Columns.Item("Nº Fornecedor")).GetText(linha);

                    int empresa = Controller.MainController.ConvertInt(((EditTextColumn)gridTitulos.Columns.Item("Nº Empresa")).GetText(linha));

                    int docEntry = Controller.MainController.ConvertInt(((EditTextColumn)gridTitulos.Columns.Item("Nº Interno")).GetText(linha));

                    int parcela = Convert.ToInt32(((EditTextColumn)gridTitulos.Columns.Item("Parcela")).GetText(linha).Split('/')[0]);

                    string tipoDoc = ((EditTextColumn)gridTitulos.Columns.Item("Tipo Doc")).GetText(linha).Trim();

                    double valorParcela = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Parcela")).GetText(linha));

                    double totalPagar = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).GetText(linha));

                    double valorDesc = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Desc.")).GetText(linha));

                    double totalJuros = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Juros")).GetText(linha));

                    double valorMulta = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Multa")).GetText(linha));

                    Model.BaixaCPModel baixaCPModel = new Model.BaixaCPModel();

                    baixaCPModel.Fornecedor = fornecedor;
                    baixaCPModel.Empresa = empresa;
                    baixaCPModel.DocEntry = docEntry;
                    baixaCPModel.Parcela = parcela;
                    baixaCPModel.TipoDoc = tipoDoc;
                    baixaCPModel.TotalPagar = totalPagar;
                    baixaCPModel.ValorParcela = valorParcela;
                    baixaCPModel.ValorDesconto = valorDesc;
                    baixaCPModel.ValorJuros = totalJuros;
                    baixaCPModel.ValorMulta = valorMulta;

                    baixaCPList.Add(baixaCPModel);                        
                }
            }

            var baixaCPGroupList = baixaCPList.GroupBy(r => new { fornecedor = r.Fornecedor, empresa = r.Empresa }).ToList();

            foreach (var baixaCPGroup in baixaCPGroupList)
            {
                string codigoBarraBoleto = "";

                double proporcionalGrupo = baixaCPGroup.Sum(r => r.TotalPagar) / baixaCPList.Sum(r => r.TotalPagar);

                SAPbobsCOM.Payments payments = (SAPbobsCOM.Payments) Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                try
                {
                    payments.CardCode = baixaCPGroup.Key.fornecedor;

                    payments.BPLID = baixaCPGroup.Key.empresa;

                    payments.BankChargeAmount = m_Encargo;                    

                    payments.DocDate = ((EditText)Form.Items.Item("etDtPagto").Specific).String == "" ? DateTime.Now : Controller.MainController.ConvertDate(((EditText)Form.Items.Item("etDtPagto").Specific).String);

                    payments.TaxDate = ((EditText)Form.Items.Item("etDtPagto").Specific).String == "" ? DateTime.Now : Controller.MainController.ConvertDate(((EditText)Form.Items.Item("etDtPagto").Specific).String);

                    if (m_DinheiroModel.valor > 0)
                    {
                        payments.CashAccount = m_DinheiroModel.contaC;
                        payments.CashSum = m_DinheiroModel.valor * proporcionalGrupo;
                    }

                    if (m_TransferenciaModel.valor > 0)
                    {
                        payments.TransferAccount = m_TransferenciaModel.contaC;
                        payments.TransferDate = m_TransferenciaModel.data;
                        payments.TransferReference = m_TransferenciaModel.ref1;
                        payments.TransferSum = m_TransferenciaModel.valor * proporcionalGrupo;
                    }

                    if (m_BoletoModel.valor > 0)
                    {
                        //payments.BoeAccount = m_BoletoModel.contaC;
                        payments.BillOfExchangeAmount = m_BoletoModel.valor * proporcionalGrupo;
                        payments.BillofExchangeStatus = SAPbobsCOM.BoBoeStatus.boes_Created;
                        payments.BillOfExchange.BillOfExchangeNo = m_BoletoModel.numero.ToString();
                        payments.BillOfExchange.ReferenceNo = m_BoletoModel.ref1;
                        payments.BillOfExchange.BillOfExchangeDueDate = m_BoletoModel.vcto;
                        payments.BillOfExchange.BPBankCountry = m_BoletoModel.pais;
                        payments.BillOfExchange.BPBankCode = m_BoletoModel.banco;
                        payments.BillOfExchange.BPBankAct = m_BoletoModel.conta;
                        payments.BillOfExchange.Remarks = m_BoletoModel.obs;
                        payments.BillOfExchange.PaymentMethodCode = m_BoletoModel.formaPagto;                        

                        codigoBarraBoleto = m_BoletoModel.codBarras;
                    }

                    int indice = 0;

                    foreach (Model.ChequeModel chequeModel in m_ChequeList)
                    {
                        if (chequeModel.valor > 0 && chequeModel.banco != string.Empty && chequeModel.filial != string.Empty &&
                            chequeModel.pais != string.Empty && chequeModel.conta != string.Empty &&
                            chequeModel.vcto != DateTime.MinValue)
                        {
                            if (indice > 0)
                                payments.Checks.Add();

                            payments.Checks.SetCurrentLine(payments.Checks.Count - 1);

                            payments.Checks.DueDate = chequeModel.vcto;
                            payments.Checks.CheckSum = chequeModel.valor * proporcionalGrupo;
                            payments.Checks.CountryCode = chequeModel.pais;
                            payments.Checks.BankCode = chequeModel.banco;
                            payments.Checks.Branch = chequeModel.filial;
                            payments.Checks.AccounttNum = chequeModel.conta;
                            payments.Checks.CheckNumber = chequeModel.numero;
                            payments.Checks.Trnsfrable = chequeModel.endosso ? SAPbobsCOM.BoYesNoEnum.tYES : SAPbobsCOM.BoYesNoEnum.tNO;
                            payments.Checks.CheckAccount = chequeModel.contaC;
                            payments.Checks.ManualCheck = chequeModel.manual ? SAPbobsCOM.BoYesNoEnum.tYES : SAPbobsCOM.BoYesNoEnum.tNO;

                            indice++;
                        }
                    }

                    indice = 0;

                    foreach (Model.CartaoModel cartaoModel in m_CartaoList)
                    {
                        if (indice > 0)
                        {
                            payments.CreditCards.Add();
                            payments.CreditCards.SetCurrentLine(indice);
                        }

                        payments.CreditCards.CreditCard = cartaoModel.nome;
                        payments.CreditCards.CreditAcct = cartaoModel.contaC;
                        payments.CreditCards.FirstPaymentDue = cartaoModel.primPagEm;
                        payments.CreditCards.NumOfPayments = cartaoModel.pgtos;
                        payments.CreditCards.NumOfCreditPayments = cartaoModel.pgtos;
                        payments.CreditCards.VoucherNum = cartaoModel.comprov;
                        payments.CreditCards.CreditSum = cartaoModel.valor;
                        payments.CreditCards.FirstPaymentSum = cartaoModel.primPagVal * proporcionalGrupo;
                        payments.CreditCards.AdditionalPaymentSum = cartaoModel.adcVal * proporcionalGrupo;
                        payments.CreditCards.SplitPayments = cartaoModel.divComp ? SAPbobsCOM.BoYesNoEnum.tYES : SAPbobsCOM.BoYesNoEnum.tNO;

                        indice++;
                    }

                    foreach (Model.BaixaCPModel baixaCPModel in baixaCPGroup)
                    {
                        if (payments.Invoices.DocEntry > 0)
                            payments.Invoices.Add();

                        payments.Invoices.SetCurrentLine(payments.Invoices.Count - 1);

                        payments.Invoices.DocEntry = baixaCPModel.DocEntry;

                        switch (baixaCPModel.TipoDoc)
                        {
                            case "NE":
                                payments.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_PurchaseInvoice;
                                payments.Invoices.InstallmentId = baixaCPModel.Parcela;
                                break;
                            case "ADT":
                                payments.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_PurchaseDownPayment;
                                break;
                            case "LC":
                                payments.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_JournalEntry;
                                break;
                        }

                        payments.Invoices.DocLine = baixaCPModel.Parcela - 1;
                        payments.Invoices.SumApplied = baixaCPModel.TotalPagar + baixaCPModel.ValorDesconto;
                        //payments.Invoices.TotalDiscount = baixaCPModel.ValorDesconto;

                        payments.Invoices.UserFields.Fields.Item("U_TotalAPagar").Value = baixaCPModel.TotalPagar;
                        payments.Invoices.UserFields.Fields.Item("U_ValorDoDesconto").Value = baixaCPModel.ValorDesconto;
                        payments.Invoices.UserFields.Fields.Item("U_ValorDoJurosMora").Value = baixaCPModel.ValorJuros;
                        payments.Invoices.UserFields.Fields.Item("U_ValorMulta").Value = baixaCPModel.ValorMulta;
                        payments.Invoices.UserFields.Fields.Item("U_TotalDoPagamento").Value = baixaCPModel.TotalPagar - baixaCPModel.ValorDesconto + baixaCPModel.ValorJuros + baixaCPModel.ValorMulta;

                    }

                    int erro = payments.Add();

                    if (erro != 0)
                    {
                        erroLog = true;

                        string msg = "";

                        Controller.MainController.Company.GetLastError(out erro, out msg);

                        throw new Exception(erro + " - " + msg);
                    }
                    else
                    {
                        if (codigoBarraBoleto != "")
                        {
                            string query = @"select MAX(""BoeKey"") from OBOE";

                            SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            recordSet.DoQuery(query);

                            string boeKey = recordSet.Fields.Item(0).Value.ToString();

                            string command = string.Format(@"update OBOE set ""BarcodeRep"" = '{0}' where ""BoeKey"" = {1}", codigoBarraBoleto, boeKey);

                            recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            recordSet.DoQuery(command);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Controller.MainController.Application.StatusBar.SetText(ex.Message);

                    System.IO.File.WriteAllText("payments.xml", payments.GetAsXML());
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(payments);
                    GC.Collect();
                }
            }

            if (!erroLog)    
                Controller.MainController.Application.StatusBar.SetText("Operação completada com êxito", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
            else
                Controller.MainController.Application.StatusBar.SetText("Operação completada com erros. Consulte o log de mensagens.", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

            //Limpar();
            Pesquisar();
        }

        private void Totalizar()
        {            
            Form.DataSources.UserDataSources.Item("totalDoc").Value = m_TotaisParcela.Count().ToString();
            Form.DataSources.UserDataSources.Item("totalParc").Value = m_TotaisParcela.Sum(r => r.Value).ToString();
            Form.DataSources.UserDataSources.Item("totalDesc").Value = m_TotaisDesconto.Sum(r => r.Value).ToString();
            Form.DataSources.UserDataSources.Item("totalJuros").Value = m_TotaisJuros.Sum(r => r.Value).ToString();
            Form.DataSources.UserDataSources.Item("totalPagar").Value = m_TotaisPagar.Sum(r => r.Value).ToString();
        }

        private void PosicionarTotais()
        {
            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            gridTitulos.AutoResizeColumns();

            int left = 32;
            left += gridTitulos.Columns.Item("Check").Width;
            left += gridTitulos.Columns.Item("Filial").Width;
            left += gridTitulos.Columns.Item("Tipo Doc").Width;

            Form.Items.Item("etTotalDoc").Left = left;
            Form.Items.Item("etTotalDoc").Width = gridTitulos.Columns.Item("Nº SAP").Width;

            left += gridTitulos.Columns.Item("Nº SAP").Width;
            left += gridTitulos.Columns.Item("Nº NF").Width;
            left += gridTitulos.Columns.Item("Fornecedor").Width;
            left += gridTitulos.Columns.Item("Data Vcto.").Width;
            left += gridTitulos.Columns.Item("Data Baixa").Width;
            left += gridTitulos.Columns.Item("Parcela").Width;

            Form.Items.Item("etTotalPar").Left = left;
            Form.Items.Item("etTotalPar").Width = gridTitulos.Columns.Item("Valor Parcela").Width;

            left += gridTitulos.Columns.Item("Valor Parcela").Width;

            Form.Items.Item("etTotalDes").Left = left;
            Form.Items.Item("etTotalDes").Width = gridTitulos.Columns.Item("Valor Desc.").Width;

            left += gridTitulos.Columns.Item("Valor Desc.").Width;
            left += gridTitulos.Columns.Item("Valor Multa").Width;

            Form.Items.Item("etTotalJur").Left = left;
            Form.Items.Item("etTotalJur").Width = gridTitulos.Columns.Item("Valor Juros").Width;

            left += gridTitulos.Columns.Item("Valor Juros").Width;

            Form.Items.Item("etTotalPag").Left = left;
            Form.Items.Item("etTotalPag").Width = gridTitulos.Columns.Item("Total a Pagar").Width;
        }
    }
}
