using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace ChessIT.KuricaUtils.View
{
    class PropostaContratoView
    {
        Form Form;

        bool Loaded;

        Model.PropostaModel m_Proposta = new Model.PropostaModel();

        public PropostaContratoView(Form form, Model.PropostaModel propostaModel)
        {
            this.Form = form;

            m_Proposta = propostaModel;

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
                                    if (pVal.ItemUID == "btFim")
                                    {
                                        if (Form.DataSources.UserDataSources.Item("tpCtr").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'CC - Tipo Contrato' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("contrato").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'CC - Contrato' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("dtIni").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Data Início' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("dtFim").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Data Fim' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("mesesCtr").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Meses Contrato' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("rota").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Rota' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("hrSaida").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Hora Saída OS' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("condPgto").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Condição Pagamento' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("forPgto").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Forma Pagamento' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("clSeg").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Segunda' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("clTerca").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Terça' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("clQuarta").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Quarta' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("clQuinta").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Quinta' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("clSexta").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Sexta' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("clSab").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Sábado' não informado");
                                            return;
                                        }

                                        if (Form.DataSources.UserDataSources.Item("clDom").Value == "")
                                        {
                                            Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Domingo' não informado");
                                            return;
                                        }

                                        Controller.MainController.Application.StatusBar.SetText("Criando contrato");

                                        Form.Freeze(true);
                                        try
                                        {
                                            SAPbobsCOM.CompanyService companyService = Controller.MainController.Company.GetCompanyService();
                                            SAPbobsCOM.BlanketAgreementsService blanketAgreementService = (SAPbobsCOM.BlanketAgreementsService) companyService.GetBusinessService(SAPbobsCOM.ServiceTypes.BlanketAgreementsService);

                                            SAPbobsCOM.BlanketAgreement blanketAgreement = (SAPbobsCOM.BlanketAgreement) blanketAgreementService.GetDataInterface(SAPbobsCOM.BlanketAgreementsServiceDataInterfaces.basBlanketAgreement);
                                            try
                                            {
                                                SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                                string query = @"select OQUT.""CardCode"",
                                                                        OQUT.""U_TitProp"",
                                                                        OQUT.""U_ModPropGG"",
						                                                OQUT.""U_ModPropCC"",
						                                                OQUT.""U_ModPropID"",
						                                                OQUT.""U_ModPropRE"",
                                                                        QUT1.""ItemCode"",
                                                                        QUT1.""Quantity"",
                                                                        QUT1.""UomCode"",
                                                                        QUT1.""Price""
                                                                 from OQUT
                                                                 inner join QUT1 on QUT1.""DocEntry"" = OQUT.""DocEntry""
                                                        where OQUT.""DocEntry"" = " + m_Proposta.Proposta.ToString();

#if DEBUG
                                                query = @"select OQUT.""CardCode"",                                                                        
                                                                        QUT1.""ItemCode"",
                                                                        QUT1.""Quantity"",
                                                                        QUT1.""UomCode"",
                                                                        QUT1.""Price""
                                                                 from OQUT
                                                                 inner join QUT1 on QUT1.""DocEntry"" = OQUT.""DocEntry""
                                                        where OQUT.""DocEntry"" = " + m_Proposta.Proposta.ToString();
#endif

                                                recordSet.DoQuery(query);

                                                if (!recordSet.EoF)
                                                {
                                                    blanketAgreement.BPCode = recordSet.Fields.Item(0).Value.ToString();
                                                    blanketAgreement.Description = recordSet.Fields.Item(1).Value.ToString();

#if !DEBUG
                                                    if (recordSet.Fields.Item(2).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RGG";
                                                    }
                                                    else if (recordSet.Fields.Item(3).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RSU";
                                                    }
                                                    else if (recordSet.Fields.Item(4).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RSI";
                                                    }
                                                    else if (recordSet.Fields.Item(5).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RT";
                                                    }

#endif

                                                    while (!recordSet.EoF)
                                                    {
                                                        SAPbobsCOM.BlanketAgreements_ItemsLine linha = blanketAgreement.BlanketAgreements_ItemsLines.Add();

#if !DEBUG
                                                        linha.ItemNo = recordSet.Fields.Item(6).Value.ToString();
                                                        linha.PlannedQuantity = Convert.ToDouble(recordSet.Fields.Item(7).Value);
                                                        linha.UnitPrice = Convert.ToDouble(recordSet.Fields.Item(9).Value);

#endif
                                                        linha.ItemNo = recordSet.Fields.Item(1).Value.ToString();
                                                        linha.PlannedQuantity = Convert.ToDouble(recordSet.Fields.Item(2).Value);
                                                        linha.UnitPrice = Convert.ToDouble(recordSet.Fields.Item(4).Value);

                                                        recordSet.MoveNext();
                                                    }

                                                    blanketAgreement.StartDate = Convert.ToDateTime(Form.DataSources.UserDataSources.Item("dtIni").Value);
                                                    blanketAgreement.EndDate = Convert.ToDateTime(Form.DataSources.UserDataSources.Item("dtFim").Value);
                                                    blanketAgreement.PaymentTerms = Convert.ToInt32(Form.DataSources.UserDataSources.Item("condPgto").Value);
                                                    blanketAgreement.PaymentMethod = Form.DataSources.UserDataSources.Item("forPgto").Value;

#if !DEBUG

                                                    blanketAgreement.UserFields.Item("U_CentroCusto").Value = Form.DataSources.UserDataSources.Item("tpCtr").Value;
                                                    blanketAgreement.UserFields.Item("U_CCContrato").Value = Form.DataSources.UserDataSources.Item("contrato").Value;                                                    
                                                    blanketAgreement.UserFields.Item("U_MesesContrato").Value = Convert.ToInt32(Form.DataSources.UserDataSources.Item("mesesCtr").Value);                                                    
                                                    blanketAgreement.UserFields.Item("U_Rota").Value = Form.DataSources.UserDataSources.Item("rota").Value;
                                                    blanketAgreement.UserFields.Item("U_HoraSaidaOS").Value = Form.DataSources.UserDataSources.Item("hrSaida").Value;
                                                    blanketAgreement.UserFields.Item("U_DiaColetSeg").Value = Form.DataSources.UserDataSources.Item("clSeg").Value == "Y" ? "Sim" : "Não";
                                                    blanketAgreement.UserFields.Item("U_DiaColetTerc").Value = Form.DataSources.UserDataSources.Item("clTerca").Value == "Y" ? "Sim" : "Não";
                                                    blanketAgreement.UserFields.Item("U_DiaColetQuart").Value = Form.DataSources.UserDataSources.Item("clQuarta").Value == "Y" ? "Sim" : "Não";
                                                    blanketAgreement.UserFields.Item("U_DiaColetQuin").Value = Form.DataSources.UserDataSources.Item("clQuinta").Value == "Y" ? "Sim" : "Não";
                                                    blanketAgreement.UserFields.Item("U_DiaColetSext").Value = Form.DataSources.UserDataSources.Item("clSexta").Value == "Y" ? "Sim" : "Não";
                                                    blanketAgreement.UserFields.Item("U_DiaColetSab").Value = Form.DataSources.UserDataSources.Item("clSab").Value == "Y" ? "Sim" : "Não";
                                                    blanketAgreement.UserFields.Item("U_DiaColetDom").Value = Form.DataSources.UserDataSources.Item("clDom").Value == "Y" ? "Sim" : "Não";

#endif

                                                    int contrato = blanketAgreementService.AddBlanketAgreement(blanketAgreement).AgreementNo;
                                                    
                                                    if (contrato == 0)
                                                    {
                                                        int erro = 0;
                                                        string msg = "";

                                                        Controller.MainController.Company.GetLastError(out erro, out msg);

                                                        m_Proposta.Resultado = "Erro ao criar contrato: " + erro + " - " + msg;
                                                    }
                                                    else
                                                    {
                                                        m_Proposta.Contrato = contrato;
                                                        m_Proposta.Resultado = "Contrato criado com sucesso";
                                                    }
                                                }
                                                else
                                                {
                                                    m_Proposta.Resultado = "Proposta não encontrada";
                                                }

                                                Controller.MainController.OpenResultadoView(m_Proposta);
                                            }
                                            finally
                                            {
                                                System.Runtime.InteropServices.Marshal.ReleaseComObject(blanketAgreement);
                                                System.Runtime.InteropServices.Marshal.ReleaseComObject(blanketAgreementService);
                                                GC.Collect();
                                            }
                                        }
                                        finally
                                        {
                                            Form.Freeze(false);
                                        }

                                        Form.Close();
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
                                        ((ComboBox)Form.Items.Item("cbTipoCtr").Specific).ValidValues.Add("", "[Selecionar]");

                                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                        recordSet.DoQuery(@"select ""PrcCode"", ""PrcName"" from OPRC where OPRC.""DimCode"" = 2 ORDER BY ""PrcName""");

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbTipoCtr").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        ((ComboBox)Form.Items.Item("cbCtr").Specific).ValidValues.Add("", "[Selecionar]");

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                        recordSet.DoQuery(@"select ""PrcCode"", ""PrcName"" from OPRC where OPRC.""DimCode"" = 3 ORDER BY ""PrcName""");

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbCtr").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        ((ComboBox)Form.Items.Item("cbCondPgto").Specific).ValidValues.Add("0", "[Selecionar]");

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                        recordSet.DoQuery(@"select ""GroupNum"", ""PymntGroup"" from OCTG ORDER BY ""PymntGroup""");

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbCondPgto").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

                                        ((ComboBox)Form.Items.Item("cbForPgto").Specific).ValidValues.Add("", "[Selecionar]");

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                        recordSet.DoQuery(@"select ""PayMethCod"", ""Descript"" from OPYM where ""Type"" = 'I' ORDER BY ""Descript""");

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbForPgto").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

#if !DEBUG
                                        ((ComboBox)Form.Items.Item("cbRota").Specific).ValidValues.Add("", "[Selecionar]");

                                        recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                        recordSet.DoQuery(@"select ""Code"", ""Name"" from ""@ROTAS"" ORDER BY ""Name""");

                                        while (!recordSet.EoF)
                                        {
                                            ((ComboBox)Form.Items.Item("cbRota").Specific).ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }
#endif

                                        ((ComboBox)Form.Items.Item("cbSegunda").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbSegunda").Specific).ValidValues.Add("Sim", "Sim");
                                        ((ComboBox)Form.Items.Item("cbSegunda").Specific).ValidValues.Add("Não", "Não");

                                        ((ComboBox)Form.Items.Item("cbTerca").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbTerca").Specific).ValidValues.Add("Sim", "Sim");
                                        ((ComboBox)Form.Items.Item("cbTerca").Specific).ValidValues.Add("Não", "Não");

                                        ((ComboBox)Form.Items.Item("cbQuarta").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbQuarta").Specific).ValidValues.Add("Sim", "Sim");
                                        ((ComboBox)Form.Items.Item("cbQuarta").Specific).ValidValues.Add("Não", "Não");

                                        ((ComboBox)Form.Items.Item("cbQuinta").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbQuinta").Specific).ValidValues.Add("Sim", "Sim");
                                        ((ComboBox)Form.Items.Item("cbQuinta").Specific).ValidValues.Add("Não", "Não");

                                        ((ComboBox)Form.Items.Item("cbSexta").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbSexta").Specific).ValidValues.Add("Sim", "Sim");
                                        ((ComboBox)Form.Items.Item("cbSexta").Specific).ValidValues.Add("Não", "Não");

                                        ((ComboBox)Form.Items.Item("cbSabado").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbSabado").Specific).ValidValues.Add("Sim", "Sim");
                                        ((ComboBox)Form.Items.Item("cbSabado").Specific).ValidValues.Add("Não", "Não");

                                        ((ComboBox)Form.Items.Item("cbDomingo").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbDomingo").Specific).ValidValues.Add("Sim", "Sim");
                                        ((ComboBox)Form.Items.Item("cbDomingo").Specific).ValidValues.Add("Não", "Não");
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
    }
}
