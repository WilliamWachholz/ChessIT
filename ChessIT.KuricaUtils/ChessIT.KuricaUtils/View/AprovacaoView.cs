using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace ChessIT.KuricaUtils.View
{
    class AprovacaoView
    {
        Form Form;

        bool Loaded;

        public AprovacaoView(Form form)
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
                                    if (pVal.ItemUID == "btOK")
                                    {
                                        Form.Close();
                                    }
                                    else if (pVal.ItemUID == "btPesq")
                                    {
                                        Pesquisar(false);
                                    }
                                    else if (pVal.ItemUID == "btDet")
                                    {
                                        Detalhar();
                                    }
                                    else if (pVal.ItemUID == "btCompr")
                                    {
                                        Comprimir();
                                    }
                                    else if (pVal.ItemUID == "btAprovar")
                                    {
                                        List<Model.AprovacaoModel> aprovacaoList = new List<Model.AprovacaoModel>();

                                        Grid gridDetalhes = (Grid)Form.Items.Item("gdDetalhes").Specific;

                                        for (int row = 0; row < gridDetalhes.Rows.Count; row++)
                                        {

                                            if (((CheckBoxColumn)gridDetalhes.Columns.Item("#")).IsChecked(row))
                                            {
                                                Model.AprovacaoModel aprovacaoModel = new Model.AprovacaoModel();
                                                aprovacaoModel.WddCode = Convert.ToInt32(((EditTextColumn)gridDetalhes.Columns.Item("WddCode")).GetText(row));
                                                aprovacaoModel.NumeroDoc = Convert.ToInt32(((EditTextColumn)gridDetalhes.Columns.Item("Nº Doc")).GetText(row));
                                                aprovacaoModel.Data = DateTime.ParseExact(((EditTextColumn)gridDetalhes.Columns.Item("Data")).GetText(row), "yyyyMMdd", CultureInfo.InvariantCulture);
                                                aprovacaoModel.Fornecedor = ((EditTextColumn)gridDetalhes.Columns.Item("Fornecedor")).GetText(row);
                                                aprovacaoModel.ValorTotal = Convert.ToDouble(((EditTextColumn)gridDetalhes.Columns.Item("Valor Total")).GetText(row), CultureInfo.InvariantCulture);

                                                aprovacaoList.Add(aprovacaoModel);
                                            }
                                        }

                                        new Controller.AprovacaoController().Aprovar(aprovacaoList);

                                        Pesquisar(false);
                                    }
                                    else if (pVal.ItemUID == "btRecusar")
                                    {
                                        List<Model.AprovacaoModel> aprovacaoList = new List<Model.AprovacaoModel>();

                                        Grid gridDetalhes = (Grid)Form.Items.Item("gdDetalhes").Specific;

                                        for (int row = 0; row < gridDetalhes.Rows.Count; row++)
                                        {

                                            if (((CheckBoxColumn)gridDetalhes.Columns.Item("#")).IsChecked(row))
                                            {
                                                Model.AprovacaoModel aprovacaoModel = new Model.AprovacaoModel();
                                                aprovacaoModel.WddCode = Convert.ToInt32(((EditTextColumn)gridDetalhes.Columns.Item("WddCode")).GetText(row));
                                                aprovacaoModel.NumeroDoc = Convert.ToInt32(((EditTextColumn)gridDetalhes.Columns.Item("Nº Doc")).GetText(row));
                                                aprovacaoModel.Data = DateTime.ParseExact(((EditTextColumn)gridDetalhes.Columns.Item("Data")).GetText(row), "yyyyMMdd", CultureInfo.InvariantCulture);
                                                aprovacaoModel.Fornecedor = ((EditTextColumn)gridDetalhes.Columns.Item("Fornecedor")).GetText(row);
                                                aprovacaoModel.ValorTotal = Convert.ToDouble(((EditTextColumn)gridDetalhes.Columns.Item("Valor Total")).GetText(row), CultureInfo.InvariantCulture);

                                                aprovacaoList.Add(aprovacaoModel);
                                            }
                                        }

                                        View.RecusaView.PosRecusaEventHandler posRecusaEvent = new RecusaView.PosRecusaEventHandler(Pesquisar);

                                        Controller.MainController.OpenRecusaView(aprovacaoList, posRecusaEvent);
                                    }
                                    else if (pVal.ItemUID == "gdDetalhes")
                                    {
                                        if (pVal.ColUID == "#")
                                        {
                                            Grid gridDetalhes = (Grid)Form.Items.Item("gdDetalhes").Specific;
                                            string s = ((EditTextColumn)gridDetalhes.Columns.Item("Nº Doc")).GetText(pVal.Row);

                                            if (s.Equals(""))
                                            {
                                                bubbleEvent = false;

                                                Controller.MainController.Application.StatusBar.SetText("Não é possível marcar item de detalhe");
                                            }
                                        }
                                    }                                    
                                }
                            }
                            break;
                        case BoEventTypes.et_ITEM_PRESSED:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID == "ckSelTodos")
                                {
                                    Pesquisar(false);
                                }
                            }
                            break;
                        case BoEventTypes.et_VALIDATE:
                            if (!pVal.BeforeAction)
                            {

                                if (pVal.ItemUID.Equals("etFornec"))
                                {
                                    if (((EditText)Form.Items.Item("etFornec").Specific).String.Trim() == "")
                                    {
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("CodFor", 0, "");
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeFor", 0, "");
                                    }
                                }
                            }

                            break;
                        case BoEventTypes.et_CHOOSE_FROM_LIST:
                            {
                                if (pVal.BeforeAction)
                                {

                                }
                                else
                                {
                                    IChooseFromListEvent chooseFromListEvent = ((IChooseFromListEvent)(pVal));

                                    if (chooseFromListEvent.SelectedObjects != null)
                                    {
                                        if (pVal.ItemUID.Equals("etFilial"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("FilialNome", 0, chooseFromListEvent.SelectedObjects.GetValue("BPLName", 0).ToString());
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("FilialCNPJ", 0, chooseFromListEvent.SelectedObjects.GetValue("TaxIdNum", 0).ToString());
                                        }

                                        if (pVal.ItemUID.Equals("etFornec"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("CodFor", 0, chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString());
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeFor", 0, chooseFromListEvent.SelectedObjects.GetValue("CardName", 0).ToString());
                                        }

                                        if (pVal.ItemUID.Equals("etNomeFor"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("CodFor", 0, chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString());
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeFor", 0, chooseFromListEvent.SelectedObjects.GetValue("CardName", 0).ToString());
                                        }
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_FORM_CLOSE:
                            {
                                if (pVal.BeforeAction)
                                {
                                }
                                else
                                {
                                    Controller.MainController.Application.ItemEvent -= HandleItemEvent;
                                }
                            }
                            break;
                        case BoEventTypes.et_GOT_FOCUS:
                            if (pVal.BeforeAction)
                            {

                            }
                            else
                            {
                                if (!Loaded)
                                {
                                    try
                                    {
                                        Form.DataSources.DataTables.Item("dtFiltro").Rows.Add();

                                        ChooseFromList choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_3");

                                        Conditions conditions = new Conditions();

                                        Condition condition = conditions.Add();

                                        condition.Alias = "CardType";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "S";

                                        choose.SetConditions(conditions);

                                        choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_4");

                                        conditions = new Conditions();

                                        condition = conditions.Add();

                                        condition.Alias = "CardType";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "S";

                                        choose.SetConditions(conditions);

#if !DEBUG
                                        ((EditText)Form.Items.Item("etFilial").Specific).String = "Kurica Ambiental (F)";
#endif
                                        ((CheckBox)Form.Items.Item("ckStaTodos").Specific).Checked = true;
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
            Pesquisar(false);
        }

        private void Pesquisar(bool detalhar)
        {
            string selecionarTodos = ((CheckBox)Form.Items.Item("ckSelTodos").Specific).Checked ? "Y" : "N";

            string dataDe = ((EditText)Form.Items.Item("3").Specific).String;

            string dataAte = ((EditText)Form.Items.Item("5").Specific).String;

            string filial = ((EditText)Form.Items.Item("etFilial").Specific).String;

            string fornecedor = ((EditText)Form.Items.Item("etFornec").Specific).String;

            string statusTodos = ((CheckBox)Form.Items.Item("ckStaTodos").Specific).Checked ? "Y" : "N";

            string statusAprovados = ((CheckBox)Form.Items.Item("ckStaAprov").Specific).Checked ? "Y" : "N";

            string statusPendentes = ((CheckBox)Form.Items.Item("ckStaPend").Specific).Checked ? "Y" : "N";
            
            string query = string.Format(@" select  '{0}' AS ""#"",
                                            OWDD.""WddCode"",
                                            ODRF.""DocEntry"" AS ""Nº Interno"",
		                                    ODRF.""DocNum"" AS ""Nº Doc"",
		                                    ODRF.""DocDate"" AS ""Data"",
		                                    ODRF.""CardName"" AS ""Fornecedor"",
		                                    ODRF.""Comments"" AS ""Observação"",
		                                    ODRF.""DocTotal"" AS ""Valor Total"",
		                                    --case when (ODRF.""DocStatus"" = 'C' AND OWDD.""Status"" = 'Y') then 'Aprovado' 
			                                --     when (ODRF.""DocStatus"" = 'O' AND OWDD.""Status"" = 'N') then 'Recusado'
			                                --     else 'Pendente'
		                                    --end as ""Decisão/Situação"",
                                            case when (OWDD.""Status"" = 'Y') then 'Aprovado' 
			                                     when (OWDD.""Status"" = 'N') then 'Recusado'
			                                     else 'Pendente'
		                                    end as ""Decisão/Situação"",
		                                    '' AS ""Descrição Item"",
		                                    NULL AS ""Quantidade"",
		                                    NULL AS ""Preço Unitário"",
		                                    NULL AS ""Total Item"",
		                                    '' AS ""CC"",
		                                    '' AS ""Contrato"",
		                                    '' AS ""Placa""
                                    from OWDD
                                    inner join ODRF on ODRF.""DocEntry"" = OWDD.""DraftEntry"" AND OWDD.""ObjType"" = 22
                                    inner join OBPL on OBPL.""BPLId"" = ODRF.""BPLId""
                                    where ODRF.""ObjType"" = 22
                                    and ((cast('{1}' as date) = cast('1990-01-01' as date) or cast(ODRF.""DocDate"" as date) >= cast('{1}' as date)))
                                    and ((cast('{2}' as date) = cast('1990-01-01' as date) or cast(ODRF.""DocDate"" as date) <= cast('{2}' as date)))                                    
                                    and ('{3}' = '' or '{3}' = OBPL.""BPLName"")
                                    and ('{4}' = '' or '{4}' = ODRF.""CardCode"")
                                    --and ('{5}' = 'Y' or ('{6}' = 'Y' and (ODRF.""DocStatus"" = 'C' AND OWDD.""Status"" = 'Y')) or ('{7}' = 'Y' and (ODRF.""DocStatus"" = 'O' AND OWDD.""Status"" = 'W')))
                                    and('{5}' = 'Y' or('{6}' = 'Y' and (OWDD.""Status"" = 'Y')) or('{7}' = 'Y' and(OWDD.""Status"" = 'W')))",
                                    selecionarTodos,
                                    dataDe == "" ? "1990-01-01" : Convert.ToDateTime(dataDe).ToString("yyyy-MM-dd"),
                                    dataAte == "" ? "1990-01-01" : Convert.ToDateTime(dataAte).ToString("yyyy-MM-dd"),
                                    filial,
                                    fornecedor,
                                    statusTodos,
                                    statusAprovados,
                                    statusPendentes);

            if (detalhar)
            {
                query = string.Format(@"select * from (
                                        select  '{0}' AS ""#"",
                                            OWDD.""WddCode"",
                                            ODRF.""DocEntry"" AS ""Nº Interno"",
		                                    ODRF.""DocNum"" AS ""Nº Doc"",
		                                    ODRF.""DocDate"" AS ""Data"",
		                                    ODRF.""CardName"" AS ""Fornecedor"",
		                                    ODRF.""Comments"" AS ""Observação"",
		                                    ODRF.""DocTotal"" AS ""Valor Total"",
		                                    --case when (ODRF.""DocStatus"" = 'C' AND OWDD.""Status"" = 'Y') then 'Aprovado' 
			                                --     when (ODRF.""DocStatus"" = 'O' AND OWDD.""Status"" = 'N') then 'Recusado'
			                                --     else 'Pendente'
		                                    --end as ""Decisão/Situação"",
                                            case when (OWDD.""Status"" = 'Y') then 'Aprovado' 
			                                     when (OWDD.""Status"" = 'N') then 'Recusado'
			                                     else 'Pendente'
		                                    end as ""Decisão/Situação"",
		                                    '' AS ""Descrição Item"",
		                                    NULL AS ""Quantidade"",
		                                    NULL AS ""Preço Unitário"",
		                                    NULL AS ""Total Item"",
		                                    '' AS ""CC"",
		                                    '' AS ""Contrato"",
		                                    '' AS ""Placa""
                                    from OWDD
                                    inner join ODRF on ODRF.""DocEntry"" = OWDD.""DraftEntry"" AND OWDD.""ObjType"" = 22
                                    inner join OBPL on OBPL.""BPLId"" = ODRF.""BPLId""                                    
                                    where ODRF.""ObjType"" = 22
                                    and ((cast('{1}' as date) = cast('1990-01-01' as date) or cast(ODRF.""DocDate"" as date) >= cast('{1}' as date)))
                                    and ((cast('{2}' as date) = cast('1990-01-01' as date) or cast(ODRF.""DocDate"" as date) <= cast('{2}' as date)))
                                    and ('{3}' = '' or '{3}' = OBPL.""BPLName"")
                                    and ('{4}' = '' or '{4}' = ODRF.""CardCode"")
                                    --and ('{5}' = 'Y' or ('{6}' = 'Y' and (ODRF.""DocStatus"" = 'C' AND OWDD.""Status"" = 'Y')) or ('{7}' = 'Y' and (ODRF.""DocStatus"" = 'O' AND OWDD.""Status"" = 'W')))
                                    and('{5}' = 'Y' or('{6}' = 'Y' and (OWDD.""Status"" = 'Y')) or('{7}' = 'Y' and(OWDD.""Status"" = 'W')))
                                    union
                                    select  'N' AS ""#"",
                                        OWDD.""WddCode"",
                                        NULL AS ""Nº Interno"",
		                                NULL AS ""Nº Doc"",
		                                NULL AS ""Data"",
		                                '' AS ""Fornecedor"",
		                                '' AS ""Observação"",
		                                NULL AS ""Valor Total"",
		                                '' as ""Decisão/Situação"",
		                                DRF1.""Dscription"" AS ""Descrição Item"",
		                                DRF1.""Quantity"" AS ""Quantidade"",
		                                DRF1.""Price"" AS ""Preço Unitário"",
		                                DRF1.""LineTotal"" AS ""Total Item"",
		                                DRF1.""OcrCode"" AS ""CC"",
		                                DRF1.""OcrCode3"" AS ""Contrato"",
		                                DRF1.""OcrCode5"" AS ""Placa""
                                    from OWDD
                                    inner join ODRF on ODRF.""DocEntry"" = OWDD.""DraftEntry"" AND OWDD.""ObjType"" = 22
                                    inner join OBPL on OBPL.""BPLId"" = ODRF.""BPLId""
                                    inner join DRF1 on DRF1.""DocEntry"" = ODRF.""DocEntry""
                                    where ODRF.""ObjType"" = 22
                                    and ((cast('{1}' as date) = cast('1990-01-01' as date) or cast(ODRF.""DocDate"" as date) >= cast('{1}' as date)))
                                    and ((cast('{2}' as date) = cast('1990-01-01' as date) or cast(ODRF.""DocDate"" as date) <= cast('{2}' as date)))                                    
                                    and ('{3}' = '' or '{3}' = OBPL.""BPLName"")
                                    and ('{4}' = '' or '{4}' = ODRF.""CardCode"")
                                    --and ('{5}' = 'Y' or ('{6}' = 'Y' and (ODRF.""DocStatus"" = 'C' AND OWDD.""Status"" = 'Y')) or ('{7}' = 'Y' and (ODRF.""DocStatus"" = 'O' AND OWDD.""Status"" = 'W')))
                                    and('{5}' = 'Y' or('{6}' = 'Y' and (OWDD.""Status"" = 'Y')) or('{7}' = 'Y' and(OWDD.""Status"" = 'W')))
                                ) vt order by ""WddCode"", ""Descrição Item""",
                                        selecionarTodos,
                                        dataDe == "" ? "1990-01-01" : Convert.ToDateTime(dataDe).ToString("yyyy-MM-dd"),
                                        dataAte == "" ? "1990-01-01" : Convert.ToDateTime(dataAte).ToString("yyyy-MM-dd"),
                                        filial,
                                        fornecedor,
                                        statusTodos,
                                        statusAprovados,
                                        statusPendentes);
            }

            Form.Freeze(true);
            try
            {
                Form.DataSources.DataTables.Item("dtGrid").ExecuteQuery(query);

                Grid gridDetalhes = (Grid)Form.Items.Item("gdDetalhes").Specific;

                gridDetalhes.Columns.Item("WddCode").Visible = false;
                gridDetalhes.Columns.Item("Nº Interno").Editable = false;
                gridDetalhes.Columns.Item("Nº Doc").Editable = false;
                gridDetalhes.Columns.Item("Data").Editable = false;
                gridDetalhes.Columns.Item("Fornecedor").Editable = false;
                gridDetalhes.Columns.Item("Observação").Editable = false;
                gridDetalhes.Columns.Item("Valor Total").Editable = false;
                gridDetalhes.Columns.Item("Decisão/Situação").Editable = false;
                gridDetalhes.Columns.Item("Descrição Item").Editable = false;
                gridDetalhes.Columns.Item("Quantidade").Editable = false;
                gridDetalhes.Columns.Item("Preço Unitário").Editable = false;
                gridDetalhes.Columns.Item("Total Item").Editable = false;
                gridDetalhes.Columns.Item("CC").Editable = false;
                gridDetalhes.Columns.Item("Contrato").Editable = false;
                gridDetalhes.Columns.Item("Placa").Editable = false;

                gridDetalhes.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;

                ((EditTextColumn)gridDetalhes.Columns.Item("Nº Interno")).LinkedObjectType = "112";

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

        private void Detalhar()
        {
            Pesquisar(true);
        }

        private void Comprimir()
        {
            Pesquisar(false);
        }
    }
}
