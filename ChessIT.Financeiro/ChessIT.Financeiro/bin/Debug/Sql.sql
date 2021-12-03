select '{8}' AS "Check",
                                    OPCH."BPLName" AS "Filial",
		                            'NE' AS "Tipo Doc",
		                            OPCH."DocEntry" AS "Nº SAP",
		                            OPCH."Serial" AS "Nº NF",
		                            OPCH."CardName" AS "Fornecedor",
                                    OPCH."DocDueDate" AS "Data Vcto.",
		                            cast(PCH6."InstlmntID" as nvarchar(max)) + '/' + cast((select count(*) from PCH6 aux where aux."DocEntry" = OPCH."DocEntry") as nvarchar(max)) AS "Parcela",
		                            PCH6."InsTotal" AS "Valor Parcela",
		                            0.0 as "Valor Desc.",
		                            0.0 as "Valor Multa",
		                            0.0 as "Valor Juros",
		                            PCH6."InsTotal" - PCH6."PaidToDate" AS "Total a Pagar",
		                            OPCH."PeyMethod" AS "Forma Pgto."
                            from OPCH
                            inner join PCH6 on PCH6."DocEntry" = OPCH."DocEntry"                                                        
                            where "DocStatus" = 'O'
                            and PCH6."PaidToDate" < PCH6."InsTotal"
                            and ('{0}' = '' or OPCH."Serial" = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDate" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDate" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDueDate" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDueDate" as date) <= cast('{4}' as date))                            
                            and ({5} = 0 or {5} = OPCH."BPLId")
                            and ('{6}' = '' or '{6}' = OPCH."CardCode")
                            and ('{7}' = '' or '{7}' = OPCH."PeyMethod")'Y' and not exists (select * from VPM2 where VPM2."DocEntry" = OPCH."DocEntry" AND VPM2."InvType" = 18)))
                            union
                            select  '{8}' AS "Check",
                                    ODPO."BPLName" AS "Filial",
		                            'ADT' AS "Tipo Doc",
		                            ODPO."DocEntry" AS "Nº SAP",
		                            '' AS "Nº NF",
		                            ODPO."CardName" AS "Fornecedor",
                                    ODPO."DocDueDate" AS "Data Vcto.",
		                            cast(DPO6."InstlmntID" as nvarchar(max)) + '/' + cast((select count(*) from DPO6 aux where aux."DocEntry" = ODPO."DocEntry") as nvarchar(max)) AS "Parcela",
		                            DPO6."InsTotal" AS "Valor Parcela",
		                            0.0 as "Valor Desc.",
		                            0.0 as "Valor Multa",
		                            0.0 as "Valor Juros",
		                            DPO6."InsTotal" - DPO6."PaidToDate" AS "Total a Pagar",
		                            ODPO."PeyMethod" AS "Forma Pgto."
                            from ODPO
                            inner join DPO6 on DPO6."DocEntry" = ODPO."DocEntry"                            
                            where "DocStatus" = 'O'
                            and DPO6."PaidToDate" < DPO6."InsTotal"
                            and ('{0}' = '')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDate" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDate" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDueDate" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDueDate" as date) <= cast('{4}' as date))                            
                            and ({5} = 0 or {5} = ODPO."BPLId")
                            and ('{6}' = '' or '{6}' = ODPO."CardCode")
                            and ('{7}' = '' or '{7}' = ODPO."PeyMethod")
                            union
                            select  '{14}' AS "Check",
                                    JDT1."BPLName" AS "Filial",
		                            'LC' AS "Tipo Doc",
		                            OJDT."TransId" AS "Nº SAP",
		                            '' AS "Nº NF",
		                            JDT1."ShortName" AS "Fornecedor",
                                    OJDT."DueDate" AS "Data Vcto.",
		                            '1/1' as "Parcela",
		                            JDT1."Credit" AS "Valor Parcela",
		                            0.0 as "Valor Desc.",
		                            0.0 as "Valor Multa",
		                            0.0 as "Valor Juros",
		                            JDT1."BalDueCred" AS "Total A Pagar",
		                            '' AS "Forma Pgto."
                            from JDT1
                            inner join OJDT on OJDT."TransId" = JDT1."TransId"
                            where "BalDueCred" > 0
                            and "MthDate" is null
                            and OJDT."TransType" <> 18
                            and OJDT."StornoToTr" IS NULL
                            and not exists (select * from OJDT aux where aux."StornoToTr" = OJDT."TransId")
                            and ('{0}' = '' or '' = '{0}')
                            and (cast('{1}' as date) = cast('1990-01-01' as date) or cast(OJDT."RefDate" as date) >= cast('{1}' as date))
                            and (cast('{2}' as date) = cast('1990-01-01' as date) or cast(OJDT."RefDate" as date) <= cast('{2}' as date))
                            and (cast('{3}' as date) = cast('1990-01-01' as date) or cast(OJDT."DueDate" as date) >= cast('{3}' as date))
                            and (cast('{4}' as date) = cast('1990-01-01' as date) or cast(OJDT."DueDate" as date) <= cast('{4}' as date))
                            and (cast('{5}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30 and cast(OVPM."DocDate" as date) >= cast('{5}' as date)))
                            and (cast('{6}' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30 and cast(OVPM."DocDate" as date) <= cast('{6}' as date)))                            
                            and ({7} = 0 or {7} >= JDT1."Credit")
                            and ({8} = 0 or {8} <= JDT1."Credit")
                            and ({9} = 0 or {9} = JDT1."BPLId")
                            and ('{10}' = '' or '{10}' = JDT1."ShortName")
                            and ('{11}' = '' or '{11}' = '')
                            and ('{12}' = 'N' or ('{12}' = 'Y' and exists (select * from VPM2 where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30)))
                            and ('{13}' = 'N' or ('{13}' = 'Y' and not exists (select * from VPM2 where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30)))