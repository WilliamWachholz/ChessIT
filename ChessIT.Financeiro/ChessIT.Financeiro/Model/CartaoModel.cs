using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIT.Financeiro.Model
{
    class CartaoModel
    {
        public int nome { get; set; }

        public string contaC { get; set; } = "";

        public double valor { get; set; }

        public int pgtos { get; set; } = 1;

        public double primPagVal { get; set; }

        public DateTime primPagEm { get; set; }

        public double adcVal { get; set; }

        public string comprov { get; set; } = "";

        public bool divComp { get; set; }
    }
}
