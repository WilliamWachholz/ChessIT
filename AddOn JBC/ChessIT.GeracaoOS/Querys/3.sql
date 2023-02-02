select sum (T0."U_VolumeM3")
from
    ORDR T0
	inner join RDR1 T1 on T1."DocEntry"=T0."DocEntry"
    left join ""@VEICULOS"" ON ""@VEICULOS"".""U_Placa"" = T0.""U_NPlaca""
    left join OCRD TRANSP ON TRANSP.""CardCode"" = T0.""U_CodTransp""
    left join OUSR ON T0.""UserSign"" = OUSR.""USERID""
    
    
select 
	T1."Quantity" volume 
	,T1."U_Densidade" densidade 
	,T2."ItmsGrpCod"
	,t3."ItmsGrpNam"
from
    ORDR T0
	inner join RDR1 T1 on T1."DocEntry"=T0."DocEntry"    
	inner join OITM T2 on T2."ItemCode"=T1."ItemCode"
	inner join OITB t3 on t3."ItmsGrpCod"=T2."ItmsGrpCod"
where 
	t3."ItmsGrpNam"='Resíduos'