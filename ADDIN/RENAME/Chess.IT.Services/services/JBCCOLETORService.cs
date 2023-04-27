using Castle.Core.Logging;
//using Chess.IT.Services.Model;
using JBC.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.IT.Services.services
{
    class JBCColetorService
    {
        private BusinessOneDAO B1DAO { get; set; }
        private ILogger Log { get; set; }
        private SAPbobsCOM.Company oCompany { get; set; }

        public JBCColetorService(BusinessOneDAO pB1DAO, ILogger pLog, SAPbobsCOM.Company poCompany)
        {
            B1DAO = pB1DAO;
            Log = pLog;
            oCompany = poCompany;
        }
        //public List<PickingItem> GetPickingItems(string pDocEntry)
        //{
        //    List<PickingItem> lstRetorno;
        //    var sql = string.Format(this.GetSQL("PickingItem.sql"), pDocEntry);
        //    lstRetorno = B1DAO.ExecuteSqlForList<PickingItem>(sql).ToList();
        //    return lstRetorno;
        //}

    }
}
