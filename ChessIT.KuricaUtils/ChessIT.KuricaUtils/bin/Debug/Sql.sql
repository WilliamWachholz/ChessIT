 select  'N' AS "#",
                                            OWDD."WddCode",
                                            ODRF."DocEntry" AS "Nº Interno",
		                                    ODRF."DocEntry" AS "Nº Doc",
		                                    ODRF."DocDate" AS "Data",
		                                    ODRF."CardName" AS "Fornecedor",
		                                    ODRF."Comments" AS "Observação",
		                                    ODRF."DocTotal" AS "Valor Total",
		                                    --case when (ODRF."DocStatus" = 'C' AND OWDD."Status" = 'Y') then 'Aprovado' 
			                                --     when (ODRF."DocStatus" = 'O' AND OWDD."Status" = 'N') then 'Recusado'
			                                --     else 'Pendente'
		                                    --end as "Decisão/Situação",
                                            case when (OWDD."Status" = 'Y') then 'Aprovado' 
			                                     when (OWDD."Status" = 'N') then 'Recusado'
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
                                    inner join ODRF on ODRF."DocEntry" = OWDD."DraftEntry" AND OWDD."ObjType" = 22
                                    inner join OBPL on OBPL."BPLId" = ODRF."BPLId"
                                    where ODRF."ObjType" = 22
                                    and ((cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODRF."DocDate" as date) >= cast('1990-01-01' as date)))
                                    and ((cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODRF."DocDate" as date) <= cast('1990-01-01' as date)))                                    
                                    and ('' = '' or '' = OBPL."BPLName")
                                    and ('' = '' or '' = ODRF."CardCode")
                                    --and ('Y' = 'Y' or ('N' = 'Y' and (ODRF."DocStatus" = 'C' AND OWDD."Status" = 'Y')) or ('N' = 'Y' and (ODRF."DocStatus" = 'O' AND OWDD."Status" = 'W')))
                                    and('Y' = 'Y' or('N' = 'Y' and (OWDD."Status" = 'Y')) or('N' = 'Y' and(OWDD."Status" = 'W')))