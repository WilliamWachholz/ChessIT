using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIT.Financeiro.Model
{
    class BoletoModel
    {
        public string contaC { get; set; } = "";

        public int numero { get; set; }

        public DateTime vcto { get; set; }

        public string ref1 { get; set; } = "";

        public string formaPagto { get; set; } = "";

        public string status { get; set; } = "";

        public string ref2 { get; set; } = "";

        public string obs { get; set; } = "";

        public string codBarras { get; set; } = "";

        public double valor { get; set; }

        public string pais { get; set; } = "";

        public string banco { get; set; } = "";

        public string conta { get; set; } = "";

        public string filial { get; set; } = "";
    }
}
