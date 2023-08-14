using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SAPbouiCOM;

namespace ChessIT.KuricaUtils.View
{
    class RecusaView
    {
        Form Form;

        bool Loaded;

        public static List<Model.AprovacaoModel> m_RecusaList = new List<Model.AprovacaoModel>();

        public delegate void PosRecusaEventHandler();

        public event PosRecusaEventHandler PosRecusaEvent;

        public RecusaView(Form form, List<Model.AprovacaoModel> recusaList, PosRecusaEventHandler posRecusaEvent)
        {
            this.Form = form;

            this.PosRecusaEvent = posRecusaEvent;

            m_RecusaList = recusaList;

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
                                    new Controller.AprovacaoController().Recusar(m_RecusaList, ((EditText)Form.Items.Item("etMotivo").Specific).String);

                                    PosRecusaEvent();

                                    Form.Close();
                                }
                            }
                        }
                        break;
                }
            }

        }
    }
}
