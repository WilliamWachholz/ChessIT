using Castle.Core.Logging;
//using Chess.IT.Services.Model;
using JBC.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.IT.Services.services
{
    class JBCKURICAService
    {
        private BusinessOneDAO B1DAO { get; set; }
        private ILogger Log { get; set; }
        private SAPbobsCOM.Company oCompany { get; set; }

        public JBCKURICAService(BusinessOneDAO pB1DAO, ILogger pLog, SAPbobsCOM.Company poCompany)
        {
            B1DAO = pB1DAO;
            Log = pLog;
            oCompany = poCompany;
        }
        public string PrimeiroDiaDoMes()
        {
            string i = B1DAO.ExecuteSqlForObject<string>(string.Format(this.GetSQL("PrimeiroDiaDoMes.sql"))).ToString();
            return i;
        }

        public string UltimoDiaDoMes()
        {
            string i = B1DAO.ExecuteSqlForObject<string>(string.Format(this.GetSQL("UltimoDiaDoMes.sql"))).ToString();
            return i;
        }
        public string UserName()
        {
            var sql = string.Format(this.GetSQL("UserName.sql"), this.oCompany.UserName);
            return B1DAO.ExecuteSqlForObject<string>(sql);
        }
        public string CertificadoNovoNumero()
        {
            var sql = string.Format(this.GetSQL("CertificadoNovoNumero.sql"), this.oCompany.UserName);
            return B1DAO.ExecuteSqlForObject<string>(sql);
        }
        public string CertificadoConsulta(
            string sCertificado,
            string sPNIni,
            string sNFSIni,
            string sOSIni
            )
        {
            if (string.IsNullOrEmpty(sNFSIni))
            {
                sNFSIni = "0";
            }

            if (string.IsNullOrEmpty(sOSIni))
            {
                sOSIni = "0";
            }

            var sql = string.Format(this.GetSQL("CertificadoConsulta.sql")
                , sCertificado,  sPNIni,  sNFSIni,  sOSIni);
            //return B1DAO.ExecuteSqlForObject<string>(sql);
            return sql;
        }
        public string DataToSql(string dataSAP)
        {
            DateTime dt = B1DAO.ExecuteSqlForObject<DateTime>(string.Format(this.GetSQL("DataToSql.sql"), dataSAP));
            string i = "'" + dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";
            return i;
        }
        public bool UserCancelaCertificado()
        {
            var sql = string.Format(this.GetSQL("UserCancelaCertificado.sql"), this.oCompany.UserName);
            int iResult= B1DAO.ExecuteSqlForObject<int>(sql);
            return iResult == 1;
        }


        public void AtualizaCertificadoOS(string pCertificado, string pDocEntry)
        {
            var sql = string.Format(this.GetSQL("AtualizaCertificadoOS.sql"), pCertificado, pDocEntry);
            B1DAO.ExecuteStatement(sql);
        }
    }
}
