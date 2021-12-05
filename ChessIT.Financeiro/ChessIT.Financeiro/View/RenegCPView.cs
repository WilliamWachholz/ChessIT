using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace ChessIT.Financeiro.View
{
    class RenegCPView
    {
        Form Form;

        bool Loaded;

        bool bloqueado = false;

        private Dictionary<int, double> m_TotaisParcela = new Dictionary<int, double>();
        private Dictionary<int, double> m_TotaisDesconto = new Dictionary<int, double>();
        private Dictionary<int, double> m_TotaisJuros = new Dictionary<int, double>();
        private Dictionary<int, double> m_TotaisMulta = new Dictionary<int, double>();
        private Dictionary<int, double> m_TotaisPagar = new Dictionary<int, double>();

        public RenegCPView(Form form)
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

                                    if (pVal.ItemUID == "btAplJM")
                                    {
                                        AplicarMultaJuros();
                                    }

                                    if (pVal.ItemUID == "btReneg")
                                    {
                                        Renegociar();
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
                                                double valorMulta = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Multa")).GetText(pVal.Row));
                                                double valorPagar = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).GetText(pVal.Row));

                                                if (!m_TotaisParcela.ContainsKey(pVal.Row))
                                                    m_TotaisParcela.Add(pVal.Row, 0);

                                                m_TotaisParcela[pVal.Row] = valorParcela;

                                                if (!m_TotaisJuros.ContainsKey(pVal.Row))
                                                    m_TotaisJuros.Add(pVal.Row, 0);

                                                m_TotaisJuros[pVal.Row] = valorJuros;

                                                if (!m_TotaisMulta.ContainsKey(pVal.Row))
                                                    m_TotaisMulta.Add(pVal.Row, 0);

                                                m_TotaisMulta[pVal.Row] = valorMulta;

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

                                                if (m_TotaisMulta.ContainsKey(pVal.Row))
                                                    m_TotaisMulta.Remove(pVal.Row);

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

                                            if (!m_TotaisMulta.ContainsKey(pVal.Row))
                                                m_TotaisMulta.Add(pVal.Row, 0);

                                            m_TotaisMulta[pVal.Row] = valorJuros;

                                            if (!m_TotaisDesconto.ContainsKey(pVal.Row))
                                                m_TotaisDesconto.Add(pVal.Row, 0);

                                            m_TotaisDesconto[pVal.Row] = valorDesc;

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

                                        if (pVal.ItemUID.Equals("etContaJM"))
                                        {
                                            Form.DataSources.UserDataSources.Item("contaJM").Value = chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString();
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

                                        choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_3");

                                        conditions = new Conditions();

                                        condition = conditions.Add();

                                        condition.Alias = "Postable";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "Y";

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
#if DEBUG

            string query = @"select '{8}' AS ""Check"",
                                    OPCH.""BPLName"" AS ""Filial"",
		                            'NE' AS ""Tipo Doc"",
		                            OPCH.""DocEntry"" AS ""Nº SAP"",
		                            OPCH.""Serial"" AS ""Nº NF"",
		                            OPCH.""CardName"" AS ""Fornecedor"",
                                    OPCH.""DocDueDate"" AS ""Data Vcto."",
		                            cast(PCH6.""InstlmntID"" as nvarchar) + '/' + cast((select count(*) from PCH6 aux where aux.""DocEntry"" = OPCH.""DocEntry"") as nvarchar) AS ""Parcela"",
		                            PCH6.""InsTotal"" AS ""Valor Parcela"",
		                            0.0 as ""Valor Desc."",
		                            0.0 as ""Valor Multa"",
		                            0.0 as ""Valor Juros"",
		                            PCH6.""InsTotal"" - PCH6.""PaidToDate"" AS ""Total a Pagar"",
		                            OPCH.""PeyMethod"" AS ""Forma Pgto.""
                            from OPCH
                            inner join PCH6 on PCH6.""DocEntry"" = OPCH.""DocEntry""                                                        
                            where ""DocStatus"" = 'O'
                            and PCH6.""PaidToDate"" < PCH6.""InsTotal""
                            and ('{0}' = '' or OPCH.""Serial"" = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDueDate"" as date) <= cast('{4}' as date))
                            and ({5} = 0 or {5} = OPCH.""BPLId"")
                            and ('{6}' = '' or '{6}' = OPCH.""CardCode"")
                            and ('{7}' = '' or '{7}' = OPCH.""PeyMethod"")
                            union
                            select  '{8}' AS ""Check"",
                                    ODPO.""BPLName"" AS ""Filial"",
		                            'ADT' AS ""Tipo Doc"",
		                            ODPO.""DocEntry"" AS ""Nº SAP"",
		                            0 AS ""Nº NF"",
		                            ODPO.""CardName"" AS ""Fornecedor"",
                                    ODPO.""DocDueDate"" AS ""Data Vcto."",
		                            cast(DPO6.""InstlmntID"" as nvarchar) + '/' + cast((select count(*) from DPO6 aux where aux.""DocEntry"" = ODPO.""DocEntry"") as nvarchar) AS ""Parcela"",
		                            DPO6.""InsTotal"" AS ""Valor Parcela"",
		                            0.0 as ""Valor Desc."",
		                            0.0 as ""Valor Multa"",
		                            0.0 as ""Valor Juros"",
		                            DPO6.""InsTotal"" - DPO6.""PaidToDate"" AS ""Total a Pagar"",
		                            ODPO.""PeyMethod"" AS ""Forma Pgto.""
                            from ODPO
                            inner join DPO6 on DPO6.""DocEntry"" = ODPO.""DocEntry""                            
                            where ""DocStatus"" = 'O'
                            and DPO6.""PaidToDate"" < DPO6.""InsTotal""
                            and ('{0}' = '')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDueDate"" as date) <= cast('{4}' as date))                            
                            and ({5} = 0 or {5} = ODPO.""BPLId"")
                            and ('{6}' = '' or '{6}' = ODPO.""CardCode"")
                            and ('{7}' = '' or '{7}' = ODPO.""PeyMethod"")
                            union
                            select  '{8}' AS ""Check"",
                                    JDT1.""BPLName"" AS ""Filial"",
		                            'LC' AS ""Tipo Doc"",
		                            OJDT.""TransId"" AS ""Nº SAP"",
		                            0 AS ""Nº NF"",
		                            JDT1.""ShortName"" AS ""Fornecedor"",
                                    OJDT.""DueDate"" AS ""Data Vcto."",
		                            '1/1' as ""Parcela"",
		                            JDT1.""Credit"" AS ""Valor Parcela"",
		                            0.0 as ""Valor Desc."",
		                            0.0 as ""Valor Multa"",
		                            0.0 as ""Valor Juros"",
		                            JDT1.""BalDueCred"" AS ""Total A Pagar"",
		                            '' AS ""Forma Pgto.""
                            from JDT1
                            inner join OJDT on OJDT.""TransId"" = JDT1.""TransId""
                            where ""BalDueCred"" > 0
                            and ""MthDate"" is null
                            and OJDT.""StornoToTr"" IS NULL
                            and not exists (select * from OJDT aux where aux.""StornoToTr"" = OJDT.""TransId"")
                            and ('{0}' = '' or '' = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OJDT.""DueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(OJDT.""DueDate"" as date) <= cast('{4}' as date))                            
                            and ({5} = 0 or {5} = JDT1.""BPLId"")
                            and ('{6}' = '' or '{6}' = JDT1.""ShortName"")
                            and ('{7}' = '' or '{7}' = '')";

#else
            string query = @"select '{8}' AS ""Check"",
                                    OPCH.""BPLName"" AS ""Filial"",
		                            'NE' AS ""Tipo Doc"",
		                            OPCH.""DocEntry"" AS ""Nº SAP"",
		                            OPCH.""Serial"" AS ""Nº NF"",
		                            OPCH.""CardName"" AS ""Fornecedor"",
                                    OPCH.""DocDueDate"" AS ""Data Vcto."",
		                            cast(PCH6.""InstlmntID"" as nvarchar) || '/' || cast((select count(*) from PCH6 aux where aux.""DocEntry"" = OPCH.""DocEntry"") as nvarchar) AS ""Parcela"",
		                            PCH6.""InsTotal"" AS ""Valor Parcela"",
		                            0.0 as ""Valor Desc."",
		                            0.0 as ""Valor Multa"",
		                            0.0 as ""Valor Juros"",
		                            PCH6.""InsTotal"" - PCH6.""PaidToDate"" AS ""Total a Pagar"",
		                            OPCH.""PeyMethod"" AS ""Forma Pgto.""
                            from OPCH
                            inner join PCH6 on PCH6.""DocEntry"" = OPCH.""DocEntry""                                                        
                            where ""DocStatus"" = 'O'
                            and PCH6.""PaidToDate"" < PCH6.""InsTotal""
                            and ('{0}' = '' or OPCH.""Serial"" = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(OPCH.""DocDueDate"" as date) <= cast('{4}' as date))
                            and ({5} = 0 or {5} = OPCH.""BPLId"")
                            and ('{6}' = '' or '{6}' = OPCH.""CardCode"")
                            and ('{7}' = '' or '{7}' = OPCH.""PeyMethod"")
                            union
                            select  '{8}' AS ""Check"",
                                    ODPO.""BPLName"" AS ""Filial"",
		                            'ADT' AS ""Tipo Doc"",
		                            ODPO.""DocEntry"" AS ""Nº SAP"",
		                            0 AS ""Nº NF"",
		                            ODPO.""CardName"" AS ""Fornecedor"",
                                    ODPO.""DocDueDate"" AS ""Data Vcto."",
		                            cast(DPO6.""InstlmntID"" as nvarchar) || '/' || cast((select count(*) from DPO6 aux where aux.""DocEntry"" = ODPO.""DocEntry"") as nvarchar) AS ""Parcela"",
		                            DPO6.""InsTotal"" AS ""Valor Parcela"",
		                            0.0 as ""Valor Desc."",
		                            0.0 as ""Valor Multa"",
		                            0.0 as ""Valor Juros"",
		                            DPO6.""InsTotal"" - DPO6.""PaidToDate"" AS ""Total a Pagar"",
		                            ODPO.""PeyMethod"" AS ""Forma Pgto.""
                            from ODPO
                            inner join DPO6 on DPO6.""DocEntry"" = ODPO.""DocEntry""                            
                            where ""DocStatus"" = 'O'
                            and DPO6.""PaidToDate"" < DPO6.""InsTotal""
                            and ('{0}' = '')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(ODPO.""DocDueDate"" as date) <= cast('{4}' as date))                            
                            and ({5} = 0 or {5} = ODPO.""BPLId"")
                            and ('{6}' = '' or '{6}' = ODPO.""CardCode"")
                            and ('{7}' = '' or '{7}' = ODPO.""PeyMethod"")
                            union
                            select  '{8}' AS ""Check"",
                                    JDT1.""BPLName"" AS ""Filial"",
		                            'LC' AS ""Tipo Doc"",
		                            OJDT.""TransId"" AS ""Nº SAP"",
		                            OJDT.""U_NDocFin"" AS ""Nº NF"",
		                            JDT1.""ShortName"" AS ""Fornecedor"",
                                    OJDT.""DueDate"" AS ""Data Vcto."",
		                            '1/1' as ""Parcela"",
		                            JDT1.""Credit"" AS ""Valor Parcela"",
		                            0.0 as ""Valor Desc."",
		                            0.0 as ""Valor Multa"",
		                            0.0 as ""Valor Juros"",
		                            JDT1.""BalDueCred"" AS ""Total A Pagar"",
		                            OJDT.""U_FPagFin"" AS ""Forma Pgto.""
                            from JDT1
                            inner join OJDT on OJDT.""TransId"" = JDT1.""TransId""
                            where ""BalDueCred"" > 0
                            and ""MthDate"" is null
                            and OJDT.""StornoToTr"" IS NULL
                            and not exists (select * from OJDT aux where aux.""StornoToTr"" = OJDT.""TransId"")
                            and ('{0}' = '' or OJDT.""U_NDocFin"" = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OJDT.""DueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(OJDT.""DueDate"" as date) <= cast('{4}' as date))                            
                            and ({5} = 0 or {5} = JDT1.""BPLId"")
                            and ('{6}' = '' or '{6}' = JDT1.""ShortName"")
                            and ('{7}' = '' or '{7}' = OJDT.""U_FPagFin"")";
#endif


            string numNF = ((EditText)Form.Items.Item("etNumNF").Specific).String;
            string dataEmissaoDe = ((EditText)Form.Items.Item("etDtEmiIni").Specific).String;
            string dataEmissaoAte = ((EditText)Form.Items.Item("etDtEmiFim").Specific).String;
            string dataVencimentoDe = ((EditText)Form.Items.Item("etDtVctIni").Specific).String;
            string dataVencimentoAte = ((EditText)Form.Items.Item("etDtVctFim").Specific).String;            
            string empresa = Controller.MainController.ConvertStringCombo(((ComboBox)Form.Items.Item("cbEmpresa").Specific));
            string fornecedor = ((EditText)Form.Items.Item("etForCod").Specific).String;
            string formaPagto = ((ComboBox)Form.Items.Item("cbForPgto").Specific).Selected.Value;
            string selTodos = ((CheckBox)Form.Items.Item("ckSelTodos").Specific).Checked ? "Y" : "N";

            query = string.Format(query,
                        numNF,
                        dataEmissaoDe == "" ? "1990-01-01" : Convert.ToDateTime(dataEmissaoDe).ToString("yyyy-MM-dd"),
                        dataEmissaoAte == "" ? "1990-01-01" : Convert.ToDateTime(dataEmissaoAte).ToString("yyyy-MM-dd"),
                        dataVencimentoDe == "" ? "1990-01-01" : Convert.ToDateTime(dataVencimentoDe).ToString("yyyy-MM-dd"),
                        dataVencimentoAte == "" ? "1990-01-01" : Convert.ToDateTime(dataVencimentoAte).ToString("yyyy-MM-dd"),
                        empresa,
                        fornecedor,
                        formaPagto,
                        selTodos);

            Form.Freeze(true);
            try
            {
                Form.DataSources.DataTables.Item("dtTitulo").ExecuteQuery(query);

                Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

                gridTitulos.Columns.Item("Check").Type = BoGridColumnType.gct_CheckBox;
                gridTitulos.Columns.Item("Check").TitleObject.Caption = "#";

                gridTitulos.Columns.Item("Filial").Editable = false;
                gridTitulos.Columns.Item("Tipo Doc").Editable = false;
                gridTitulos.Columns.Item("Nº SAP").Editable = false;
                gridTitulos.Columns.Item("Nº NF").Editable = false;
                gridTitulos.Columns.Item("Fornecedor").Editable = false;
                gridTitulos.Columns.Item("Data Vcto.").Editable = false;
                gridTitulos.Columns.Item("Parcela").Editable = false;
                gridTitulos.Columns.Item("Valor Parcela").Editable = false;
                gridTitulos.Columns.Item("Total a Pagar").Editable = false;
                gridTitulos.Columns.Item("Forma Pgto.").Editable = false;

                PosicionarTotais();

                m_TotaisParcela = new Dictionary<int, double>();
                m_TotaisDesconto = new Dictionary<int, double>();
                m_TotaisJuros = new Dictionary<int, double>();
                m_TotaisMulta = new Dictionary<int, double>();
                m_TotaisPagar = new Dictionary<int, double>();
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

            m_TotaisParcela = new Dictionary<int, double>();
            m_TotaisDesconto = new Dictionary<int, double>();
            m_TotaisJuros = new Dictionary<int, double>();
            m_TotaisMulta = new Dictionary<int, double>();
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

                    if (!m_TotaisMulta.ContainsKey(linha))
                        m_TotaisMulta.Add(linha, 0);

                    m_TotaisMulta[linha] = valorMulta;

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

        private void Totalizar()
        {
            Form.DataSources.UserDataSources.Item("totalDoc").Value = m_TotaisParcela.Count().ToString();
            Form.DataSources.UserDataSources.Item("totalParc").Value = m_TotaisParcela.Sum(r => r.Value).ToString();
            Form.DataSources.UserDataSources.Item("totalDesc").Value = m_TotaisDesconto.Sum(r => r.Value).ToString();
            Form.DataSources.UserDataSources.Item("totalJuros").Value = m_TotaisJuros.Sum(r => r.Value).ToString();
            Form.DataSources.UserDataSources.Item("totalPagar").Value = m_TotaisPagar.Sum(r => r.Value).ToString();

            Form.DataSources.UserDataSources.Item("valJM").Value = (m_TotaisJuros.Sum(r => r.Value) + m_TotaisMulta.Sum(r => r.Value)).ToString();
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
        
        private void Renegociar()
        {
            Controller.MainController.Application.StatusBar.SetText("Gerando reconciliação", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning);

            string contaJurosMulta = ((EditText)Form.Items.Item("etContaJM").Specific).String;

            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            for (int linha = 0; linha < gridTitulos.Rows.Count; linha++)
            {
                if (((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(linha))
                {
                    string tipoDoc = ((EditTextColumn)gridTitulos.Columns.Item("Tipo Doc")).GetText(linha).Trim();

                    int docEntry = Convert.ToInt32(((EditTextColumn)gridTitulos.Columns.Item("Nº SAP")).GetText(linha));

                    string parcela = ((EditTextColumn)gridTitulos.Columns.Item("Parcela")).GetText(linha).Trim();

                    string parcelaID = ((EditTextColumn)gridTitulos.Columns.Item("Parcela")).GetText(linha).Trim().Split('/')[0];

                    string parcelaQtd = ((EditTextColumn)gridTitulos.Columns.Item("Parcela")).GetText(linha).Trim().Split('/')[1];

                    double valorParcela = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Parcela")).GetText(linha));

                    double valorMulta = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Multa")).GetText(linha));

                    double valorJuros = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Juros")).GetText(linha));

                    double totalPagar = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).GetText(linha));


                    switch (tipoDoc)
                    {
                        case "NE":
                            SAPbobsCOM.Recordset recordSetNE = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            recordSetNE.DoQuery(@"select ""TransId"" from OPCH where ""DocEntry"" = " + docEntry.ToString());

                            int transIdNE = Convert.ToInt32(recordSetNE.Fields.Item(0).Value);

                            GerarContraLC(transIdNE, Convert.ToInt32(parcelaID), Convert.ToInt32(parcelaQtd));
                            break;

                        case "ADT":
                            SAPbobsCOM.Recordset recordSetADT = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            recordSetADT.DoQuery(@"select ""TransId"" from ODPO where ""DocEntry"" = " + docEntry.ToString());

                            int transIdADT = Convert.ToInt32(recordSetADT.Fields.Item(0).Value);

                            GerarContraLC(transIdADT, Convert.ToInt32(parcelaID), Convert.ToInt32(parcelaQtd));
                            break;

                        case "LC":
                            SAPbobsCOM.JournalEntries journalEntries = (SAPbobsCOM.JournalEntries)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                            journalEntries.GetByKey(docEntry);
                            journalEntries.Cancel();
                            break;
                    }

                    Controller.MainController.Application.ActivateMenuItem("1540");

                    Form lcForm = null;

                    for (int i = 0; i <= 100; i++)
                    {
                        System.Threading.Thread.Sleep(200);

                        try
                        {
                            lcForm = Controller.MainController.Application.Forms.GetFormByTypeAndCount(392, i);
                        }
                        catch
                        {
                            break;
                        }
                    }

                    if (lcForm != null)
                    {
                        string query = @"select OPCH.""CardCode"",
                                                OPCH.""CardName"",
                                                OPCH.""TaxDate"", 
                                                OPCH.""DocDate"",
                                                OPCH.""DocDueDate"",
                                                OPCH.""Serial"",
                                                OPCH.""PeyMethod"",
                                                PCH1.""AcctCode""
                                        from OPCH 
                                        inner join PCH1 on PCH1.""DocEntry"" = OPCH.""DocEntry"" AND PCH1.""LineNum"" = 0
                                        where OPCH.""DocEntry"" = " + docEntry;

                        if (tipoDoc == "ADT")
                        {
                            query = @"select    ODPO.""CardCode"",
                                                ODPO.""CardName"",
                                                ODPO.""TaxDate"", 
                                                ODPO.""DocDate"",
                                                ODPO.""DocDueDate"",
                                                ODPO.""Serial"",
                                                ODPO.""PeyMethod"",
                                                DPO1.""AcctCode""
                                        from ODPO
                                        inner join DPO1 on DPO1.""DocEntry"" = ODPO.""DocEntry"" AND DPO1.""LineNum"" = 0
                                        where ODPO.""DocEntry"" = " + docEntry;
                        }
                        else if (tipoDoc == "LC")
                        {
#if DEBUG
                            query = @"select    OCRD.""CardCode"",
                                                OCRD.""CardName"",
                                                OJDT.""TaxDate"", 
                                                OJDT.""RefDate"",
                                                OJDT.""DueDate"",
                                                0,
                                                ''
                                        from OJDT
                                        inner join JDT1 on JDT1.""TransId"" = OJDT.""TransId"" and JDT1.""Line_ID"" = 0
                                        inner join OCRD on OCRD.""CardCode"" = JDT1.""ShortName""
                                        where OJDT.""TransId"" = " + docEntry;

#else

                            query = @"select    OCRD.""CardCode"",
                                                OCRD.""CardName"",
                                                OJDT.""TaxDate"", 
                                                OJDT.""RefDate"",
                                                OJDT.""DueDate"",
                                                OJDT.""U_NDocFin"",
                                                OJDT.""U_FPagFin"",
                                                JDT1.""ContraAct""
                                        from OJDT
                                        inner join JDT1 on JDT1.""TransId"" = OJDT.""TransId"" and JDT1.""Line_ID"" = 0
                                        inner join OCRD on OCRD.""CardCode"" = JDT1.""ShortName""
                                        where OJDT.""TransId"" = " + docEntry;
#endif
                        }

                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        recordSet.DoQuery(query);

                        if (!recordSet.EoF)
                        {
                            ((EditText)lcForm.Items.Item("10").Specific).String = recordSet.Fields.Item(1).Value.ToString();

                            ((EditText)lcForm.Items.Item("97").Specific).String = Convert.ToDateTime(recordSet.Fields.Item(2).Value).ToString("dd/MM/yyyy");
                            ((EditText)lcForm.Items.Item("6").Specific).String = Convert.ToDateTime(recordSet.Fields.Item(3).Value).ToString("dd/MM/yyyy");
                            ((EditText)lcForm.Items.Item("102").Specific).String = Convert.ToDateTime(recordSet.Fields.Item(4).Value).ToString("dd/MM/yyyy");
#if !DEBUG
                            ((EditText)lcForm.Items.Item("U_NDocFin").Specific).String = recordSet.Fields.Item(5).Value.ToString();
                            ((EditText)lcForm.Items.Item("U_FPagFin").Specific).String = recordSet.Fields.Item(6).Value.ToString();
#endif

                            Matrix grid = (Matrix)lcForm.Items.Item("76").Specific;

                            grid.Columns.Item("1").Cells.Item(1).Click();

                            foreach (char c in recordSet.Fields.Item(0).Value.ToString().ToArray())
                                Controller.MainController.Application.SendKeys(c.ToString());
                            
                            Controller.MainController.Application.SendKeys("^{TAB}");

                            ((EditText)grid.Columns.Item("6").Cells.Item(1).Specific).String = totalPagar.ToString();

                            ((EditText)grid.Columns.Item("9").Cells.Item(1).Specific).String = parcela;

                            //Linha débito conta contrapartida

                            ((EditText)grid.Columns.Item("1").Cells.Item(2).Specific).String = recordSet.Fields.Item(7).Value.ToString();

                            ((EditText)grid.Columns.Item("5").Cells.Item(2).Specific).String = valorParcela.ToString();

                            ((EditText)grid.Columns.Item("9").Cells.Item(2).Specific).String = parcela;

                            //Linha débito juros e multa
                            if (contaJurosMulta != "")
                            {
                                ((EditText)grid.Columns.Item("1").Cells.Item(3).Specific).String = contaJurosMulta;

                                ((EditText)grid.Columns.Item("5").Cells.Item(3).Specific).String = (valorJuros + valorMulta).ToString();

                                ((EditText)grid.Columns.Item("9").Cells.Item(3).Specific).String = parcela;
                            }
                        }
                    }
                }
            }

            Controller.MainController.Application.StatusBar.SetText("Operação completada com êxito", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success);
        }

        private void GerarContraLC(int transIdRef, int parcela, int qtdParcela)
        {
            int transId = 0;

            int lineNumRef = 0;

            int lineNumEntry = 0;

            string fornecedor = "";

            SAPbobsCOM.JournalEntries journalRef = (SAPbobsCOM.JournalEntries)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
            SAPbobsCOM.JournalEntries journalEntry = (SAPbobsCOM.JournalEntries)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
            try
            {
                journalRef.GetByKey(transIdRef);

                journalEntry.Memo = "Renegociação NF (Estorno Principal)";

                journalEntry.DueDate = journalRef.DueDate;
                journalEntry.Reference = journalRef.Reference;
                journalEntry.Reference2 = journalRef.Reference2;
                journalEntry.Reference3 = journalRef.Reference3;
                journalEntry.ReferenceDate = journalRef.ReferenceDate;
                journalEntry.TaxDate = journalRef.TaxDate;

                int linhaEntry = 0;

                double somaCredito = 0;

                double somaDebito = 0;

                string contaContra = "";

                int linhaContra = -1;

                double diferencaContra = 0;

                for (int linha = 0; linha < journalRef.Lines.Count; linha++)
                {
                    journalRef.Lines.SetCurrentLine(linha);

#if DEBUG
                    if (journalRef.Lines.ShortName.StartsWith("V") && parcela - 1 != linha)
                        continue;
#else
                    if (journalRef.Lines.ShortName.StartsWith("F") && parcela - 1 != linha)
                        continue;

#endif
                    if (linhaEntry > 0)
                        journalEntry.Lines.Add();


                    journalEntry.Lines.SetCurrentLine(linhaEntry);

                    linhaEntry++;

                    journalEntry.Lines.AccountCode = journalRef.Lines.AccountCode;
                    journalEntry.Lines.ShortName = journalRef.Lines.ShortName;

#if DEBUG
                    if (journalRef.Lines.ShortName.StartsWith("V"))
                        fornecedor = journalRef.Lines.ShortName;
#else
                    if (journalRef.Lines.ShortName.StartsWith("F"))
                    {
                        fornecedor = journalRef.Lines.ShortName;

                        contaContra = journalRef.Lines.ContraAccount;

                        lineNumRef = linha;
                        lineNumEntry = linhaEntry - 1;
                    }

#endif

                    journalEntry.Lines.BPLID = journalRef.Lines.BPLID;
                    journalEntry.Lines.CostingCode = journalRef.Lines.CostingCode;
                    journalEntry.Lines.CostingCode2 = journalRef.Lines.CostingCode2;
                    journalEntry.Lines.CostingCode3 = journalRef.Lines.CostingCode3;
                    journalEntry.Lines.CostingCode4 = journalRef.Lines.CostingCode4;
                    journalEntry.Lines.CostingCode5 = journalRef.Lines.CostingCode5;
                    journalEntry.Lines.LineMemo = "Renegociação NF (Estorno Principal)";
                    journalEntry.Lines.ProjectCode = journalRef.Lines.ProjectCode;
                    journalEntry.Lines.Reference1 = journalRef.Lines.Reference1;
                    journalEntry.Lines.Reference2 = journalRef.Lines.Reference2;
                    journalEntry.Lines.ReferenceDate1 = journalRef.Lines.ReferenceDate1;
                    journalEntry.Lines.ReferenceDate2 = journalRef.Lines.ReferenceDate2;
                    journalEntry.Lines.TaxDate = journalRef.Lines.TaxDate;

#if DEBUG
                    if (journalRef.Lines.ShortName.StartsWith("V"))
                    {
                        if (journalRef.Lines.Debit > 0)
                            journalEntry.Lines.Credit = journalRef.Lines.Debit;
                        else
                            journalEntry.Lines.Debit = journalRef.Lines.Credit;
                    }
                    else
                    {
                        if (journalRef.Lines.Debit > 0)
                            journalEntry.Lines.Credit = journalRef.Lines.Debit / qtdParcela;
                        else
                            journalEntry.Lines.Debit = journalRef.Lines.Credit / qtdParcela;
                    }
#else
                    if (journalRef.Lines.ShortName.StartsWith("F"))
                    {
                        if (journalRef.Lines.Debit > 0)
                            journalEntry.Lines.Credit = journalRef.Lines.Debit;
                        else
                            journalEntry.Lines.Debit = journalRef.Lines.Credit;
                    }
                    else
                    {
                        if (journalRef.Lines.Debit > 0)
                            journalEntry.Lines.Credit = journalRef.Lines.Debit / qtdParcela;
                        else
                            journalEntry.Lines.Debit = journalRef.Lines.Credit / qtdParcela;
                    }

                    somaCredito += journalEntry.Lines.Credit;
                    somaDebito += journalEntry.Lines.Debit;

                    if (journalEntry.Lines.AccountCode == contaContra)
                        linhaContra = linhaEntry - 1;
#endif

                }

                if (linhaContra >= 0)
                {
                    diferencaContra = somaCredito - somaDebito;

                    journalEntry.Lines.SetCurrentLine(linhaContra);

                    journalEntry.Lines.Credit -= diferencaContra;
                }

                System.IO.File.WriteAllText("journal.xml", journalEntry.GetAsXML());

                int erro = journalEntry.Add();

                if (erro != 0)
                {
                    string msg = "";

                    Controller.MainController.Company.GetLastError(out erro, out msg);

                    throw new Exception(erro + " - " + msg);
                }
                else
                {
                    string sResult = "";

                    Controller.MainController.Company.GetNewObjectCode(out sResult);


                    transId = Convert.ToInt32(sResult);
                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(journalRef);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(journalEntry);
            }

#if !DEBUG
            SAPbobsCOM.CompanyService companyService2 = null;
            SAPbobsCOM.InternalReconciliationsService internalReconciliationService2 = null;
            SAPbobsCOM.InternalReconciliationOpenTransParams internalReconciliationParams2 = null;
            SAPbobsCOM.InternalReconciliationOpenTrans internalReconciliationOpenTrans2 = null;
            try
            {
                companyService2 = Controller.MainController.Company.GetCompanyService();

                internalReconciliationService2 = (SAPbobsCOM.InternalReconciliationsService)companyService2.GetBusinessService(SAPbobsCOM.ServiceTypes.InternalReconciliationsService);

                internalReconciliationParams2 = (SAPbobsCOM.InternalReconciliationOpenTransParams)internalReconciliationService2.GetDataInterface(SAPbobsCOM.InternalReconciliationsServiceDataInterfaces.irsInternalReconciliationOpenTransParams);

                internalReconciliationParams2.ReconDate = DateTime.Today;

                internalReconciliationParams2.CardOrAccount = SAPbobsCOM.CardOrAccountEnum.coaCard;
                internalReconciliationParams2.InternalReconciliationBPs.Add();
                internalReconciliationParams2.InternalReconciliationBPs.Item(0).BPCode = fornecedor;

                internalReconciliationOpenTrans2 = internalReconciliationService2.GetOpenTransactions(internalReconciliationParams2);

                foreach (SAPbobsCOM.InternalReconciliationOpenTransRow internalReconciliationOpenTransRow in internalReconciliationOpenTrans2.InternalReconciliationOpenTransRows)
                {
                    if (internalReconciliationOpenTransRow.TransId == transIdRef && internalReconciliationOpenTransRow.TransRowId == lineNumRef)
                    {
                        internalReconciliationOpenTransRow.Selected = SAPbobsCOM.BoYesNoEnum.tYES;
                    }

                    if (internalReconciliationOpenTransRow.TransId == transId && internalReconciliationOpenTransRow.TransRowId == lineNumEntry)
                    {
                        internalReconciliationOpenTransRow.Selected = SAPbobsCOM.BoYesNoEnum.tYES;
                    }
                }

                internalReconciliationService2.Add(internalReconciliationOpenTrans2);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(companyService2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(internalReconciliationService2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(internalReconciliationParams2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(internalReconciliationOpenTrans2);

                GC.Collect();
            }
#endif
        }
    }
}
