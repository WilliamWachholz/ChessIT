using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIT.Financeiro.Model
{
    class TransferenciaModel
    {
        public string contaC { get; set; } = "";

        public DateTime data { get; set; }

        public string ref1 { get; set; } = "";

        public double valor { get; set; }
    }
}
