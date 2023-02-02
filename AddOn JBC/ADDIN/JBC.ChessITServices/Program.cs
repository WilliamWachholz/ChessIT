using Castle.Core.Logging;
using JBC.Framework.Attribute;
using JBC.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessIT.GeracaoOS.Controller;

namespace JBC.ChessITServices
{
    //[ResourceBOM("JBC.ChessITServices.resources.BOM.UDF.xml", ResourceType.UserField)]
    [AddIn(
        Name = "JBC.ChessITServices"
        , Description = "JBC.ChessITServices - Claudino Soluções em Software"
        , Namespace = "JBC"
        ,InitMethod = "CheckInit"
        )]
    class Program
    {
        public SAPbobsCOM.Company oCompany { get; set; }
        public SAPbouiCOM.Application oApplication { get; set; }
        public BusinessOneDAO oB1DAO { get; set; }
        public ILogger Log { get; set; }

        static void Main(string[] args)
        {

        }
        public void CheckInit()
        {
            MainController mainController = new MainController(oApplication, oCompany);
        }
    }
}
