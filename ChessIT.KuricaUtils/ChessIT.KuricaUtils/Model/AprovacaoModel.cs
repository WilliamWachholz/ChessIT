using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessIT.KuricaUtils.Model
{
    class AprovacaoModel
    {
        public int WddCode { get; set; }

        public int NumeroDoc { get; set; }

        public DateTime Data { get; set; }

        public string Fornecedor { get; set; }

        public double ValorTotal { get; set; }

        public string Resultado { get; set; }
    }
}
