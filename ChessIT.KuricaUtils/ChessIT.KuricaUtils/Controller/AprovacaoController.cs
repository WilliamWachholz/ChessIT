using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace ChessIT.KuricaUtils.Controller
{
    class AprovacaoController
    {
        public void Aprovar(List<Model.AprovacaoModel> aprovacaoList)
        {
            ApprovalRequestsService approvalRequestService = (ApprovalRequestsService) MainController.Company.GetCompanyService().GetBusinessService(ServiceTypes.ApprovalRequestsService);
            
            foreach (Model.AprovacaoModel aprovacaoModel in aprovacaoList)
            {
                try
                {
                    ApprovalRequestParams approvalRequestParams = (ApprovalRequestParams)approvalRequestService.GetDataInterface(ApprovalRequestsServiceDataInterfaces.arsApprovalRequestParams);
                    approvalRequestParams.Code = aprovacaoModel.WddCode;

                    ApprovalRequest approvalRequest = approvalRequestService.GetApprovalRequest(approvalRequestParams);

                    approvalRequest.ApprovalRequestDecisions.Add();
                    approvalRequest.ApprovalRequestDecisions.Item(0).Status = BoApprovalRequestDecisionEnum.ardApproved;
                    approvalRequest.ApprovalRequestDecisions.Item(0).Remarks = "Aprovado";

                    approvalRequestService.UpdateRequest(approvalRequest);

                    aprovacaoModel.Resultado = "Aprovado com sucesso";
                }
                catch (Exception ex)
                {
                    aprovacaoModel.Resultado = "Erro ao aprovar: " + ex.Message;
                }
            }
            
            Controller.MainController.OpenResultadoView(aprovacaoList);
        }

        public void Recusar(List<Model.AprovacaoModel> aprovacaoList)
        {
            ApprovalRequestsService approvalRequestService = (ApprovalRequestsService)MainController.Company.GetCompanyService().GetBusinessService(ServiceTypes.ApprovalRequestsService);

            foreach (Model.AprovacaoModel aprovacaoModel in aprovacaoList)
            {
                try
                {
                    ApprovalRequestParams approvalRequestParams = (ApprovalRequestParams)approvalRequestService.GetDataInterface(ApprovalRequestsServiceDataInterfaces.arsApprovalRequestParams);
                    approvalRequestParams.Code = aprovacaoModel.WddCode;

                    ApprovalRequest approvalRequest = approvalRequestService.GetApprovalRequest(approvalRequestParams);

                    approvalRequest.ApprovalRequestDecisions.Add();
                    approvalRequest.ApprovalRequestDecisions.Item(0).Status = BoApprovalRequestDecisionEnum.ardNotApproved;
                    approvalRequest.ApprovalRequestDecisions.Item(0).Remarks = "Recusado";

                    approvalRequestService.UpdateRequest(approvalRequest);

                    aprovacaoModel.Resultado = "Recusado com sucesso";
                }
                catch (Exception ex)
                {
                    aprovacaoModel.Resultado = "Erro ao recusar: " + ex.Message;
                }
            }

            Controller.MainController.OpenResultadoView(aprovacaoList);
        }
    }
}
