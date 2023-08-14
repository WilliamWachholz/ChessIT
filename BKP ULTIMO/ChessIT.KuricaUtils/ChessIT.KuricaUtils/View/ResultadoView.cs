using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SAPbouiCOM;

namespace ChessIT.KuricaUtils.View
{
    class ResultadoView
    {
        Form Form;

        bool Loaded;

        List<Model.AprovacaoModel> m_ResultadoAprovacaoList = new List<Model.AprovacaoModel>();

        Model.PropostaModel m_ResultadoProposta = new Model.PropostaModel();

        public ResultadoView(Form form, List<Model.AprovacaoModel> resultadoList)
        {
            this.Form = form;

            m_ResultadoAprovacaoList = resultadoList;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);

            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
        }

        public ResultadoView(Form form, Model.PropostaModel propostaModel)
        {
            this.Form = form;

            m_ResultadoProposta = propostaModel;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);

            Controller.MainController.Application.ItemEvent += HandleItemEvent;

            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
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
                        case BoEventTypes.et_MATRIX_LINK_PRESSED:
                            {
                                if (pVal.BeforeAction)
                                {
                                    if (pVal.ItemUID == "grid")
                                    {
                                        if (pVal.ColUID == "Nº Contrato")
                                        {
                                            bubbleEvent = false;

                                            Grid gridOS = (Grid)Form.Items.Item("grid").Specific;

                                            Controller.MainController.Application.OpenForm((BoFormObjectEnum)1250000025, "", ((EditTextColumn)gridOS.Columns.Item("DocEntry")).GetText(pVal.Row));
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
                    }
                }
            }
            catch (Exception exception)
            {
                Controller.MainController.Application.StatusBar.SetText(exception.Message);
            }
        }

        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_Timer.Stop();

            if (m_ResultadoAprovacaoList.Count > 0)
            {
                string query = "";

                foreach (Model.AprovacaoModel resultado in m_ResultadoAprovacaoList)
                {
                    query += "select " + resultado.NumeroDoc + " as \"Nº Doc\", " +
                                   "'" + resultado.Data.ToString("dd/MM/yyyy") + "' as \"Data\", " +
                                   "'" + resultado.Fornecedor.Trim() + "' as \"Fornecedor\", " +
                                   "'" + resultado.ValorTotal.ToString("C") + "' as \"Valor Total\", " +
                                   "'" + resultado.Resultado + "' as \"Resultado\" " +
                        " from dummy " +
                        " union all ";
                }

                if (query != string.Empty)
                {
                    query = query.Substring(0, query.Length - 10);

                    Form.Freeze(true);
                    try
                    {
                        Form.DataSources.DataTables.Item("dtGrid").ExecuteQuery(query);

                        Grid grid = (Grid)Form.Items.Item("grid").Specific;

                        grid.Columns.Item("Nº Doc").Editable = false;
                        grid.Columns.Item("Data").Editable = false;
                        grid.Columns.Item("Fornecedor").Editable = false;
                        grid.Columns.Item("Valor Total").Editable = false;
                        grid.Columns.Item("Resultado").Editable = false;
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.WriteAllText("Sql.sql", query);
                    }
                    finally
                    {
                        Form.Freeze(false);
                    }
                }
            }
            else
            {
                string query = "";

                query = "select (select \"DocNum\" from OQUT where \"DocEntry\" = " + m_ResultadoProposta.Proposta + ") as \"Nº Proposta\", " +
                               " (select \"Number\" from OOAT where \"AbsID\" = " + m_ResultadoProposta.Contrato.ToString() + ") as \"Nº Contrato\", " +
                               "" + m_ResultadoProposta.Contrato.ToString() + " as \"DocEntry\", " +
                               "'" + m_ResultadoProposta.Resultado + "' as \"Resultado\" " +
                        " from dummy ";

                if (query != string.Empty)
                {                    
                    Form.Freeze(true);
                    try
                    {
                        Form.DataSources.DataTables.Item("dtGrid").ExecuteQuery(query);

                        Grid grid = (Grid)Form.Items.Item("grid").Specific;

                        grid.Columns.Item("Nº Proposta").Editable = false;
                        grid.Columns.Item("Nº Contrato").Editable = false;
                        grid.Columns.Item("DocEntry").Visible = false;
                        grid.Columns.Item("Resultado").Editable = false;

                        ((EditTextColumn)grid.Columns.Item("Nº Contrato")).LinkedObjectType = "999";
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.WriteAllText("Sql.sql", query);
                    }
                    finally
                    {
                        Form.Freeze(false);
                    }
                }
            }
        }

        Timer m_Timer = new Timer(1000);

    }
}
