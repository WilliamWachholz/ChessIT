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
                                    if (pVal.ItemUID == "btPesquisa")
                                    {
                                        Pesquisar();
                                    }

                                    if (pVal.ItemUID == "btLimpar")
                                    {
                                    }

                                    if (pVal.ItemUID == "btMeioPgto")
                                    {
                                    }

                                    if (pVal.ItemUID == "btAplJM")
                                    {
                                    }

                                    if (pVal.ItemUID == "btPagar")
                                    {
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
                                    OPCH.""BPLName"" AS ""Filial"",
		                            'NE' AS ""Tipo Doc"",
		                            OPCH.""DocEntry"" AS ""Nº SAP"",
		                            OPCH.""Serial"" AS ""Nº NF"",
		                            OPCH.""CardName"" AS ""Fornecedor"",
		                            PCH6.""InstlmntID"" || '/' || (select count(*) from PCH6 aux where aux.""DocEntry"" = OPCH.""DocEntry"") AS ""Parcela"",
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
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = OPCH.""DocEntry"" AND VPM2.""InvType"" = 18 and cast(OVPM.""DocDate"" as date) >= cast('{5}' as date)))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = OPCH.""DocEntry"" AND VPM2.""InvType"" = 18 and cast(OVPM.""DocDate"" as date) <= cast('{6}' as date)))
                            and ({7} = 0 or {7} >= OPCH.""DocTotal"")
                            and ({8} = 0 or {8} <= OPCH.""DocTotal"")
                            and ({9} = 0 or {9} = OPCH.""BPLId"")
                            and ('{10}' = '' or '{10}' = OPCH.""CardName"")
                            and ('{11}' = '' or '{11}' = OPCH.""PeyMethod"")
                            and ('{12}' = '' or ('{12}' = 'Y' and exists (select * from VPM2 where VPM2.""DocEntry"" = OPCH.""DocEntry"" AND VPM2.""InvType"" = 18)))
                            and ('{13}' = '' or ('{13}' = 'Y' and not exists (select * from VPM2 where VPM2.""DocEntry"" = OPCH.""DocEntry"" AND VPM2.""InvType"" = 18)))
                            union
                            select  '{14}' AS ""Check"",
                                    ODPO.""BPLName"" AS ""Filial"",
		                            'ADT' AS ""Tipo Doc"",
		                            ODPO.""DocEntry"" AS ""Nº SAP"",
		                            '' AS ""Nº NF"",
		                            ODPO.""CardName"" AS ""Fornecedor"",
		                            DPO6.""InstlmntID"" || '/' || (select count(*) from DPO6 aux where aux.""DocEntry"" = ODPO.""DocEntry"") AS ""Parcela"",
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
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = ODPO.""DocEntry"" AND VPM2.""InvType"" = 204 and cast(OVPM.""DocDate"" as date) >= cast('{5}' as date)))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = ODPO.""DocEntry"" AND VPM2.""InvType"" = 204 and cast(OVPM.""DocDate"" as date) <= cast('{6}' as date)))
                            and ({7} = 0 or {7} >= ODPO.""DocTotal"")
                            and ({8} = 0 or {8} <= ODPO.""DocTotal"")
                            and ({9} = 0 or {9} = ODPO.""BPLId"")
                            and ('{10}' = '' or '{10}' = ODPO.""CardName"")
                            and ('{11}' = '' or '{11}' = ODPO.""PeyMethod"")
                            and ('{12}' = '' or ('{12}' = 'Y' and exists (select * from VPM2 where VPM2.""DocEntry"" = ODPO.""DocEntry"" AND VPM2.""InvType"" = 204)))
                            and ('{13}' = '' or ('{13}' = 'Y' and not exists (select * from VPM2 where VPM2.""DocEntry"" = ODPO.""DocEntry"" AND VPM2.""InvType"" = 204)))
                            union
                            select  '{14}' AS ""Check"",
                                    OJDT.""BPLName"" AS ""Filial"",
		                            'LC' AS ""Tipo Doc"",
		                            OJDT.""TransId"" AS ""Nº SAP"",
		                            OJDT.""U_NDocFin"" AS ""Nº NF"",
		                            JDT1.""ShortName"" AS ""Fornecedor"",
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
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30 and cast(OVPM.""DocDate"" as date) >= cast('{5}' as date)))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM.""DocEntry"" = VPM2.""DocNum"" where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30 and cast(OVPM.""DocDate"" as date) <= cast('{6}' as date)))                            
                            and ({7} = 0 or {7} >= JDT1.""Credit"")
                            and ({8} = 0 or {8} <= JDT1.""Credit"")
                            and ({9} = 0 or {9} = JDT1.""BPLId"")
                            and ('{10}' = '' or '{10}' = JDT1.""ShortName"")
                            and ('{11}' = '' or '{11}' = OJDT.""U_FPagFin"")
                            and ('{12}' = '' or ('{12}' = 'Y' and exists (select * from VPM2 where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30)))
                            and ('{13}' = '' or ('{13}' = 'Y' and not exists (select * from VPM2 where VPM2.""DocEntry"" = JDT1.""TransId"" AND VPM2.""InvType"" = 30)))";


            string numNF = ((EditText)Form.Items.Item("etNumNF").Specific).String;
            string dataEmissaoDe = ((EditText)Form.Items.Item("etDtEmiIni").Specific).String;
            string dataEmissaoAte = ((EditText)Form.Items.Item("etDtEmiFim").Specific).String;
            string dataVencimentoDe = ((EditText)Form.Items.Item("etDtVctIni").Specific).String;
            string dataVencimentoAte = ((EditText)Form.Items.Item("etDtVctFim").Specific).String;
            string dataBaixaDe = ((EditText)Form.Items.Item("etDtBaiDe").Specific).String;
            string dataBaixaAte = ((EditText)Form.Items.Item("etDtBaiAte").Specific).String;
            string valorDocDe = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etValDocDe").Specific).String).ToString("0.00");
            string valorDocAte = Controller.MainController.ConvertDouble(((EditText)Form.Items.Item("etValDocAte").Specific).String).ToString("0.00");
            string empresa = ((ComboBox)Form.Items.Item("cbEmpresa").Specific).Selected.Value;
            string fornecedor = ((EditText)Form.Items.Item("etForCod").Specific).String;
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
                        baixados,
                        pendentes,
                        todos);

            Form.Freeze(true);
            try
            {
                Form.DataSources.DataTables.Item("dtTitulo").ExecuteQuery(query);

                Grid gridContratos = (Grid)Form.Items.Item("gridTitulo").Specific;

                gridContratos.Columns.Item("Check").Type = BoGridColumnType.gct_CheckBox;

                gridContratos.Columns.Item("Filial").Editable = false;
                gridContratos.Columns.Item("Tipo Doc").Editable = false;
                gridContratos.Columns.Item("Nº SAP").Editable = false;
                gridContratos.Columns.Item("Nº NF").Editable = false;
                gridContratos.Columns.Item("Fornecedor").Editable = false;
                gridContratos.Columns.Item("Parcela").Editable = false;
                gridContratos.Columns.Item("Valor Parcela").Editable = false;
                gridContratos.Columns.Item("Total A Pagar").Editable = false;
                gridContratos.Columns.Item("Forma Pgto.").Editable = false;
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

        private void Pagar()
        {

        }
    }
}
