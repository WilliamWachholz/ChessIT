using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.IT.Services.Helper
{
    public class LogHelper
    {
        public static void InfoError(string pMesg)
        {
            Program.oApplicationS.StatusBar.SetText(
            pMesg
            , BoMessageTime.bmt_Short
            , BoStatusBarMessageType.smt_Error);
        }
        public static void InfoSuccess(string pMesg)
        {
            Program.oApplicationS.StatusBar.SetText(
            pMesg
            , BoMessageTime.bmt_Short
            , BoStatusBarMessageType.smt_Success);
        }
        public static void InfoWarning(string pMesg)
        {
            Program.oApplicationS.StatusBar.SetText(
            pMesg
            , BoMessageTime.bmt_Short
            , BoStatusBarMessageType.smt_Warning);
        }

        public static void MostraBalanca(string peso, string hora, Form pForm)
        {

            try
            {
                SAPbouiCOM.Item oItem = null;
                //SAPbouiCOM.StaticText oStaticText = null;

                oItem = pForm.Items.Add("lblBalanca", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 600;
                oItem.Width = 148;
                oItem.Top = 600;
                oItem.Height = 14;
            }
            catch (Exception)
            {

                //throw;
            }

            StaticText lblBalanca = (StaticText)pForm.Items.Item("lblBalanca").Specific;

            int greenColor = Color.Green.R | (Color.Green.G << 8) | (Color.Green.B << 16);
            int yellowColor = Color.YellowGreen.R | (Color.YellowGreen.G << 8) | (Color.YellowGreen.B << 16);
            int BlueColor = Color.Blue.R | (Color.Blue.G << 8) | (Color.Blue.B << 16);


            if (lblBalanca.Item.ForeColor == yellowColor)
            {
                lblBalanca.Item.ForeColor = greenColor;
            }else if (lblBalanca.Item.ForeColor == greenColor)
            {
                lblBalanca.Item.ForeColor = BlueColor;
            }
            else
            {
                lblBalanca.Item.ForeColor = yellowColor;
            }


            lblBalanca.Item.FontSize = 20;
            lblBalanca.Item.TextStyle = 1;

            lblBalanca.Item.Height = 50;

            if (string.IsNullOrEmpty(peso) && string.IsNullOrEmpty(hora))
            {
                //lblBalanca.Caption = "";
            }
            else
            {
                lblBalanca.Caption = string.Format("{0} às {1}", peso, hora);
                //InfoWarning(lblBalanca.Caption);
            }



            lblBalanca.Item.Visible = true;

            //Form.Update();
            Program.oApplicationS.Forms.ActiveForm.Refresh();
        }
    }
}
