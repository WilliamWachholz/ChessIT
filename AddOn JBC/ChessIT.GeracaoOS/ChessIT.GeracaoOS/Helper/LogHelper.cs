using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIT.GeracaoOS.Helper
{
    public class LogHelper
    {
        public static void InfoError(string pMesg)
        {
            Controller.MainController.oApplication.StatusBar.SetText(
            pMesg
            , BoMessageTime.bmt_Short
            , BoStatusBarMessageType.smt_Error);
        }
        public static void InfoSuccess(string pMesg)
        {
            Controller.MainController.oApplication.StatusBar.SetText(
            pMesg
            , BoMessageTime.bmt_Short
            , BoStatusBarMessageType.smt_Success);
        }
        public static void InfoWarning(string pMesg)
        {
            Controller.MainController.oApplication.StatusBar.SetText(
            pMesg
            , BoMessageTime.bmt_Short
            , BoStatusBarMessageType.smt_Warning);
        }

        public static void MostraBalanca(string peso, string hora, Form pForm)
        {

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
            Controller.MainController.oApplication.Forms.ActiveForm.Refresh();
        }
    }
}
