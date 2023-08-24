using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.IT.Services.Model
{
    class FaturaModel
    {
        public string CardCode { get; set; }      

        public int BaseEntry { get; set; }

        public string TpOper { get; set; }

        public string ItemCode { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public string ItmsGrpCod { get; set; }

        public bool ManBtchNum { get; set; }

        public int LineNum { get; set; }
        public string ISS { get; set; }


        public string PIS { get; set; }
        public string COFINS { get; set; }
        public string CSLL { get; set; }
        public string IRPJ { get; set; }
        public string INSS { get; set; }

        public string Usage { get; set; }
        public string TaxCode { get; set; }
        public bool Draft { get; set; }

        public string GroupNum { get; set; }
        
        public int PropostaComercial { get; set; }

        public int SeqCode { get; set; }

        public string RespFat { get; set; }

    }
}
