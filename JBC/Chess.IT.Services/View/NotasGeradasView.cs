using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Chess.IT.Services.Model;
using SAPbouiCOM;

namespace Chess.IT.Services.View
{
    class NotasGeradasView
    {
        Form Form;

        //bool Loaded;

        List<NotaGerada> m_Notas =new  List<NotaGerada>();

        public NotasGeradasView(Form form, List<NotaGerada> notas)
        {
            this.Form = form;

            m_Notas = notas;

            Form.EnableMenu("1282", false);
            Form.EnableMenu("1281", false);
            Form.EnableMenu("1283", false);
            Form.EnableMenu("1284", false);
            Form.EnableMenu("1285", false);
            Form.EnableMenu("1286", false);

            m_Timer.Elapsed += M_Timer_Elapsed;
            m_Timer.Start();
        }

        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_Timer.Stop();

            if (m_Notas.Count > 0)
            {
                string query = "";

                //foreach (int nota in m_Notas)
                //{
                //    query += "select " + nota + " as \"Nº Nota\"" + " from dummy union all ";
                //}

                foreach (NotaGerada nota in m_Notas)
                {
                    query += string.Format(@" select {0} as ""Nº Nota"", {1} as ""Nº Esboço"" from dummy union all", nota.NF, nota.Esboco);
                    //update = string.Format(@"UPDATE RDR1 
                    //                         SET ""U_NumFat"" = {0}
                    //                         where ""U_DocEntry"" = {1}", nota.Key, nota.Value);
                }

                if (query != string.Empty)
                {
                    query = query.Substring(0, query.Length - 10);

                    Form.Freeze(true);
                    try
                    {
                        Form.DataSources.DataTables.Item("dtNotas").ExecuteQuery(query);

                        Grid gridOS = (Grid)Form.Items.Item("grNotas").Specific;

                        gridOS.Columns.Item("Nº Nota").Editable = false;

                        ((EditTextColumn)gridOS.Columns.Item("Nº Nota")).LinkedObjectType = "13";


                        gridOS.Columns.Item("Nº Nota").Editable = false;
                        gridOS.Columns.Item("Nº Esboço").Editable = false;

                        ((EditTextColumn)gridOS.Columns.Item("Nº Esboço")).LinkedObjectType = "112";
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

                Program.oApplicationS.StatusBar.SetText("Z" + m_Notas.Count);

            }
        }

        Timer m_Timer = new Timer(1000);

    }
}