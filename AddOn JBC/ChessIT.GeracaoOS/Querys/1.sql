select 
	'N' as "#"
    ,T0."DocEntry" as "Nº Interno"
    ,T0."DocNum" AS "Nº OS"
    ,T0."CardCode" 
    ,T0."CardName" "Cliente"
    ,T0."U_NPlaca" "Placa"
    ,T0."U_VolumeM3" "m3"
    ,T0."U_Tara" "Tara"
    ,T0."U_PesoBruto" "Peso Bruto"
    ,T0."U_PesoLiq" "Peso Liq."
    ,T1."unitMsr"
from
    ORDR T0    
    inner join RDR1 T1 on T1."DocEntry"=T0."DocEntry"
	left join "@VEICULOS" ON "@VEICULOS"."U_Placa" = T0."U_NPlaca"
	left join OCRD TRANSP ON TRANSP."CardCode" = T0."U_CodTransp"
	left join OUSR ON T0."UserSign" = OUSR."USERID"
where 
	T0."CANCELED" = 'N'
	and T0."DocStatus" = 'O'
	and T0."U_Status" = 'P' 
	and T0."U_Situacao" = 3
	
	and ('{0}' = '' or '{0}' = T0."CardCode")
	and ('{1}' = '' or '{1}' = T0."U_NPlaca")
	and ('{2}' = '' or '{2}' = (select max(OOAT."U_Rota") from RDR1 inner join OOAT on OOAT."AbsID" = RDR1."AgrNo" where RDR1."DocEntry" = T0."DocEntry"))
	and ('{3}' = '[Selecionar]' or '{3}' = T0."U_DiaSemRot")
limit 0;


select * from "@PLACA"
select 
    sum(T1."Quantity")
from
    ORDR T0    
    inner join RDR1 T1 on T1."DocEntry"=T0."DocEntry"
where
	T0."DocEntry"=129
	and T1."unitMsr"='Metros Cúbicos'