select 'N' AS "Check",
                                    OPCH."BPLName" AS "Filial",
		                            'NE' AS "Tipo Doc",
		                            OPCH."DocEntry" AS "Nº SAP",
		                            OPCH."Serial" AS "Nº NF",
		                            OPCH."CardName" AS "Fornecedor",
                                    OPCH."DocDueDate" AS "Data Vcto.",
		                            PCH6."InstlmntID" + '/' + (select count(*) from PCH6 aux where aux."DocEntry" = OPCH."DocEntry") AS "Parcela",
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
                            and ('' = '' or OPCH."Serial" = '')
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDueDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDueDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = OPCH."DocEntry" AND VPM2."InvType" = 18 and cast(OVPM."DocDate" as date) >= cast('1990-01-01' as date)))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = OPCH."DocEntry" AND VPM2."InvType" = 18 and cast(OVPM."DocDate" as date) <= cast('1990-01-01' as date)))
                            and (0 = 0 or 0 >= OPCH."DocTotal")
                            and (0 = 0 or 0 <= OPCH."DocTotal")
                            and (0 = 0 or 0 = OPCH."BPLId")
                            and ('' = '' or '' = OPCH."CardName")
                            and ('' = '' or '' = OPCH."PeyMethod")
                            and ('N' = '' or ('N' = 'Y' and exists (select * from VPM2 where VPM2."DocEntry" = OPCH."DocEntry" AND VPM2."InvType" = 18)))
                            and ('N' = '' or ('N' = 'Y' and not exists (select * from VPM2 where VPM2."DocEntry" = OPCH."DocEntry" AND VPM2."InvType" = 18)))
                            union
                            select  'N' AS "Check",
                                    ODPO."BPLName" AS "Filial",
		                            'ADT' AS "Tipo Doc",
		                            ODPO."DocEntry" AS "Nº SAP",
		                            '' AS "Nº NF",
		                            ODPO."CardName" AS "Fornecedor",
                                    ODPO."DocDueDate" AS "Data Vcto.",
		                            DPO6."InstlmntID" + '/' + (select count(*) from DPO6 aux where aux."DocEntry" = ODPO."DocEntry") AS "Parcela",
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
                            and ('' = '')
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDueDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDueDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = ODPO."DocEntry" AND VPM2."InvType" = 204 and cast(OVPM."DocDate" as date) >= cast('1990-01-01' as date)))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = ODPO."DocEntry" AND VPM2."InvType" = 204 and cast(OVPM."DocDate" as date) <= cast('1990-01-01' as date)))
                            and (0 = 0 or 0 >= ODPO."DocTotal")
                            and (0 = 0 or 0 <= ODPO."DocTotal")
                            and (0 = 0 or 0 = ODPO."BPLId")
                            and ('' = '' or '' = ODPO."CardName")
                            and ('' = '' or '' = ODPO."PeyMethod")
                            and ('N' = '' or ('N' = 'Y' and exists (select * from VPM2 where VPM2."DocEntry" = ODPO."DocEntry" AND VPM2."InvType" = 204)))
                            and ('N' = '' or ('N' = 'Y' and not exists (select * from VPM2 where VPM2."DocEntry" = ODPO."DocEntry" AND VPM2."InvType" = 204)))
                            union
                            select  'N' AS "Check",
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
                            and OJDT."StornoToTr" IS NULL
                            and not exists (select * from OJDT aux where aux."StornoToTr" = OJDT."TransId")
                            and ('' = '' or '' = '')
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."RefDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."RefDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."DueDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."DueDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30 and cast(OVPM."DocDate" as date) >= cast('1990-01-01' as date)))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or exists (select * from VPM2 left join OVPM on OVPM."DocEntry" = VPM2."DocNum" where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30 and cast(OVPM."DocDate" as date) <= cast('1990-01-01' as date)))                            
                            and (0 = 0 or 0 >= JDT1."Credit")
                            and (0 = 0 or 0 <= JDT1."Credit")
                            and (0 = 0 or 0 = JDT1."BPLId")
                            and ('' = '' or '' = JDT1."ShortName")
                            and ('' = '' or '' = '')
                            and ('N' = '' or ('N' = 'Y' and exists (select * from VPM2 where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30)))
                            and ('N' = '' or ('N' = 'Y' and not exists (select * from VPM2 where VPM2."DocEntry" = JDT1."TransId" AND VPM2."InvType" = 30)))