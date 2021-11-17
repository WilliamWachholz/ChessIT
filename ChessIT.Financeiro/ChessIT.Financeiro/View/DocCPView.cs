using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SAPbouiCOM;

namespace ChessIT.Financeiro.View
{
    class DocCPView
    {
        Form Form;

        bool Loaded;

        public DocCPView(Form form)
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
                                    if (pVal.ItemUID == "btPrevia")
                                    {
                                        GerarLC();
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_VALIDATE:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID.Equals("etCNPJ"))
                                {
                                    string cnpj = ((EditText)Form.Items.Item("etCNPJ").Specific).String.Trim();

                                    if (cnpj != "")
                                    {
                                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(string.Format(@"select OCRD.""CardCode"", OCRD.""CardName"" from CRD7 INNER JOIN OCRD ON OCRD.""CardCode"" = CRD7.""CardCode"" where ""TaxId0"" = '{0}' or ""TaxId4"" = '{0}'", cnpj));

                                        if (!recordSet.EoF)
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("CodFor", 0, recordSet.Fields.Item(0).Value);
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeFor", 0, recordSet.Fields.Item(1).Value);
                                        }
                                    }
                                }

                                if (pVal.ItemUID.Equals("etCodFor"))
                                {
                                    if (((EditText)Form.Items.Item("etCodFor").Specific).String.Trim() == "")
                                    {
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeFor", 0, "");
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("CNPJ", 0, "");                                        
                                    }
                                }

                                if (pVal.ItemUID.Equals("etTotalDoc") || pVal.ItemUID.Equals("etNumPar"))
                                {
                                    double totalDoc = Convert.ToDouble(Form.DataSources.DataTables.Item("dtFiltro").GetValue("TotalDoc", 0));

                                    int numeroParcelas = Convert.ToInt32(Form.DataSources.DataTables.Item("dtFiltro").GetValue("NumPar", 0));

                                    if (numeroParcelas > 0)
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("ValorPar", 0, totalDoc / numeroParcelas);
                                    else
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("ValorPar", 0, 0);
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
                                        if (pVal.ItemUID.Equals("etCodFor") || pVal.ItemUID.Equals("etNomeFor"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("CodFor", 0, chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString());
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeFor", 0, chooseFromListEvent.SelectedObjects.GetValue("CardName", 0).ToString());

                                            SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                            recordSet.DoQuery(string.Format(@"select coalesce(""TaxId0"", ""TaxId4"") from CRD7 where ""CardCode"" = '{0}'", chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString()));

                                            if (!recordSet.EoF)
                                            {
                                                Form.DataSources.DataTables.Item("dtFiltro").SetValue("CNPJ", 0, recordSet.Fields.Item(0).Value);
                                            }
                                        }

                                        if (pVal.ItemUID.Equals("etContaC"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("ContaC", 0, chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString());
                                        }

                                        if (pVal.ItemUID.Equals("etContaJ"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("ContaJ", 0, chooseFromListEvent.SelectedObjects.GetValue("AcctCode", 0).ToString());
                                        }

                                        if (pVal.ItemUID.Equals("etForPgto"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("ForPgto", 0, chooseFromListEvent.SelectedObjects.GetValue("PayMethCod", 0).ToString());
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

                                        condition.Alias = "CardType";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "S";

                                        choose.SetConditions(conditions);

                                        choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_4");

                                        conditions = new Conditions();

                                        condition = conditions.Add();

                                        condition.Alias = "Postable";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "Y";

                                        choose.SetConditions(conditions);

                                        choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_5");

                                        conditions = new Conditions();

                                        condition = conditions.Add();

                                        condition.Alias = "Postable";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "Y";

                                        choose.SetConditions(conditions);

                                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(@"select ""BPLId"", ""BPLName"" from OBPL");

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbEmpresa").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }
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


        System.Timers.Timer m_timerLC = new System.Timers.Timer(1000);

        Form lcForm = null;

        int numeroParcelas = 0;

        int parcela = 0;

        double valorJuros = 0;

        double valorParcela = 0;

        double diferenca = 0;

        DateTime dataVcto1 = new DateTime();

        DateTime dataVencimento = new DateTime();

        private void GerarLC()
        {
            if (((EditText)Form.Items.Item("etCodFor").Specific).String == "")
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Cód. Fornecedor] não preenchido");
                return;
            }

            if (((EditText)Form.Items.Item("etCNPJ").Specific).String == "")
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [CNPJ. Fornecedor] não preenchido");
                return;
            }

            if (((ComboBox)Form.Items.Item("cbEmpresa").Specific).Selected == null)
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Empresa] não preenchido");
                return;
            }

            if (Convert.ToInt32(Form.DataSources.DataTables.Item("dtFiltro").GetValue("NumDoc", 0)) == 0)
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Nº Doc.] não preenchido");
                return;
            }

            if (((EditText)Form.Items.Item("etDataDoc").Specific).String == "")
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Data Documento] não preenchido");
                return;
            }

            if (((EditText)Form.Items.Item("etDataLcto").Specific).String == "")
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Data Lançamento] não preenchido");
                return;
            }

            if (((EditText)Form.Items.Item("etObs").Specific).String == "")
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Observação] não preenchido");
                return;
            }

            if (((EditText)Form.Items.Item("etContaC").Specific).String == "")
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Conta Contábil] não preenchido");
                return;
            }

            if (Convert.ToDouble(Form.DataSources.DataTables.Item("dtFiltro").GetValue("TotalDoc", 0)) == 0)
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Total Doc.] não preenchido");
                return;
            }

            if (Convert.ToInt32(Form.DataSources.DataTables.Item("dtFiltro").GetValue("NumPar", 0)) == 0)
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Nº Parcelas] não preenchido");
                return;
            }

            if (((EditText)Form.Items.Item("etDtVcto1").Specific).String == "")
            {
                Controller.MainController.Application.StatusBar.SetText("Campo obrigatório [Data Vcto. 1ª Parcela] não preenchido");
                return;
            }

            Controller.MainController.Application.ActivateMenuItem("1540");

            for (int i = 5; i >= 0; i--)
            {
                System.Threading.Thread.Sleep(200);

                try
                {
                    lcForm = Controller.MainController.Application.Forms.GetFormByTypeAndCount(392, i);

                    break;
                }
                catch { }                
            }

            if (lcForm != null)
            {
#if !DEBUG
                    ((EditText)lcForm.Items.Item("U_NDocFin").Specific).String = ((EditText)Form.Items.Item("etNumDoc").Specific).String;
                    ((EditText)lcForm.Items.Item("U_FPagFin").Specific).String = ((EditText)Form.Items.Item("etForPgto").Specific).String;
#endif

                ((EditText)lcForm.Items.Item("97").Specific).String = ((EditText)Form.Items.Item("etDataDoc").Specific).String;
                ((EditText)lcForm.Items.Item("6").Specific).String = ((EditText)Form.Items.Item("etDataLcto").Specific).String;

                ((ComboBox)lcForm.Items.Item("1320002034").Specific).Select(((ComboBox)Form.Items.Item("cbEmpresa").Specific).Selected.Value);

                ((EditText)lcForm.Items.Item("102").Specific).String = Convert.ToDateTime(Form.DataSources.DataTables.Item("dtFiltro").GetValue("DataVcto1", 0)).AddMonths(numeroParcelas).ToString("dd/MM/yyyy");

                ((EditText)lcForm.Items.Item("10").Specific).String = ((EditText)Form.Items.Item("etObs").Specific).String;

                numeroParcelas = Convert.ToInt32(Form.DataSources.DataTables.Item("dtFiltro").GetValue("NumPar", 0));

                parcela = 1;

                double valorTotal = Convert.ToDouble(Form.DataSources.DataTables.Item("dtFiltro").GetValue("TotalDoc", 0));

                valorJuros = Convert.ToDouble(Form.DataSources.DataTables.Item("dtFiltro").GetValue("ValorJuros", 0));

                valorParcela = Convert.ToDouble(Form.DataSources.DataTables.Item("dtFiltro").GetValue("ValorPar", 0));

                valorParcela = valorParcela + (valorJuros / numeroParcelas);

                diferenca = Math.Round((valorTotal + valorJuros) - (Math.Round(valorParcela, 2) * numeroParcelas), 2);

                dataVcto1 = Convert.ToDateTime(Form.DataSources.DataTables.Item("dtFiltro").GetValue("DataVcto1", 0));

                dataVencimento = dataVcto1;

                m_timerLC.Elapsed += FinalizarLC;
                m_timerLC.Enabled = true;
            }          
        }


        private void FinalizarLC(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_timerLC.Enabled = false;

            if (lcForm != null)
            {
                lcForm.Select();

                Matrix grid = (Matrix)lcForm.Items.Item("76").Specific;

                if (parcela > 1)
                {
                    Controller.MainController.Application.SendKeys("^{TAB}");

                    dataVencimento = dataVcto1.AddMonths(parcela - 2);

                    ((EditText)grid.Columns.Item("13").Cells.Item(parcela - 1).Specific).String = dataVencimento.ToString("dd/MM/yyyy");

                    if (parcela == numeroParcelas + 1)
                        ((EditText)grid.Columns.Item("6").Cells.Item(parcela - 1).Specific).String = (valorParcela + diferenca).ToString();                   
                    else
                        ((EditText)grid.Columns.Item("6").Cells.Item(parcela - 1).Specific).String = valorParcela.ToString();

                    ((EditText)grid.Columns.Item("9").Cells.Item(parcela - 1).Specific).String = ((EditText)Form.Items.Item("etObs").Specific).String + " Contas a Pagar " + (parcela - 1).ToString() + "/" + numeroParcelas.ToString();
                }

                if (parcela == numeroParcelas + 1)
                {
                    lcForm.Freeze(true);
                    try
                    {
                        ((EditText)grid.Columns.Item("1").Cells.Item(numeroParcelas + 1).Specific).String = ((EditText)Form.Items.Item("etContaC").Specific).String;

                        ((EditText)grid.Columns.Item("13").Cells.Item(numeroParcelas + 1).Specific).String = dataVencimento.ToString("dd/MM/yyyy");

                        ((EditText)grid.Columns.Item("5").Cells.Item(numeroParcelas + 1).Specific).String = ((EditText)Form.Items.Item("etTotalDoc").Specific).String;

                        ((EditText)grid.Columns.Item("9").Cells.Item(numeroParcelas + 1).Specific).String = ((EditText)Form.Items.Item("etObs").Specific).String;

                        if (((EditText)Form.Items.Item("etContaJ").Specific).String != "")
                        {
                            ((EditText)grid.Columns.Item("1").Cells.Item(numeroParcelas + 2).Specific).String = ((EditText)Form.Items.Item("etContaJ").Specific).String;

                            ((EditText)grid.Columns.Item("13").Cells.Item(numeroParcelas + 2).Specific).String = dataVencimento.ToString("dd/MM/yyyy");

                            ((EditText)grid.Columns.Item("5").Cells.Item(numeroParcelas + 2).Specific).String = valorJuros.ToString();

                            ((EditText)grid.Columns.Item("9").Cells.Item(numeroParcelas + 2).Specific).String = ((EditText)Form.Items.Item("etObs").Specific).String;
                        }                        
                    }
                    finally
                    {
                        lcForm.Freeze(false);
                    }
                }
                else
                {
                    grid.Columns.Item("1").Cells.Item(parcela).Click();

                    SetarCodigoPNThread(((EditText)Form.Items.Item("etCodFor").Specific).String);

                    System.Windows.Forms.SendKeys.SendWait("^{v}");                    

                    parcela++;

                    m_timerLC.Enabled = true;
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
