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
    }
}
