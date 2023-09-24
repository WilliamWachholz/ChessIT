using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.IT.Services.Controller;

using Chess.IT.Services.Helper;
using Chess.IT.Services.Model;
using SAPbouiCOM;


namespace Chess.IT.Services.View
{
    class GeraOSView
    {
        Form Form;

        bool Loaded;

        List<int> m_LinhasRateioPeso = new List<int>();
        List<int> m_absIDs = new List<int>();

        public GeraOSView(Form form)
        {
            this.Form = form;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);


            //Form.DataSources.UserDataSources.Add("DtOSI", SAPbouiCOM.BoDataType.dt_DATE, 0);
            //Form.DataSources.UserDataSources.Add("DtOSF", SAPbouiCOM.BoDataType.dt_DATE, 0);

            //((EditText)Form.Items.Item("etDtOSI").Specific).DataBind.SetBound(true, "", "DtOSI");
            //((EditText)Form.Items.Item("etDtOSF").Specific).DataBind.SetBound(true, "", "DtOSF");

            Program.oApplicationS.ItemEvent += HandleItemEvent;
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
                                            //    Form.Items.Item("2").Left = 6;
                                            //    Form.Items.Item("2").Top = 13;
                                            //    Form.Items.Item("etCliente").Top = 13;
                                            //    Form.Items.Item("etClienteN").Top = 13;
                                            //    Form.Items.Item("etClienteN").Visible = true;
                                            //    Form.Items.Item("etCliente").Visible = true;
                                            //    Form.Items.Item("2").Visible = true;
                                            //    ((EditText)Form.Items.Item("etCliente").Specific).TabOrder = 1;

                                            //    //data OS
                                            //    Form.Items.Item("59").Left = 6;
                                            //    Form.Items.Item("59").Top = 31;
                                            //    Form.Items.Item("58").Top = 31;
                                            //    Form.Items.Item("etDtOSI").Top = 31;
                                            //    Form.Items.Item("etDtOSF").Top = 31;

                                            //    Form.Items.Item("59").Visible = true;
                                            //    Form.Items.Item("58").Visible = true;
                                            //    Form.Items.Item("etDtOSI").Visible = true;
                                            //    Form.Items.Item("etDtOSF").Visible = true;
                                            //    ((EditText)Form.Items.Item("etDtOSI").Specific).TabOrder = 2;
                                            //    ((EditText)Form.Items.Item("etDtOSF").Specific).TabOrder = 3;

                                            //    //placa
                                            //    Form.Items.Item("26").Left = 6;
                                            //    Form.Items.Item("26").Top = 50;
                                            //    Form.Items.Item("26").Visible = true;
                                            //    Form.Items.Item("etNrPlaca").Left = Form.Items.Item("etCliente").Left;
                                            //    Form.Items.Item("etNrPlaca").Top = 50;
                                            //    Form.Items.Item("etNrPlaca").Visible = true;
                                            //    ((EditText)Form.Items.Item("etNrPlaca").Specific).TabOrder = 4;

                                            //    //rota
                                            //    Form.Items.Item("18").Left = 6;
                                            //    Form.Items.Item("18").Top = 68;
                                            //    Form.Items.Item("etNrRota").Left = Form.Items.Item("etCliente").Left;
                                            //    Form.Items.Item("etNrRota").Top = 68;
                                            //    Form.Items.Item("18").Visible = true;
                                            //    Form.Items.Item("etNrRota").Visible = true;
                                            //    ((EditText)Form.Items.Item("etNrRota").Specific).TabOrder = 5;

                                            //    // Número de Série
                                            //    Form.Items.Item("Item_4").Left = 6;
                                            //    Form.Items.Item("Item_4").Top = 86;
                                            //    Form.Items.Item("Item_5").Left = Form.Items.Item("etCliente").Left;
                                            //    Form.Items.Item("Item_5").Top = 86;
                                            //    Form.Items.Item("Item_4").Visible = true;
                                            //    Form.Items.Item("Item_5").Visible = true;
                                            //    ((EditText)Form.Items.Item("Item_5").Specific).TabOrder = 6;

                                            //    Form.Items.Item("btPesqPes").Visible = true;

                                            //    Form.Items.Item("6658").Visible = false;
                                            //    Form.Items.Item("cbUtlRes").Visible = false;

                                            //    ((ComboBox)Form.Items.Item("cbDCol").Specific).TabOrder = 7;
                                            //    ((EditText)Form.Items.Item("edtPeso").Specific).TabOrder = 8;
                                            //    ((ComboBox)Form.Items.Item("cbTpRateio").Specific).TabOrder = 9;

                                            //    string query = @"
                                            //            select 
                                            //             'N' as ""#""
                                            //                , T0.""DocEntry"" as ""Nº Interno""
                                            //                , T0.""DocNum"" AS ""Nº OS""
                                            //                , T0.""CardCode""
                                            //                , T0.""CardName"" ""Cliente""
                                            //                , T0.""U_NPlaca"" ""Placa""
                                            //                ,(
                                            //                 select 
                                            //                     sum(T11.""Quantity"")
                                            //                    from
                                            //                        ORDR T00
                                            //                     inner join RDR1 T11 on T11.""DocEntry""=T00.""DocEntry""    
                                            //                     inner join OITM T22 on T22.""ItemCode""=T11.""ItemCode""
                                            //                     inner join OITB t33 on t33.""ItmsGrpCod""=T22.""ItmsGrpCod""
                                            //                    where 
                                            //                     t33.""ItmsGrpCod"" IN (102, 118)
                                            //                        and ( T00.""DocEntry""  in (T0.""DocEntry"") )

                                            //                ) ""m3""
                                            //                , T0.""U_Tara"" ""Tara""
                                            //                , T0.""U_PesoBruto"" ""Peso Bruto""
                                            //                , T0.""U_PesoLiq"" ""Peso Liq.""
                                            //            from
                                            //                ORDR T0
                                            //                left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = T0.""U_NPlaca""
                                            //                left join OCRD TRANSP ON TRANSP.""CardCode"" = T0.""U_CodTransp""
                                            //                left join OUSR ON T0.""UserSign"" = OUSR.""USERID""
                                            //            where
                                            //                T0.""CANCELED"" = 'N'
                                            //                and T0.""DocStatus"" = 'O'
                                            //                and T0.""U_Status"" = 'P'
                                            //                and T0.""U_Situacao"" = 33
                                            //    ";

                                            //    Form.DataSources.DataTables.Item("dtPes").ExecuteQuery(query);

                                            //    ((CheckBox)Form.Items.Item("ckSelTPes").Specific).ValOff = "N";
                                            //    ((CheckBox)Form.Items.Item("ckSelTPes").Specific).ValOn = "Y";

                                            //    ConfiguraGridPes();
                                            //    ((ComboBox)Form.Items.Item("cbAgrOS").Specific).Select("0");
                                            //    ((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Select("0");
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
                                            ////cliente
                                            //Form.Items.Item("2").Left = 6;
                                            //Form.Items.Item("2").Top = 13;
                                            //Form.Items.Item("etCliente").Top = 13;
                                            //Form.Items.Item("etClienteN").Top = 13;

                                            //Form.Items.Item("8").Left = 6;
                                            //Form.Items.Item("etNrCtr").Left = Form.Items.Item("etCliente").Left;

                                            //Form.Items.Item("11").Left = 6;
                                            //Form.Items.Item("11").Top = 70;
                                            //Form.Items.Item("cbModCtr").Left = Form.Items.Item("etCliente").Left;
                                            //Form.Items.Item("cbModCtr").Top = 70;

                                            //Form.Items.Item("26").Visible = false;
                                            //Form.Items.Item("etNrPlaca").Visible = false;
                                            //Form.Items.Item("btPesqPes").Visible = false;

                                            ////rota
                                            //Form.Items.Item("18").Left = 6;
                                            //Form.Items.Item("18").Top = 88;
                                            //Form.Items.Item("etNrRota").Left = Form.Items.Item("etCliente").Left;
                                            //Form.Items.Item("etNrRota").Top = 88;

                                            //Form.Items.Item("20").Left = 6;
                                            //Form.Items.Item("20").Top = 106;
                                            //Form.Items.Item("cbDiaCol").Left = Form.Items.Item("etCliente").Left;
                                            //Form.Items.Item("cbDiaCol").Top = 106;

                                            ////motorista
                                            //Form.Items.Item("22").Top = Form.Items.Item("etDtCtrI").Top;
                                            //Form.Items.Item("22").Left = Form.Items.Item("6658").Left;
                                            //Form.Items.Item("etMotora").Top = Form.Items.Item("etDtCtrI").Top;
                                            //Form.Items.Item("etMotora").Left = Form.Items.Item("cbUtlRes").Left;
                                            //Form.Items.Item("etMotoraN").Top = Form.Items.Item("etNrCtr").Top;
                                            //Form.Items.Item("etMotoraN").Left = Form.Items.Item("6658").Left;

                                            //Form.Items.Item("6658").Top = Form.Items.Item("etCliente").Top;
                                            //Form.Items.Item("6658").Visible = true;
                                            //Form.Items.Item("cbUtlRes").Top = Form.Items.Item("etCliente").Top;
                                            //Form.Items.Item("cbUtlRes").Visible = true;

                                            ////placa
                                            //Form.Items.Item("31").Left = Form.Items.Item("6658").Left;
                                            //Form.Items.Item("31").Top = Form.Items.Item("cbModCtr").Top;
                                            //Form.Items.Item("etDtSaida").Left = Form.Items.Item("cbUtlRes").Left;
                                            //Form.Items.Item("etDtSaida").Top = Form.Items.Item("cbModCtr").Top;

                                            //Form.Items.Item("32").Left = Form.Items.Item("6658").Left;
                                            //Form.Items.Item("32").Top = Form.Items.Item("etNrRota").Top;
                                            //Form.Items.Item("etHrSaida").Left = Form.Items.Item("cbUtlRes").Left;
                                            //Form.Items.Item("etHrSaida").Top = Form.Items.Item("etNrRota").Top;

                                            //Form.Items.Item("34").Left = Form.Items.Item("6658").Left;
                                            //Form.Items.Item("34").Top = Form.Items.Item("cbDiaCol").Top;
                                            //Form.Items.Item("etNrPlOS").Left = Form.Items.Item("cbUtlRes").Left;
                                            //Form.Items.Item("etNrPlOS").Top = Form.Items.Item("cbDiaCol").Top;



                                            //string query = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", 0 as ""UtlRes"", 0 as ""UtlLoc"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"", cast('' as varchar(254)) as ""CodTransp"" from dummy";

                                            //Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(query);

                                            //((ComboBox)Form.Items.Item("cbAgrOS").Specific).Select("0");
                                            //((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Select("0");
                                            //((ComboBox)Form.Items.Item("cbRespFat").Specific).Select("0");
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
                                            ////cliente
                                            //Form.Items.Item("2").Left = 6;
                                            //Form.Items.Item("2").Top = 13;
                                            //Form.Items.Item("etCliente").Top = 13;
                                            //Form.Items.Item("etClienteN").Top = 13;

                                            ////data OS
                                            //Form.Items.Item("59").Left = 6;
                                            //Form.Items.Item("59").Top = 31;
                                            //Form.Items.Item("58").Top = 31;
                                            //Form.Items.Item("etDtOSI").Top = 31;
                                            //Form.Items.Item("etDtOSF").Top = 31;
                                            //Form.Items.Item("59").Visible = true;
                                            //Form.Items.Item("58").Visible = true;
                                            //Form.Items.Item("etDtOSI").Visible = true;
                                            //Form.Items.Item("etDtOSF").Visible = true;

                                            //Form.Items.Item("49").Left = 6;
                                            //Form.Items.Item("49").Top = 49;
                                            //Form.Items.Item("etNrOS").Top = 49;

                                            //Form.Items.Item("51").Left = 6;
                                            //Form.Items.Item("51").Top = 67;
                                            //Form.Items.Item("cbTpOper").Top = 67;

                                            //Form.Items.Item("53").Left = 6;
                                            //Form.Items.Item("53").Top = 85;
                                            //Form.Items.Item("cbSitOS").Top = 85;

                                            //Form.Items.Item("45").Left = 6;
                                            //Form.Items.Item("45").Top = 103;
                                            //Form.Items.Item("cbAgrOS").Left = Form.Items.Item("etCliente").Left;
                                            //Form.Items.Item("cbAgrOS").Top = 103;

                                            //Form.Items.Item("62").Left = 6;
                                            //Form.Items.Item("62").Top = 121;
                                            //Form.Items.Item("cbPeriodOS").Left = Form.Items.Item("etCliente").Left;
                                            //Form.Items.Item("cbPeriodOS").Top = 121;



                                            //Form.Items.Item("6658").Top = Form.Items.Item("etCliente").Top;
                                            //Form.Items.Item("6658").Visible = false;
                                            //Form.Items.Item("cbUtlRes").Top = Form.Items.Item("etCliente").Top;
                                            //Form.Items.Item("cbUtlRes").Visible = false;


                                            //Form.Items.Item("57").Top = Form.Items.Item("etCliente").Top;
                                            //Form.Items.Item("57").Left = Form.Items.Item("6658").Left;
                                            //Form.Items.Item("etUsuResp").Top = Form.Items.Item("etCliente").Top;
                                            //Form.Items.Item("etUsuResp").Left = Form.Items.Item("cbUtlRes").Left;


                                            //Form.Items.Item("8").Left = Form.Items.Item("47").Left;
                                            //Form.Items.Item("etNrCtr").Left = Form.Items.Item("cbRespFat").Left;

                                            ////motorista
                                            //Form.Items.Item("22").Top = Form.Items.Item("etDtOSI").Top;
                                            //Form.Items.Item("22").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("etMotora").Top = Form.Items.Item("etDtOSI").Top;
                                            //Form.Items.Item("etMotora").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("etMotoraN").Top = Form.Items.Item("etNrOS").Top;
                                            //Form.Items.Item("etMotoraN").Left = Form.Items.Item("57").Left;



                                            ////placa
                                            //Form.Items.Item("31").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("31").Top = Form.Items.Item("cbModCtr").Top;
                                            //Form.Items.Item("etDtSaida").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("etDtSaida").Top = Form.Items.Item("cbModCtr").Top;

                                            //Form.Items.Item("32").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("32").Top = Form.Items.Item("etNrRota").Top;
                                            //Form.Items.Item("etHrSaida").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("etHrSaida").Top = Form.Items.Item("etNrRota").Top;

                                            //Form.Items.Item("26").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("26").Top = Form.Items.Item("cbTpOper").Top;
                                            //Form.Items.Item("etNrPlaca").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("etNrPlaca").Top = Form.Items.Item("cbTpOper").Top;

                                            //Form.Items.Item("11").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("11").Top = Form.Items.Item("cbSitOS").Top;
                                            //Form.Items.Item("cbModCtr").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("cbModCtr").Top = Form.Items.Item("cbSitOS").Top;

                                            //Form.Items.Item("Item_9").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("Item_9").Top = Form.Items.Item("cbAgrOS").Top;
                                            //Form.Items.Item("edTransp").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("edTransp").Top = Form.Items.Item("cbAgrOS").Top;

                                            //Form.Items.Item("Item_9").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("Item_9").Top = Form.Items.Item("cbAgrOS").Top;
                                            //Form.Items.Item("edTransp").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("edTransp").Top = Form.Items.Item("cbAgrOS").Top;

                                            //Form.Items.Item("18").Left = Form.Items.Item("57").Left;
                                            //Form.Items.Item("18").Top = Form.Items.Item("cbPeriodOS").Top;
                                            //Form.Items.Item("etNrRota").Left = Form.Items.Item("etUsuResp").Left;
                                            //Form.Items.Item("etNrRota").Top = Form.Items.Item("cbPeriodOS").Top;

                                            //Form.Items.Item("26").Visible = true;
                                            //Form.Items.Item("etNrPlaca").Visible = true;
                                            //Form.Items.Item("btPesqPes").Visible = false;

                                            //Form.Items.Item("6658").Visible = false;
                                            //Form.Items.Item("cbUtlRes").Visible = false;

                                            //Form.Items.Item("20").Top = 10;
                                            //Form.Items.Item("20").Left = 670;
                                            //Form.Items.Item("cbDiaCol").Top = 10;
                                            //Form.Items.Item("cbDiaCol").Left = 750;

                                            //string query = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", 0 as ""UtlRes"", 0 as ""UtlLoc"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"", cast('' as varchar(254)) as ""CodTransp"" from dummy";

                                            //Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(query);

                                            //((ComboBox)Form.Items.Item("cbAgrOS").Specific).Select("0");
                                            //((ComboBox)Form.Items.Item("cbPeriodOS").Specific).Select("0");
                                            //((ComboBox)Form.Items.Item("cbRespFat").Specific).Select("0");
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
                                        // MostrarOSGeradas("50,1,2", new List<ErroGerOS>() { new ErroGerOS() { absID=1,Erro="asdasd"}, new ErroGerOS() { absID = 22, Erro = "asdasd" } });
                                        GerarOS();
                                    }
                                    else if (pVal.ItemUID == "btPesqOS")
                                    {
                                        Form.ActiveItem = "etDtOSF";
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
                                        if ((pVal.ColUID != "#") && (pVal.ColUID != "Esboço"))
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

                                        return;
                                    }

                                    if (((ComboBox)Form.Items.Item("cbTpRateio").Specific).Selected.Value.Equals("0"))
                                    {
                                        LogHelper.InfoError("Selecione o Tipo de Rateio");

                                        return;
                                    }

                                    string tiporateio = ((ComboBox)Form.Items.Item("cbTpRateio").Specific).Selected.Value;

                                    if (tiporateio.Equals("R"))//ROTA
                                    {

                                        if (string.IsNullOrEmpty(((EditText)Form.Items.Item("edtPeso").Specific).Value))
                                        {
                                            LogHelper.InfoError("Informe um peso válido!!");

                                            return;
                                        }
                                        if (Convert.ToDouble(((EditText)Form.Items.Item("edtPeso").Specific).Value) <= 0)
                                        {
                                            LogHelper.InfoError("Informe um peso válido!!");

                                            return;
                                        }

                                        double TotalQuantidadeM3 = VolumeM3TotalSelecionados();
                                        //dou
                                        Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;

                                        double dPeso = 0;// = Convert.ToDouble(oBalancaController.OBalanca.peso);
                                                         //if (((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                                         //{

                                        //    BalancaController oBalancaController = new BalancaController(Form);

                                        //    StaticText lblBalanca = (StaticText)Form.Items.Item("lblBalanca").Specific;
                                        //    lblBalanca.Item.Visible = true;
                                        //    Form.Freeze(false);
                                        //    LogHelper.MostraBalanca("", "", Form);

                                        //    dPeso = Convert.ToDouble(oBalancaController.OBalanca.peso);

                                        //}
                                        //else
                                        //{
                                        dPeso = Convert.ToDouble(((EditText)Form.Items.Item("edtPeso").Specific).Value);
                                        //}
                                        //for (int i = 0; i < gridPes.Rows.Count; i++)
                                        foreach (int linhaRateio in m_LinhasRateioPeso)
                                        {
                                            int i = linhaRateio;

                                            //if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                            //{
                                            SAPbobsCOM.Documents oOrder;
                                            oOrder = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                            if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                            {
                                                double dM3Order = VolumeM3TotalOS(gridPes.DataTable.GetValue(1, i).ToString());


                                                LogHelper.InfoWarning(string.Format("Processando OS {0}", gridPes.DataTable.GetValue(1, i).ToString()));

                                                //qual a percentagem do item no total m3
                                                double p = (dM3Order * 100) / TotalQuantidadeM3;

                                                //double dPesoBruto = (dPeso / 100) * p;

                                                double tara = Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value);
                                                double PesoLiquido = ((p / 100) * (dPeso - Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value)));
                                                //- Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value);
                                                //Se[Peso Líquido] > 0,000000 significa que a ordem de serviço já possui o peso da[Tara] e com o[Peso Bruto]
                                                //inserido no passo(IV)
                                                if (PesoLiquido > 0)
                                                {
                                                    DateTime myTime = DateTime.Today.Date;
                                                    myTime = myTime.AddHours(DateTime.Now.Hour);
                                                    myTime = myTime.AddMinutes(DateTime.Now.Minute);


                                                    //a OS pode ser faturada.Nesse cenário atualize os campos:
                                                    //	ORDR.U_Situacao = 10(Coleta não faturada)
                                                    //	ORDR.U_DataEntradaOS = Data atual
                                                    //	ORDR.U_HoraEntradaOS = Horário atualOBalanca.pesoHora
                                                    oOrder.UserFields.Fields.Item("U_Situacao").Value = "11";
                                                    oOrder.UserFields.Fields.Item("U_DataEntradaOS").Value = DateTime.Now;
                                                    oOrder.UserFields.Fields.Item("U_HoraEntradaOS").Value = myTime.ToString("hh:mm");

                                                    oOrder.UserFields.Fields.Item("U_PesoLiq").Value = PesoLiquido;
                                                    oOrder.UserFields.Fields.Item("U_PesoBruto").Value = dPeso;
                                                }
                                                //o Se[Peso Líquido] <= [Peso Bruto] significa que a ordem de serviço em questão
                                                //não possui o peso da[Tara].Nesse cenário execute os passos abaixo:
                                                if (PesoLiquido <= dPeso)
                                                {
                                                    //	ORDR.U_Situacao = 2(Aguardando Peso Tara)
                                                    //Essas ordens/ pedidos continuarão na tela de pesagem aguardando a pesagem da tara.
                                                    //oOrder.UserFields.Fields.Item("U_Situacao").Value = "3";
                                                }




                                                if (oOrder.Update() == 0)
                                                {
                                                    Program.oApplicationS.StatusBar.SetText(
                                                        string.Format("OS Nº {0} Peso Bruto Atualizado!!", oOrder.DocEntry)
                                                        , BoMessageTime.bmt_Short
                                                        , BoStatusBarMessageType.smt_Success
                                                    );
                                                }
                                                else
                                                {
                                                    int temp_int;
                                                    string temp_string;
                                                    Program.oCompanyS.GetLastError(out temp_int, out temp_string);
                                                    Program.oApplicationS.StatusBar.SetText(
                                                        string.Format("Erro ao Alterar OS Nº {0} : {1}!!", oOrder.DocEntry, temp_string)
                                                        , BoMessageTime.bmt_Short
                                                        , BoStatusBarMessageType.smt_Error
                                                    );
                                                }

                                            }

                                            Program.LimparObjeto(oOrder);
                                            //}
                                        }

                                    }
                                    else if (tiporateio.Equals("C"))//Carga Composto
                                    {
                                        Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;

                                        DateTime myTime = DateTime.Today.Date;
                                        myTime = myTime.AddHours(DateTime.Now.Hour);
                                        myTime = myTime.AddMinutes(DateTime.Now.Minute);


                                        double dPesoEstimadoTotal = PesoEstimadoTotalSelecionados();

                                        double dPeso;

                                        //dPeso = Convert.ToDouble(((EditText)Form.Items.Item("edtPeso").Specific).Value);

                                        //dPeso = PesoLiquidoTotalSelecionados();

                                        //PASSO 2 – Encontrar o peso total envolvendo todos os clientes/ ordens.
                                        //•	Após encontrar o peso estimado de todas as ordens / clientes faça o cálculo e encontre o peso estimado total.
                                        //o Peso estimado Total = Peso estimado ClienteA +ClienteB + ClienteC

                                        foreach (int linhaRateio in m_LinhasRateioPeso)
                                        {
                                            int i = linhaRateio;

                                            //if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                            //{
                                            SAPbobsCOM.Documents oOrder;
                                            oOrder = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                            if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                            {
                                                //double dM3Order = Convert.ToDouble(oOrder.UserFields.Fields.Item("U_VolumeM3").Value);
                                                LogHelper.InfoWarning(string.Format("Processando OS {0}", gridPes.DataTable.GetValue(1, i).ToString()));

                                                //if (((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                                //{
                                                //    BalancaController oBalancaController = new BalancaController(Form);

                                                //    StaticText lblBalanca = (StaticText)Form.Items.Item("lblBalanca").Specific;
                                                //    lblBalanca.Item.Visible = true;
                                                //    Form.Freeze(false);
                                                //    LogHelper.MostraBalanca("", "", Form);
                                                //    dPeso = Convert.ToDouble(oBalancaController.OBalanca.peso);
                                                //}
                                                //else
                                                //{

                                                //}


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

                                                dPeso = Convert.ToDouble(oOrder.UserFields.Fields.Item("U_PesoLiq").Value);

                                                //double dPesoCliente = (dPeso - Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value)) * dPClienteA;

                                                double dPesoCliente = dPeso * dPClienteA;

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

                                                //oOrder.UserFields.Fields.Item("U_PesoBruto").Value = dPeso;
                                                oOrder.UserFields.Fields.Item("U_PesoLiq").Value = dPesoCliente; 
                                                oOrder.UserFields.Fields.Item("U_Situacao").Value = "11";
                                                oOrder.UserFields.Fields.Item("U_DataEntradaOS").Value = DateTime.Now;
                                                oOrder.UserFields.Fields.Item("U_HoraEntradaOS").Value = myTime.ToString("hh:mm");

                                                //}




                                                if (oOrder.Update() == 0)
                                                {
                                                    Program.oApplicationS.StatusBar.SetText(
                                                        string.Format("OS Nº {0} Peso Liquido Atualizado!!", oOrder.DocEntry)
                                                        , BoMessageTime.bmt_Short
                                                        , BoStatusBarMessageType.smt_Success
                                                    );
                                                }
                                                else
                                                {
                                                    int temp_int;
                                                    string temp_string;
                                                    Program.oCompanyS.GetLastError(out temp_int, out temp_string);
                                                    Program.oApplicationS.StatusBar.SetText(
                                                        string.Format("Erro ao Alterar OS Nº {0} : {1}!!", oOrder.DocEntry, temp_string)
                                                        , BoMessageTime.bmt_Short
                                                        , BoStatusBarMessageType.smt_Error
                                                    );
                                                }

                                            }

                                            Program.LimparObjeto(oOrder);
                                            //}
                                        }
                                    }


                                    CarregarPes();
                                    LogHelper.InfoSuccess("Rateio Concluído!!!");


                                }
                                if (pVal.ItemUID == "btCapPes")
                                {
                                    //ComboBox cbDCol= (ComboBox)Form.Items.Item("cbDCol").Specific);
                                    if (((ComboBox)Form.Items.Item("cbDCol").Specific).Selected == null)
                                    {
                                        LogHelper.InfoError("Selecione o Tipo de Pessagem");
                                    }
                                    else if (((ComboBox)Form.Items.Item("cbDCol").Specific).Selected.Value.Equals("0"))
                                    {
                                        LogHelper.InfoError("Selecione o Tipo de Pessagem");
                                    }
                                    else if
                                        (
                                        (!((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                        && string.IsNullOrEmpty(((EditText)Form.Items.Item("edtPeso").Specific).Value)
                                        )
                                    {
                                        LogHelper.InfoError("Informe o peso manualmente!!");
                                    }
                                    else
                                    {
                                        //verifica se tem algo selecionado
                                        Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
                                        //bool bSelecionado = false;                                                                              

                                        bool bSelecionado = m_LinhasRateioPeso.Count > 0;

                                        if (bSelecionado)
                                        {

                                            StaticText lblBalanca = (StaticText)Form.Items.Item("lblBalanca").Specific;


                                            foreach (int linhaRateio in m_LinhasRateioPeso)
                                            {
                                                int i = linhaRateio;

                                                string peso = "";

                                                //for (int i = 0; i < gridPes.Rows.Count; i++)
                                                //{
                                                if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                                                {
                                                    LogHelper.InfoWarning(string.Format("Processando Contrato {0}", gridPes.DataTable.GetValue(1, i).ToString()));


                                                    if (((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                                    {
                                                        BalancaController oBalancaController = new BalancaController(Form);
                                                        Balanca OBalanca;
                                                        lblBalanca.Item.Visible = true;
                                                        Form.Freeze(false);
                                                        LogHelper.MostraBalanca("", "", Form);
                                                        OBalanca = oBalancaController.OBalanca;
                                                        ((EditText)Form.Items.Item("edtPeso").Specific).Item.Enabled = true;
                                                        ((EditText)Form.Items.Item("edtPeso").Specific).Value = OBalanca.peso;
                                                        ((EditText)Form.Items.Item("etPlacaPes").Specific).Value = ((EditText)Form.Items.Item("etPlacaPes").Specific).Value;
                                                        ((EditText)Form.Items.Item("edtPeso").Specific).Item.Enabled = false;

                                                        peso = OBalanca.peso;
                                                    }
                                                    else
                                                    {
                                                        peso = ((EditText)Form.Items.Item("edtPeso").Specific).Value;
                                                    }


                                                    SAPbobsCOM.Documents oOrder;
                                                    oOrder = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                                    if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                                    {
                                                        string tipoPesagem = ((ComboBox)Form.Items.Item("cbDCol").Specific).Selected.Value;

                                                        if (tipoPesagem.Equals("T"))//tara
                                                        {
                                                            //if (Convert.ToDouble(oOrder.UserFields.Fields.Item("U_PesoBruto").Value) > 0)
                                                            //{
                                                            oOrder.UserFields.Fields.Item("U_Tara").Value = peso;
                                                            //já tiverem o[Peso Bruto] preenchido > 0,0000, execute cálculo do peso líquido dessas ordens:
                                                            //ORDR.U_PesoBruto - ORDR.U_Tara => ORDR.U_PesoLiquido.
                                                            oOrder.UserFields.Fields.Item("U_PesoLiq").Value =
                                                            (Convert.ToDouble(oOrder.UserFields.Fields.Item("U_PesoBruto").Value) -
                                                                Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value)).ToString();

                                                            //}

                                                        }
                                                        else if (tipoPesagem.Equals("PB"))//Peso Bruto
                                                        {
                                                            oOrder.UserFields.Fields.Item("U_PesoLiq").Value =
                                                                    Convert.ToDouble(peso) -
                                                                    Convert.ToDouble(oOrder.UserFields.Fields.Item("U_Tara").Value);


                                                            oOrder.UserFields.Fields.Item("U_PesoBruto").Value = Convert.ToDouble(peso);
                                                        }

                                                        if (oOrder.Update() == 0)
                                                        {
                                                            Program.oApplicationS.StatusBar.SetText(
                                                                string.Format("OS Nº {0} Atualizada!!", oOrder.DocEntry)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Success
                                                            );
                                                        }
                                                        else
                                                        {
                                                            int temp_int;
                                                            string temp_string;
                                                            Program.oCompanyS.GetLastError(out temp_int, out temp_string);
                                                            Program.oApplicationS.StatusBar.SetText(
                                                                string.Format("Erro ao Alterar OS Nº {0} : {1}!!", oOrder.DocEntry, temp_string)
                                                                , BoMessageTime.bmt_Short
                                                                , BoStatusBarMessageType.smt_Error
                                                            );
                                                        }

                                                    }

                                                    Program.LimparObjeto(oOrder);





                                                    //bSelecionado = true;
                                                    //break;
                                                }
                                            }
                                            lblBalanca.Item.Visible = false;
                                            LogHelper.InfoWarning("Pessagem concluída!!!");
                                            //CarregarPes();

                                            //string arquivo = Helper.ApiHelper.GetArquivo(ip, porta, caminho);

                                            //if (arquivo == "ARQUIVO NÃO ENCONTRADO")
                                            //{
                                            //    throw new Exception("Falha na comunicaçao com a balança capital: arquivo não encontrado");
                                            //}
                                            CarregarPes();
                                        }
                                        else
                                        {
                                            LogHelper.InfoError(string.Format("Não existe registro selecionado, selecione!!"));
                                        }
                                    }
                                }
                                if (pVal.ItemUID == "btUptPlaca")
                                {
                                    if (Program.oApplicationS.MessageBox("Confirma Atualização das Placas da OS?", 1, "Sim", "Não") == 1)
                                    {
                                        LogHelper.InfoWarning(string.Format("Atualização das Placas Iniciada...!!"));

                                        Grid gridPes = (Grid)Form.Items.Item("gridPes").Specific;
                                        //int M3 = 0;
                                        SAPbobsCOM.Recordset recordSetPlaca = null;
                                        try
                                        {
                                            recordSetPlaca = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                            string sSQL = string.Format(@"
		                                                    SELECT 
		                                                        T0.""U_Placa"" ""Placa""
                                                                , T0.""U_UFPlaca"" ""UFPlaca""
                                                                , T0.""U_PNCode"" AS ""TRANSP""

                                                                , T1.""U_MotoPrinc"" AS ""MOTORISTA""
                                                                , 1 TARA
                                                            FROM
                                                                ""@VEICULOS"" T0
                                                                left join ""@VEICULOS_DADOS"" T1 on T1.""Code""=T0.""Code""
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
                                                oOrder = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                                bool bModificou = false;
                                                if (oOrder.GetByKey(Convert.ToInt32(gridPes.DataTable.GetValue(1, i).ToString())))
                                                {
                                                    //for (int x = 0; x < oOrder.Lines.UserFields.Fields.Count; x++)
                                                    //{
                                                    //    Program.Application.StatusBar.SetText(
                                                    //       oOrder.Lines.UserFields.Fields.Item(0).Name
                                                    //        , BoMessageTime.bmt_Short
                                                    //        , BoStatusBarMessageType.smt_Success
                                                    //    );

                                                    //}
                                                    oOrder.UserFields.Fields.Item("U_NPlaca").Value = recordSetPlaca.Fields.Item("Placa").Value.ToString();
                                                    oOrder.UserFields.Fields.Item("U_EstPlaca").Value = recordSetPlaca.Fields.Item("UFPlaca").Value.ToString();
                                                    oOrder.UserFields.Fields.Item("U_Motorista").Value = recordSetPlaca.Fields.Item("MOTORISTA").Value.ToString();
                                                    oOrder.UserFields.Fields.Item("U_Tara").Value = recordSetPlaca.Fields.Item("TARA").Value.ToString();
                                                    oOrder.TaxExtension.Carrier = recordSetPlaca.Fields.Item("TRANSP").Value.ToString();
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
                                                        Program.oApplicationS.StatusBar.SetText(
                                                            string.Format("OS Nº {0} Atualizada!!", oOrder.DocEntry)
                                                            , BoMessageTime.bmt_Short
                                                            , BoStatusBarMessageType.smt_Success
                                                        );
                                                    }
                                                    else
                                                    {
                                                        int temp_int;
                                                        string temp_string;
                                                        Program.oCompanyS.GetLastError(out temp_int, out temp_string);
                                                        Program.oApplicationS.StatusBar.SetText(
                                                            string.Format("Erro ao Alterar OS Nº {0} : {1}!!", oOrder.DocEntry, temp_string)
                                                            , BoMessageTime.bmt_Short
                                                            , BoStatusBarMessageType.smt_Error
                                                        );
                                                    }
                                                }
                                                Program.LimparObjeto(oOrder);
                                            }


                                            //M3 = M3 + CalculaM3OS(i, gridPes.DataTable.GetValue(1, i).ToString());
                                        }

                                        Program.LimparObjeto(recordSetPlaca);
                                        Program.oApplicationS.StatusBar.SetText(
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

                                if (pVal.ItemUID == "chkBal")
                                {
                                    if (((CheckBox)Form.Items.Item("chkBal").Specific).Checked)
                                    {
                                        ((EditText)Form.Items.Item("etPlacaPes").Specific).Value = ((EditText)Form.Items.Item("etPlacaPes").Specific).Value;

                                        ((EditText)Form.Items.Item("edtPeso").Specific).Item.Enabled = false;
                                    }
                                    else
                                    {
                                        ((EditText)Form.Items.Item("edtPeso").Specific).Item.Enabled = true;
                                    }
                                }

                                if (pVal.ItemUID == "ckSelOS")
                                {
                                    if (Form.DataSources.DataTables.Item("dtOS").Rows.Count > 0)
                                        CarregarOS();
                                }
                                if (pVal.ItemUID == "ckSelTPes")
                                {
                                    //if (Form.DataSources.DataTables.Item("dtOS").Rows.Count > 0)
                                    Program.oApplicationS.StatusBar.SetText(
                                        "Selecionando OS..."
                                        , BoMessageTime.bmt_Short
                                        , BoStatusBarMessageType.smt_Warning
                                    );
                                    CarregarPes();

                                    if (!((CheckBox)Form.Items.Item("ckSelTPes").Specific).Checked)
                                    {
                                        m_LinhasRateioPeso.Clear();
                                    }
                                }
                                if (pVal.ItemUID == "gridContr")
                                {
                                    if (pVal.ColUID == "#")
                                    {
                                        Grid gridContr = (Grid)Form.Items.Item("gridContr").Specific;
                                        StaticText lblContratoTot = (StaticText)Form.Items.Item("Item_1").Specific;

                                        int ContratoTot = Convert.ToInt32(lblContratoTot.Caption);
                                        //int M3Tot = Convert.ToInt32(lblM3Tot.Caption);
                                        //int M3 = 0;


                                        if (gridContr.DataTable.GetValue(0, pVal.Row).ToString().Equals("Y"))
                                        {
                                            ContratoTot++;
                                            m_absIDs.Add(pVal.Row);
                                        }
                                        else
                                        {
                                            ContratoTot--;
                                            m_absIDs.Remove(pVal.Row);
                                        }

                                        lblContratoTot.Caption = ContratoTot.ToString();
                                    }
                                }
                                if (pVal.ItemUID == "gridPes")
                                {
                                    if (pVal.ColUID == "#")
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

                                            m_LinhasRateioPeso.Add(pVal.Row);
                                        }
                                        else
                                        {
                                            OsTot--;
                                            M3Tot = M3Tot - M3;
                                            m_LinhasRateioPeso.Remove(pVal.Row);
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
                                    {
                                        Form.DataSources.DataTables.Item("dtFiltro").SetValue("NomeCliente", 0, "");

                                        ((StaticText)Form.Items.Item("Item_6").Specific).Item.Visible = false;
                                        ComboBox CbEndereco = (ComboBox)Form.Items.Item("Item_8").Specific;
                                        CbEndereco.Item.Visible = false;
                                    }
                                    else
                                    {
                                        CarregaEnderecos(((EditText)Form.Items.Item("etCliente").Specific).String);

                                    }
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
                                            CarregaEnderecos(chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString());
                                        }
                                        else if (pVal.ItemUID.Equals("etNrCtr"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("NrContrato", 0, chooseFromListEvent.SelectedObjects.GetValue("Number", 0).ToString());
                                        }
                                        //else if (pVal.ItemUID.Equals("etCentroC"))
                                        //{
                                        //    Form.DataSources.DataTables.Item("dtFiltro").SetValue("CentroCusto", 0, chooseFromListEvent.SelectedObjects.GetValue("PrcCode", 0).ToString());
                                        //}
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
                                        else if (pVal.ItemUID.Equals("etNrPlPs"))
                                        {
                                            Form.DataSources.UserDataSources.Item("PlacaPs").Value = chooseFromListEvent.SelectedObjects.GetValue("U_Placa", 0).ToString();
                                        }
                                        else if (pVal.ItemUID.Equals("edTransp"))
                                        {
                                            Form.DataSources.DataTables.Item("dtFiltro").SetValue("CodTransp", 0, chooseFromListEvent.SelectedObjects.GetValue("CardCode", 0).ToString());

                                            Program.oApplicationS.SendKeys("{TAB}");
                                        }
                                    }
                                    else
                                    {
                                        ((StaticText)Form.Items.Item("Item_6").Specific).Item.Visible = false;
                                        ComboBox CbEndereco = (ComboBox)Form.Items.Item("Item_8").Specific;
                                        CbEndereco.Item.Visible = false;
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
                                    Program.oApplicationS.ItemEvent -= HandleItemEvent;
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
                                    //try
                                    //{
                                    //    Form.DataSources.UserDataSources.Add("DtOSI", SAPbouiCOM.BoDataType.dt_DATE, 0);
                                    //    Form.DataSources.UserDataSources.Add("DtOSF", SAPbouiCOM.BoDataType.dt_DATE, 0);

                                    //    ((EditText)Form.Items.Item("etDtOSI").Specific).DataBind.SetBound(true, "", "DtOSI");
                                    //    ((EditText)Form.Items.Item("etDtOSF").Specific).DataBind.SetBound(true, "", "DtOSF");
                                    //}
                                    //catch 
                                    //{
                                    //}
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

                                        //Form.Items.Item("11").Visible = false;
                                        //Form.Items.Item("cbModCtr").Visible = false;

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

                                        ComboBox cbUtlRes = (ComboBox)Form.Items.Item("cbUtlRes").Specific;

                                        SAPbobsCOM.Recordset recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                        recordSet.DoQuery(@"select 0 as ""ID"", '[Selecionar]' as ""Usage"" from dummy union select ""ID"", ""Usage"" from OUSG where ""Usage"" LIKE 'V%'");

                                        while (!recordSet.EoF)
                                        {
                                            cbUtlRes.ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());

                                            recordSet.MoveNext();
                                        }

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
                Program.oApplicationS.StatusBar.SetText(exception.Message);
            }
        }

        private void CarregaEnderecos(string sCardCode)
        {
            ((StaticText)Form.Items.Item("Item_6").Specific).Item.Visible = true;
            ComboBox CbEndereco = (ComboBox)Form.Items.Item("Item_8").Specific;
            CbEndereco.Item.Visible = true;

            while (CbEndereco.ValidValues.Count > 0)
            {
                CbEndereco.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            CbEndereco.ValidValues.Add("0", "Selecione");

            string SQL = string.Format(@"
                            select 
                            	T0.""Address""
	                            ,coalesce(T0.""AddrType"",'')
	                            ||  ' ' ||  coalesce(T0.""Street"" ,'')
	                            || ' ,'  || coalesce(T0.""StreetNo"" ,'')
	                            || ' - ' || coalesce(T0.""Block"",'')
	                            || ' - ' || coalesce(T0.""City"" ,'')
	                            || ' - ' || coalesce(T0.""State"",'')
	                            || ' - ' || coalesce(T0.""ZipCode"",'')	as ""Endereco""
                            from CRD1 T0	
                            Where 
	                            T0.""CardCode""='{0}'
                                            ", sCardCode);

            SAPbobsCOM.Recordset recordSetEndereco = null;
            recordSetEndereco = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recordSetEndereco.DoQuery(SQL);
            while (!recordSetEndereco.EoF)
            {
                CbEndereco.ValidValues.Add(recordSetEndereco.Fields.Item(0).Value.ToString(), recordSetEndereco.Fields.Item(1).Value.ToString());
                recordSetEndereco.MoveNext();
            }
            //if (CbEndereco.ValidValues.Count>0)
            //{

            //}
            //CbEndereco.Select(0, BoSearchKey.psk_Index);
            //if (recordSetEndereco.RecordCount > 0)
            //{
            //    M3 = Convert.ToInt32(recordSetVerificacao.Fields.Item(0).Value.ToString());
            //    recordSetEndereco.MoveNext();
            //}
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
	                        t3.""ItmsGrpCod"" IN (102, 118)
                            and ( T0.""DocEntry""  in ({0}) )

                "
                , sDocEntrys);

                recordSetTotal = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Program.LimparObjeto(recordSetTotal);
                return dRetorno;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private double PesoLiquidoTotalSelecionados()
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
                            sum(T0.""U_PesoLiq"")
                        from
                            ORDR T0
	                        inner join RDR1 T1 on T1.""DocEntry""=T0.""DocEntry""    
	                        inner join OITM T2 on T2.""ItemCode""=T1.""ItemCode""
	                        inner join OITB t3 on t3.""ItmsGrpCod""=T2.""ItmsGrpCod""
                        where 
	                        t3.""ItmsGrpCod"" IN (102, 118)
                            and ( T0.""DocEntry""  in ({0}) )

                "
                , sDocEntrys);

                recordSetTotal = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Program.LimparObjeto(recordSetTotal);
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
	                        t3.""ItmsGrpCod"" IN (102, 118)
                            and ( T0.""DocEntry""  in ({0}) )

                "
                , sDocEntry);

                recordSetTotal = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Program.LimparObjeto(recordSetTotal);
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

                recordSetTotal = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Program.LimparObjeto(recordSetTotal);
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

                recordSetTotal = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                recordSetTotal.DoQuery(sSQL);

                double dRetorno = Convert.ToDouble(recordSetTotal.Fields.Item(0).Value.ToString());

                Program.LimparObjeto(recordSetTotal);
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
            //for (int i = 0; i < gridPes.Rows.Count; i++)
            foreach (int linhaRateio in m_LinhasRateioPeso)
            {
                int i = linhaRateio;
                //if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                //{

                string sDocEntry = gridPes.DataTable.GetValue(1, i).ToString();
                if (string.IsNullOrEmpty(sDocEntrys))
                {
                    sDocEntrys = sDocEntry;
                }
                else
                {
                    sDocEntrys = sDocEntrys + "," + sDocEntry;
                }
                //}
            }

            return sDocEntrys;
        }

        private static int CalculaM3OS(int pRow, string DocEntry)
        {
            int M3 = 0;
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


                recordSetVerificacao = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recordSetVerificacao.DoQuery(SQL);
                if (recordSetVerificacao.RecordCount > 0)
                {
                    M3 = Convert.ToInt32(recordSetVerificacao.Fields.Item(0).Value.ToString());
                    recordSetVerificacao.MoveNext();
                }
            }
            catch
            {

            }
            finally
            {
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSetVerificacao);
                //GC.Collect();
            }

            return M3;
        }

        private void CarregarContratos()
        {
            m_absIDs = new List<int>();

            var dtFiltro = Form.DataSources.DataTables.Item("dtFiltro");

            string selecionar = ((CheckBox)Form.Items.Item("ckSelCtr").Specific).Checked ? "Y" : "N";

            string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;

            string dataDe = ((EditText)Form.Items.Item("etDtCtrI").Specific).String;

            string dataAte = ((EditText)Form.Items.Item("etDtCtrF").Specific).String;

            string nrContrato = ((EditText)Form.Items.Item("etNrCtr").Specific).String;

            string modeloContrato = ((ComboBox)Form.Items.Item("cbModCtr").Specific).Selected.Value;

            string centroCusto = ""; // ((EditText)Form.Items.Item("etCentroC").Specific).String;

            string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;

            string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Value;

            string motorista = ((EditText)Form.Items.Item("etMotoraN").Specific).String;

            //string placa = ((EditText)Form.Items.Item("etNrPlaca").Specific).String;

            string placa = "";

            ComboBox cbEndereco = ((ComboBox)Form.Items.Item("Item_8").Specific);
            string endereco = cbEndereco.Selected == null ? "0" : cbEndereco.Selected.Value;

            string query = string.Format(@"SELECT '{0}' AS ""#"",
                                                      OOAT.""AbsID"" AS ""Nº Interno"",
                                                      STRING_AGG(OAT1.""AgrLineNum"", ',')as ""AgrLineNum"",
                                                      OOAT.""Number"" as ""Nº CTR"",
                                                      OOAT.""BpName"" as ""Cliente"",
                                                      OOAT.""Descript"" as ""Desc. CTR"",
                                                      STRING_AGG(OAT1.""ItemName"", ',') as ""Serviço"",
                                                      sum(OAT1.""PlanQty"") as ""Coleta Planejada"",
                                                      sum(OAT1.""PlanQty"" - 0) as ""Coleta Pendente"",
                                                      (select max(""DocNum"")
                                                       from ORDR 
                                                       where exists (select * 
                                                                     from RDR1 
                                                                     where RDR1.""AgrNo"" = OOAT.""AbsID"" 
                                                                     --and RDR1.""AgrLnNum"" = OAT1.""AgrLineNum""
                                                                     and RDR1.""DocEntry"" = ORDR.""DocEntry"")
                                                       ) as ""Última OS"",
                                                      (select max(""DocDate"")
                                                       from ORDR 
                                                       where exists (select * 
                                                                     from RDR1 
                                                                     where RDR1.""AgrNo"" = OOAT.""AbsID"" 
                                                                     --and RDR1.""AgrLnNum"" = OAT1.""AgrLineNum""
                                                                     and RDR1.""DocEntry"" = ORDR.""DocEntry"")
                                                       ) as ""Data OS""
                                            from OOAT
                                            inner join OAT1 on OAT1.""AgrNo"" = OOAT.""AbsID""
                                            inner join OITM T22 on T22.""ItemCode"" = OAT1.""ItemCode""   
                                            inner join OITB t33 on t33.""ItmsGrpCod"" = T22.""ItmsGrpCod"" and t33.""ItmsGrpCod"" IN (102, 118)
                                            left join ""@ROTAS"" on OOAT.""U_Rota"" = ""@ROTAS"".""Code""
                                            left join ""@VEICULOS"" on ""@ROTAS"".""U_Veiculo"" = ""@VEICULOS"".""U_Placa""
                                            where OOAT.""BpType"" = 'C'
                                            and OOAT.""Status"" = 'A'
                                            and OOAT.""Cancelled"" <> 'Y'
                                            and exists (select * from OAT1 TX inner join OITM TY on TY.""ItemCode"" = TX.""ItemCode"" where TX.""AgrNo"" = OOAT.""AbsID"" and TY.""ItmsGrpCod"" = 118)
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
                                            and ('0'='0' or 
					                                            exists(
																		select 1
																		from ORDR 
																		where exists (select * 
																		                 from RDR1 
																		                 where RDR1.""AgrNo"" = OOAT.""AbsID"" 
																		             and RDR1.""DocEntry"" = ORDR.""DocEntry"")
																		         and ORDR.""ShipToCode""='0'
					                                            )
                                            )
                                        group by
                                                                                              OOAT.""AbsID""
                                                                                              --, OAT1.""AgrLineNum""
                                                                                              , OOAT.""Number""
                                                                                              , OOAT.""BpName""
                                                                                              , OOAT.""Descript""
                                                                                              --, OAT1.""PlanQty""
                                                                                              --, OAT1.""PlanQty""

                                            
                                            ", selecionar, cliente
                                            , dataDe == "" ? "1990-01-01" : DateTime.Parse(dtFiltro.GetValue("DataCtrIni", 0).ToString()).ToString("yyyy-MM-dd")
                                            , dataAte == "" ? "1990-01-01" : DateTime.Parse(dtFiltro.GetValue("DataCtrFim", 0).ToString()).ToString("yyyy-MM-dd")
                                            , nrContrato, modeloContrato
                                            , centroCusto, nrRota, diaColeta, motorista, placa, endereco);

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


                if (((CheckBox)Form.Items.Item("ckSelCtr").Specific).Checked)
                {
                    ((StaticText)Form.Items.Item("Item_1").Specific).Caption = Form.DataSources.DataTables.Item("dtContr").Rows.Count.ToString();
                }
                else
                {
                    ((StaticText)Form.Items.Item("Item_1").Specific).Caption = "0";
                }


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

            //Program.oApplicationS.StatusBar.SetText(
            //    "Consulta Pessagem Iniciada..."
            //    , BoMessageTime.bmt_Short
            //    , BoStatusBarMessageType.smt_Warning
            //);

            m_LinhasRateioPeso = new List<int>();

            string selecionar = ((CheckBox)Form.Items.Item("ckSelTPes").Specific).Checked ? "Y" : "N";
            string cliente = ((EditText)Form.Items.Item("etCliente").Specific).String;
            string placa = ((EditText)Form.Items.Item("etNrPlPs").Specific).String;
            string nrRota = ((EditText)Form.Items.Item("etNrRota").Specific).String;
            string nrSerie = ((EditText)Form.Items.Item("Item_5").Specific).String;
            ComboBox cbEndereco = ((ComboBox)Form.Items.Item("Item_8").Specific);
            string endereco = cbEndereco.Selected == null ? "0" : cbEndereco.Selected.Value;
            string dataDe = ((EditText)Form.Items.Item("etDtOSI").Specific).String;
            string dataAte = ((EditText)Form.Items.Item("etDtOSF").Specific).String;

            string query = string.Format(@"
                    select 
	                    '{0}' as ""#""
                        , T0.""DocEntry"" as ""Nº Interno""
                        , T0.""DocNum"" AS ""Nº OS""
                        , T0.""CardCode""
                        , T0.""CardName"" ""Cliente""
                        , T0.""U_NPlaca"" ""Placa""
                        ,(
	                        select 
	                            sum(T11.""Quantity"")
                            from
                                ORDR T00
	                            inner join RDR1 T11 on T11.""DocEntry""=T00.""DocEntry""    
	                            inner join OITM T22 on T22.""ItemCode""=T11.""ItemCode""
	                            inner join OITB t33 on t33.""ItmsGrpCod""=T22.""ItmsGrpCod""
                            where 
	                            t33.""ItmsGrpCod"" IN (102, 118)
                                and ( T00.""DocEntry""  in (T0.""DocEntry"") )

                        ) ""m3""
                        , T0.""U_Tara"" ""Tara""
                        , T0.""U_PesoBruto"" ""Peso Bruto""
                        , T0.""U_PesoLiq"" ""Peso Liq.""
                        , ( SELECT MAX(T5.""DistNumber"")
                            FROM RDR1 T1
                            INNER JOIN OITL T3 ON T1.""DocEntry"" = T3.""ApplyEntry"" AND T1.""LineNum"" = T3.""ApplyLine""
                            INNER JOIN ITL1 T4 ON T3.""LogEntry"" = T4.""LogEntry""
                            INNER JOIN OSRN T5 ON T4.""ItemCode"" = T5.""ItemCode"" AND T4.""MdAbsEntry"" = T5.""AbsEntry""
                            WHERE T1.""DocEntry"" = T0.""DocEntry"") AS ""Nº Série""
                    from
                        ORDR T0
                        left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = T0.""U_NPlaca""
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
                        and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(T0.""DocDueDate"" as date) >= '{4}')
                        and (cast('{5}' as date) = cast('1990-01-01' as date) or cast(T0.""DocDueDate"" as date) <= '{5}') 
                        and ('{6}' = '' or exists ( SELECT T5.""DistNumber""
                                                    FROM RDR1 T1
                                                    INNER JOIN OITL T3 ON T1.""DocEntry"" = T3.""ApplyEntry"" AND T1.""LineNum"" = T3.""ApplyLine""
                                                    INNER JOIN ITL1 T4 ON T3.""LogEntry"" = T4.""LogEntry""
                                                    INNER JOIN OSRN T5 ON T4.""ItemCode"" = T5.""ItemCode"" AND T4.""MdAbsEntry"" = T5.""AbsEntry""
                                                    WHERE T1.""DocEntry"" = T0.""DocEntry"" 
                                                    AND T5.""DistNumber"" = '{6}'))                        
                        and ('{7}'='0' or ""ShipToCode""='{7}')
                    order by T0.""DocNum"" DESC

            ",
                                            selecionar,
                                            cliente,
                                            placa,
                                            nrRota,
                                            dataDe == "" ? "1990-01-01" : DateTime.ParseExact(Form.DataSources.UserDataSources.Item("DtOSI").ValueEx, "yyyyMMdd", null).ToString("yyyy-MM-dd"),
                                            dataAte == "" ? "1990-01-01" : DateTime.ParseExact(Form.DataSources.UserDataSources.Item("DtOSF").ValueEx, "yyyyMMdd", null).ToString("yyyy-MM-dd"),
                                            nrSerie,
                                            endereco);

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
                        M3 = M3 + CalculaM3OS(i, gridPes.DataTable.GetValue(1, i).ToString());
                        m_LinhasRateioPeso.Add(i);
                    }
                    ((StaticText)Form.Items.Item("lblM3Tot").Specific).Caption = M3.ToString();
                }
                else
                {
                    ((StaticText)Form.Items.Item("lblOsTot").Specific).Caption = "0";
                    ((StaticText)Form.Items.Item("lblM3Tot").Specific).Caption = "0";
                }


                ConfiguraGridPes();

                Program.oApplicationS.StatusBar.SetText(
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
            try
            {
                gridPes.Columns.Item("Nº Série").Editable = false;
            }
            catch { }
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

            string motorista = ((EditText)Form.Items.Item("etMotoraN").Specific).String;

            string tipoOperacao = ((ComboBox)Form.Items.Item("cbTpOper").Specific).Selected.Value;

            string respFat = ((ComboBox)Form.Items.Item("cbRespFat").Specific).Selected.Description;

            string agrupamento = ((ComboBox)Form.Items.Item("cbAgrOS").Specific).Selected.Value;

            string transportadora = ((EditText)Form.Items.Item("edTransp").Specific).String;


            ComboBox cbEndereco = ((ComboBox)Form.Items.Item("Item_8").Specific);
            string endereco = cbEndereco.Selected == null ? "0" : cbEndereco.Selected.Value;



            string query = string.Format(@"SELECT '{0}' AS ""#"",
                                                      ORDR.""DocEntry"" AS ""Nº Interno"",
                                                      ORDR.""DocNum"" AS ""Nº OS"",
                                                      'N' AS ""Esboço"",
                                                      ORDR.""DocDate"" AS ""Dt Gerada"",
                                                        ORDR.""DocDueDate"" AS ""Dt Entrega"",
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
                                                    ,ORDR.""U_TipoOper"" as ""TipoOper""
                                            from ORDR
                                            --left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = ORDR.""U_NPlaca""
                                            left join OCRD TRANSP ON TRANSP.""CardCode"" = ORDR.""U_CodTransp""
                                            left join OUSR ON ORDR.""UserSign"" = OUSR.""USERID""
                                            where ORDR.""CANCELED"" = 'N'
                                            and ORDR.""DocStatus"" = 'O'                                         
                                            and ('{1}' = '' or '{1}' = ORDR.""CardCode"")
                                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(ORDR.""DocDate"" as date) >= '{2}')
                                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(ORDR.""DocDate"" as date) <= '{3}') 
                                            and ('{4}' = '' or '{4}' = ORDR.""DocNum"")
                                            and ('{5}' = '' or '{5}' = ORDR.""U_Situacao"")
                                            and ('{6}' = '' or '{6}' = ORDR.""U_NPlaca"")
                                            and ('{7}' = '' or '{7}' = (select max(OOAT.""U_Rota"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{8}' = '' or '{8}' = OUSR.""U_NAME"")
                                            and ('{9}' = '' or '{9}' = (select max(OOAT.""Number"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{10}' = '' or '{10}' = (select max(OOAT.""U_Modelo"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{11}' = '[Selecionar]' or '{11}' = ORDR.""U_DiaSemRot"")
                                            and ('{12}' = '' or '{12}' = (select max(OOAT.""U_Motorista"") from RDR1 inner join OOAT on OOAT.""AbsID"" = RDR1.""AgrNo"" where RDR1.""DocEntry"" = ORDR.""DocEntry""))
                                            and ('{13}' = '' or '{13}' = ORDR.""U_TipoOper"")
                                            and ('{14}' = '[Selecionar]' or '{14}' = ORDR.""U_RespFat"")
                                            and ('{15}'='0' or ""ShipToCode""='{15}')
                                            and ('{16}'='' or ORDR.""U_Status""='{16}')
                                            and ('{17}' = '' or '{17}' = ORDR.""U_CodTransp"")
",
                                            selecionar,
                                            cliente,
                                            dataDe == "" ? "1990-01-01" : DateTime.ParseExact(Form.DataSources.UserDataSources.Item("DtOSI").ValueEx, "yyyyMMdd", null).ToString("yyyy-MM-dd"),
                                            dataAte == "" ? "1990-01-01" : DateTime.ParseExact(Form.DataSources.UserDataSources.Item("DtOSF").ValueEx, "yyyyMMdd", null).ToString("yyyy-MM-dd"),
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
                                            respFat,
                                            endereco,
                                            statusOS,
                                            transportadora
                                            );

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
                gridOS.Columns.Item("Dt Gerada").Editable = false;
                gridOS.Columns.Item("Dt Entrega").Editable = false;
                gridOS.Columns.Item("Resp. Faturamento").Editable = false;
                gridOS.Columns.Item("Cód. Cliente").Editable = false;
                gridOS.Columns.Item("Nome Cliente").Editable = false;
                gridOS.Columns.Item("Motorista").Editable = false;
                gridOS.Columns.Item("Veículo").Editable = false;
                gridOS.Columns.Item("Situação").Editable = false;
                gridOS.Columns.Item("Status").Editable = false;

                gridOS.Columns.Item("TipoOper").Editable = false;

                gridOS.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;
                gridOS.Columns.Item("Esboço").Type = BoGridColumnType.gct_CheckBox;

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

        private bool ValidaCentroDeCusto()
        {
            bool bRetorno = false;
            SAPbobsCOM.Recordset recordSetVerificacao = null;
            try
            {
                string SQL = string.Format(@"
                                                    select T0.""PrcCode"" from OPRC T0 where T0.""PrcCode""='{0}'
                                            ", Form.DataSources.UserDataSources.Item("nrPlacaOS").Value);


                recordSetVerificacao = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recordSetVerificacao.DoQuery(SQL);
                if (recordSetVerificacao.RecordCount > 0)
                {
                    bRetorno = true;
                    //recordSetVerificacao.MoveNext();
                }
            }
            catch
            {

            }
            finally
            {
                Program.LimparObjeto(recordSetVerificacao);
            }

            return bRetorno;
        }

        private bool ValidaCentroDeCusto(string placa)
        {
            bool bRetorno = false;
            SAPbobsCOM.Recordset recordSetVerificacao = null;
            try
            {
                string SQL =$@"select T0.""PrcCode"" from OPRC T0 where T0.""PrcCode"" = '{placa}';";

                recordSetVerificacao = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recordSetVerificacao.DoQuery(SQL);
                if (recordSetVerificacao.RecordCount > 0)
                {
                    bRetorno = true;
                    //recordSetVerificacao.MoveNext();
                }
            }
            catch
            {

            }
            finally
            {
                Program.LimparObjeto(recordSetVerificacao);
            }

            return bRetorno;
        }

        private void GerarOS()
        {
            string placa = Form.DataSources.DataTables.Item("dtFiltro").GetValue("NrPlaca", 0).ToString(); 
            DateTime dataSaidaOS = Form.DataSources.UserDataSources.Item("dtSaidaOS").Value == "" ? DateTime.MinValue : DateTime.ParseExact(Form.DataSources.UserDataSources.Item("dtSaidaOS").ValueEx, "yyyyMMdd", null); // Convert.ToDateTime(Form.DataSources.UserDataSources.Item("dtSaidaOS").Value);
            string horaSaidaOS = Form.DataSources.UserDataSources.Item("hrSaidaOS").Value;
            //string motora = ((EditText)Form.Items.Item("etMotora").Specific).String;
            string motoristaNome = Form.DataSources.DataTables.Item("dtFiltro").GetValue("NomeMotorista", 0).ToString();
            string diaColeta = ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected == null ? "[Selecionar]" : ((ComboBox)Form.Items.Item("cbDiaCol").Specific).Selected.Description;
            string utilizacaoResiduo = Form.DataSources.DataTables.Item("dtFiltro").GetValue("UtlRes", 0).ToString();
            //string utilizacaoLocacao = Form.DataSources.DataTables.Item("dtFiltro").GetValue("UtlLoc", 0).ToString();            

            if (dataSaidaOS == DateTime.MinValue)
            {
                Program.oApplicationS.StatusBar.SetText("Informe a data de saída da OS", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            if (motoristaNome == "")
            {
                Program.oApplicationS.StatusBar.SetText("Informe o motorista da OS", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            if (horaSaidaOS == "")
            {
                Program.oApplicationS.StatusBar.SetText("Informe a hora de saída da OS", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            if (placa == "")
            {
                Program.oApplicationS.StatusBar.SetText("Informe a placa", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            if (utilizacaoResiduo == "")
            {
                Program.oApplicationS.StatusBar.SetText("Informe a utilização resíduo", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            //if (utilizacaoLocacao == "")
            //{
            //    Program.oApplicationS.StatusBar.SetText("Informe a utilização locação", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

            //    return;
            //}

            //valida o centro de custo 
            if (!ValidaCentroDeCusto(placa))
            {
                Program.oApplicationS.StatusBar.SetText("Placa sem centro de Custo!!", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }
            if (diaColeta == "[Selecionar]")
            {
                Program.oApplicationS.StatusBar.SetText("Informe o dia da coleta", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }
            Grid gridPes = (Grid)Form.Items.Item("gridContr").Specific;
            bool bSelecionado = false;
            for (int i = 0; i < gridPes.Rows.Count; i++)
            {
                if (gridPes.DataTable.GetValue(0, i).ToString().Equals("Y"))
                {
                    bSelecionado = true;
                    break;
                }
            }
            if (!bSelecionado)
            {
                LogHelper.InfoError(string.Format("Não existe registro selecionado, selecione!!"));
                return;
            }


            Program.oApplicationS.StatusBar.SetText("Gerando ordens de serviço", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
            string sOSsGeradas = string.Empty;
            //PEGA TODOS OS SELECIONADOS
            Grid gridContratos = (Grid)Form.Items.Item("gridContr").Specific;
            List<int> absIDs = new List<int>();

            if (((CheckBox)Form.Items.Item("ckSelCtr").Specific).Checked)
            {
                for (int row = 0; row < gridContratos.Rows.Count; row++)
                {
                    absIDs.Add(Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("Nº Interno")).GetText(row)));
                }
            }

            foreach (int absRow in m_absIDs)
            {
                absIDs.Add(Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("Nº Interno")).GetText(absRow)));
            }

            bool osGeradas = false;
            string sEsbocos = "";
            List<int> absIDsNotasVencidas = new List<int>();
            List<ErroGerOS> ErroGerOSs = new List<ErroGerOS>();
            //string sTelasGeradas
            //if (!Program.oCompanyS.InTransaction)
            //Program.oCompanyS.StartTransaction();
            //para cada absId faz o loop agrupando as linhas do mesmo...
            foreach (int absID1 in absIDs)
            {
                int iNumber = 0;
                try
                {
                    LogHelper.InfoSuccess(string.Format("Processando Contrato {0}", absID1));
                    //NotasVencidas(absID).Count();

                    if (NotasVencidas(absID1).Count() > 0)
                    {
                        LogHelper.InfoError(string.Format("Notas {0} do cliente {1} em aberto. Não é possível gerar OS.",
                            string.Join(",", NotasVencidas(absID1).Select(r => r.Key).ToArray()), NotasVencidas(absID1).Select(r => r.Value).First()));
                        absIDsNotasVencidas.Add(absID1);
                        continue;
                    }

                    //cabeçalho
                    //SAPbobsCOM.Documents documents;//= (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                    //linhas
                    for (int row = 0; row < gridContratos.Rows.Count; row++)
                    {
                        if (((CheckBoxColumn)gridContratos.Columns.Item("#")).IsChecked(row))
                        {
                            if (Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("Nº Interno")).GetText(row)) == absID1)
                            {
                                SAPbobsCOM.Documents documents = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                                SAPbobsCOM.Recordset recordSet = ConsultaContratoGeracao(placa, gridContratos, absID1, utilizacaoResiduo, row);
                                iNumber = Convert.ToInt32(recordSet.Fields.Item(11).Value.ToString());
                                while (!recordSet.EoF)
                                {
                                    MontaOS(placa, dataSaidaOS, horaSaidaOS, motoristaNome, diaColeta, absID1, documents, recordSet);
                                    recordSet.MoveNext();
                                }
                                Program.LimparObjeto(recordSet);

                                int result = documents.Add();

                                if (result != 0)
                                {
                                    int codigoErro;
                                    string msgErro;

                                    Program.oCompanyS.GetLastError(out codigoErro, out msgErro);

                                    if (msgErro.Contains("RDR1.AgrNo"))
                                    {
                                        msgErro = "Data Térmido do Contrato expirou!!!";
                                        LogHelper.InfoError(msgErro);
                                    }
                                    else
                                    {
                                        LogHelper.InfoError(msgErro);
                                    }
                                    // Convert.ToInt32(recordSet.Fields.Item(11).Value.ToString());
                                    ErroGerOSs.Add(new ErroGerOS() { absID = iNumber, Erro = msgErro });
                                    //throw new Exception(msgErro);
                                }
                                else
                                {
                                    osGeradas = true;

                                    //LogHelper.InfoSuccess(string.Format("OS {0} Gerado {1}", iNumber, Program.oCompanyS.GetNewObjectKey()));

                                    if (string.IsNullOrEmpty(sOSsGeradas))
                                    {
                                        if (Program.oCompanyS.GetNewObjectType() == "112")
                                            sEsbocos = Program.oCompanyS.GetNewObjectKey();
                                        else
                                            sOSsGeradas = Program.oCompanyS.GetNewObjectKey();
                                        //LogHelper.InfoSuccess($"sOSsGeradas {sOSsGeradas}");

                                    }
                                    else
                                    {
                                        if (Program.oCompanyS.GetNewObjectType() == "112")
                                            sEsbocos = sEsbocos + "," + Program.oCompanyS.GetNewObjectKey();
                                        else
                                            sOSsGeradas = sOSsGeradas + "," + Program.oCompanyS.GetNewObjectKey();
                                    }
                                }
                                Program.LimparObjeto(documents);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msgErro = string.Format("{0} - {0}", ex.Message, ex.StackTrace);
                    LogHelper.InfoError(string.Format("Erro Processando OS {0}: {1}", absID1, msgErro));

                    ErroGerOSs.Add(new ErroGerOS() { absID = iNumber, Erro = msgErro });
                }
            }

            if (absIDsNotasVencidas.Count > 0)
            {
                foreach (int absID1 in absIDsNotasVencidas)
                {
                    for (int row = 0; row < gridContratos.Rows.Count; row++)
                    {
                        int iNumber = 0;
                        if (((CheckBoxColumn)gridContratos.Columns.Item("#")).IsChecked(row))
                        {
                            if (Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("Nº Interno")).GetText(row)) == absID1)
                            {
                                if (Program.oApplicationS.MessageBox(
                                        string.Format("Notas {0} do cliente {1} em aberto. Deseja Gerar a OS?.",
                                        string.Join(",", NotasVencidas(absID1).Select(r => r.Key).ToArray()), NotasVencidas(absID1).Select(r => r.Value).First())

                                    , 1, "Sim", "Não") == 1)
                                {
                                    try
                                    {

                                        SAPbobsCOM.Documents documents = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                        SAPbobsCOM.Recordset recordSet = ConsultaContratoGeracao(placa, gridContratos, absID1, utilizacaoResiduo, row);
                                        iNumber = Convert.ToInt32(recordSet.Fields.Item(11).Value.ToString());
                                        while (!recordSet.EoF)
                                        {
                                            MontaOS(placa, dataSaidaOS, horaSaidaOS, motoristaNome, diaColeta, absID1, documents, recordSet);
                                            recordSet.MoveNext();
                                        }
                                        Program.LimparObjeto(recordSet);
                                        int result = documents.Add();

                                        if (result != 0)
                                        {
                                            int codigoErro;
                                            string msgErro;

                                            Program.oCompanyS.GetLastError(out codigoErro, out msgErro);

                                            if (msgErro.Contains("RDR1.AgrNo"))
                                            {
                                                msgErro = "Data Térmido do Contrato expirou!!!";
                                                LogHelper.InfoError(msgErro);
                                            }
                                            else
                                            {
                                                LogHelper.InfoError(msgErro);
                                            }


                                            ErroGerOSs.Add(new ErroGerOS() { absID = iNumber, Erro = msgErro });
                                            //throw new Exception(msgErro);
                                        }
                                        else
                                        {
                                            osGeradas = true;
                                            //LogHelper.InfoSuccess(string.Format("OS {0} Gerado {1}", iNumber, Program.oCompanyS.GetNewObjectKey()));

                                            if (string.IsNullOrEmpty(sOSsGeradas))
                                            {
                                                if (Program.oCompanyS.GetNewObjectType() == "112")
                                                    sEsbocos = Program.oCompanyS.GetNewObjectKey();
                                                else
                                                    sOSsGeradas = Program.oCompanyS.GetNewObjectKey();
                                                //LogHelper.InfoSuccess($"sOSsGeradas {sOSsGeradas}");

                                            }
                                            else
                                            {
                                                if (Program.oCompanyS.GetNewObjectType() == "112")
                                                    sOSsGeradas = sOSsGeradas + "," + Program.oCompanyS.GetNewObjectKey();
                                                else
                                                    sEsbocos = sEsbocos + "," + Program.oCompanyS.GetNewObjectKey();
                                            }
                                        }
                                        Program.LimparObjeto(documents);

                                    }
                                    catch (Exception ex)
                                    {
                                        string msgErro = string.Format("{0} - {0}", ex.Message, ex.StackTrace);
                                        LogHelper.InfoError(string.Format("Erro Processando OS {0}: {1}", iNumber, msgErro));
                                        ErroGerOSs.Add(new ErroGerOS() { absID = iNumber, Erro = msgErro });
                                    }
                                }
                                else
                                {
                                    ErroGerOSs.Add(new ErroGerOS() { absID = iNumber, Erro = "Notas em aberto!!" });
                                }
                            }
                        }
                        //break;
                    }
                }
            }

            //Program.oCompanyS.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

            if (osGeradas)
            {
                // Program.oApplicationS.MessageBox("Ordens de serviço geradas", 1, "OK");

                Form.DataSources.UserDataSources.Item("nrPlacaOS").Value = string.Empty;
                Form.DataSources.UserDataSources.Item("dtSaidaOS").Value = string.Empty;
                Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = string.Empty;

                ((Grid)Form.Items.Item("gridContr").Specific).DataTable.Clear();

                string queryFiltro = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"",  0 as ""UtlRes"", 0 as ""UtlLoc"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"" from dummy";

                Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(queryFiltro);

                MostrarOSGeradas(sOSsGeradas, sEsbocos, ErroGerOSs);

                ((EditText)Form.Items.Item("etMotora").Specific).String = "";
                ((EditText)Form.Items.Item("etMotoraN").Specific).String = "";

                ////frmOSsGeradas tela = this.CreateForm<frmOSsGeradas>();
                ////tela.NGTME = _oForm.DataSources.DBDataSources.Item(JBC_GTME).GetValue("DocEntry", 0);
                ////tela._oMatrixGTME = _oMatrix;
                ////tela.oDBDataSource = oDBDataSource;
                ////tela.oFormFather = _oForm;
                //tela.Show();
            }

            //if (!Program.oCompanyS.InTransaction)
            //Program.oCompanyS.StartTransaction();
            //try
            //{





            //    for (int row = 0; row < gridContratos.Rows.Count; row++)
            //    {
            //        Dictionary<string, string> notasVencidas = new Dictionary<string, string>();

            //        SAPbobsCOM.Documents documents = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            //        try
            //        {
            //            if (((CheckBoxColumn)gridContratos.Columns.Item("#")).IsChecked(row))
            //            {
            //                int absID = Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("Nº Interno")).GetText(row));
            //                int agrLineNum = Convert.ToInt32(((EditTextColumn)gridContratos.Columns.Item("AgrLineNum")).GetText(row));

            //                string queryVerificacao = string.Format(@"select ""DocNum"", ""CardName"" from OINV 
            //                                          where ""CardCode"" = (select ""BpCode"" from OOAT where OOAT.""AbsID"" = {0})
            //                                          and ""DocStatus"" = 'O'
            //                                          and exists (select * from INV6 
            //                                                      where INV6.""DocEntry"" = OINV.""DocEntry"" 
            //                                                      and DAYS_BETWEEN(INV6.""DueDate"", current_date) > 30)", absID);

            //                SAPbobsCOM.Recordset recordSetVerificacao = null;
            //                try
            //                {
            //                    recordSetVerificacao = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //                    recordSetVerificacao.DoQuery(queryVerificacao);
            //                    while (!recordSetVerificacao.EoF)
            //                    {
            //                        notasVencidas.Add(recordSetVerificacao.Fields.Item(0).Value.ToString(), recordSetVerificacao.Fields.Item(1).Value.ToString());

            //                        recordSetVerificacao.MoveNext();
            //                    }
            //                }
            //                finally
            //                {
            //                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSetVerificacao);
            //                    GC.Collect();
            //                }        

            //                if (notasVencidas.Count > 0)
            //                {
            //                    Program.oApplicationS.MessageBox(string.Format("Notas {0} do cliente {1} em aberto. Não é possível gerar OS.",
            //                        string.Join(",", notasVencidas.Select(r => r.Key).ToArray()), notasVencidas.Select(r => r.Value).First()));

            //                    continue;
            //                }

            //                string query = string.Format(@"select OOAT.""BpCode"",                                                                  
            //                                                      OOAT.""U_Motorista"",
            //                                                      OAT1.""ItemCode"",
            //                                                      OAT1.""U_Capacidade"",
            //                                                      OAT1.""UnitPrice"",
            //                                                      0,
            //                                                      COALESCE(""@VEICULOS_DADOS"".""U_Tara"", 0),                                                                  
            //                                                      OOAT.""U_Rota"",
            //                                                      COALESCE(""@VEICULOS"".""U_UFPlaca"", ''),
            //                                                      (SELECT T0.""ID"" FROM OUSG T0 WHERE T0.""Usage"" = OOAT.""U_Utilizacao"")
            //                                                from OOAT
            //                                                inner join OAT1 on OAT1.""AgrNo"" = OOAT.""AbsID""                                                            
            //                                                left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = '{2}'
            //                                                left join ""@VEICULOS_DADOS"" ON ""@VEICULOS_DADOS"".""Code"" = ""@VEICULOS"".""Code""
            //                                                where OOAT.""AbsID"" = {0}
            //                                                and OAT1.""AgrLineNum"" = {1}
            //                                                ", absID, agrLineNum, placaOS);

            //                SAPbobsCOM.Recordset recordSet = null;
            //                try
            //                {
            //                    recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //                    recordSet.DoQuery(query);
            //                    if (!recordSet.EoF)
            //                    {
            //                        DateTime docDate = DateTime.Now;
            //                        DateTime docDueDate = dataSaidaOS;
            //                        DateTime taxDate = DateTime.Now;

            //                        string cardCode = recordSet.Fields.Item(0).Value.ToString();                                    
            //                        string motorista = recordSet.Fields.Item(1).Value.ToString();
            //                        string itemCode = recordSet.Fields.Item(2).Value.ToString();
            //                        double quantity = Convert.ToDouble(recordSet.Fields.Item(3).Value);
            //                        double unitPrice = Convert.ToDouble(recordSet.Fields.Item(4).Value);
            //                        double pesoBruto = Convert.ToDouble(recordSet.Fields.Item(5).Value);
            //                        double tara = Convert.ToDouble(recordSet.Fields.Item(6).Value);
            //                        string rota = recordSet.Fields.Item(7).Value.ToString();
            //                        string estPlaca = recordSet.Fields.Item(8).Value.ToString();
            //                        string usage = recordSet.Fields.Item(9).Value.ToString();

            //                        int bplID = 3;
            //                        string tpOper = "C-GG";
            //                        string respFat = "Cliente";
            //                        string codTransp = "CLI0001";                                                                        
            //                        string status = "P";
            //                        string situacao = "3";                                    
            //                        string warehouse = "01";

            //                        documents.CardCode = cardCode;
            //                        documents.DocDate = docDate;
            //                        documents.DocDueDate = docDueDate;
            //                        documents.TaxDate = taxDate;
            //                        documents.BPL_IDAssignedToInvoice = bplID;

            //                        documents.UserFields.Fields.Item("U_EstPlaca").Value = estPlaca;                                    
            //                        documents.UserFields.Fields.Item("U_TipoOper").Value = tpOper;
            //                        documents.UserFields.Fields.Item("U_RespFat").Value = respFat;
            //                        documents.UserFields.Fields.Item("U_CodTransp").Value = codTransp;
            //                        documents.UserFields.Fields.Item("U_NPlaca").Value = placaOS;
            //                        documents.UserFields.Fields.Item("U_HoraSaidaOS").Value = horaSaidaOS;
            //                        documents.UserFields.Fields.Item("U_Motorista").Value = motorista;
            //                        documents.UserFields.Fields.Item("U_PesoBruto").Value = pesoBruto;
            //                        documents.UserFields.Fields.Item("U_Tara").Value = tara;
            //                        documents.UserFields.Fields.Item("U_Status").Value = status;
            //                        documents.UserFields.Fields.Item("U_Situacao").Value = situacao;
            //                        documents.UserFields.Fields.Item("U_RotaOS").Value = rota;
            //                        documents.UserFields.Fields.Item("U_DiaSemRot").Value = diaColeta;

            //                        documents.Lines.ItemCode = itemCode;
            //                        documents.Lines.Usage = usage;
            //                        documents.Lines.Quantity = quantity;
            //                        documents.Lines.WarehouseCode = warehouse;
            //                        documents.Lines.UnitPrice = unitPrice;                                    
            //                        documents.Lines.AgreementNo = absID;
            //                        documents.Lines.AgreementRowNumber = agrLineNum;
            //                        documents.Lines.UserFields.Fields.Item("U_UtilTax").Value = usage;

            //                        int result = documents.Add();

            //                        if (result != 0)
            //                        {
            //                            int codigoErro;
            //                            string msgErro;

            //                            Program.oCompanyS.GetLastError(out codigoErro, out msgErro);

            //                            throw new Exception(msgErro);
            //                        }

            //                        osGeradas = true;
            //                    }
            //                }
            //                finally
            //                {
            //                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
            //                    GC.Collect();
            //                }
            //            }
            //        }
            //        finally
            //        {
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(documents);
            //            GC.Collect();
            //        }
            //    }

            //    Program.oCompanyS.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

            //    if (osGeradas)
            //    {
            //        Program.oApplicationS.MessageBox("Ordens de serviço geradas", 1, "OK");

            //        Form.DataSources.UserDataSources.Item("nrPlacaOS").Value = string.Empty;
            //        Form.DataSources.UserDataSources.Item("dtSaidaOS").Value = string.Empty;
            //        Form.DataSources.UserDataSources.Item("hrSaidaOS").Value = string.Empty;

            //        ((Grid)Form.Items.Item("gridContr").Specific).DataTable.Clear();

            //        string queryFiltro = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"" from dummy";

            //        Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(queryFiltro);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (Program.oCompanyS.InTransaction)
            //        Program.oCompanyS.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

            //    Program.oApplicationS.MessageBox("Erro ao gerar ordens de serviço: " + ex.Message, 1, "OK");
            //}
        }

        private static void MontaOS(string placaOS, DateTime dataSaidaOS, string horaSaidaOS, string motorista, string diaColeta,
            int contrato, SAPbobsCOM.Documents documents, SAPbobsCOM.Recordset recordSet)
        {
            DateTime docDate = DateTime.Now;
            DateTime docDueDate = dataSaidaOS;
            DateTime taxDate = DateTime.Now;

            string cardCode = recordSet.Fields.Item(0).Value.ToString();
            //string motorista = recordSet.Fields.Item(1).Value.ToString();
            string itemCode = recordSet.Fields.Item(2).Value.ToString();
            double quantity = Convert.ToDouble(recordSet.Fields.Item(3).Value);
            double unitPrice = Convert.ToDouble(recordSet.Fields.Item(4).Value);
            double pesoBruto = Convert.ToDouble(recordSet.Fields.Item(5).Value);
            double tara = Convert.ToDouble(recordSet.Fields.Item(6).Value);
            string rota = recordSet.Fields.Item(7).Value.ToString();
            string estPlaca = recordSet.Fields.Item(8).Value.ToString();
            string usage = recordSet.Fields.Item(9).Value.ToString();
            int AgrLineNum = Convert.ToInt32(recordSet.Fields.Item(10).Value.ToString());

            string IR_ISS = recordSet.Fields.Item(12).Value.ToString();

            int bplID = 1;
            string tpOper = "C-GG";
            string respFat = "Cliente";
            string codTransp = "FOR00002";
            string status = "P";
            string situacao = "3";
            string warehouse = "01";

            documents.CardCode = cardCode;
            documents.DocDate = docDate;
            documents.DocDueDate = docDueDate;
            documents.TaxDate = taxDate;
            documents.BPL_IDAssignedToInvoice = bplID;
            documents.TaxExtension.Carrier = codTransp;
            documents.ShipToCode = recordSet.Fields.Item("U_EnderEntrega").Value.ToString();
            documents.PayToCode = recordSet.Fields.Item("U_EnderAPag").Value.ToString();

            if (recordSet.Fields.Item("salesPrson").Value.ToString() != "0")
            {
                documents.SalesPersonCode = int.Parse(recordSet.Fields.Item("salesPrson").Value.ToString());
            }

            documents.UserFields.Fields.Item("U_TipoFat").Value = "1";

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

            documents.UserFields.Fields.Item("U_IR_ISS").Value = IR_ISS;

            documents.Lines.ItemCode = itemCode;
            documents.Lines.Usage = usage;
            documents.Lines.Quantity = quantity;
            documents.Lines.WarehouseCode = warehouse;
            documents.Lines.UnitPrice = unitPrice;
            documents.Lines.AgreementNo = contrato;
            documents.Lines.AgreementRowNumber = AgrLineNum;
            documents.Lines.TaxCode = "ISS_5";
            documents.Lines.UserFields.Fields.Item("U_UtilTax").Value = usage;

            documents.Lines.Add();
        }

        private static SAPbobsCOM.Recordset ConsultaContratoGeracao(string placaOS, Grid gridContratos, int absID, string utilizacaoResiduo, int row)
        {
            string agrLineNums = (((EditTextColumn)gridContratos.Columns.Item("AgrLineNum")).GetText(row));
            string query = string.Format(@"select distinct OOAT.""BpCode"",                                                                  
                                                                      OOAT.""U_Motorista"",
                                                                      OAT1.""ItemCode"",
                                                                      OAT1.""PlanQty"",
                                                                      OAT1.""UnitPrice"",
                                                                      0,
                                                                      COALESCE(""@VEICULOS_DADOS"".""U_Tara"", 0),                                                                  
                                                                      OOAT.""U_Rota"",
                                                                      COALESCE(""@VEICULOS"".""U_UFPlaca"", ''),
                                                                      (	
                                                                            select case when t33.""ItmsGrpCod"" = 118 then {3}
		                                                                                else coalesce(OUSG.ID,0)
                                                                                    end
	                                                                        from 
		                                                                        OITM 
		                                                                        left join OUSG on OITM.""U_Utilizacao"" =OUSG.""Usage""
                                                                            where
                                                                                OITM.""ItemCode"" = OAT1.""ItemCode""
                                                                        )
                                                                        ,OAT1.""AgrLineNum""
                                                                        ,OOAT.""Number""
                                                                        ,OOAT.""U_IR_ISS""
                                                                        ,OOAT.""U_EnderEntrega""
                                                                        ,OOAT.""U_EnderAPag""
                                                                        ,OHEM.""salesPrson""
                                                                from OOAT
                                                                inner join OAT1 on OAT1.""AgrNo"" = OOAT.""AbsID""        

                                                                inner join OITM T22 on T22.""ItemCode""=OAT1.""ItemCode""   
                                                                inner join OITB t33 on t33.""ItmsGrpCod"" = T22.""ItmsGrpCod"" and t33.""ItmsGrpCod"" IN (118)


                                                                left join ""@VEICULOS"" ON ""@VEICULOS"".""DocEntry"" = (select MAX(TX.""DocEntry"") from ""@VEICULOS"" TX WHERE TX.""U_Placa"" = '{2}')
                                                                left join ""@VEICULOS_DADOS"" ON ""@VEICULOS_DADOS"".""Code"" = ""@VEICULOS"".""Code""
                                                                left join OHEM on OHEM.""Code"" = OOAT.""Owner""
                                                                where OOAT.""AbsID"" = {0}
                                                                and OAT1.""AgrLineNum"" in ({1})
                                                                ", absID, agrLineNums, placaOS, utilizacaoResiduo);

            SAPbobsCOM.Recordset recordSet = null;

            recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recordSet.DoQuery(query);

            return recordSet;
        }

        private static void MostrarOSGeradas(string sOSsGeradas, string sEsbocos, List<ErroGerOS> ErroGerOSs)
        {
            SAPbouiCOM.Form oForm;
            SAPbouiCOM.Item oItem = null;
            SAPbouiCOM.Button oButton = null;
            SAPbouiCOM.Matrix oMatrix;
            SAPbouiCOM.Matrix oMatrix2;

            SAPbouiCOM.Column oColumn;
            DataTable dtMatrix1;
            DataTable dtMatrix2;


            SAPbouiCOM.FormCreationParams oCreationParams = null;
            oCreationParams = ((SAPbouiCOM.FormCreationParams)(Program.oApplicationS.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)));
            oCreationParams.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed;
            oCreationParams.UniqueID = "frmOSs";
            try
            {
                oForm = Program.oApplicationS.Forms.AddEx(oCreationParams);
            }
            catch (Exception)
            {

                Program.oApplicationS.Forms.Item("frmOSs").Close();
                oForm = Program.oApplicationS.Forms.AddEx(oCreationParams);

            }


            //setar as propriedades do form
            oForm.Title = "OS´s Geradas";
            //posição inicial na tela
            oForm.Left = 400;
            oForm.Top = 100;

            //tamanho inicial
            oForm.ClientHeight = 380;
            oForm.ClientWidth = 700;






            oItem = oForm.Items.Add("matrix1", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
            oItem.Height = Convert.ToInt32(300);
            oItem.Width = Convert.ToInt32(360);
            oItem.Top = 10;

            oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;
            oMatrix.Layout = SAPbouiCOM.BoMatrixLayoutType.mlt_Normal;
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_None;

            dtMatrix1 = oForm.DataSources.DataTables.Add("DT_01");

            if (string.IsNullOrEmpty(sOSsGeradas))
            {
                sOSsGeradas = "0";
            }
            if (string.IsNullOrEmpty(sEsbocos))
            {
                sEsbocos = "0";
            }
            string select = string.Format(@" 
                                                    select 
	                                                    T0.""DocEntry"" as ""OS""
                                                        ,0 as ""Esboco""
                                                        , T0.""DocNum""
                                                        , T0.""CardCode""
                                                        , T0.""CardName""
                                                    from
                                                        ORDR T0
                                                    where
                                                        T0.""DocEntry"" in ({0})
                                                    union
                                                    select 
	                                                     0 as ""OS""
                                                        ,T0.""DocEntry"" as ""Esboco""
                                                        , T0.""DocNum""
                                                        , T0.""CardCode""
                                                        , T0.""CardName""
                                                    from
                                                        ODRF T0
                                                    where
                                                        T0.""DocEntry"" in ({1})
                                            ", sOSsGeradas, sEsbocos);

            dtMatrix1.ExecuteQuery(select);

            int iCountCol = 0;
            oColumn = oMatrix.Columns.Add('C' + iCountCol.ToString(), SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.Editable = false;
            oColumn.TitleObject.Caption = "#";
            iCountCol++;

            oMatrix.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
            oMatrix.Columns.Item(iCountCol).DataBind.Bind("DT_01", "OS");
            oMatrix.Columns.Item(iCountCol).TitleObject.Caption = "OS";
            oMatrix.Columns.Item(iCountCol).TitleObject.Sortable = false;
            oMatrix.Columns.Item(iCountCol).Width = 250;
            oMatrix.Columns.Item(iCountCol).Editable = false;
            ((LinkedButton)oMatrix.Columns.Item(iCountCol).ExtendedObject).LinkedObject = BoLinkedObject.lf_Order;
            iCountCol++;

            oMatrix.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
            oMatrix.Columns.Item(iCountCol).DataBind.Bind("DT_01", "Esboco");
            oMatrix.Columns.Item(iCountCol).TitleObject.Caption = "Esboço";
            oMatrix.Columns.Item(iCountCol).TitleObject.Sortable = false;
            oMatrix.Columns.Item(iCountCol).Width = 250;
            oMatrix.Columns.Item(iCountCol).Editable = false;
            ((LinkedButton)oMatrix.Columns.Item(iCountCol).ExtendedObject).LinkedObject = (BoLinkedObject)112;
            iCountCol++;



            oMatrix.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
            oMatrix.Columns.Item(iCountCol).DataBind.Bind("DT_01", "DocNum");
            oMatrix.Columns.Item(iCountCol).TitleObject.Caption = "Nº Doc";
            oMatrix.Columns.Item(iCountCol).TitleObject.Sortable = false;
            oMatrix.Columns.Item(iCountCol).Width = 150;
            oMatrix.Columns.Item(iCountCol).Editable = false;
            iCountCol++;

            oMatrix.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
            oMatrix.Columns.Item(iCountCol).DataBind.Bind("DT_01", "CardCode");
            oMatrix.Columns.Item(iCountCol).TitleObject.Caption = "CardCode";
            oMatrix.Columns.Item(iCountCol).TitleObject.Sortable = false;
            oMatrix.Columns.Item(iCountCol).Width = 150;
            oMatrix.Columns.Item(iCountCol).Editable = false;
            iCountCol++;

            oMatrix.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
            oMatrix.Columns.Item(iCountCol).DataBind.Bind("DT_01", "CardName");
            oMatrix.Columns.Item(iCountCol).TitleObject.Caption = "CardName";
            oMatrix.Columns.Item(iCountCol).TitleObject.Sortable = false;
            oMatrix.Columns.Item(iCountCol).Width = 150;
            oMatrix.Columns.Item(iCountCol).Editable = false;
            iCountCol++;

            oMatrix.AutoResizeColumns();
            oMatrix.LoadFromDataSource();

            oItem = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Left = 0;
            oItem.Width = 65;
            oItem.Top = oMatrix.Item.Height + 10;
            oItem.Height = 19;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "Ok";

            oItem = oForm.Items.Add("QTD", SAPbouiCOM.BoFormItemTypes.it_STATIC);
            oItem.Left = 0;
            oItem.Width = 150;
            oItem.Top = oMatrix.Item.Height + 10;
            oItem.Height = 19;
            ((SAPbouiCOM.StaticText)(oItem.Specific)).Caption = string.Format("Total de OS Geradas: {0}", dtMatrix1.Rows.Count);

            oItem = oForm.Items.Add("matrix2", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
            oItem.Height = Convert.ToInt32(300);
            oItem.Width = Convert.ToInt32(330);
            oItem.Top = 10;
            oItem.Left = 350;

            oMatrix2 = (SAPbouiCOM.Matrix)oItem.Specific;
            oMatrix2.Layout = SAPbouiCOM.BoMatrixLayoutType.mlt_Normal;
            oMatrix2.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_None;

            dtMatrix2 = oForm.DataSources.DataTables.Add("DT_02");
            select = string.Empty;
            if (ErroGerOSs.Count() > 0)
            {
                foreach (ErroGerOS erroGerOS in ErroGerOSs)
                {
                    if (!string.IsNullOrEmpty(select))
                    {
                        select = select + " union all ";
                    }
                    select = select + string.Format(@" 
                                                        select 
	                                                        {0} ""Number""
                                                            , '{1}' as ""MSG""
                                                        from
                                                            dummy
                                                    
                                                        ", erroGerOS.absID, erroGerOS.Erro.Replace("'", "''"));
                }
                select = select + "order by 1;";
                dtMatrix2.ExecuteQuery(select);

                iCountCol = 0;
                oColumn = oMatrix2.Columns.Add('C' + iCountCol.ToString(), SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oColumn.Editable = false;
                oColumn.TitleObject.Caption = "#";
                iCountCol++;

                oMatrix2.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_LINKED_BUTTON);
                oMatrix2.Columns.Item(iCountCol).DataBind.Bind("DT_02", "Number");
                oMatrix2.Columns.Item(iCountCol).TitleObject.Caption = "Number";
                oMatrix2.Columns.Item(iCountCol).TitleObject.Sortable = false;
                oMatrix2.Columns.Item(iCountCol).Width = 250;
                oMatrix2.Columns.Item(iCountCol).Editable = false;
                //((LinkedButton)oMatrix2.Columns.Item(iCountCol).ExtendedObject).LinkedObject = BoLinkedObject.lf_ContractTemplete;
                iCountCol++;



                oMatrix2.Columns.Add('C' + iCountCol.ToString(), BoFormItemTypes.it_EDIT);
                oMatrix2.Columns.Item(iCountCol).DataBind.Bind("DT_02", "MSG");
                oMatrix2.Columns.Item(iCountCol).TitleObject.Caption = "MSG";
                oMatrix2.Columns.Item(iCountCol).TitleObject.Sortable = false;
                oMatrix2.Columns.Item(iCountCol).Width = 150;
                oMatrix2.Columns.Item(iCountCol).Editable = false;
                iCountCol++;

                oMatrix2.AutoResizeColumns();
                oMatrix2.LoadFromDataSource();
            }
            oForm.Visible = true;
        }

        private Dictionary<string, string> NotasVencidas(int absID)
        {
            Dictionary<string, string> notasVencidas = new Dictionary<string, string>();
            string queryVerificacao = string.Format(@"select ""DocNum"", ""CardName"" from OINV 
                                                      where ""CardCode"" = (select ""BpCode"" from OOAT where OOAT.""AbsID"" = {0})
                                                      and ""DocStatus"" = 'O'
                                                      and exists (select * from INV6 
                                                                  where INV6.""DocEntry"" = OINV.""DocEntry"" 
                                                                  and DAYS_BETWEEN(INV6.""DueDate"", current_date) > 15)", absID);

            SAPbobsCOM.Recordset recordSetVerificacao = null;
            try
            {
                recordSetVerificacao = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recordSetVerificacao.DoQuery(queryVerificacao);
                while (!recordSetVerificacao.EoF)
                {
                    notasVencidas.Add(recordSetVerificacao.Fields.Item(0).Value.ToString(), recordSetVerificacao.Fields.Item(1).Value.ToString());

                    recordSetVerificacao.MoveNext();
                }
            }
            finally
            {
                Program.LimparObjeto(recordSetVerificacao);
            }
            return notasVencidas;
        }

        private void GerarFatura()
        {
            string tipoFaturamento = Form.DataSources.UserDataSources.Item("tpFatOS").Value;

            string respFaturamento = ((ComboBox)Form.Items.Item("cbRespFat").Specific).Selected.Value;

            bool faturamentoAgrupado = Form.DataSources.UserDataSources.Item("fatAgrp").Value == "Y";

            if (tipoFaturamento == "")
            {
                Program.oApplicationS.StatusBar.SetText("Selecionar o tipo de faturamento", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                return;
            }

            Program.oApplicationS.StatusBar.SetText("Gerando faturas", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

            if (!Program.oCompanyS.InTransaction)
                Program.oCompanyS.StartTransaction();
            try
            {
                List<Model.FaturaModel> faturaList = new List<Model.FaturaModel>();

                Grid gridOS = (Grid)Form.Items.Item("gridOS").Specific;

                for (int row = 0; row < gridOS.Rows.Count; row++)
                {
                    if (((CheckBoxColumn)gridOS.Columns.Item("#")).IsChecked(row))
                    {
                        int docEntry = Convert.ToInt32(((EditTextColumn)gridOS.Columns.Item("Nº Interno")).GetText(row));

                        string query = string.Format(@"select case '{1}' when '2' then ORDR.""U_CodTransp"" when '3' then ORDR.""U_CodTransp"" else ORDR.""CardCode"" end as ""CardCode"",
                                                              ORDR.""U_TipoOper"",
                                                              RDR1.""ItemCode"",
                                                              RDR1.""Quantity"" - coalesce((select sum(INV1.""Quantity"") from INV1 inner join OINV on OINV.""DocEntry"" = INV1.""DocEntry"" where OINV.""CANCELED"" = 'N' and INV1.""U_OSEntry"" = RDR1.""DocEntry"" AND INV1.""U_OSLine"" = RDR1.""LineNum""), 0) as ""Quantity"",
                                                              RDR1.""Price"",
                                                              OITM.""ItmsGrpCod"",
                                                              OITM.""ManBtchNum"",
                                                              RDR1.""LineNum"",
                                                              coalesce(ORDR.""U_IR_ISS"",'') ISS,
                                                              RDR1.""Usage"",
                                                              RDR1.""TaxCode"",
                                                              ORDR.""GroupNum"",
                                                              case OCRD.""U_TipoFat"" when '1' then (select NFN1.""SeqCode"" from NFN1 where NFN1.""SeqName"" like 'NFSe_v1')
                                                              						  when '2' then case OITM.""ItmsGrpCod"" when 102 then (select NFN1.""SeqCode"" from NFN1 where NFN1.""SeqName"" like 'FAT') 
                                                              															     when 118 then (select NFN1.""SeqCode"" from NFN1 where NFN1.""SeqName"" like 'NFSe_v1')  end 
                                                              end as ""SeqCode"",
                                                              ORDR.""U_Status"" as ""Status""
                                                         from ORDR
                                                         inner join RDR1 on RDR1.""DocEntry"" = ORDR.""DocEntry""
                                                         inner join OITM on OITM.""ItemCode"" = RDR1.""ItemCode""   
                                                         inner join OCRD on OCRD.""CardCode"" = ORDR.""CardCode""
                                                          left JOIN OOAT T2 ON RDR1.""AgrNo"" = T2.""AbsID""
                                                         where ORDR.""DocEntry"" = {0}                                                            
                                                           and OITM.""ItmsGrpCod""in (102, 118)
                                                            
                                                            ", docEntry, respFaturamento);

                        SAPbobsCOM.Recordset recordSet = null;
                        try
                        {
                            recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            recordSet.DoQuery(query);
                            while (!recordSet.EoF)
                            {
                                if (string.IsNullOrEmpty(recordSet.Fields.Item(1).Value.ToString()))
                                {
                                    LogHelper.InfoError("Tipo de Operação Não Definido!!");
                                }
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
                                faturaModel.ISS = recordSet.Fields.Item(8).Value.ToString();
                                faturaModel.Usage = recordSet.Fields.Item(9).Value.ToString();
                                faturaModel.TaxCode = recordSet.Fields.Item(10).Value.ToString();
                                faturaModel.Draft = ((CheckBoxColumn)gridOS.Columns.Item("Esboço")).IsChecked(row);
                                faturaModel.GroupNum = recordSet.Fields.Item(11).Value.ToString();
                                faturaModel.SeqCode = Convert.ToInt32(recordSet.Fields.Item("SeqCode").Value);
                                faturaModel.StatusFat = recordSet.Fields.Item("Status").Value.ToString();

                                if (faturaModel.Quantity <= 0)
                                {
                                    Program.oApplicationS.StatusBar.SetText("OS não possuí saldo para faturar.", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);

                                    return;
                                }

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

                var faturaGroupList = faturaList.GroupBy(r => new { Valor1 = r.BaseEntry.ToString(), Valor2 = r.SeqCode }).ToList();

                if (faturamentoAgrupado)
                {
                    faturaGroupList = faturaList.GroupBy(r => new { Valor1 = r.CardCode.ToString(), Valor2 = r.SeqCode }).ToList();
                }

                List<int> notasTransporteCliente = new List<int>();
                Dictionary<int, int> notasTransporteTransportadora = new Dictionary<int, int>();

                //List<int>[] notasGeradas = new List<int>[2];
                //Dictionary<int, int> notasGeradas = new Dictionary<int, int>();
                List<NotaGerada> notasGeradas = new List<NotaGerada>();

                if (faturaGroupList.Count() == 0)
                {
                    Program.oApplicationS.StatusBar.SetText("Nada para faturar. Vefifique os grupos de itens.", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);
                    return;
                }

                foreach (var faturaGroup in faturaGroupList)
                {
                    //if (((CheckBoxColumn)gridOS.Columns.Item("Esboço")).IsChecked(row))
                    //{

                    //}
                    SAPbobsCOM.Documents documentNFSE;
                    SAPbobsCOM.Documents documentFAT;
                    if (faturaGroup.First().Draft)
                    {
                        documentNFSE = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                        documentFAT = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                        documentNFSE.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices;
                        documentFAT.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices;
                    }
                    else
                    {
                        documentNFSE = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                        documentFAT = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                    }

                    try
                    {
                        int erro = 0;

                        documentNFSE.CardCode = faturaGroup.First().CardCode;
                        documentNFSE.DocDate = DateTime.Now;
                        documentNFSE.TaxDate = DateTime.Now;
                        //documentNFSE.DocDueDate = DateTime.Now;
                        documentNFSE.BPL_IDAssignedToInvoice = 1;
                        documentNFSE.GroupNumber = Convert.ToInt32(faturaGroup.First().GroupNum);
                        documentNFSE.UserFields.Fields.Item("U_Status").Value = faturaGroup.First().StatusFat;

                        foreach (Model.FaturaModel faturaModel in faturaGroup)
                        {
                            if (documentNFSE.Lines.ItemCode != string.Empty)
                            {
                                documentNFSE.Lines.Add();
                                documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                            }

                            documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                            documentNFSE.Lines.Quantity = faturaModel.Quantity;
                            documentNFSE.Lines.UnitPrice = faturaModel.Price;
                            documentNFSE.Lines.Usage = faturaModel.Usage;
                            documentNFSE.Lines.TaxCode = faturaModel.TaxCode;
                            documentNFSE.Lines.WarehouseCode = "01";

                            if (respFaturamento == "1")
                            {
                                documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                                documentNFSE.Lines.BaseType = 17;
                                documentNFSE.Lines.BaseLine = faturaModel.LineNum;
                            }

                            documentNFSE.Lines.UserFields.Fields.Item("U_OSEntry").Value = faturaModel.BaseEntry;
                            documentNFSE.Lines.UserFields.Fields.Item("U_OSLine").Value = faturaModel.LineNum;

                            ImpostoRetido(documentNFSE, faturaModel);

                        }

                        documentNFSE.SequenceCode = faturaGroup.First().SeqCode;

                        erro = documentNFSE.Add();

                        if (erro != 0)
                        {
                            string msg = "";

                            Program.oCompanyS.GetLastError(out erro, out msg);

                            throw new Exception(erro + " - " + msg);
                        }

                        if (Program.oCompanyS.GetNewObjectType() == "112")
                        {
                            notasGeradas.Add(new NotaGerada() { NF = "0", Esboco = Program.oCompanyS.GetNewObjectKey() });
                        }
                        else
                        {
                            notasGeradas.Add(new NotaGerada() { NF = Program.oCompanyS.GetNewObjectKey(), Esboco = "0" });
                        }


                        ////switch (faturaGroup.Key.Valor2)
                        ////{
                        //if (faturaGroup.Key.Valor2.Equals("C-GG") 
                        //    | faturaGroup.Key.Valor2.Equals("C-PJG")
                        //    | faturaGroup.Key.Valor2.Equals("C-PF")
                        //    ) {
                        //    documentNFSE.CardCode = faturaGroup.First().CardCode;
                        //    documentNFSE.DocDate = DateTime.Now;
                        //    documentNFSE.TaxDate = DateTime.Now;
                        //    //documentNFSE.DocDueDate = DateTime.Now;
                        //    documentNFSE.BPL_IDAssignedToInvoice = 1;
                        //    documentNFSE.GroupNumber = Convert.ToInt32(faturaGroup.First().GroupNum);

                        //    foreach (Model.FaturaModel faturaModel in faturaGroup)
                        //    {
                        //        if (documentNFSE.Lines.ItemCode != string.Empty)
                        //        {
                        //            documentNFSE.Lines.Add();
                        //            documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                        //        }

                        //        documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                        //        documentNFSE.Lines.Quantity = faturaModel.Quantity;
                        //        documentNFSE.Lines.UnitPrice = faturaModel.Price;
                        //        documentNFSE.Lines.Usage = faturaModel.Usage;
                        //        documentNFSE.Lines.TaxCode = faturaModel.TaxCode;
                        //        documentNFSE.Lines.WarehouseCode = "01";

                        //        if (respFaturamento == "1")
                        //        {
                        //            documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                        //            documentNFSE.Lines.BaseType = 17;
                        //            documentNFSE.Lines.BaseLine = faturaModel.LineNum;
                        //        }

                        //        documentNFSE.Lines.UserFields.Fields.Item("U_OSEntry").Value = faturaModel.BaseEntry;
                        //        documentNFSE.Lines.UserFields.Fields.Item("U_OSLine").Value = faturaModel.LineNum;

                        //        ImpostoRetido(documentNFSE, faturaModel);

                        //    }

                        //   documentNFSE.SequenceCode = faturaGroup.First().SeqCode;

                        //    erro = documentNFSE.Add();

                        //    if (erro != 0)
                        //    {
                        //        string msg = "";

                        //        Program.oCompanyS.GetLastError(out erro, out msg);

                        //        throw new Exception(erro + " - " + msg);
                        //    }

                        //    if (Program.oCompanyS.GetNewObjectType() == "112")
                        //    {
                        //        notasGeradas.Add(new NotaGerada() { NF = "0", Esboco = Program.oCompanyS.GetNewObjectKey() });
                        //    }
                        //    else 
                        //    {
                        //        notasGeradas.Add(new NotaGerada() { NF = Program.oCompanyS.GetNewObjectKey(), Esboco = "0" });
                        //    }


                        //    //break;
                        //}
                        //else if (faturaGroup.Key.Valor2.Equals("C-TRT")){

                        //    documentNFSE.CardCode = faturaGroup.First().CardCode;
                        //    documentNFSE.DocDate = DateTime.Now;
                        //    documentNFSE.TaxDate = DateTime.Now;
                        //    //documentNFSE.DocDueDate = DateTime.Now.AddDays(1);
                        //    documentNFSE.BPL_IDAssignedToInvoice = 1;
                        //    documentNFSE.GroupNumber = Convert.ToInt32(faturaGroup.First().GroupNum);

                        //    documentNFSE.SequenceCode = 35;

                        //    if (tipoFaturamento == "0")
                        //    {
                        //        foreach (Model.FaturaModel faturaModel in faturaGroup)
                        //        {
                        //            if (documentNFSE.Lines.ItemCode != string.Empty)
                        //            {
                        //                documentNFSE.Lines.Add();
                        //                documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                        //            }

                        //            documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                        //            documentNFSE.Lines.Quantity = faturaModel.Quantity;
                        //            documentNFSE.Lines.UnitPrice = faturaModel.Price;                                   
                        //            documentNFSE.Lines.Usage = faturaModel.Usage;
                        //            documentNFSE.Lines.TaxCode = faturaModel.TaxCode;
                        //            documentNFSE.Lines.WarehouseCode = "01";

                        //            if (respFaturamento == "1")
                        //            {
                        //                documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                        //                documentNFSE.Lines.BaseType = 17;
                        //                documentNFSE.Lines.BaseLine = faturaModel.LineNum;
                        //            }

                        //            documentNFSE.Lines.UserFields.Fields.Item("U_OSEntry").Value = faturaModel.BaseEntry;
                        //            documentNFSE.Lines.UserFields.Fields.Item("U_OSLine").Value = faturaModel.LineNum;

                        //            ImpostoRetido(documentNFSE, faturaModel);

                        //            notasTransporteCliente.Add(faturaModel.BaseEntry);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        foreach (Model.FaturaModel faturaModel in faturaGroup)
                        //        {
                        //            if (documentNFSE.Lines.ItemCode != string.Empty)
                        //            {
                        //                documentNFSE.Lines.Add();
                        //                documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                        //            }

                        //            documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                        //            documentNFSE.Lines.Usage = faturaModel.Usage;
                        //            documentNFSE.Lines.TaxCode = faturaModel.TaxCode;
                        //            documentNFSE.Lines.Quantity = faturaModel.Quantity;
                        //            documentNFSE.Lines.UnitPrice = faturaModel.Price;
                        //            documentNFSE.Lines.WarehouseCode = "01";

                        //            if (respFaturamento == "1")
                        //            {
                        //                documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                        //                documentNFSE.Lines.BaseType = 17;
                        //                documentNFSE.Lines.BaseLine = faturaModel.LineNum;
                        //            }

                        //            documentNFSE.Lines.UserFields.Fields.Item("U_OSEntry").Value = faturaModel.BaseEntry;
                        //            documentNFSE.Lines.UserFields.Fields.Item("U_OSLine").Value = faturaModel.LineNum;

                        //            ImpostoRetido(documentNFSE, faturaModel);
                        //        }

                        //        documentNFSE.Comments = string.Join(",", faturaGroup.ToList().Select(r => r.BaseEntry).ToArray());
                        //    }

                        //    erro = documentNFSE.Add();

                        //    if (erro != 0)
                        //    {
                        //        string msg = "";

                        //        Program.oCompanyS.GetLastError(out erro, out msg);

                        //        throw new Exception(erro + " - " + msg);
                        //    }

                        //    if (tipoFaturamento == "1")
                        //    {
                        //        int nota = Convert.ToInt32(Program.oCompanyS.GetNewObjectKey());

                        //        foreach (Model.FaturaModel faturaModel in faturaGroup)
                        //        {
                        //            notasTransporteTransportadora.Add(faturaModel.BaseEntry, nota);
                        //        }
                        //    }

                        //    if (Program.oCompanyS.GetNewObjectType() == "112")
                        //    {
                        //        notasGeradas.Add(new NotaGerada() { NF = "0", Esboco = Program.oCompanyS.GetNewObjectKey() });
                        //    }
                        //    else
                        //    {
                        //        notasGeradas.Add(new NotaGerada() { NF = Program.oCompanyS.GetNewObjectKey(), Esboco = "0" });
                        //    }

                        //    //break;
                        //}
                        //else if (faturaGroup.Key.Valor2.Equals("LOC")) {
                        //    documentNFSE.CardCode = faturaGroup.First().CardCode;
                        //    documentNFSE.DocDate = DateTime.Now;
                        //    documentNFSE.TaxDate = DateTime.Now;
                        //    //documentNFSE.DocDueDate = DateTime.Now.AddDays(1);
                        //    documentNFSE.BPL_IDAssignedToInvoice = 1;
                        //    documentNFSE.GroupNumber = Convert.ToInt32(faturaGroup.First().GroupNum);                            

                        //    bool gerarNFSE = false;


                        //    foreach (Model.FaturaModel faturaModel in faturaGroup)
                        //    {
                        //        if (faturaModel.ItmsGrpCod == "118")
                        //        {
                        //            gerarNFSE = true;

                        //            if (documentNFSE.Lines.ItemCode != string.Empty)
                        //            {
                        //                documentNFSE.Lines.Add();
                        //                documentNFSE.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                        //            }

                        //            documentNFSE.Lines.ItemCode = faturaModel.ItemCode;
                        //            documentNFSE.Lines.Quantity = faturaModel.Quantity;
                        //            documentNFSE.Lines.UnitPrice = faturaModel.Price;
                        //            documentNFSE.Lines.Usage = faturaModel.Usage;
                        //            documentNFSE.Lines.TaxCode = faturaModel.TaxCode;
                        //            documentNFSE.Lines.WarehouseCode = "01";

                        //            if (respFaturamento == "1")
                        //            {
                        //                documentNFSE.Lines.BaseEntry = faturaModel.BaseEntry;
                        //                documentNFSE.Lines.BaseType = 17;
                        //                documentNFSE.Lines.BaseLine = faturaModel.LineNum;
                        //            }

                        //            documentNFSE.Lines.UserFields.Fields.Item("U_OSEntry").Value = faturaModel.BaseEntry;
                        //            documentNFSE.Lines.UserFields.Fields.Item("U_OSLine").Value = faturaModel.LineNum;

                        //            ImpostoRetido(documentNFSE, faturaModel);

                        //            if (faturaModel.ManBtchNum)
                        //            {
                        //                SAPbobsCOM.Documents oDocumentsRef = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                        //                try
                        //                {
                        //                    for (int x = 0; x < oDocumentsRef.Lines.BatchNumbers.Count; x++)
                        //                    {
                        //                        if (x > 0)
                        //                            documentNFSE.Lines.BatchNumbers.Add();

                        //                        oDocumentsRef.Lines.BatchNumbers.SetCurrentLine(x);

                        //                        documentNFSE.Lines.BatchNumbers.BatchNumber = oDocumentsRef.Lines.BatchNumbers.BatchNumber;
                        //                        documentNFSE.Lines.BatchNumbers.Quantity = oDocumentsRef.Lines.BatchNumbers.Quantity;
                        //                    }
                        //                }
                        //                finally
                        //                {
                        //                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentsRef);
                        //                    GC.Collect();
                        //                }
                        //            }
                        //        }
                        //    }

                        //    documentNFSE.SequenceCode = 35;

                        //    if (gerarNFSE)
                        //    {
                        //        erro = documentNFSE.Add();

                        //        if (erro != 0)
                        //        {
                        //            string msg = "";

                        //            Program.oCompanyS.GetLastError(out erro, out msg);

                        //            throw new Exception(erro + " - " + msg);
                        //        }

                        //        if (Program.oCompanyS.GetNewObjectType() == "112")
                        //        {
                        //            notasGeradas.Add(new NotaGerada() { NF = "0", Esboco = Program.oCompanyS.GetNewObjectKey() });
                        //        }
                        //        else
                        //        {
                        //            notasGeradas.Add(new NotaGerada() { NF = Program.oCompanyS.GetNewObjectKey(), Esboco = "0" });
                        //        }
                        //    }

                        //    bool gerarFAT = false;

                        //    documentFAT.CardCode = faturaGroup.First().CardCode;
                        //    documentFAT.DocDate = DateTime.Now;
                        //    documentFAT.TaxDate = DateTime.Now;
                        //    //documentFAT.DocDueDate = DateTime.Now.AddDays(1);
                        //    documentFAT.BPL_IDAssignedToInvoice = 1;
                        //    documentFAT.GroupNumber = Convert.ToInt32(faturaGroup.First().GroupNum);                            

                        //    foreach (Model.FaturaModel faturaModel in faturaGroup)
                        //    {
                        //        if (faturaModel.ItmsGrpCod == "102")
                        //        {
                        //            gerarFAT = true;

                        //            if (documentFAT.Lines.ItemCode != string.Empty)
                        //            {
                        //                documentFAT.Lines.Add();
                        //                documentFAT.Lines.SetCurrentLine(documentNFSE.Lines.Count - 1);
                        //            }

                        //            documentFAT.Lines.ItemCode = faturaModel.ItemCode;
                        //            documentFAT.Lines.Quantity = faturaModel.Quantity;
                        //            documentFAT.Lines.UnitPrice = faturaModel.Price;
                        //            documentFAT.Lines.Usage = faturaModel.Usage;
                        //            documentFAT.Lines.TaxCode = faturaModel.TaxCode;
                        //            documentFAT.Lines.WarehouseCode = "01";

                        //            if (respFaturamento == "1")
                        //            {
                        //                documentFAT.Lines.BaseEntry = faturaModel.BaseEntry;
                        //                documentFAT.Lines.BaseType = 17;
                        //                documentFAT.Lines.BaseLine = faturaModel.LineNum;
                        //            }

                        //            documentFAT.Lines.UserFields.Fields.Item("U_OSEntry").Value = faturaModel.BaseEntry;
                        //            documentFAT.Lines.UserFields.Fields.Item("U_OSLine").Value = faturaModel.LineNum;

                        //            ImpostoRetido(documentFAT, faturaModel);
                        //        }
                        //    }

                        //    documentFAT.SequenceCode = 29;

                        //    if (gerarFAT)
                        //    {
                        //        erro = documentFAT.Add();

                        //        if (erro != 0)
                        //        {
                        //            string msg = "";

                        //            Program.oCompanyS.GetLastError(out erro, out msg);

                        //            throw new Exception(erro + " - " + msg);
                        //        }

                        //        if (Program.oCompanyS.GetNewObjectType() == "112")
                        //        {
                        //            notasGeradas.Add(new NotaGerada() { NF = "0", Esboco = Program.oCompanyS.GetNewObjectKey() });
                        //        }
                        //        else
                        //        {
                        //            notasGeradas.Add(new NotaGerada() { NF = Program.oCompanyS.GetNewObjectKey(), Esboco = "0" });
                        //        }
                        //    }


                        //    if (!gerarNFSE && !gerarFAT)
                        //        throw new Exception("Nenhum item na OS para gerar fatura de locação.");
                        //    //break;

                        //}
                        //else
                        //{
                        //    Program.oApplicationS.StatusBar.SetText("Tipo de Operação inválida: " + faturaGroup.Key.Valor2, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                        //}
                        // }
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(documentNFSE);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(documentFAT);
                        GC.Collect();
                    }
                }

                Program.oCompanyS.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                //if (notasTransporteCliente.Count() > 0)
                //{
                //    string update = string.Format(@"UPDATE ORDR 
                //                                    SET ""U_Status"" = 'F', 
                //                                    ""U_Situacao"" = 11 
                //                                    where ""U_DocEntry"" IN ({0})", string.Join(",", notasTransporteCliente.ToArray()));

                //}

                //if (notasTransporteTransportadora.Count() > 0)
                //{
                //    string update = string.Format(@"UPDATE ORDR 
                //                                    SET ""U_Status"" = 'F', 
                //                                    ""U_Situacao"" = 11 
                //                                    where ""U_DocEntry"" IN ({0})", string.Join(",", notasTransporteTransportadora.Select(r => r.Key).ToArray()));

                //    foreach (KeyValuePair<int, int> nota in notasTransporteTransportadora)
                //    {
                //        update = string.Format(@"UPDATE RDR1 
                //                             SET ""U_NumFat"" = {0}
                //                             where ""U_DocEntry"" = {1}", nota.Key, nota.Value);
                //    }
                //}

                foreach (Model.NotaGerada notaGerada in notasGeradas)
                {
                    string query = @"select COALESCE(INV1.""U_OSEntry"", 0)                                       
                                from INV1
                                where ""DocEntry"" = {0}
                                and COALESCE(INV1.""U_OSEntry"", 0) > 0
                                and (select count(*) from RDR1 where RDR1.""DocEntry""= COALESCE(INV1.""U_OSEntry"", 0)) = 
                                    (
                                        select count(*) from RDR1 
                                        where RDR1.""DocEntry""= COALESCE(INV1.""U_OSEntry"", 0) 
                                        and RDR1.""Quantity"" = (   select sum(TX.""Quantity"") 
                                                                    from INV1 TX 
                                                                    inner join OINV on OINV.""DocEntry"" = TX.""DocEntry"" 
                                                                    where OINV.""CANCELED"" = 'N' 
                                                                    and TX.""U_OSEntry"" = RDR1.""DocEntry"" 
                                                                    and TX.""U_OSLine"" = RDR1.""LineNum""
                                                                )
                                    )";

                    query = string.Format(query, notaGerada.NF);

                    SAPbobsCOM.Recordset recordSet = null;
                    try
                    {
                        recordSet = (SAPbobsCOM.Recordset)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        recordSet.DoQuery(query);
                        while (!recordSet.EoF)
                        {
                            int osEntry = Convert.ToInt32(recordSet.Fields.Item(0).Value);

                            SAPbobsCOM.Documents oDocuments = (SAPbobsCOM.Documents)Program.oCompanyS.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                            if (oDocuments.GetByKey(osEntry))
                            {
                                oDocuments.UserFields.Fields.Item("U_Status").Value = "F";
                                oDocuments.UserFields.Fields.Item("U_Situacao").Value = "11";

                                oDocuments.Update();

                                if (oDocuments.DocumentStatus == SAPbobsCOM.BoStatus.bost_Open)
                                    oDocuments.Close();
                            }

                            recordSet.MoveNext();
                        }
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                        GC.Collect();
                    }
                }

                Program.oApplicationS.StatusBar.SetText("Faturas geradas", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

                Program.OpenNotasGeradasView(notasGeradas);

                ((Grid)Form.Items.Item("gridOS").Specific).DataTable.Clear();

                string queryFiltro = @"select cast('' as varchar(254)) as ""CodCliente"", cast('' as varchar(254)) as ""NomeCliente"", cast(null as date) as ""DataCtrIni"", cast(null as date) as ""DataCtrFim"", cast('' as varchar(254)) as ""NrContrato"", cast('' as varchar(254)) as ""ModeloCtr"", cast('' as varchar(254)) as ""CentroCusto"", cast('' as varchar(254)) as ""NrRota"", 0 as ""DiaColeta"", 0 as ""UtlRes"", 0 as ""UtlLoc"", cast('' as varchar(254)) as ""Motorista"", cast('' as varchar(254)) as ""NomeMotorista"", cast('' as varchar(254)) as ""NrPlaca"", cast(null as date) as ""DataOSIni"", cast(null as date) as ""DataOSFim"", cast('' as varchar(254)) as ""NrOS"", cast('' as varchar(254)) as ""TpOper"", 0 as ""RespFatura"", cast('' as varchar(254)) as ""SitOS"", cast('' as varchar(254)) as ""StaOS"", cast('' as varchar(254)) as ""UsuResp"", cast('' as varchar(254)) as ""CodTransp"" from dummy";

                Form.DataSources.DataTables.Item("dtFiltro").ExecuteQuery(queryFiltro);
            }
            catch (Exception ex)
            {
                if (Program.oCompanyS.InTransaction)
                    Program.oCompanyS.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                Program.oApplicationS.StatusBar.SetText("Erro ao gerar faturas: " + ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
            }
        }

        private static void ImpostoRetido(SAPbobsCOM.Documents documentNFSE, FaturaModel faturaModel)
        {
            if (!String.IsNullOrEmpty(faturaModel.ISS))
            {
                documentNFSE.Lines.WithholdingTaxLines.WTCode = faturaModel.ISS;
                documentNFSE.Lines.WithholdingTaxLines.Add();
            }
            if (!String.IsNullOrEmpty(faturaModel.PIS))
            {
                documentNFSE.Lines.WithholdingTaxLines.WTCode = faturaModel.PIS;
                documentNFSE.Lines.WithholdingTaxLines.Add();
            }
            if (!String.IsNullOrEmpty(faturaModel.COFINS))
            {
                documentNFSE.Lines.WithholdingTaxLines.WTCode = faturaModel.COFINS;
                documentNFSE.Lines.WithholdingTaxLines.Add();
            }
            if (!String.IsNullOrEmpty(faturaModel.CSLL))
            {
                documentNFSE.Lines.WithholdingTaxLines.WTCode = faturaModel.CSLL;
                documentNFSE.Lines.WithholdingTaxLines.Add();
            }
            if (!String.IsNullOrEmpty(faturaModel.IRPJ))
            {
                documentNFSE.Lines.WithholdingTaxLines.WTCode = faturaModel.IRPJ;
                documentNFSE.Lines.WithholdingTaxLines.Add();
            }
            if (!String.IsNullOrEmpty(faturaModel.INSS))
            {
                documentNFSE.Lines.WithholdingTaxLines.WTCode = faturaModel.INSS;
                documentNFSE.Lines.WithholdingTaxLines.Add();
            }
        }
    }
}
