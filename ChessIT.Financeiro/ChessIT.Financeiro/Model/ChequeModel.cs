using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIT.Financeiro.Model
{
    class ChequeModel
    {
        public DateTime vcto { get; set; }

        public double valor { get; set; }

        public string pais { get; set; } = "";

        public string banco { get; set; } = "";

        public string filial { get; set; } = "";

        public string conta { get; set; } = "";

        public bool manual { get; set; }

        public int numero { get; set; }

        public string contaC { get; set; } = "";

        public bool endosso { get; set; }
    }
}
