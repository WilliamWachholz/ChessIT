select DISTINCT
	'N' SEL
	,T0."CardCode"
	,T0."CardName"
	,T4."Carrier"
	,T5."CardName" "CardNameT"
	
	,T3."DocEntry" OS
	,T3."DocNum" DocNumOS
	,T3."DocDate" DocDateOS
		
	
	,T0."DocEntry" NFS
	,T0."DocNum" as "DocNumNFS"
	,T0."Serial" as "NNF"
	,T3."U_JBC_CERTI" as "CertificadoOS"
	,T3."U_JBC_DTCERTI" as "DataCertificadoOS"
	
	
	,T0."DocDate" DocDateNFS
	,T0."DocDueDate"  DocDueDateNFS
	,T0."TaxDate" TaxDateNFS
	,T0."U_JBC_CERTI" as "CertificadoNFS"

	,T3."DocEntry" OS
	,T3."DocNum" DocNumOS
	,T3."DocDate" DocDateOS
	,T3."DocDueDate" DocDueDateOS
	,T3."TaxDate" TaxDateOS
	,T3."U_JBC_CERTI" as "CertificadoOS"

	,T2."AgrNo"
	,T7."Descript"
from
	OINV T0
	inner join INV1 T1 on T1."DocEntry"=T0."DocEntry"
	inner join RDR1 T2 on T2."DocEntry"=T1."BaseEntry" and T1."BaseType"=T2."ObjType"
	inner join ORDR T3 on T3."DocEntry"=T2."DocEntry" 	
	inner join RDR12 T4 on T4."DocEntry"=T3."DocEntry"
	left join OCRD T5 on T5."CardCode"=T4."Carrier"
	left join OOAT T7 on T7."AbsID"=T2."AgrNo"
where
		(
			('{0}'='')
			or
			('{0}'=T0."U_JBC_CERTI" or '{0}'=T3."U_JBC_CERTI")
		)
	
		
		and
		(
			( T0."CardCode"='{1}' )
			or
			('{1}'='' )
		)
	
	
		and
		(
			(T0."Serial" = {2})
			or
			({2}=0 )
		)
		
		and
		(
			(T3."DocNum" = {3})
			or
			({3}=0 )
		)				
;