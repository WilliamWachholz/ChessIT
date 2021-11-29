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
		                            cast(PCH6.""InstlmntID"" as nvarchar(max)) + '/' + cast((select count(*) from PCH6 aux where aux.""DocEntry"" = OPCH.""DocEntry"") as nvarchar(max)) AS ""Parcela"",
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
                            and ('{7}' = '' or '{7}' = OPCH.""PeyMethod"")'Y' and not exists (select * from VPM2 where VPM2.""DocEntry"" = OPCH.""DocEntry"" AND VPM2.""InvType"" = 18)))
                            union
                            select  '{8}' AS ""Check"",
                                    ODPO.""BPLName"" AS ""Filial"",
		                            'ADT' AS ""Tipo Doc"",
		                            ODPO.""DocEntry"" AS ""Nº SAP"",
		                            '' AS ""Nº NF"",
		                            ODPO.""CardName"" AS ""Fornecedor"",
                                    ODPO.""DocDueDate"" AS ""Data Vcto."",
		                            cast(DPO6.""InstlmntID"" as nvarchar(max)) + '/' + cast((select count(*) from DPO6 aux where aux.""DocEntry"" = ODPO.""DocEntry"") as nvarchar(max)) AS ""Parcela"",
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
                            select  '{14}' AS ""Check"",
                                    JDT1.""BPLName"" AS ""Filial"",
		                            'LC' AS ""Tipo Doc"",
		                            OJDT.""TransId"" AS ""Nº SAP"",
		                            '' AS ""Nº NF"",
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
                            and OJDT.""TransType"" <> 18
                            and OJDT.""StornoToTr"" IS NULL
                            and not exists (select * from OJDT aux where aux.""StornoToTr"" = OJDT.""TransId"")
                            and ('{0}' = '' or '' = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OJDT.""RefDate"" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OJDT.""DueDate"" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(OJDT.""DueDate"" as date) <= cast('{4}' as date))
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30 and cast(OVPM.""DocDate"" as date) >= cast('{5}' as date)))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30 and cast(OVPM.""DocDate"" as date) <= cast('{6}' as date)))                            
                            and ({7} = 0 or {7} >= JDT1.""Credit"")
                            and ({8} = 0 or {8} <= JDT1.""Credit"")
                            and ({9} = 0 or {9} = JDT1.""BPLId"")
                            and ('{10}' = '' or '{10}' = JDT1.""ShortName"")
                            and ('{11}' = '' or '{11}' = '')
                            and ('{12}' = 'N' or ('{12}' = 'Y' and exists (select * from VPM2 where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30)))
                            and ('{13}' = 'N' or ('{13}' = 'Y' and not exists (select * from VPM2 where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30)))";

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

        Dictionary<string, Form> lcForms = new Dictionary<string, Form>();

        System.Timers.Timer m_timerLC = new System.Timers.Timer(1000);

        private void Renegociar()
        {
            lcForms = new Dictionary<string, Form>();

            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            for (int linha = 0; linha < gridTitulos.Rows.Count; linha++)
            {
                if (((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(linha))
                {
                    string tipoDoc = ((EditTextColumn)gridTitulos.Columns.Item("Tipo Doc")).GetText(linha).Trim();

                    string docEntry = ((EditTextColumn)gridTitulos.Columns.Item("Nº Interno")).GetText(linha).Trim();

                    string parcela = ((EditTextColumn)gridTitulos.Columns.Item("Parcela")).GetText(linha).Trim();

                    string chave = tipoDoc + "-" + docEntry;

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
                            lcForms.Add(chave, lcForm);

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
                                                OPCH.""PeyMethod"" 
                                        from OPCH where ""DocEntry"" = " + docEntry;

                        if (tipoDoc == "ADT")
                        {
                            query = @"select    ODPO.""CardCode"",
                                                ODPO.""CardName"",
                                                ODPO.""TaxDate"", 
                                                ODPO.""DocDate"",
                                                ODPO.""DocDueDate"",
                                                ODPO.""Serial"",
                                                ODPO.""PeyMethod"" 
                                        from ODPO where ""DocEntry"" = " + docEntry;
                        }
                        else if (tipoDoc == "LC")
                        {
                            query = @"select    OCRD.""CardCode"",
                                                OCRD.""CardName"",
                                                OJDT.""TaxDate"", 
                                                OJDT.""DocDate"",
                                                OJDT.""DueDate"",
                                                OJDT.""U_NDocFin"",
                                                OJDT.""U_FPagFin"" 
                                        from OJDT
                                        inner join JDT1 on JDT1.""TransId"" = OJDT.""TransId"" and JDT1.""LineNum"" = 0
                                        inner join OCRD on OCRD.""CardCode"" = JDT1.""ShortName""
                                        where OJDT.""TransId"" = " + docEntry;
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

                            grid.Columns.Item("1").Cells.Item(parcela).Click();

                            SetarCodigoPNThread(recordSet.Fields.Item(1).Value.ToString());

                            System.Windows.Forms.SendKeys.SendWait("^{v}");
                        }
                    }
                }
            }

            m_timerLC.Elapsed += FinalizarLC;
            m_timerLC.Enabled = true;
        }

        private void FinalizarLC(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_timerLC.Enabled = false;

            string contaJurosMulta = ((EditText)Form.Items.Item("etContaJM").Specific).String;

            Grid gridTitulos = (Grid)Form.Items.Item("gridTitulo").Specific;

            for (int linha = 0; linha < gridTitulos.Rows.Count; linha++)
            {
                if (((CheckBoxColumn)gridTitulos.Columns.Item("Check")).IsChecked(linha))
                {
                    string tipoDoc = ((EditTextColumn)gridTitulos.Columns.Item("Tipo Doc")).GetText(linha).Trim();

                    string docEntry = ((EditTextColumn)gridTitulos.Columns.Item("Nº Interno")).GetText(linha).Trim();                    

                    string chave = tipoDoc + "-" + docEntry;

                    if (lcForms.ContainsKey(chave))
                    {
                        Form lcForm = lcForms[chave];

                        lcForm.Select();

                        string contaContrapartida = "";

                        string query = string.Format(@"select PCH1.""AcctCode"" FROM PCH1 WHERE ""DocEntry"" = {0} AND ""LineNum"" = 0 ", docEntry);

                        if (tipoDoc == "ADT")
                        {
                            query = string.Format(@"select DPO1.""AcctCode"" FROM DPO1 WHERE ""DocEntry"" = {0} AND ""LineNum"" = 0 ", docEntry);
                        }
                        else if (tipoDoc == "LC")
                        {
                            query = string.Format(@"select JDT1.""Account"" FROM JDT1 WHERE ""TransId"" = {0} AND ""Line_ID"" = 0 ", docEntry);
                        }

                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        recordSet.DoQuery(query);

                        if (!recordSet.EoF)
                        {
                            contaContrapartida = recordSet.Fields.Item(0).Value.ToString();

                            string parcela = ((EditTextColumn)gridTitulos.Columns.Item("Parcela")).GetText(linha).Trim();

                            double valorParcela = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Parcela")).GetText(linha));

                            double valorMulta = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Multa")).GetText(linha));

                            double valorJuros = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Valor Juros")).GetText(linha));

                            double totalPagar = Controller.MainController.ConvertDouble(((EditTextColumn)gridTitulos.Columns.Item("Total a Pagar")).GetText(linha));
                            

                            Matrix grid = (Matrix)lcForm.Items.Item("76").Specific;

                            //Linha crédito PN

                            ((EditText)grid.Columns.Item("6").Cells.Item(1).Specific).String = totalPagar.ToString();

                            ((EditText)grid.Columns.Item("9").Cells.Item(1).Specific).String = parcela;

                            //Linha débito conta contrapartida

                            ((EditText)grid.Columns.Item("1").Cells.Item(2).Specific).String = contaContrapartida;

                            ((EditText)grid.Columns.Item("5").Cells.Item(2).Specific).String = valorParcela.ToString();

                            ((EditText)grid.Columns.Item("9").Cells.Item(2).Specific).String = parcela;

                            //Linha débito juros e multa

                            ((EditText)grid.Columns.Item("1").Cells.Item(3).Specific).String = contaJurosMulta;

                            ((EditText)grid.Columns.Item("5").Cells.Item(3).Specific).String = (valorJuros + valorMulta).ToString();

                            ((EditText)grid.Columns.Item("9").Cells.Item(3).Specific).String = parcela;
                        }
                    }
                }
            }
        }

        private void SetarCodigoPNThread(string valor)
        {
            Thread td = new Thread(new ParameterizedThreadStart(SetarCodigoPN));

            td.SetApartmentState(ApartmentState.STA);
            td.IsBackground = true;
            td.Start(valor);
        }

        [STAThread]
        private void SetarCodigoPN(object valor)
        {
            System.Windows.Forms.Clipboard.SetText(valor.ToString());

        }
    }
}
