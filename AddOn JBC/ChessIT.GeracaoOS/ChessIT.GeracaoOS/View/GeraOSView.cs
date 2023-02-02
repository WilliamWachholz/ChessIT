using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessIT.GeracaoOS.Controller;
using ChessIT.GeracaoOS.Helper;
using ChessIT.GeracaoOS.Model;
using SAPbouiCOM;


namespace ChessIT.GeracaoOS.View
{
    class GeraOSView
    {
        Form Form;

        bool Loaded;

        public GeraOSView(Form form)
        {
            this.Form = form;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);



            Controller.MainController.oApplication.ItemEvent += HandleItemEvent;
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
                                    if (pVal.ItemUID == "fldPes")
                                    {
                                        Form.PaneLevel = 3;

                                        Form.Freeze(true);
                                        try
                                        {
                                            Form.Items.Item("8").Left = Form.Items.Item("20").Left;
                                            Form.Items.Item("etNrCtr").Left = Form.Items.Item("cbDiaCol").Left;
                                            Form.Items.Item("11").Left = Form.Items.Item("20").Left;
                                            Form.Items.Item("cbModCtr").Left = Form.Items.Item("cbDiaCol").Left;
                                            Form.Items.Item("26").Top = 59;
                                            Form.Items.Item("etNrPlaca").Top = 59;
                                            Form.Items.Item("26").Visible = true;
                                            Form.Items.Item("etNrPlaca").Visible = true;


                                            Form.Items.Item("18").Visible = true;
                                            Form.Items.Item("etNrRota").Visible = true;
                                            Form.Items.Item("20").Visible = true;
                                            Form.Items.Item("cbDiaCol").Visible = true;

                                            Form.Items.Item("btPesqPes").Visible = true;

                                            Form.Items.Item("etClienteN").Visible = true;
                                            Form.Items.Item("etCliente").Visible = true;
                                            Form.Items.Item("2").Visible = true;

                                            //rota
                                            Form.Items.Item("18").Left = 17;
                                            Form.Items.Item("etNrRota").Left = 98;
                                            //placa
                                            Form.Items.Item("26").Left = 17;
                                            Form.Items.Item("26").Top = 27;
                                            Form.Items.Item("etNrPlaca").Left = 98;
                                            Form.Items.Item("etNrPlaca").Top = 27;


                                            //cliente
                                            Form.Items.Item("2").Top = 60;
                                            Form.Items.Item("etCliente").Top = 60;
                                            Form.Items.Item("etClienteN").Top = 60;

                                            //dia coleta
                                            Form.Items.Item("20").Top = 42;
                                            Form.Items.Item("cbDiaCol").Top = 42;
                                            Form.Items.Item("20").Left = 17;
                                            Form.Items.Item("cbDiaCol").Left = 98;


                                            string query = @"
                                                    select 
	                                                    'N' as ""#""
                                                        , T0.""DocEntry"" as ""Nº Interno""
                                                        , T0.""DocNum"" AS ""Nº OS""
                                                        , T0.""CardCode""
                                                        , T0.""CardName"" ""Cliente""
                                                        , T0.""U_NPlaca"" ""Placa""
                                                        , T0.""U_VolumeM3"" ""m3""
                                                        , T0.""U_Tara"" ""Tara""
                                                        , T0.""U_PesoBruto"" ""Peso Bruto""
                                                        , T0.""U_PesoLiq"" ""Peso Liq.""
                                                    from
                                                        ORDR T0
                                                        left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = T0.""U_NPlaca""
                                                        left join OCRD TRANSP ON TRANSP.""CardCode"" = T0.""U_CodTransp""
                                                        left join OUSR ON T0.""UserSign"" = OUSR.""USERID""
                                                    where
                                                        T0.""CANCELED"" = 'N'
                                                        and T0.""DocStatus"" = 'O'
                                                        and T0.""U_Status"" = 'P'
                                                        and T0.""U_Situacao"" = 33
                                            ";

                                            Form.DataSources.DataTables.Item("dtPes").ExecuteQuery(query);

                                            //Form.DataSources.UserDataSources.Add("chkTdPes", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
                                            //((CheckBox)Form.Items.Item("ckSelTPes").Specific).DataBind.SetBound(true, "", "chkTdPes");

                                            ((CheckBox)Form.Items.Item("ckSelTPes").Specific).ValOff = "N";
                                            ((CheckBox)Form.Items.Item("ckSelTPes").Specific).ValOn = "Y";

                                            ConfiguraGridPes();
                                            ((ComboBox)Form.Items.Item("cbAgrOS").Specific).Select("0");
                                            ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Select("0");
                                            //((ComboBox)Form.Items.Item("cbRespFat").Specific).Select("0");

                                        }
                                        finally
                                        {
                                            Form.Freeze(false);
                                        }
                                    }
                                    else if (pVal.ItemUID == "fldCtr")
                                    {
                                        Form.PaneLevel = 1;

                                        Form.Freeze(true);
                                        try
                                        {
                                            Form.Items.Item("8").Left = 17;
                                            Form.Items.Item("etNrCtr").Left = 98;
                                            Form.Items.Item("11").Left = 17;
                                            Form.Items.Item("cbModCtr").Left = 98;
                                            //Form.Items.Item("26").Top = 42;
                                            //Form.Items.Item("etNrPlaca").Top = 42;
                                            Form.Items.Item("26").Visible = false;
                                            Form.Items.Item("etNrPlaca").Visible = false;
                                            Form.Items.Item("btPesqPes").Visible = false;


                                            //rota
                                            Form.Items.Item("18").Left = 510;
                                            Form.Items.Item("etNrRota").Left = 590;
                                            //placa
                                            Form.Items.Item("26").Left = 510;
                                            Form.Items.Item("26").Top = 42;
                                            Form.Items.Item("etNrPlaca").Left = 590;
                                            Form.Items.Item("etNrPlaca").Top = 42;


                                            //cliente
                                            Form.Items.Item("2").Top = 13;
                                            Form.Items.Item("etCliente").Top = 13;
                                            Form.Items.Item("etClienteN").Top = 13;

 
                                            string query = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"" from dummy";

                                            Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(query);

                                            ((ComboBox)Form.Items.Item("cbAgrOS").Specific).Select("0");
                                            ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Select("0");
                                            ((ComboBox)Form.Items.Item("cbRespFat").Specific).Select("0");
                                        }
                                        finally
                                        {
                                            Form.Freeze(false);
                                        }
                                    }
                                    else if (pVal.ItemUID == "fldOS")
                                    {
                                        Form.PaneLevel = 2;

                                        Form.Freeze(true);
                                        try
                                        {
                                            Form.Items.Item("8").Left = Form.Items.Item("20").Left;
                                            Form.Items.Item("etNrCtr").Left = Form.Items.Item("cbDiaCol").Left;
                                            Form.Items.Item("11").Left = Form.Items.Item("20").Left;
                                            Form.Items.Item("cbModCtr").Left = Form.Items.Item("cbDiaCol").Left;
                                            Form.Items.Item("26").Top = 59;
                                            Form.Items.Item("etNrPlaca").Top = 59;
                                            Form.Items.Item("26").Visible = true;
                                            Form.Items.Item("etNrPlaca").Visible = true;
                                            Form.Items.Item("btPesqPes").Visible = false;

                                            //rota
                                            Form.Items.Item("18").Left = 510;
                                            Form.Items.Item("etNrRota").Left = 590;
                                            //placa
                                            Form.Items.Item("26").Left = 510;
                                            Form.Items.Item("26").Top = 42;
                                            Form.Items.Item("etNrPlaca").Left = 590;
                                            Form.Items.Item("etNrPlaca").Top = 42;


                                            //cliente
                                            Form.Items.Item("2").Top = 13;
                                            Form.Items.Item("etCliente").Top = 13;
                                            Form.Items.Item("etClienteN").Top = 13;

                                            string query = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"" from dummy";

                                            Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(query);

                                            ((ComboBox)Form.Items.Item("cbAgrOS").Specific).Select("0");
                                            ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Select("0");
                                            ((ComboBox)Form.Items.Item("cbRespFat").Specific).Select("0");
                                        }
                                        finally
                                        {
                                            Form.Freeze(false);
                                        }
                                    }
                                    else if (pVal.ItemUID == "btOK")
                                    {
                                        Form.Close();
                                    }
                                    else if (pVal.ItemUID == "btPesqCtr")
                                    {
                                        CarregarContratos();
                                    }
                                    else if (pVal.ItemUID == "btGerarOS")
                                    {
                                        GerarOS();
                                    }
                                    else if (pVal.ItemUID == "btPesqOS")
                                    {
                                        CarregarOS();
                                    }
                                    else if (pVal.ItemUID == "btGerarFat")
                                    {
                                        GerarFatura();
                                    }
                                    else if (pVal.ItemUID == "btPesqPes")
                                    {
                                        CarregarPes();
                                    }
                                    else if (pVal.ItemUID == "gridOS")
                                    {
                                        if (pVal.ColUID != "#")
                                        {
                                            Grid gridOS = (Grid)Form.Items.Item("gridOS").Specific;
                                            ((CheckBoxColumn)gridOS.Columns.Item("#")).Check(pVal.Row, !((CheckBoxColumn)gridOS.Columns.Item("#")).IsChecked(pVal.Row));
                                            gridOS.Rows.SelectedRows.Add(pVal.Row);
                                        }
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_ITEM_PRESSED:
                            if (pVal.BeforeAction)
                            {
                                if (pVal.ItemUID == "btRateio")
                                {
                                    if (((ComboBox)Form.Items.Item("cbTpRateio").Specific).Selected == null)
                                    {
                                        LogHelper.InfoError("Selecione o Tipo de Rateio");
                                    }
                                    else if (((ComboBox)Form.Items.Item("cbTpRateio").Specific).Selected.Value.Equals("0"))
                                    {
                                        LogHelper.InfoError("Selecione o Tipo de Rateio");
                                    }
                                    else
                                    {
                                        string tiporateio = ((ComboBox)Form.Items.Item("cbTpRateio").Specific).Selected.Value;

                                        if (tiporateio.Equals("R"))//ROTA
                                        {                                            
                                            double TotalQuantidadeM3 = VolumeM3TotalSelecionados();
                                            //dou
                                            Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
                                            for (int i = 0; i < gridPes.Rows.Count; i++)
                                            {
                                                if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                                {
                                                    SAPbobsCOM.Documents oOrder;
                                                    oOrder = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                                    if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                                    {
                                                        double dM3Order = VolumeM3TotalOS(gridPes.DataTable.GetValue(1, i).ToString());


                                                        LogHelper.InfoWarning(string.Format("Processando OS {0}", gridPes.DataTable.GetValue(1, i).ToString()));


                                                        double dPeso;// = Convert.ToDouble(oBalancaController.OBalanca.peso);
                                                        if (((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                                        {

                                                            BalancaController oBalancaController = new BalancaController(Form);

                                                            StaticText lblBalanca = (StaticText)Form.Items.Item("lblBalanca").Specific;
                                                            lblBalanca.Item.Visible = true;
                                                            Form.Freeze(false);
                                                            LogHelper.MostraBalanca("", "", Form);

                                                            dPeso = Convert.ToDouble(oBalancaController.OBalanca.peso);
                                                            
                                                        }
                                                        else
                                                        {
                                                            dPeso = Convert.ToDouble(((EditText)Form.Items.Item("edtPeso").Specific).Value);
                                                        }
                                                            

                                                        //qual a percentagem do item no total m3
                                                        double p = (dM3Order * 100) / TotalQuantidadeM3;


                                                        //double dPesoBruto = (dPeso / 100) * p;


                                                        double PesoLiquido = (p/100) * dPeso;
                                                            //- Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value);
                                                        //Se[Peso Líquido] > 0,000000 significa que a ordem de serviço já possui o peso da[Tara] e com o[Peso Bruto]
                                                        //inserido no passo(IV)
                                                        if (PesoLiquido>0)
                                                        {
                                                            DateTime myTime = DateTime.Today.Date;
                                                            myTime = myTime.AddHours(DateTime.Now.Hour);
                                                            myTime = myTime.AddMinutes(DateTime.Now.Minute);


                                                            //a OS pode ser faturada.Nesse cenário atualize os campos:
                                                            //	ORDR.U_Situacao = 10(Coleta não faturada)
                                                            //	ORDR.U_DataEntradaOS = Data atual
                                                            //	ORDR.U_HoraEntradaOS = Horário atualOBalanca.pesoHora
                                                            oOrder.UserFields.Fields.Item("U_Situacao").Value = "10";
                                                            oOrder.UserFields.Fields.Item("U_DataEntradaOS").Value = DateTime.Now;
                                                            oOrder.UserFields.Fields.Item("U_HoraEntradaOS").Value = myTime;

                                                            oOrder.UserFields.Fields.Item("U_PesoLiq").Value = PesoLiquido- Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value);
                                                            oOrder.UserFields.Fields.Item("U_PesoBruto").Value = dPeso;
                                                        }
                                                        //o Se[Peso Líquido] <= [Peso Bruto] significa que a ordem de serviço em questão
                                                        //não possui o peso da[Tara].Nesse cenário execute os passos abaixo:
                                                        if (PesoLiquido<= dPeso)
                                                        {
                                                            //	ORDR.U_Situacao = 2(Aguardando Peso Tara)
                                                            //Essas ordens/ pedidos continuarão na tela de pesagem aguardando a pesagem da tara.
                                                            oOrder.UserFields.Fields.Item("U_Situacao").Value = "2";
                                                        }




                                                        if (oOrder.Update() == 0)
                                                        {
                                                            Controller.MainController.oApplication.StatusBar.SetText(
                                                                string.Format("OS Nº {0} Peso Bruto Atualizado!!", oOrder.DocEntry)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Success
                                                            );
                                                        }
                                                        else
                                                        {
                                                            int temp_int;
                                                            string temp_string;
                                                            Controller.MainController.oCompany.GetLastError(out temp_int, out temp_string);
                                                            Controller.MainController.oApplication.StatusBar.SetText(
                                                                string.Format("Erro ao Alterar OS Nº {0} !!", oOrder.DocEntry)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Error
                                                            );
                                                        }

                                                    }

                                                    Controller.MainController.LimparObjeto(oOrder);
                                                }
                                            }

                                        }
                                        else if (tiporateio.Equals("C"))//Carga Composto
                                        {
                                            Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;

                                            DateTime myTime = DateTime.Today.Date;                                            
                                            myTime = myTime.AddHours(DateTime.Now.Hour);
                                            myTime = myTime.AddMinutes(DateTime.Now.Minute);


                                            double dPesoEstimadoTotal = PesoEstimadoTotalSelecionados();
                                            //PASSO 2 – Encontrar o peso total envolvendo todos os clientes/ ordens.
                                            //•	Após encontrar o peso estimado de todas as ordens / clientes faça o cálculo e encontre o peso estimado total.
                                            //o Peso estimado Total = Peso estimado ClienteA +ClienteB + ClienteC

                                            for (int i = 0; i < gridPes.Rows.Count; i++)
                                            {
                                                if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                                {
                                                    SAPbobsCOM.Documents oOrder;
                                                    oOrder = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                                    if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                                    {
                                                        //double dM3Order = Convert.ToDouble(oOrder.UserFields.Fields.Item("U_VolumeM3").Value);
                                                        LogHelper.InfoWarning(string.Format("Processando OS {0}", gridPes.DataTable.GetValue(1, i).ToString()));
                                                        double dPeso;
                                                        if (((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                                        {
                                                            BalancaController oBalancaController = new BalancaController(Form);

                                                            StaticText lblBalanca = (StaticText)Form.Items.Item("lblBalanca").Specific;
                                                            lblBalanca.Item.Visible = true;
                                                            Form.Freeze(false);
                                                            LogHelper.MostraBalanca("", "", Form);
                                                            dPeso = Convert.ToDouble(oBalancaController.OBalanca.peso);
                                                        }
                                                        else
                                                        {
                                                            dPeso = Convert.ToDouble(((EditText)Form.Items.Item("edtPeso").Specific).Value);
                                                        }
        

                                                        //PASSO 1 – Encontrar o peso previsto da ordem de serviço para o cliente.
                                                        //•	Encontre o volume(RDR1.Quantity) e a densidade(RDR1.U_Densidade) do item resíduo(OTIM.ItmsGrpCod = Resíduo)
                                                        //que compõe a ordem de serviço.
                                                        //•	Com base nessas informações faça o cálculo:
                                                        //o Peso estimado ClienteA = Volume * Densidade.
                                                        double dPesoEstimadoOS = PesoEstimadoOS(gridPes.DataTable.GetValue(1, i).ToString());

                                                        //PASSO 3 – Encontrar o porcentagem do peso para cada cliente.
                                                        //•	Após encontrar o peso estimado do cliente e o peso estimado total calcule qual
                                                        //será a porcentagem correspondendo que deverá ser aplicada na ordem de serviço.
                                                        //o Porcentagem ClienteA = Peso estimado ClienteA / Peso estimado Total
                                                        double dPClienteA = dPesoEstimadoOS / dPesoEstimadoTotal;

                                                        //PASSO 4 – Definir o peso que será alocado na ordem de serviço.
                                                        //•	Após encontrar o peso estimado do cliente e o peso estimado total calcule qual será a porcentagem
                                                        //correspondendo que deverá ser aplicada na ordem de serviço.
                                                        //o Peso Cliente A = Peso Líquido Balança / Porcentagem ClienteA
                                                        
                                                        double dPesoCliente = (dPeso - Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value) )* dPClienteA;

                                                        //double dPeso = Convert.ToDouble(oBalancaController.OBalanca.peso);

                                                        ////qual a percentagem do item no total m3
                                                        //double p = (dM3Order * 100) / TotalQuantidadeM3;
                                                        //double dPesoBruto = (dPeso / 100) * p;
                                                        //double PesoLiquido = dPesoBruto
                                                        //    - Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value);
                                                        ////Se[Peso Líquido] > 0,000000 significa que a ordem de serviço já possui o peso da[Tara] e com o[Peso Bruto]
                                                        ////inserido no passo(IV)
                                                        //if (PesoLiquido > 0)
                                                        //{
                                                        //    //a OS pode ser faturada.Nesse cenário atualize os campos:
                                                        //    //	ORDR.U_PesoLiquido = Peso Cliente A
                                                        //    //	ORDR.U_Situacao = 10(Coleta não faturada)
                                                        //    //	ORDR.U_DataEntradaOS = Data atual
                                                        //    //	ORDR.U_HoraEntradaOS = Horário atualOBalanca.pesoHora

                                                        oOrder.UserFields.Fields.Item("U_PesoBruto").Value = dPeso;
                                                        oOrder.UserFields.Fields.Item("U_PesoLiq").Value = dPesoCliente; ;
                                                        oOrder.UserFields.Fields.Item("U_Situacao").Value = "10";
                                                        oOrder.UserFields.Fields.Item("U_DataEntradaOS").Value = DateTime.Now;
                                                        oOrder.UserFields.Fields.Item("U_HoraEntradaOS").Value = myTime;
                  
                                                        //}




                                                        if (oOrder.Update() == 0)
                                                        {
                                                            Controller.MainController.oApplication.StatusBar.SetText(
                                                                string.Format("OS Nº {0} Peso Liquido Atualizado!!", oOrder.DocEntry)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Success
                                                            );
                                                        }
                                                        else
                                                        {
                                                            int temp_int;
                                                            string temp_string;
                                                            Controller.MainController.oCompany.GetLastError(out temp_int, out temp_string);
                                                            Controller.MainController.oApplication.StatusBar.SetText(
                                                                string.Format("Erro ao Alterar OS Nº {0} !!", oOrder.DocEntry)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Error
                                                            );
                                                        }

                                                    }

                                                    Controller.MainController.LimparObjeto(oOrder);
                                                }
                                            }
                                        }


                                        CarregarPes();
                                        LogHelper.InfoSuccess("Rateio Concluído!!!");

                                    }
                                }
                                if (pVal.ItemUID == "btCapPes")
                                {
                                    //ComboBox cbDCol= (ComboBox)Form.Items.Item("cbDCol").Specific);
                                    if (((ComboBox)Form.Items.Item("cbDCol").Specific).Selected==null)
                                    {
                                        LogHelper.InfoError("Selecione o Tipo de Pessagem");
                                    }
                                    else if (((ComboBox)Form.Items.Item("cbDCol").Specific).Selected.Value.Equals("0"))
                                    {
                                        LogHelper.InfoError("Selecione o Tipo de Pessagem");
                                    }
                                    else
                                    {
                                        //verifica se tem algo selecionado
                                        Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
                                        bool bSelecionado = false;
                                        for (int i = 0; i < gridPes.Rows.Count; i++)
                                        {
                                            if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                            {
                                                bSelecionado = true;
                                                break;
                                            }
                                        }
                                        if (bSelecionado)
                                        {

                                            StaticText lblBalanca = (StaticText)Form.Items.Item("lblBalanca").Specific;
                                            for (int i = 0; i < gridPes.Rows.Count; i++)
                                            {
                                                if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                                {
                                                    LogHelper.InfoWarning(string.Format("Processando OS {0}", gridPes.DataTable.GetValue(1, i).ToString()));

                                                    Balanca OBalanca;
                                                    if (((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                                    {
                                                        BalancaController oBalancaController = new BalancaController(Form);
                                                        lblBalanca.Item.Visible = true;
                                                        Form.Freeze(false);
                                                        LogHelper.MostraBalanca("", "", Form);
                                                        OBalanca = oBalancaController.OBalanca;
                                                    }
                                                    else
                                                    {
                                                        OBalanca = new Balanca();
                                                        OBalanca.peso = ((EditText)Form.Items.Item("edtPeso").Specific).Value;
                                                    }


                                                    SAPbobsCOM.Documents oOrder;
                                                    oOrder = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                                    if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                                    {
                                                        string tipoPesagem = ((ComboBox)Form.Items.Item("cbDCol").Specific).Selected.Value;

                                                        if (tipoPesagem.Equals("T"))//tara
                                                        {
                                                            if (Convert.ToDouble(oOrder.UserFields.Fields.Item("U_PesoBruto").Value) > 0)
                                                            {
                                                                oOrder.UserFields.Fields.Item("U_Tara").Value = OBalanca.peso;
                                                                //já tiverem o[Peso Bruto] preenchido > 0,0000, execute cálculo do peso líquido dessas ordens:
                                                                //ORDR.U_PesoBruto - ORDR.U_Tara => ORDR.U_PesoLiquido.
                                                                oOrder.UserFields.Fields.Item("U_PesoLiq").Value =
                                                                (Convert.ToDouble(oOrder.UserFields.Fields.Item("U_PesoBruto").Value) -
                                                                    Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value)).ToString();

                                                            }
                                                            
                                                        }
                                                        else if (tipoPesagem.Equals("PB"))//Peso Bruto
                                                        {
                                                            oOrder.UserFields.Fields.Item("U_PesoLiq").Value = 
                                                                    Convert.ToDouble(OBalanca.peso) -
                                                                    Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value);


                                                            oOrder.UserFields.Fields.Item("U_PesoBruto").Value = Convert.ToDouble(OBalanca.peso);
                                                        }





                                                        if (oOrder.Update() == 0)
                                                        {
                                                            Controller.MainController.oApplication.StatusBar.SetText(
                                                                string.Format("OS Nº {0} Tara Atualizada!!", oOrder.DocEntry)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Success
                                                            );
                                                        }
                                                        else
                                                        {
                                                            int temp_int;
                                                            string temp_string;
                                                            Controller.MainController.oCompany.GetLastError(out temp_int, out temp_string);
                                                            Controller.MainController.oApplication.StatusBar.SetText(
                                                                string.Format("Erro ao Alterar OS Nº {0} !!", oOrder.DocEntry)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Error
                                                            );
                                                        }

                                                    }

                                                    Controller.MainController.LimparObjeto(oOrder);


                                                    


                                                    //bSelecionado = true;
                                                    //break;
                                                }
                                            }
                                            lblBalanca.Item.Visible = false;
                                            LogHelper.InfoWarning("Pessagem concluída!!!");
                                            CarregarPes();

                                            //string arquivo = Helper.ApiHelper.GetArquivo(ip, porta, caminho);

                                            //if (arquivo == "ARQUIVO NÃO ENCONTRADO")
                                            //{
                                            //    throw new Exception("Falha na comunicaçao com a balança capital: arquivo não encontrado");
                                            //}
                                        }
                                        else
                                        {
                                            LogHelper.InfoError(string.Format("Não existe registro selecionado, selecione!!"));
                                        }
                                    }
                                }
                                if (pVal.ItemUID == "btUptPlaca") {
                                    if (Controller.MainController.oApplication.MessageBox("Confirma Atualização das Placas da OS?", 1, "Sim", "Não") == 1)
                                    {
                                        LogHelper.InfoWarning(string.Format("Atualização das Placas Iniciada...!!"));

                                        Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
                                        //int M3 = 0;
                                        SAPbobsCOM.Recordset recordSetPlaca = null;
                                        try
                                        {
                                            recordSetPlaca = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                            string sSQL = string.Format(@"
		                                                    SELECT 
		                                                        T0.""U_Placa"" ""Placa""
                                                                , T0.""U_UFPlaca"" ""UFPlaca""
                                                                , T0.""U_PNCode"" AS ""MOTORISTA""
                                                                , 1 TARA
                                                            FROM
                                                                ""@VEICULOS"" T0
                                                            WHERE
                                                                T0.""U_Placa"" = '{0}'
                                            ", ((EditText)Form.Items.Item("etPlacaPes").Specific).Value);
                                            recordSetPlaca.DoQuery(sSQL);
                                        }
                                        catch (Exception)
                                        {

                                            throw;
                                        }



                                        for (int i = 0; i < gridPes.Rows.Count; i++)
                                        {
                                            if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                            {
                                                SAPbobsCOM.Documents oOrder;
                                                oOrder = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                                bool bModificou = false;
                                                if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                                {
                                                    //for (int x = 0; x < oOrder.Lines.UserFields.Fields.Count; x++)
                                                    //{
                                                    //    Controller.MainController.Application.StatusBar.SetText(
                                                    //       oOrder.Lines.UserFields.Fields.Item(0).Name
                                                    //        , BoMessageTime.bmt_Short
                                                    //        , BoStatusBarMessageType.smt_Success
                                                    //    );

                                                    //}
                                                    oOrder.UserFields.Fields.Item("U_NPlaca").Value = recordSetPlaca.Fields.Item("Placa").Value.ToString();
                                                    oOrder.UserFields.Fields.Item("U_EstPlaca").Value = recordSetPlaca.Fields.Item("UFPlaca").Value.ToString();
                                                    oOrder.UserFields.Fields.Item("U_CodTransp").Value = recordSetPlaca.Fields.Item("MOTORISTA").Value.ToString();
                                                    oOrder.UserFields.Fields.Item("U_Tara").Value = recordSetPlaca.Fields.Item("TARA").Value.ToString();
                                                    bModificou = true;
                                                    //for (int ii = 0; ii < oOrder.Lines.Count; ii++)
                                                    //{
                                                    //    oOrder.Lines.SetCurrentLine(ii);
                                                    //    if (oOrder.Lines.MeasureUnit.Equals("Metros Cúbicos"))
                                                    //    {
                                                    //        string t = recordSetPlaca.Fields.Item("Placa").Value.ToString();
                                                    //        //Order.Lines.UserFields.Fields.Item("U_NPlaca").Value = recordSetPlaca.Fields.Item("Placa").Value.ToString();
                                                    //        oOrder.Lines.UserFields.Fields.Item("U_Estado").Value = recordSetPlaca.Fields.Item("UFPlaca").Value.ToString();
                                                    //        oOrder.Lines.UserFields.Fields.Item("U_Motorista").Value = recordSetPlaca.Fields.Item("MOTORISTA").Value.ToString();
                                                    //        oOrder.Lines.UserFields.Fields.Item("U_Tara").Value = recordSetPlaca.Fields.Item("TARA").Value.ToString();
                                                    //        bModificou = true;

                                                    //    }
                                                    //}
                                                }
                                                if (bModificou)
                                                {
                                                    if (oOrder.Update() == 0)
                                                    {
                                                        Controller.MainController.oApplication.StatusBar.SetText(
                                                            string.Format("OS Nº {0} Atualizada!!", oOrder.DocEntry)
                                                            , BoMessageTime.bmt_Short
                                                            , BoStatusBarMessageType.smt_Success
                                                        );
                                                    }
                                                    else
                                                    {
                                                        int temp_int;
                                                        string temp_string;
                                                        Controller.MainController.oCompany.GetLastError(out temp_int, out temp_string);
                                                        Controller.MainController.oApplication.StatusBar.SetText(
                                                            string.Format("Erro ao Alterar OS Nº {0} !!", oOrder.DocEntry)
                                                            , BoMessageTime.bmt_Short
                                                            , BoStatusBarMessageType.smt_Error
                                                        );
                                                    }
                                                }
                                                Controller.MainController.LimparObjeto(oOrder);
                                            }


                                            //M3 = M3 + CalculaM3OS(i, gridPes.DataTable.GetValue(1, i).ToString());
                                        }

                                        Controller.MainController.LimparObjeto(recordSetPlaca);
                                        Controller.MainController.oApplication.StatusBar.SetText(
                                            string.Format("Atualização Concluída!!")
                                            , BoMessageTime.bmt_Short
                                            , BoStatusBarMessageType.smt_Success
                                        );
                                        CarregarPes();
                                    }
                                }

                                if (pVal.ItemUID == "ckSelCtr")
                                {
                                    if (Form.DataSources.DataTables.Item("dtContr").Rows.Count > 0)
                                        CarregarContratos();
                                }

                                if (pVal.ItemUID == "ckSelOS")
                                {
                                    if (Form.DataSources.DataTables.Item("dtOS").Rows.Count > 0)
                                        CarregarOS();
                                }
                                if (pVal.ItemUID == "ckSelTPes")
                                {
                                    //if (Form.DataSources.DataTables.Item("dtOS").Rows.Count > 0)
                                        CarregarPes();
                                }
                                if (pVal.ItemUID == "gridPes")
                                {
                                    if (pVal.ColUID=="#")
                                    {
                                        Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
                                        StaticText lblOsTot = (StaticText)Form.Items.Item("lblOsTot").Specific;
                                        StaticText lblM3Tot = (StaticText)Form.Items.Item("lblM3Tot").Specific;
                                        int OsTot = Convert.ToInt32(lblOsTot.Caption);
                                        int M3Tot = Convert.ToInt32(lblM3Tot.Caption);
                                        int M3 = 0;

                                        M3 = CalculaM3OS(pVal.Row, gridPes.DataTable.GetValue(1, pVal.Row).ToString());

                                        if (gridPes.DataTable.GetValue(0, pVal.Row).ToString().Equals("Y"))
                                        {
                                            OsTot++;
                                            M3Tot = M3Tot + M3;
                                        }
                                        else
                                        {
                                            OsTot--;
                                            M3Tot = M3Tot - M3;
                                        }

                                        lblOsTot.Caption = OsTot.ToString();
                                        lblM3Tot.Caption = M3Tot.ToString();
                                    }
                                }
                            }

                            break;
                        case BoEventTypes.et_COMBO_SELECT:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID == "cbPeriodOS")
                                {
                                    string periodicidade = ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Selected.Value;


                                    switch (periodicidade)
                                    {
                                        case "1":
                                            ((EditText)Form.Items.Item("etDtOSI").Specific).String = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");

                                            ((EditText)Form.Items.Item("etDtOSF").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                            break;
                                        case "2":
                                            ((EditText)Form.Items.Item("etDtOSI").Specific).String = DateTime.Now.AddDays(-15).ToString("dd/MM/yyyy");

                                            ((EditText)Form.Items.Item("etDtOSF").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                            break;
                                        case "3":
                                            ((EditText)Form.Items.Item("etDtOSI").Specific).String = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");

                                            ((EditText)Form.Items.Item("etDtOSF").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                            break;
                                        case "4":
                                            ((EditText)Form.Items.Item("etDtOSI").Specific).String = DateTime.Now.AddDays(-60).ToString("dd/MM/yyyy");

                                            ((EditText)Form.Items.Item("etDtOSF").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                            break;
                                        case "5":
                                            ((EditText)Form.Items.Item("etDtOSI").Specific).String = DateTime.Now.AddDays(-90).ToString("dd/MM/yyyy");

                                            ((EditText)Form.Items.Item("etDtOSF").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                            break;
                                        case "6":
                                            ((EditText)Form.Items.Item("etDtOSI").Specific).String = DateTime.Now.AddDays(-120).ToString("dd/MM/yyyy");

                                            ((EditText)Form.Items.Item("etDtOSF").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                            break;
                                        case "7":
                                            ((EditText)Form.Items.Item("etDtOSI").Specific).String = DateTime.Now.AddDays(-180).ToString("dd/MM/yyyy");

                                            ((EditText)Form.Items.Item("etDtOSF").Specific).String = DateTime.Now.ToString("dd/MM/yyyy");
                                            break;
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_VALIDATE:
                            if (!pVal.BeforeAction)
                            {
                                if (pVal.ItemUID.Equals("etCliente"))
                                {
                                    if (((EditText)Form.Items.Item("etCliente").Specific).String == string.Empty)
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeCliente", 0, "");
                                }

                                if (pVal.ItemUID.Equals("etMotora"))
                                {
                                    if (((EditText)Form.Items.Item("etMotora").Specific).String == string.Empty)
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeMotorista", 0, "");
                                }

                                if (pVal.ItemUID == "etHrSaida")
                                {
                                    string valor = ((EditText)Form.Items.Item("etHrSaida").Specific).String;

                                    bool somenteNumeros = valor.Replace(":", "").Any(x => !char.IsLetter(x));

                                    if (!somenteNumeros)
                                        Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = "";
                                    else
                                    {
                                        if (valor.Length > 5)
                                        {
                                            Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = "";
                                        }
                                        else if (valor.Length == 5 && valor.Substring(2, 1) != ":")
                                        {
                                            Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = "";
                                        }
                                        else if (valor.Length == 4)
                                        {
                                            Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = valor.Insert(2, ":");
                                        }
                                        else if (valor.Length == 3)
                                        {
                                            Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = valor.Insert(0, "0").Insert(2, ":");
                                        }
                                        else if (valor.Length == 2)
                                        {
                                            Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = valor.Insert(2, ":00");
                                        }
                                        else if (valor.Length == 1)
                                        {
                                            Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = "0" + valor + ":00";
                                        }
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
                                        if (pVal.ItemUID.Equals("etCliente"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("CodCliente", 0, chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString());
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeCliente", 0, chooseFromListEvent.SelectedObjects.GetValue("CardName", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etNrCtr"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NrContrato", 0, chooseFromListEvent.SelectedObjects.GetValue("Number", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etCentroC"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("CentroCusto", 0, chooseFromListEvent.SelectedObjects.GetValue("PrcCode", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etNrRota"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NrRota", 0, chooseFromListEvent.SelectedObjects.GetValue("Code", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etMotora"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("Motorista", 0, chooseFromListEvent.SelectedObjects.GetValue("Code", 0).ToString());
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeMotorista", 0, chooseFromListEvent.SelectedObjects.GetValue("U_Nome", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etNrPlaca"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NrPlaca", 0, chooseFromListEvent.SelectedObjects.GetValue("U_Placa", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etNrPlOS"))
                                        {
                                            Form.DataSources.UserDataSources.Item("nrPlacaOS").Value = chooseFromListEvent.SelectedObjects.GetValue("U_Placa", 0).ToString();
                                        }
                                        else if (pVal.ItemUID.Equals("etNrOS"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NrOS", 0, chooseFromListEvent.SelectedObjects.GetValue("DocEntry", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etUsuResp"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("UsuResp", 0, chooseFromListEvent.SelectedObjects.GetValue("U_NAME", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etPlacaPes"))
                                        {
                                            try
                                            {
                                                ((EditText)Form.Items.Item("etPlacaPes").Specific).Value = chooseFromListEvent.SelectedObjects.GetValue("U_Placa", 0).ToString();
                                            }
                                            catch (Exception)
                                            {

                                               // throw;
                                            }
                                            
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
                                    Controller.MainController.oApplication.ItemEvent -= HandleItemEvent;
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
                                        Form.Items.Item("fldCtr").Click();

                                        ((ComboBox)Form.Items.Item("cbModCtr").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbModCtr").Specific).ValidValues.Add("RCC", "Construção Civil");
                                        ((ComboBox)Form.Items.Item("cbModCtr").Specific).ValidValues.Add("RGG", "Grande Gerador");
                                        ((ComboBox)Form.Items.Item("cbModCtr").Specific).ValidValues.Add("RPV", "Poda e Varrição");
                                        ((ComboBox)Form.Items.Item("cbModCtr").Specific).ValidValues.Add("RSS", "Serviços de Saúde");
                                        ((ComboBox)Form.Items.Item("cbModCtr").Specific).ValidValues.Add("RSI", "Sólidos Industriais");
                                        ((ComboBox)Form.Items.Item("cbModCtr").Specific).ValidValues.Add("RSU", "Sólido Urbano");

                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("0", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("1", "Segunda-feira");
                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("2", "Terça-feira");
                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("3", "Quarta-feira");
                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("4", "Quinta-feira");
                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("5", "Sexta-feira");
                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("6", "Sábado");
                                        ((ComboBox)Form.Items.Item("cbDiaCol").Specific).ValidValues.Add("7", "Domingo");

                                        ((ComboBox)Form.Items.Item("cbTpFat").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbTpFat").Specific).ValidValues.Add("0", "Cliente");
                                        ((ComboBox)Form.Items.Item("cbTpFat").Specific).ValidValues.Add("1", "Transportadora");
                                        ((ComboBox)Form.Items.Item("cbTpFat").Specific).ValidValues.Add("2", "Locação");

                                        ((ComboBox)Form.Items.Item("cbAgrOS").Specific).ValidValues.Add("0", "Padrão");
                                        ((ComboBox)Form.Items.Item("cbAgrOS").Specific).ValidValues.Add("1", "Cliente");
                                        ((ComboBox)Form.Items.Item("cbAgrOS").Specific).ValidValues.Add("2", "Transportadora");

                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("2", "Aguardando envio App");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("3", "Aguardando realização coleta");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("4", "Coleta Realizada");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("5", "Coleta validada  ");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("6", "OS com restrição  ");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("7", "Coleta estação");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("8", "Pedido do cliente");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("9", "Coleta enviada");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("10", "Coleta não faturada");
                                        ((ComboBox)Form.Items.Item("cbSitOS").Specific).ValidValues.Add("11", "Coleta concluída");

                                        ((ComboBox)Form.Items.Item("cbStatusOS").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbStatusOS").Specific).ValidValues.Add("P", "Pendente");
                                        ((ComboBox)Form.Items.Item("cbStatusOS").Specific).ValidValues.Add("F", "Faturado");

                                        ((ComboBox)Form.Items.Item("cbTpOper").Specific).ValidValues.Add("", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbTpOper").Specific).ValidValues.Add("C-GG", "Coleta - Grandes Geradores");
                                        ((ComboBox)Form.Items.Item("cbTpOper").Specific).ValidValues.Add("C-TRT", "Coleta - Transportadora");
                                        ((ComboBox)Form.Items.Item("cbTpOper").Specific).ValidValues.Add("C-PJG", "Coleta - Pessoa Jurídica");
                                        ((ComboBox)Form.Items.Item("cbTpOper").Specific).ValidValues.Add("C-PF", "Coleta - Pessoa Física");
                                        ((ComboBox)Form.Items.Item("cbTpOper").Specific).ValidValues.Add("LOC", "Locação");

                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("0", "Sem Periodicidade");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("1", "Semanal");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("2", "Quinzenal");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("3", "Mensal");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("4", "Bimestral");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("5", "Trimestral");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("6", "Quadrimestral");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).ValidValues.Add("7", "Semestral");

                                        ((ComboBox)Form.Items.Item("cbRespFat").Specific).ValidValues.Add("0", "[Selecionar]");
                                        ((ComboBox)Form.Items.Item("cbRespFat").Specific).ValidValues.Add("1", "Cliente");
                                        ((ComboBox)Form.Items.Item("cbRespFat").Specific).ValidValues.Add("2", "Transportadora");
                                        ((ComboBox)Form.Items.Item("cbRespFat").Specific).ValidValues.Add("3", "Terceiros");

                                        ChooseFromList choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_4");

                                        Conditions conditions = new Conditions();

                                        Condition condition = conditions.Add();

                                        condition.Alias = "DimCode";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "3";

                                        choose.SetConditions(conditions);

                                        choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_2");

                                        conditions = new Conditions();

                                        condition = conditions.Add();

                                        condition.Alias = "CardType";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "C";

                                        choose.SetConditions(conditions);

                                        choose = (ChooseFromList)this.Form.ChooseFromLists.Item("CFL_9");

                                        conditions = new Conditions();

                                        condition = conditions.Add();

                                        condition.Alias = "DocStatus";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "O";
                                        condition.Relationship = BoConditionRelationship.cr_AND;

                                        condition = conditions.Add();

                                        condition.BracketOpenNum = 2;
                                        condition.Alias = "U_Situacao";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "4";
                                        condition.BracketCloseNum = 1;
                                        condition.Relationship = BoConditionRelationship.cr_OR;

                                        condition = conditions.Add();

                                        condition.BracketOpenNum = 1;
                                        condition.Alias = "U_Situacao";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "10";
                                        condition.BracketCloseNum = 2;
                                        condition.Relationship = BoConditionRelationship.cr_AND;

                                        condition = conditions.Add();

                                        condition.Alias = "U_Status";
                                        condition.Operation = BoConditionOperation.co_EQUAL;
                                        condition.CondVal = "P";

                                        choose.SetConditions(conditions);

                                        ((ComboBox)Form.Items.Item("cbAgrOS").Specific).Select("0");
                                        ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Select("0");
                                        ((ComboBox)Form.Items.Item("cbRespFat").Specific).Select("0");
                                    }
                                    catch { }
                                    finally
                                    {
                                        Loaded = true;
                                    }
                                }
                            }
                            break;
                        case BoEventTypes.et_FORM_RESIZE:
                            if (!pVal.BeforeAction)
                            {
                                //if (Loaded)
                                //    Form.Items.Item("33").Height = 1;
                            }                            
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Controller.MainController.oApplication.StatusBar.SetText(exception.Message);
            }
        }
        private double PesoEstimadoTotalSelecionados()
        {
            string sDocEntrys = DoscEntrys();

            SAPbobsCOM.Recordset recordSetTotal = null;
            try
            {

                string selecionar = ((CheckBox)Form.Items.Item("ckSelTPes").Specific).Checked ? "Y" : "N";
                string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;
                string placa = ((EditText)Form.Items.Item("etNrPlaca").Specific).String;
                string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;
                string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Description;

                string sSQL = string.Format(@"
                        select 
	                        sum(T1.""Quantity"" * coalesce(T1.""U_Densidade"",0))
                        from
                            ORDR T0
	                        inner join RDR1 T1 on T1.""DocEntry""=T0.""DocEntry""    
	                        inner join OITM T2 on T2.""ItemCode""=T1.""ItemCode""
	                        inner join OITB t3 on t3.""ItmsGrpCod""=T2.""ItmsGrpCod""
                        where 
	                        t3.""ItmsGrpNam""='Resíduos'
                            and ( T0.""DocEntry""  in ({0}) )

                "
                , sDocEntrys);

                recordSetTotal = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Controller.MainController.LimparObjeto(recordSetTotal);
                return dRetorno;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private double PesoEstimadoOS(string sDocEntry)
        {

            SAPbobsCOM.Recordset recordSetTotal = null;
            try
            {

                string selecionar = ((CheckBox)Form.Items.Item("ckSelTPes").Specific).Checked ? "Y" : "N";
                string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;
                string placa = ((EditText)Form.Items.Item("etNrPlaca").Specific).String;
                string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;
                string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Description;

                string sSQL = string.Format(@"
                        select 
	                        sum(T1.""Quantity"" * coalesce(T1.""U_Densidade"",0))
                        from
                            ORDR T0
	                        inner join RDR1 T1 on T1.""DocEntry""=T0.""DocEntry""    
	                        inner join OITM T2 on T2.""ItemCode""=T1.""ItemCode""
	                        inner join OITB t3 on t3.""ItmsGrpCod""=T2.""ItmsGrpCod""
                        where 
	                        t3.""ItmsGrpNam""='Resíduos'
                            and ( T0.""DocEntry""  in ({0}) )

                "
                , sDocEntry);

                recordSetTotal = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Controller.MainController.LimparObjeto(recordSetTotal);
                return dRetorno;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private double VolumeM3TotalOS(string docEntry)
        {
            string sDocEntrys = DoscEntrys();

            SAPbobsCOM.Recordset recordSetTotal = null;
            try
            {



                string sSQL = string.Format(@"
                        select 
	                        sum (T1.""Quantity"")
                        from
                            ORDR T0
                            inner
                        join RDR1 T1 on T1.""DocEntry"" = T0.""DocEntry""
                        where
                            T0.""CANCELED"" = 'N'
                            and T1.""unitMsr"" = 'Metros Cúbicos'
                            and ( T0.""DocEntry""  in ({0}) )

                ", docEntry);

                recordSetTotal = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Controller.MainController.LimparObjeto(recordSetTotal);
                return dRetorno;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private double VolumeM3TotalSelecionados()
        {
            string sDocEntrys = DoscEntrys();

            SAPbobsCOM.Recordset recordSetTotal = null;
            try
            {

                string selecionar = ((CheckBox)Form.Items.Item("ckSelTPes").Specific).Checked ? "Y" : "N";
                string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;
                string placa = ((EditText)Form.Items.Item("etNrPlaca").Specific).String;
                string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;
                string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Description;

                string sSQL = string.Format(@"
                        select 
	                        sum (T1.""Quantity"")
                        from
                            ORDR T0
                            inner
                        join RDR1 T1 on T1.""DocEntry"" = T0.""DocEntry""
                        where
                            T0.""CANCELED"" = 'N'
                            and T1.""unitMsr"" = 'Metros Cúbicos'
                            and ( T0.""DocEntry""  in ({5}) )

                ",
                selecionar,
                cliente,
                placa,
                nrRota,
                diaColeta
                , sDocEntrys);

                recordSetTotal = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Controller.MainController.LimparObjeto(recordSetTotal);
                return dRetorno;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string DoscEntrys()
        {
            Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;

            string sDocEntrys = string.Empty;
            for (int i = 0; i < gridPes.Rows.Count; i++)
            {
                if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                {

                    string sDocEntry = gridPes.DataTable.GetValue(1, i).ToString();
                    if (string.IsNullOrEmpty(sDocEntrys))
                    {
                        sDocEntrys = sDocEntry;
                    }
                    else
                    {
                        sDocEntrys = sDocEntrys + "," + sDocEntry;
                    }
                }
            }

            return sDocEntrys;
        }

        private static int CalculaM3OS(int pRow, string DocEntry )
        {
            int M3=0;
            SAPbobsCOM.Recordset recordSetVerificacao = null;
            try
            {
                string SQL = string.Format(@"
                                                    select 
                                                        sum(T1.""Quantity"")
                                                    from
                                                        ORDR T0
                                                        inner
                                                    join RDR1 T1 on T1.""DocEntry"" = T0.""DocEntry""
                                                    where

                                                        T0.""DocEntry"" = {0}

                                                        and T1.""unitMsr"" = 'Metros Cúbicos'
                                            ", DocEntry);


                recordSetVerificacao = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recordSetVerificacao.DoQuery(SQL);
                if (recordSetVerificacao.RecordCount > 0)
                {
                    M3 = Convert.ToInt32(recordSetVerificacao.Fields.Item(0).Value.ToString());
                    recordSetVerificacao.MoveNext();
                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSetVerificacao);
                GC.Collect();
            }

            return M3;
        }

        private void CarregarContratos()
        {
            string selecionar = ((CheckBox)Form.Items.Item("ckSelCtr").Specific).Checked ? "Y" : "N";

            string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;

            string dataDe = ((EditText)Form.Items.Item("etDtCtrI").Specific).String;

            string dataAte = ((EditText)Form.Items.Item("etDtCtrF").Specific).String;

            string nrContrato = ((EditText)Form.Items.Item("etNrCtr").Specific).String;

            string modeloContrato = ((ComboBox)Form.Items.Item("cbModCtr").Specific).Selected.Value;

            string centroCusto = ((EditText)Form.Items.Item("etCentroC").Specific).String;

            string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;

            string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Value;

            string motorista = ((EditText)Form.Items.Item("etMotora").Specific).String;

            //string placa = ((EditText)Form.Items.Item("etNrPlaca").Specific).String;

            string placa = "";

            string query = string.Format(@"SELECT '{0}' AS ""#"",
                                                      OOAT.""AbsID"" AS ""Nº Interno"",
                                                      OAT1.""AgrLineNum"",
                                                      OOAT.""Number"" as ""Nº CTR"",
                                                      OOAT.""BpName"" as ""Cliente"",
                                                      OOAT.""Descript"" as ""Desc. CTR"",
                                                      OAT1.""ItemName"" as ""Serviço"",
                                                      OAT1.""PlanQty"" as ""Coleta Planejada"",
                                                      OAT1.""PlanQty"" - 0 as ""Coleta Pendente"",
                                                      (select max(""DocNum"")
                                                       from ORDR 
                                                       where exists (select * 
                                                                     from RDR1 
                                                                     where RDR1.""AgrNo"" = OOAT.""AbsID"" 
                                                                     and RDR1.""AgrLnNum"" = OAT1.""AgrLineNum""
                                                                     and RDR1.""DocEntry"" = ORDR.""DocEntry"")
                                                       ) as ""Última OS"",
                                                      (select max(""DocDate"")
                                                       from ORDR 
                                                       where exists (select * 
                                                                     from RDR1 
                                                                     where RDR1.""AgrNo"" = OOAT.""AbsID"" 
                                                                     and RDR1.""AgrLnNum"" = OAT1.""AgrLineNum""
                                                                     and RDR1.""DocEntry"" = ORDR.""DocEntry"")
                                                       ) as ""Data OS""
                                            from OOAT
                                            inner join OAT1 on OAT1.""AgrNo"" = OOAT.""AbsID""
                                            left join ""@ROTAS"" on OOAT.""U_Rota"" = ""@ROTAS"".""Code""
                                            left join ""@VEICULOS"" on ""@ROTAS"".""U_Veiculo"" = ""@VEICULOS"".""U_Placa""
                                            where OOAT.""BpType"" = 'C'
                                            and OOAT.""Status"" = 'A'
                                            and ('{1}' = '' or '{1}' = OOAT.""BpCode"")
                                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OOAT.""StartDate"" as date) >= '{2}')
                                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OOAT.""StartDate"" as date) <= '{3}') 
                                            and ('{4}' = '' or '{4}' = cast(OOAT.""Number"" as varchar))
                                            and ('{5}' = '' or '{5}' = OOAT.""U_Modelo"")
                                            and ('{6}' = '' or '{6}' = OOAT.""U_CentroCusto"")
                                            and ('{7}' = '' or '{7}' = OOAT.""U_Rota"")
                                            and ('{8}' = '0' or ('{8}' = '1' AND OOAT.""U_DiaColetSeg"" = 'Sim')
                                                             or ('{8}' = '2' AND OOAT.""U_DiaColetTerc"" = 'Sim') 
                                                             or ('{8}' = '3' AND OOAT.""U_DiaColetQuart"" = 'Sim') 
                                                             or ('{8}' = '4' AND OOAT.""U_DiaColetQuin"" = 'Sim') 
                                                             or ('{8}' = '5' AND OOAT.""U_DiaColetSext"" = 'Sim') 
                                                             or ('{8}' = '6' AND OOAT.""U_DiaColetSab"" = 'Sim') 
                                                             or ('{8}' = '7' AND OOAT.""U_DiaColetDom"" = 'Sim'))
                                            and ('{9}' = '' or '{9}' = OOAT.""U_Motorista"")
                                            and ('{10}' = '' or '{10}' = ""@VEICULOS"".""U_Placa"")
                                            ", selecionar, cliente, dataDe == "" ? "1990-01-01" : Convert.ToDateTime(dataDe).ToString("yyyy-MM-dd"), dataAte == "" ? "1990-01-01" : Convert.ToDateTime(dataAte).ToString("yyyy-MM-dd"), nrContrato, modeloContrato, centroCusto, nrRota, diaColeta, motorista, placa);

            Form.Freeze(true);
            try
            {
                Form.DataSources.DataTables.Item("dtContr").ExecuteQuery(query);

                Grid gridContratos = (Grid)Form.Items.Item("gridContr").Specific;

                gridContratos.Columns.Item("AgrLineNum").Visible = false;

                gridContratos.Columns.Item("Nº CTR").Editable = false;

                gridContratos.Columns.Item("Nº Interno").Editable = false;
                gridContratos.Columns.Item("Nº CTR").Editable = false;
                gridContratos.Columns.Item("Cliente").Editable = false;
                gridContratos.Columns.Item("Desc. CTR").Editable = false;
                gridContratos.Columns.Item("Serviço").Editable = false;
                gridContratos.Columns.Item("Coleta Planejada").Editable = false;
                gridContratos.Columns.Item("Coleta Pendente").Editable = false;
                gridContratos.Columns.Item("Última OS").Editable = false;
                gridContratos.Columns.Item("Data OS").Editable = false;
                
                gridContratos.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;

                ((EditTextColumn)gridContratos.Columns.Item("Nº Interno")).LinkedObjectType = "1250000025";                
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

        private void CarregarPes()
        {

            Controller.MainController.oApplication.StatusBar.SetText(
                "Consulta Pessagem Iniciada..."
                , BoMessageTime.bmt_Short
                , BoStatusBarMessageType.smt_Warning
            );

            string selecionar = ((CheckBox)Form.Items.Item("ckSelTPes").Specific).Checked ? "Y" : "N";
            string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;
            string placa = ((EditText)Form.Items.Item("etNrPlaca").Specific).String;
            string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;
            string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Description;

            string query = string.Format(@"
                    select 
	                    '{0}' as ""#""
                        , T0.""DocEntry"" as ""Nº Interno""
                        , T0.""DocNum"" AS ""Nº OS""
                        , T0.""CardCode""
                        , T0.""CardName"" ""Cliente""
                        , T0.""U_NPlaca"" ""Placa""
                        , T0.""U_VolumeM3"" ""m3""
                        , T0.""U_Tara"" ""Tara""
                        , T0.""U_PesoBruto"" ""Peso Bruto""
                        , T0.""U_PesoLiq"" ""Peso Liq.""
                    from
                        ORDR T0
                        left
                    join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = T0.""U_NPlaca""
                        left join OCRD TRANSP ON TRANSP.""CardCode"" = T0.""U_CodTransp""
                        left join OUSR ON T0.""UserSign"" = OUSR.""USERID""
                    where
                        T0.""CANCELED"" = 'N'
                        and T0.""DocStatus"" = 'O'
                        and T0.""U_Status"" = 'P'
                        and T0.""U_Situacao"" = 3
                        and('{1}' = '' or '{1}' = T0.""CardCode"")
                        and('{2}' = '' or '{2}' = T0.""U_NPlaca"")
                        and('{3}' = '' or '{3}' = (select max(OOAT.""U_Rota"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = T0.""DocEntry""))
                        and('{4}' = '[Selecionar]' or '{4}' = T0.""U_DiaSemRot"")

            ",
                                            selecionar,
                                            cliente,
                                            placa,
                                            nrRota,
                                            diaColeta);

            //and ORDR.""U_Situacao"" IN(4, 10)

            //and('{11}' = '0' or('{11}' = '1' AND(select max(OOAT.""U_DiaColetSeg"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '2' AND(select max(OOAT.""U_DiaColetTerc"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '3' AND(select max(OOAT.""U_DiaColetQuart"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '4' AND(select max(OOAT.""U_DiaColetQuin"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '5' AND(select max(OOAT.""U_DiaColetSext"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '6' AND(select max(OOAT.""U_DiaColetSab"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '7' AND(select max(OOAT.""U_DiaColetDom"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y'))

            //switch (agrupamento)
            //{
            //    case "0":
            //        query += @" order by T0.""DocNum"" DESC";
            //        break;
            //    case "1":
            //        query += @" order by T0.""CardName"" DESC";
            //        break;
            //    case "2":
            //        query += @" order by T0.""CardName"" DESC";
            //        break;
            //}

            Form.Freeze(true);
            try
            {
                Form.DataSources.DataTables.Item("dtPes").ExecuteQuery(query);

                if (((CheckBox)Form.Items.Item("ckSelTPes").Specific).Checked)
                {
                    ((StaticText)Form.Items.Item("lblOsTot").Specific).Caption = Form.DataSources.DataTables.Item("dtPes").Rows.Count.ToString();
                    Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
                    int M3 = 0;
                    for (int i = 0; i < gridPes.Rows.Count; i++)
                    {
                        M3 =M3 +  CalculaM3OS(i, gridPes.DataTable.GetValue(1, i).ToString());
                    }
                    ((StaticText)Form.Items.Item("lblM3Tot").Specific).Caption = M3.ToString();
                }
                else
                {
                    ((StaticText)Form.Items.Item("lblOsTot").Specific).Caption = "0";
                    ((StaticText)Form.Items.Item("lblM3Tot").Specific).Caption = "0";
                }
                

                ConfiguraGridPes();

                Controller.MainController.oApplication.StatusBar.SetText(
                    "Consulta Pessagem Finalizada!!!"
                    , BoMessageTime.bmt_Short
                    , BoStatusBarMessageType.smt_Success
                );
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

        private void ConfiguraGridPes()
        {
            Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
            gridPes.Columns.Item(1).Editable = false;
            gridPes.Columns.Item(2).Editable = false;
            gridPes.Columns.Item(3).Editable = false;
            gridPes.Columns.Item(4).Editable = false;

            //gridOS.Columns.Item("Nº Interno").Editable = false;
            //gridOS.Columns.Item("Nº OS").Editable = false;
            //gridOS.Columns.Item("Dt Saída").Editable = false;
            //gridOS.Columns.Item("Resp. Faturamento").Editable = false;
            //gridOS.Columns.Item("Cód. Cliente").Editable = false;
            //gridOS.Columns.Item("Nome Cliente").Editable = false;
            //gridOS.Columns.Item("Motorista").Editable = false;
            //gridOS.Columns.Item("Veículo").Editable = false;
            //gridOS.Columns.Item("Situação").Editable = false;
            //gridOS.Columns.Item("Status").Editable = false;

            gridPes.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;

            ((EditTextColumn)gridPes.Columns.Item("Nº Interno")).LinkedObjectType = "17";
            ((EditTextColumn)gridPes.Columns.Item(3)).LinkedObjectType = "2";
        }

        private void CarregarOS()
        {
            string selecionar = ((CheckBox)Form.Items.Item("ckSelOS").Specific).Checked ? "Y" : "N";

            string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;

            string dataDe = ((EditText)Form.Items.Item("etDtOSI").Specific).String;

            string dataAte = ((EditText)Form.Items.Item("etDtOSF").Specific).String;                                   

            string nrOS = ((EditText)Form.Items.Item("etNrOS").Specific).String;

            string situacaoOS = ((ComboBox)Form.Items.Item("cbSitOS").Specific).Selected.Value;

            string statusOS = ((ComboBox)Form.Items.Item("cbStatusOS").Specific).Selected.Value;

            string placa = ((EditText)Form.Items.Item("etNrPlaca").Specific).String;

            string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;

            string usuResp = ((EditText)Form.Items.Item("etUsuResp").Specific).String;

            string nrContrato = ((EditText)Form.Items.Item("etNrCtr").Specific).String;

            string modeloContrato = ((ComboBox)Form.Items.Item("cbModCtr").Specific).Selected.Value;   
                                    
            string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Description;

            string motorista = ((EditText)Form.Items.Item("etMotora").Specific).String;

            string tipoOperacao = ((ComboBox)Form.Items.Item("cbTpOper").Specific).Selected.Value;
                        
            string respFat = ((ComboBox)Form.Items.Item("cbRespFat").Specific).Selected.Description;

            string agrupamento = ((ComboBox)Form.Items.Item("cbAgrOS").Specific).Selected.Value;            

            string query = string.Format(@"SELECT '{0}' AS ""#"",
                                                      ORDR.""DocEntry"" AS ""Nº Interno"",
                                                      ORDR.""DocNum"" AS ""Nº OS"",
                                                      ORDR.""DocDate"" AS ""Dt Saída"",
                                                      ORDR.""U_RespFat"" AS ""Resp. Faturamento"",
                                                      ORDR.""CardCode"" AS ""Cód. Cliente"",
                                                      ORDR.""CardName"" AS ""Nome Cliente"",                                                      
                                                      ORDR.""U_Motorista"" AS ""Motorista"",
                                                      ORDR.""U_NPlaca"" as ""Veículo"",
                                                      case ORDR.""U_Situacao"" 
                                                           when 2 then 'Aguardando envio App'
                                                           when 3 then 'Aguardando realização coleta'
                                                           when 4 then 'Coleta realizada'
                                                           when 5 then 'Coleta validada'
                                                           when 6 then 'OS com restrição'
                                                           when 7 then 'Coleta estação'
                                                           when 8 then 'Pedido do cliente'
                                                           when 9 then 'Coleta enviada'
                                                           when 10 then 'Coleta não faturada'
                                                           when 11 then 'Coleta concluída'
                                                      end as ""Situação"",
                                                      case ORDR.""U_Status""
                                                           when 'P' then 'Pendente'
                                                           when 'F' then 'Faturado'
                                                      end as ""Status""
                                            from ORDR
                                            left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = ORDR.""U_NPlaca""
                                            left join OCRD TRANSP ON TRANSP.""CardCode"" = ORDR.""U_CodTransp""
                                            left join OUSR ON ORDR.""UserSign"" = OUSR.""USERID""
                                            where ORDR.""CANCELED"" = 'N'
                                            and ORDR.""DocStatus"" = 'O'
                                            and ORDR.""U_Status"" = 'P'                                            
                                            and ('{1}' = '' or '{1}' = ORDR.""CardCode"")
                                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(ORDR.""DocDate"" as date) >= '{2}')
                                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(ORDR.""DocDate"" as date) <= '{3}') 
                                            and ('{4}' = '' or '{4}' = ORDR.""DocEntry"")
                                            and ('{5}' = '' or '{5}' = ORDR.""U_Situacao"")
                                            and ('{6}' = '' or '{6}' = ORDR.""U_NPlaca"")
                                            and ('{7}' = '' or '{7}' = (select max(OOAT.""U_Rota"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{8}' = '' or '{8}' = OUSR.""U_NAME"")
                                            and ('{9}' = '' or '{9}' = (select max(OOAT.""Number"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{10}' = '' or '{10}' = (select max(OOAT.""U_Modelo"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{11}' = '[Selecionar]' or '{11}' = ORDR.""U_DiaSemRot"")
                                            and ('{12}' = '' or '{12}' = (select max(OOAT.""U_Motorista"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{13}' = '' or '{13}' = ORDR.""U_TipoOper"")
                                            and ('{14}' = '[Selecionar]' or '{14}' = ORDR.""U_RespFat"")",                                            
                                            selecionar, 
                                            cliente,
                                            dataDe == "" ? "1990-01-01" : Convert.ToDateTime(dataDe).ToString("yyyy-MM-dd"), 
                                            dataAte == "" ? "1990-01-01" : Convert.ToDateTime(dataAte).ToString("yyyy-MM-dd"),
                                            nrOS,
                                            situacaoOS,
                                            placa,
                                            nrRota,
                                            usuResp,
                                            nrContrato,
                                            modeloContrato,
                                            diaColeta,
                                            motorista,
                                            tipoOperacao,
                                            respFat);

            //and ORDR.""U_Situacao"" IN(4, 10)

            //and('{11}' = '0' or('{11}' = '1' AND(select max(OOAT.""U_DiaColetSeg"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '2' AND(select max(OOAT.""U_DiaColetTerc"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '3' AND(select max(OOAT.""U_DiaColetQuart"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '4' AND(select max(OOAT.""U_DiaColetQuin"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '5' AND(select max(OOAT.""U_DiaColetSext"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '6' AND(select max(OOAT.""U_DiaColetSab"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y')
            //                                                 or('{11}' = '7' AND(select max(OOAT.""U_DiaColetDom"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry"") = 'Y'))

            switch (agrupamento)
            {
                case "0":
                    query += @" order by ORDR.""DocNum"" DESC"; 
                    break;
                case "1":
                    query += @" order by ORDR.""CardName"" DESC";
                    break;
                case "2":
                    query += @" order by TRANSP.""CardName"" DESC";
                    break;
            }

            Form.Freeze(true);
            try
            {
                Form.DataSources.DataTables.Item("dtOS").ExecuteQuery(query);

                Grid gridOS = (Grid)Form.Items.Item("gridOS").Specific;

                gridOS.Columns.Item("Nº Interno").Editable = false;
                gridOS.Columns.Item("Nº OS").Editable = false;
                gridOS.Columns.Item("Dt Saída").Editable = false;
                gridOS.Columns.Item("Resp. Faturamento").Editable = false;
                gridOS.Columns.Item("Cód. Cliente").Editable = false;
                gridOS.Columns.Item("Nome Cliente").Editable = false;
                gridOS.Columns.Item("Motorista").Editable = false;
                gridOS.Columns.Item("Veículo").Editable = false;
                gridOS.Columns.Item("Situação").Editable = false;
                gridOS.Columns.Item("Status").Editable = false;

                gridOS.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;

                ((EditTextColumn)gridOS.Columns.Item("Nº Interno")).LinkedObjectType = "17";
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

        private void GerarOS()
        {
            string placaOS = Form.DataSources.UserDataSources.Item("nrPlacaOS").Value;
            DateTime dataSaidaOS = Form.DataSources.UserDataSources.Item("dtSaidaOS").Value == "" ? DateTime.MinValue : Convert.ToDateTime(Form.DataSources.UserDataSources.Item("dtSaidaOS").Value);
            string horaSaidaOS = Form.DataSources.UserDataSources.Item("hrSaidaOS").Value;
            string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected == null ? "[Selecionar]" : ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Description;

            if (dataSaidaOS == DateTime.MinValue)
            {
                Controller.MainController.oApplication.StatusBar.SetText("Informe a data de saída da OS", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }


            if (horaSaidaOS == "")
            {
                Controller.MainController.oApplication.StatusBar.SetText("Informe a hora de saída da OS", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            if (placaOS == "")
            {
                Controller.MainController.oApplication.StatusBar.SetText("Informe a placa", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }


            if (diaColeta == "[Selecionar]")
            {
                Controller.MainController.oApplication.StatusBar.SetText("Informe o dia da coleta", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            Controller.MainController.oApplication.StatusBar.SetText("Gerando ordens de serviço", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);

            if (!Controller.MainController.oCompany.InTransaction)
                Controller.MainController.oCompany.StartTransaction();
            try
            {
                
                Grid gridContratos = (Grid)Form.Items.Item("gridContr").Specific;

                bool osGeradas = false;

                for (int row = 0; row < gridContratos.Rows.Count; row++)
                {
                    Dictionary<string, string> notasVencidas = new Dictionary<string, string>();

                    SAPbobsCOM.Documents documents = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                    try
                    {
                        if (((CheckBoxColumn)gridContratos.Columns.Item("#")).IsChecked(row))
                        {
                            int absID = Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("Nº Interno")).GetText(row));
                            int agrLineNum = Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("AgrLineNum")).GetText(row));

                            string queryVerificacao = string.Format(@"select ""DocNum"", ""CardName"" from OINV 
                                                      where ""CardCode"" = (select ""BpCode"" from OOAT where OOAT.""AbsID"" = {0})
                                                      and ""DocStatus"" = 'O'
                                                      and exists (select * from INV6 
                                                                  where INV6.""DocEntry"" = OINV.""DocEntry"" 
                                                                  and DAYS_BETWEEN(INV6.""DueDate"", current_date) > 30)", absID);

                            SAPbobsCOM.Recordset recordSetVerificacao = null;
                            try
                            {
                                recordSetVerificacao = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                recordSetVerificacao.DoQuery(queryVerificacao);
                                while (!recordSetVerificacao.EoF)
                                {
                                    notasVencidas.Add(recordSetVerificacao.Fields.Item(0).Value.ToString(), recordSetVerificacao.Fields.Item(1).Value.ToString());

                                    recordSetVerificacao.MoveNext();
                                }
                            }
                            finally
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSetVerificacao);
                                GC.Collect();
                            }        
                            
                            if (notasVencidas.Count > 0)
                            {
                                Controller.MainController.oApplication.MessageBox(string.Format("Notas {0} do cliente {1} em aberto. Não é possível gerar OS.",
                                    string.Join(",", notasVencidas.Select(r => r.Key).ToArray()), notasVencidas.Select(r => r.Value).First()));

                                continue;
                            }

                            string query = string.Format(@"select OOAT.""BpCode"",                                                                  
                                                                  OOAT.""U_Motorista"",
                                                                  OAT1.""ItemCode"",
                                                                  OAT1.""U_Capacidade"",
                                                                  OAT1.""UnitPrice"",
                                                                  0,
                                                                  COALESCE(""@VEICULOS_DADOS"".""U_Tara"", 0),                                                                  
                                                                  OOAT.""U_Rota"",
                                                                  COALESCE(""@VEICULOS"".""U_UFPlaca"", ''),
                                                                  (SELECT T0.""ID"" FROM OUSG T0 WHERE T0.""Usage"" = OOAT.""U_Utilizacao"")
                                                            from OOAT
                                                            inner join OAT1 on OAT1.""AgrNo"" = OOAT.""AbsID""                                                            
                                                            left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = '{2}'
                                                            left join ""@VEICULOS_DADOS"" ON ""@VEICULOS_DADOS"".""Code"" = ""@VEICULOS"".""Code""
                                                            where OOAT.""AbsID"" = {0}
                                                            and OAT1.""AgrLineNum"" = {1}
                                                            ", absID, agrLineNum, placaOS);

                            SAPbobsCOM.Recordset recordSet = null;
                            try
                            {
                                recordSet = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                recordSet.DoQuery(query);
                                if (!recordSet.EoF)
                                {
                                    DateTime docDate = DateTime.Now;
                                    DateTime docDueDate = dataSaidaOS;
                                    DateTime taxDate = DateTime.Now;

                                    string cardCode = recordSet.Fields.Item(0).Value.ToString();                                    
                                    string motorista = recordSet.Fields.Item(1).Value.ToString();
                                    string itemCode = recordSet.Fields.Item(2).Value.ToString();
                                    double quantity = Convert.ToDouble(recordSet.Fields.Item(3).Value);
                                    double unitPrice = Convert.ToDouble(recordSet.Fields.Item(4).Value);
                                    double pesoBruto = Convert.ToDouble(recordSet.Fields.Item(5).Value);
                                    double tara = Convert.ToDouble(recordSet.Fields.Item(6).Value);
                                    string rota = recordSet.Fields.Item(7).Value.ToString();
                                    string estPlaca = recordSet.Fields.Item(8).Value.ToString();
                                    string usage = recordSet.Fields.Item(9).Value.ToString();

                                    int bplID = 3;
                                    string tpOper = "C-GG";
                                    string respFat = "Cliente";
                                    string codTransp = "CLI0001";                                                                        
                                    string status = "P";
                                    string situacao = "3";                                    
                                    string warehouse = "01";

                                    documents.CardCode = cardCode;
                                    documents.DocDate = docDate;
                                    documents.DocDueDate = docDueDate;
                                    documents.TaxDate = taxDate;
                                    documents.BPL_IDAssignedToInvoice = bplID;

                                    documents.UserFields.Fields.Item("U_EstPlaca").Value = estPlaca;                                    
                                    documents.UserFields.Fields.Item("U_TipoOper").Value = tpOper;
                                    documents.UserFields.Fields.Item("U_RespFat").Value = respFat;
                                    documents.UserFields.Fields.Item("U_CodTransp").Value = codTransp;
                                    documents.UserFields.Fields.Item("U_NPlaca").Value = placaOS;
                                    documents.UserFields.Fields.Item("U_HoraSaidaOS").Value = horaSaidaOS;
                                    documents.UserFields.Fields.Item("U_Motorista").Value = motorista;
                                    documents.UserFields.Fields.Item("U_PesoBruto").Value = pesoBruto;
                                    documents.UserFields.Fields.Item("U_Tara").Value = tara;
                                    documents.UserFields.Fields.Item("U_Status").Value = status;
                                    documents.UserFields.Fields.Item("U_Situacao").Value = situacao;
                                    documents.UserFields.Fields.Item("U_RotaOS").Value = rota;
                                    documents.UserFields.Fields.Item("U_DiaSemRot").Value = diaColeta;

                                    documents.Lines.ItemCode = itemCode;
                                    documents.Lines.Usage = usage;
                                    documents.Lines.Quantity = quantity;
                                    documents.Lines.WarehouseCode = warehouse;
                                    documents.Lines.UnitPrice = unitPrice;                                    
                                    documents.Lines.AgreementNo = absID;
                                    documents.Lines.AgreementRowNumber = agrLineNum;
                                    documents.Lines.UserFields.Fields.Item("U_UtilTax").Value = usage;

                                    int result = documents.Add();

                                    if (result != 0)
                                    {
                                        int codigoErro;
                                        string msgErro;

                                        Controller.MainController.oCompany.GetLastError(out codigoErro, out msgErro);

                                        throw new Exception(msgErro);
                                    }

                                    osGeradas = true;
                                }
                            }
                            finally
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                                GC.Collect();
                            }
                        }
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(documents);
                        GC.Collect();
                    }
                }

                Controller.MainController.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                if (osGeradas)
                {
                    Controller.MainController.oApplication.MessageBox("Ordens de serviço geradas", 1, "OK");

                    Form.DataSources.UserDataSources.Item("nrPlacaOS").Value = string.Empty;
                    Form.DataSources.UserDataSources.Item("dtSaidaOS").Value = string.Empty;
                    Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = string.Empty;

                    ((Grid)Form.Items.Item("gridContr").Specific).DataTable.Clear();

                    string queryFiltro = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"" from dummy";

                    Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(queryFiltro);
                }
            }
            catch (Exception ex)
            {
                if (Controller.MainController.oCompany.InTransaction)
                    Controller.MainController.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                Controller.MainController.oApplication.MessageBox("Erro ao gerar ordens de serviço: " + ex.Message, 1, "OK");
            }
        }

        private void GerarFatura()
        {
            string tipoFaturamento = Form.DataSources.UserDataSources.Item("tpFatOS").Value;

            bool faturamentoAgrupado = Form.DataSources.UserDataSources.Item("fatAgrp").Value == "Y";

            if (tipoFaturamento == "")
            {
                Controller.MainController.oApplication.StatusBar.SetText("Selecionar o tipo de faturamento", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            Controller.MainController.oApplication.StatusBar.SetText("Gerando faturas", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning);

            if (!Controller.MainController.oCompany.InTransaction)
                Controller.MainController.oCompany.StartTransaction();
            try
            {
                List<Model.FaturaModel> faturaList = new List<Model.FaturaModel>();

                Grid gridOS = (Grid)Form.Items.Item("gridOS").Specific;

                for (int row = 0; row < gridOS.Rows.Count; row++)
                {
                    if (((CheckBoxColumn)gridOS.Columns.Item("#")).IsChecked(row))
                    {                        
                        int docEntry = Convert.ToInt32(((EditTextColumn)gridOS.Columns.Item("Nº Interno")).GetText(row));                        

                        string query = string.Format(@"select case '{1}' when '0' then ORDR.""CardCode"" when '1' then case when ORDR.""U_TipoOper"" = 'C-TRT' then ORDR.""U_CodTransp"" else ORDR.""CardCode"" end else ORDR.""CardCode"" end as ""CardCode"",
                                                              ORDR.""U_TipoOper"",
                                                              RDR1.""ItemCode"",
                                                              RDR1.""Quantity"",
                                                              RDR1.""Price"",
                                                              OITM.""ItmsGrpCod"",
                                                              OITM.""ManBtchNum"",
                                                              RDR1.""LineNum""
                                                            from ORDR
                                                            inner join RDR1 on RDR1.""DocEntry"" = ORDR.""DocEntry""
                                                            inner join OITM on OITM.""ItemCode"" = RDR1.""ItemCode""                                                            
                                                            where ORDR.""DocEntry"" = {0}
                                                            and (RDR1.""LineNum"" = 0 OR {1} = '2')
                                                            ", docEntry, tipoFaturamento);

                        SAPbobsCOM.Recordset recordSet = null;
                        try
                        {
                            recordSet = (SAPbobsCOM.Recordset)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            recordSet.DoQuery(query);
                            while (!recordSet.EoF)
                            {
                                Model.FaturaModel faturaModel = new Model.FaturaModel();

                                faturaModel.BaseEntry = docEntry;
                                faturaModel.CardCode = recordSet.Fields.Item(0).Value.ToString();                                
                                faturaModel.TpOper = recordSet.Fields.Item(1).Value.ToString();
                                faturaModel.ItemCode = recordSet.Fields.Item(2).Value.ToString();
                                faturaModel.Quantity = Convert.ToDouble(recordSet.Fields.Item(3).Value);
                                faturaModel.Price = Convert.ToDouble(recordSet.Fields.Item(4).Value);
                                faturaModel.ItmsGrpCod = recordSet.Fields.Item(5).Value.ToString();
                                faturaModel.ManBtchNum = recordSet.Fields.Item(6).Value.ToString() == "Y";
                                faturaModel.LineNum = Convert.ToInt32(recordSet.Fields.Item(7).Value);

                                faturaList.Add(faturaModel);

                                recordSet.MoveNext();
                            }
                        }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                            GC.Collect();
                        }
                    }
                }

                var faturaGroupList = faturaList.GroupBy(r => new { Valor1 = r.BaseEntry.ToString(), Valor2 = r.TpOper }).ToList();

                if (faturamentoAgrupado)
                {
                    faturaGroupList = faturaList.GroupBy(r => new { Valor1 = r.CardCode.ToString(), Valor2 = r.TpOper }).ToList();
                }

                List<int> notasTransporteCliente = new List<int>();
                Dictionary<int, int> notasTransporteTransportadora = new Dictionary<int, int>();

                List<int> notasGeradas = new List<int>();

                foreach (var faturaGroup in faturaGroupList)
                {
                    SAPbobsCOM.Documents documentNFSE = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                    SAPbobsCOM.Documents documentFAT = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                    try
                    {
                        int erro = 0;

                        switch (faturaGroup.Key.Valor2)
                        {
                            case "C-GG":
                                documentNFSE.CardCode = faturaGroup.First().CardCode;
                                documentNFSE.DocDate = DateTime.Now;
                                documentNFSE.TaxDate = DateTime.Now;
                                documentNFSE.DocDueDate = DateTime.Now;
                                documentNFSE.BPL_IDAssignedToInvoice = 3;

                                foreach (Model.FaturaModel faturaModel in faturaGroup)
                                {
                                    if (documentNFSE.Lines.ItemCode != string.Empty)
                                    {
                                        documentNFSE.Lines.Add();
                                        documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                                    }

                                    documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                                    documentNFSE.Lines.Quantity = faturaModel.Quantity;
                                    documentNFSE.Lines.Price = faturaModel.Price;
                                    //documentNFSE.Lines.Usage = "14";
                                    //documentNFSE.Lines.TaxCode = "V_DIF100";
                                    documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                                    documentNFSE.Lines.BaseType = 17;
                                    documentNFSE.Lines.BaseLine = 0;
                                }

                                documentNFSE.SequenceCode = 32;

                                erro = documentNFSE.Add();

                                if (erro != 0)
                                {
                                    string msg = "";

                                    Controller.MainController.oCompany.GetLastError(out erro, out msg);

                                    throw new Exception(erro + " - " + msg);
                                }

                                notasGeradas.Add(Convert.ToInt32(Controller.MainController.oCompany.GetNewObjectKey()));

                                break;
                            case "C-TRT":

                                documentNFSE.CardCode = faturaGroup.First().CardCode;
                                documentNFSE.DocDate = DateTime.Now;
                                documentNFSE.TaxDate = DateTime.Now;
                                documentNFSE.DocDueDate = DateTime.Now.AddDays(1);
                                documentNFSE.BPL_IDAssignedToInvoice = 3;

                                documentNFSE.SequenceCode = 32;

                                if (tipoFaturamento == "0")
                                {
                                    foreach (Model.FaturaModel faturaModel in faturaGroup)
                                    {
                                        if (documentNFSE.Lines.ItemCode != string.Empty)
                                        {
                                            documentNFSE.Lines.Add();
                                            documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                                        }

                                        documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                                        documentNFSE.Lines.Quantity = faturaModel.Quantity;
                                        documentNFSE.Lines.Price = faturaModel.Price;
                                        documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                                        documentNFSE.Lines.BaseType = 17;
                                        documentNFSE.Lines.BaseLine = 0;

                                        notasTransporteCliente.Add(faturaModel.BaseEntry);
                                    }                                    
                                }
                                else
                                {
                                    foreach (Model.FaturaModel faturaModel in faturaGroup)
                                    {
                                        if (documentNFSE.Lines.ItemCode != string.Empty)
                                        {
                                            documentNFSE.Lines.Add();
                                            documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                                        }

                                        documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                                        documentNFSE.Lines.Quantity = faturaModel.Quantity;
                                        documentNFSE.Lines.Price = faturaModel.Price;                                                                                
                                    }

                                    documentNFSE.Comments = string.Join(",", faturaGroup.ToList().Select(r => r.BaseEntry).ToArray());
                                }

                                erro = documentNFSE.Add();

                                if (erro != 0)
                                {
                                    string msg = "";

                                    Controller.MainController.oCompany.GetLastError(out erro, out msg);

                                    throw new Exception(erro + " - " + msg);
                                }

                                if (tipoFaturamento == "1")
                                {
                                    int nota = Convert.ToInt32(Controller.MainController.oCompany.GetNewObjectKey());
                                    
                                    foreach (Model.FaturaModel faturaModel in faturaGroup)
                                    {
                                        notasTransporteTransportadora.Add(faturaModel.BaseEntry, nota);
                                    }
                                }

                                notasGeradas.Add(Convert.ToInt32(Controller.MainController.oCompany.GetNewObjectKey()));

                            break;
                            case "LOC":
                                documentNFSE.CardCode = faturaGroup.First().CardCode;
                                documentNFSE.DocDate = DateTime.Now;
                                documentNFSE.TaxDate = DateTime.Now;
                                documentNFSE.DocDueDate = DateTime.Now.AddDays(1);
                                documentNFSE.BPL_IDAssignedToInvoice = 3;

                                bool gerarNFSE = false;
                                                                

                                foreach (Model.FaturaModel faturaModel in faturaGroup)
                                {
                                    if (faturaModel.ItmsGrpCod == "120")
                                    {
                                        gerarNFSE = true;

                                        if (documentNFSE.Lines.ItemCode != string.Empty)
                                        {
                                            documentNFSE.Lines.Add();
                                            documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                                        }

                                        documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                                        documentNFSE.Lines.Quantity = faturaModel.Quantity;
                                        documentNFSE.Lines.Price = faturaModel.Price;
                                        documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                                        documentNFSE.Lines.BaseType = 17;
                                        documentNFSE.Lines.BaseLine = faturaModel.LineNum;

                                        if (faturaModel.ManBtchNum)
                                        {
                                            SAPbobsCOM.Documents oDocumentsRef = (SAPbobsCOM.Documents)Controller.MainController.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                            try
                                            {
                                                for (int x = 0; x < oDocumentsRef.Lines.BatchNumbers.Count; x++)
                                                {
                                                    if (x > 0)
                                                        documentNFSE.Lines.BatchNumbers.Add();

                                                    oDocumentsRef.Lines.BatchNumbers.SetCurrentLine(x);

                                                    documentNFSE.Lines.BatchNumbers.BatchNumber = oDocumentsRef.Lines.BatchNumbers.BatchNumber;
                                                    documentNFSE.Lines.BatchNumbers.Quantity = oDocumentsRef.Lines.BatchNumbers.Quantity;
                                                }
                                            }
                                            finally
                                            {
                                                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentsRef);
                                                GC.Collect();
                                            }
                                        }
                                    }
                                }

                                documentNFSE.SequenceCode = 32;

                                if (gerarNFSE)
                                {
                                    erro = documentNFSE.Add();

                                    if (erro != 0)
                                    {
                                        string msg = "";

                                        Controller.MainController.oCompany.GetLastError(out erro, out msg);

                                        throw new Exception(erro + " - " + msg);
                                    }

                                    notasGeradas.Add(Convert.ToInt32(Controller.MainController.oCompany.GetNewObjectKey()));
                                }

                                bool gerarFAT = false;

                                documentFAT.CardCode = faturaGroup.First().CardCode;
                                documentFAT.DocDate = DateTime.Now;
                                documentFAT.TaxDate = DateTime.Now;
                                documentFAT.DocDueDate = DateTime.Now.AddDays(1);
                                documentFAT.BPL_IDAssignedToInvoice = 3;

                                foreach (Model.FaturaModel faturaModel in faturaGroup)
                                {
                                    if (faturaModel.ItmsGrpCod == "102")
                                    {
                                        gerarFAT = true;

                                        if (documentFAT.Lines.ItemCode != string.Empty)
                                        {
                                            documentFAT.Lines.Add();
                                            documentFAT.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                                        }

                                        documentFAT.Lines.ItemCode = faturaModel.ItemCode;
                                        documentFAT.Lines.Quantity = faturaModel.Quantity;
                                        documentFAT.Lines.Price = faturaModel.Price;
                                        documentFAT.Lines.BaseEntry = faturaModel.BaseEntry;
                                        documentFAT.Lines.BaseType = 17;
                                        documentFAT.Lines.BaseLine = faturaModel.LineNum;
                                    }
                                }

                                documentFAT.SequenceCode = 31;

                                if (gerarFAT)
                                {
                                    erro = documentFAT.Add();

                                    if (erro != 0)
                                    {
                                        string msg = "";

                                        Controller.MainController.oCompany.GetLastError(out erro, out msg);

                                        throw new Exception(erro + " - " + msg);
                                    }

                                    notasGeradas.Add(Convert.ToInt32(Controller.MainController.oCompany.GetNewObjectKey()));
                                }


                                if (!gerarNFSE && !gerarFAT)
                                    throw new Exception("Nenhum item na OS para gerar fatura de locação.");
                                break;
                        }
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(documentNFSE);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(documentFAT);
                        GC.Collect();
                    }
                }

                Controller.MainController.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                if (notasTransporteCliente.Count() > 0)
                {
                    string update = string.Format(@"UPDATE ORDR 
                                                    SET ""U_Status"" = 'F', 
                                                    ""U_Situacao"" = 11 
                                                    where ""U_DocEntry"" IN ({0})", string.Join(",", notasTransporteCliente.ToArray()));

                }

                if (notasTransporteTransportadora.Count() > 0)
                {
                    string update = string.Format(@"UPDATE ORDR 
                                                    SET ""U_Status"" = 'F', 
                                                    ""U_Situacao"" = 11 
                                                    where ""U_DocEntry"" IN ({0})", string.Join(",", notasTransporteTransportadora.Select(r => r.Key).ToArray()));

                    foreach (KeyValuePair<int, int> nota in notasTransporteTransportadora)
                    {
                        update = string.Format(@"UPDATE RDR1 
                                             SET ""U_NumFat"" = {0}
                                             where ""U_DocEntry"" = {1}", nota.Key, nota.Value);
                    }
                }

                Controller.MainController.oApplication.StatusBar.SetText("Faturas geradas", BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Success);

                Controller.MainController.OpenNotasGeradasView(notasGeradas);

                ((Grid)Form.Items.Item("gridOS").Specific).DataTable.Clear();

                string queryFiltro = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"" from dummy";

                Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(queryFiltro);
            }
            catch (Exception ex)
            {                
                if (Controller.MainController.oCompany.InTransaction)
                    Controller.MainController.oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                Controller.MainController.oApplication.StatusBar.SetText("Erro ao gerar faturas: " + ex.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
