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

                                        //if (Form.DataSources.UserDataSources.Item("rota").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Rota' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("hrSaida").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Hora Saída OS' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("condPgto").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Condição Pagamento' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("forPgto").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Forma Pagamento' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("clSeg").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Segunda' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("clTerca").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Terça' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("clQuarta").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Quarta' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("clQuinta").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Quinta' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("clSexta").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Sexta' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("clSab").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Sábado' não informado");
                                        //    return;
                                        //}

                                        //if (Form.DataSources.UserDataSources.Item("clDom").Value == "")
                                        //{
                                        //    Controller.MainController.Application.StatusBar.SetText("Campo obrigatório 'Coleta Domingo' não informado");
                                        //    return;
                                        //}

                                        Controller.MainController.Application.SendKeys("{TAB}");

                                        Controller.MainController.Application.StatusBar.SetText("Criando contrato", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);

                                        Form.Freeze(true);
                                        try
                                        {
                                            SAPbobsCOM.CompanyService companyService = Controller.MainController.Company.GetCompanyService();
                                            SAPbobsCOM.BlanketAgreementsService blanketAgreementService = (SAPbobsCOM.BlanketAgreementsService) companyService.GetBusinessService(SAPbobsCOM.ServiceTypes.BlanketAgreementsService);

                                            SAPbobsCOM.BlanketAgreement blanketAgreement = (SAPbobsCOM.BlanketAgreement) blanketAgreementService.GetDataInterface(SAPbobsCOM.BlanketAgreementsServiceDataInterfaces.basBlanketAgreement);
                                            try
                                            {
                                                SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Controller.MainController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                                string query = string.Format(@"select   OQUT.""CardCode"",
                                                                                        OQUT.""U_TitProp"",
                                                                                        OQUT.""U_ModPropGG"",
						                                                                OQUT.""U_ModPropCC"",
						                                                                OQUT.""U_ModPropID"",
						                                                                OQUT.""U_ModPropRE"",
                                                                                        QUT1.""ItemCode"",
                                                                                        QUT1.""Quantity"",
                                                                                        QUT1.""UomCode"",
                                                                                        QUT1.""Price"",
                                                                                        ""@ROTAS"".""U_RotaTransp"",
                                                                                        ""@ROTAS"".""U_Motorista"",
                                                                                        QUT1.""U_NumColetasMes"",
                                                                                        QUT1.""UomEntry"",
                                                                                        QUT1.""U_NumPedidosMes"",
                                                                                        QUT1.""DocEntry"",
                                                                                        QUT1.""LineNum""
                                                                                 from OQUT
                                                                                 inner join QUT1 on QUT1.""DocEntry"" = OQUT.""DocEntry""
                                                                                 left join ""@ROTAS"" on ""@ROTAS"".""Code"" = '{0}'
                                                                                 where OQUT.""DocEntry"" = {1}",
                                                                                    Form.DataSources.UserDataSources.Item("rota").Value, 
                                                                                    m_Proposta.Proposta.ToString());

                                                recordSet.DoQuery(query);

                                                if (!recordSet.EoF)
                                                {
                                                    blanketAgreement.BPCode = recordSet.Fields.Item(0).Value.ToString();
                                                    blanketAgreement.Description = recordSet.Fields.Item(1).Value.ToString();

                                                    if (recordSet.Fields.Item(2).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RGG";
                                                    }
                                                    else if (recordSet.Fields.Item(3).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RCC";
                                                    }
                                                    else if (recordSet.Fields.Item(4).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RSI";
                                                    }
                                                    else if (recordSet.Fields.Item(5).Value.ToString() == "Sim")
                                                    {
                                                        blanketAgreement.UserFields.Item("U_Modelo").Value = "RT";
                                                    }

                                                    while (!recordSet.EoF)
                                                    {
                                                        SAPbobsCOM.BlanketAgreements_ItemsLine linha = blanketAgreement.BlanketAgreements_ItemsLines.Add();

                                                        linha.ItemNo = recordSet.Fields.Item(6).Value.ToString();
                                                        linha.PlannedQuantity = Convert.ToDouble(recordSet.Fields.Item(7).Value);
                                                        linha.UnitPrice = Convert.ToDouble(recordSet.Fields.Item(9).Value);
                                                        linha.UoMEntry = Convert.ToInt32(recordSet.Fields.Item(13).Value);

                                                        if (!recordSet.Fields.Item(14).Value.Equals(""))
                                                            linha.UserFields.Item("U_NumPedidosMes").Value = Convert.ToInt32(recordSet.Fields.Item(14).Value);

                                                        if (!Form.DataSources.UserDataSources.Item("tpModal").Value.Equals(""))
                                                            linha.UserFields.Item("U_TipoModal").Value = Form.DataSources.UserDataSources.Item("tpModal").Value;

                                                        if (!recordSet.Fields.Item(12).Value.Equals("") && Convert.ToInt32(recordSet.Fields.Item(12).Value) > 0)
                                                        {
                                                            linha.UserFields.Item("U_NumColetasMes").Value = Convert.ToInt32(recordSet.Fields.Item(12).Value);
                                                            linha.UserFields.Item("U_QtdColMes").Value = linha.PlannedQuantity * Convert.ToInt32(recordSet.Fields.Item(12).Value);
                                                            linha.UserFields.Item("U_NumColetasTotal").Value = Convert.ToInt32(recordSet.Fields.Item(12).Value) * Convert.ToInt32(Form.DataSources.UserDataSources.Item("mesesCtr").Value);

                                                            linha.UserFields.Item("U_QtdColTotal").Value = Convert.ToInt32(Form.DataSources.UserDataSources.Item("mesesCtr").Value) * Convert.ToDouble(linha.UserFields.Item("U_QtdColMes").Value);
                                                        }
                                                        else if (!recordSet.Fields.Item(14).Value.Equals("") && Convert.ToInt32(recordSet.Fields.Item(14).Value) > 0)
                                                        {
                                                            linha.UserFields.Item("U_NumColetasMes").Value = Convert.ToInt32(recordSet.Fields.Item(14).Value);
                                                            linha.UserFields.Item("U_QtdColMes").Value = linha.PlannedQuantity * Convert.ToInt32(recordSet.Fields.Item(14).Value);
                                                            linha.UserFields.Item("U_NumColetasTotal").Value = Convert.ToInt32(recordSet.Fields.Item(14).Value) * Convert.ToInt32(Form.DataSources.UserDataSources.Item("mesesCtr").Value);
                                                            linha.UserFields.Item("U_QtdColTotal").Value = Convert.ToInt32(Form.DataSources.UserDataSources.Item("mesesCtr").Value) * Convert.ToDouble(linha.UserFields.Item("U_QtdColMes").Value);
                                                        }

                                                        linha.UserFields.Item("U_PropostaEntry").Value = Convert.ToInt32(recordSet.Fields.Item(15).Value.ToString());
                                                        linha.UserFields.Item("U_PropostaLine").Value = Convert.ToInt32(recordSet.Fields.Item(16).Value.ToString());

                                                        if (recordSet.Fields.Item(10).Value.ToString() != "")
                                                            blanketAgreement.UserFields.Item("U_Transportadora").Value = recordSet.Fields.Item(10).Value;
                                                        if (recordSet.Fields.Item(10).Value.ToString() != "")
                                                            blanketAgreement.UserFields.Item("U_Motorista").Value = recordSet.Fields.Item(11).Value;                                                        

                                                        recordSet.MoveNext();
                                                    }

                                                    blanketAgreement.AgreementType = SAPbobsCOM.BlanketAgreementTypeEnum.atSpecific;
                                                    blanketAgreement.Renewal = SAPbobsCOM.BoYesNoEnum.tYES;
                                                    blanketAgreement.RemindUnit = SAPbobsCOM.BoRemindUnits.reu_Days;
                                                    blanketAgreement.RemindTime = 5;
                                                    blanketAgreement.Status = SAPbobsCOM.BlanketAgreementStatusEnum.asOnHold;
                                                    blanketAgreement.StartDate = Convert.ToDateTime(Form.DataSources.UserDataSources.Item("dtIni").Value);
                                                    blanketAgreement.EndDate = Convert.ToDateTime(Form.DataSources.UserDataSources.Item("dtFim").Value);
                                                    if (!Form.DataSources.UserDataSources.Item("condPgto").Value.Equals(""))
                                                        blanketAgreement.PaymentTerms = Convert.ToInt32(Form.DataSources.UserDataSources.Item("condPgto").Value);
                                                    if (!Form.DataSources.UserDataSources.Item("forPgto").Value.Equals(""))
                                                        blanketAgreement.PaymentMethod = Form.DataSources.UserDataSources.Item("forPgto").Value;

                                                    blanketAgreement.UserFields.Item("U_CentroCusto").Value = Form.DataSources.UserDataSources.Item("tpCtr").Value;
                                                    blanketAgreement.UserFields.Item("U_CCContrato").Value = Form.DataSources.UserDataSources.Item("contrato").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("mesesCtr").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_MesesContrato").Value = Convert.ToInt32(Form.DataSources.UserDataSources.Item("mesesCtr").Value);
                                                    if (!Form.DataSources.UserDataSources.Item("rota").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_Rota").Value = Form.DataSources.UserDataSources.Item("rota").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("hrSaida").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_HoraSaidaOS").Value = Form.DataSources.UserDataSources.Item("hrSaida").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("clSeg").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_DiaColetSeg").Value = Form.DataSources.UserDataSources.Item("clSeg").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("clTerca").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_DiaColetTerc").Value = Form.DataSources.UserDataSources.Item("clTerca").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("clQuarta").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_DiaColetQuart").Value = Form.DataSources.UserDataSources.Item("clQuarta").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("clQuinta").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_DiaColetQuin").Value = Form.DataSources.UserDataSources.Item("clQuinta").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("clSexta").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_DiaColetSext").Value = Form.DataSources.UserDataSources.Item("clSexta").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("clSab").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_DiaColetSab").Value = Form.DataSources.UserDataSources.Item("clSab").Value;
                                                    if (!Form.DataSources.UserDataSources.Item("clDom").Value.Equals(""))
                                                        blanketAgreement.UserFields.Item("U_DiaColetDom").Value = Form.DataSources.UserDataSources.Item("clDom").Value;                                                    

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
                                            catch (Exception ex)
                                            {
                                                m_Proposta.Resultado = "Erro ao criar contrato: " + ex.Message;

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

                                        recordSet.DoQuery(string.Format(@"select ""PayMethCod"", ""Descript"" from OPYM 
                                                                        inner join CRD2 ON CRD2.""PymCode"" = OPYM.""PayMethCod""
                                                                        where CRD2.""CardCode"" = '{0}' AND OPYM.""Type"" = 'I' AND OPYM.""Active"" = 'Y'
                                                                        order by OPYM.""Descript""", m_Proposta.CardCode));

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

                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Toco/Truck", "Toco/Truck");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Basculante", "Basculante");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Compactador", "Compactador");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Pipa", "Pipa");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Poliguindaste", "Poliguindaste");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Rollon/Rolloff", "Rollon/Rolloff");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Tanque", "Tanque");
                                        ((ComboBox)Form.Items.Item("cbTipMod").Specific).ValidValues.Add("Julieta", "Julieta");                                        
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
