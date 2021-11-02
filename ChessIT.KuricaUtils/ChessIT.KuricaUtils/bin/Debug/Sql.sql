select * from (
                                        select  'N' AS "#",
                                            OWDD."WddCode",
		                                    ODRF."DocEntry" AS "Nº Doc",
		                                    ODRF."DocDate" AS "Data",
		                                    ODRF."CardName" AS "Fornecedor",
		                                    ODRF."Comments" AS "Observação",
		                                    ODRF."DocTotal" AS "Valor Total",
		                                    case when (ODRF."DocStatus" = 'C' AND OWDD."Status" = 'Y') then 'Aprovado' 
			                                     when (ODRF."DocStatus" = 'O' AND OWDD."Status" = 'N') then 'Recusado'
			                                     else 'Pendente'
		                                    end as "Decisão/Situação",
		                                    '' AS "Descrição Item",
		                                    NULL AS "Quantidade",
		                                    NULL AS "Preço Unitário",
		                                    NULL AS "Total Item",
		                                    '' AS "CC",
		                                    '' AS "Contrato",
		                                    '' AS "Placa"
                                    from OWDD
                                    inner join ODRF on ODRF."DocEntry" = OWDD."DocEntry"
                                    inner join OBPL on OBPL."BPLId" = ODRF."BplId"
                                    inner join DRF1 on DRF1."DocEntry" = ODRF."DocEntry"
                                    where ODRF."ObjType" = 22 AND OWDD."ObjType" = 112
                                    and (((cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODRF."DocDate" as date) >= cast('1990-01-01' as date)) and 'Y' = 'Y') or 'Y' = 'N')
                                    and (((cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODRF."DocDate" as date) <= cast('1990-01-01' as date)) and 'Y' = 'Y') or 'Y' = 'N')                                    
                                    and ('' = '' or '' = OBPL."BPLName")
                                    and ('' = '' or '' = ODRF."CardCode")
                                    and ('Y' = 'Y' or ('N' = 'Y' and (ODRF."DocStatus" = 'C' AND OWDD."Status" = 'Y')) or ('N' = 'Y' and (ODRF."DocStatus" = 'O' AND OWDD."Status" = 'W')))
                                    union
                                    select  'N' AS "#",
                                        OWDD."WddCode",
		                                NULL AS "Nº Doc",
		                                NULL AS "Data",
		                                '' AS "Fornecedor",
		                                '' AS "Observação",
		                                NULL AS "Valor Total",
		                                '' as "Decisão/Situação",
		                                DRF1."Dscription" AS "Descrição Item",
		                                DRF1."Quantity" AS "Quantidade",
		                                DRF1."Price" AS "Preço Unitário",
		                                DRF1."LineTotal" AS "Total Item",
		                                DRF1."OcrCode" AS "CC",
		                                DRF1."OcrCode3" AS "Contrato",
		                                DRF1."OcrCode5" AS "Placa"
                                from OWDD
                                inner join ODRF on ODRF."DocEntry" = OWDD."DocEntry"
                                inner join OBPL on OBPL."BPLId" = ODRF."BplId"
                                inner join DRF1 on DRF1."DocEntry" = ODRF."DocEntry"
                                where ODRF."ObjType" = 22 AND OWDD."ObjType" = 112
                                and (((cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODRF."DocDate" as date) >= cast('1990-01-01' as date)) and 'Y' = 'Y') or 'Y' = 'N')
                                and (((cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODRF."DocDate" as date) <= cast('1990-01-01' as date)) and 'Y' = 'Y') or 'Y' = 'N')                                    
                                and ('' = '' or '' = OBPL."BPLName")
                                and ('' = '' or '' = ODRF."CardCode")
                                and ('Y' = 'Y' or ('N' = 'Y' and (ODRF."DocStatus" = 'C' AND OWDD."Status" = 'Y')) or ('N' = 'Y' and (ODRF."DocStatus" = 'O' AND OWDD."Status" = 'W')))
                                ) vt order by "WddCode", "Dscription"