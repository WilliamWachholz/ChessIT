select 'N' AS "Check",
                                    --OPCH."BPLName" AS "Filial",
                                    CASE OPCH."BPLId" WHEN 1 THEN 'M' ELSE 'F' END AS "Filial",
		                            'NE' AS "Tipo Doc",
                                    OPCH."BPLId" AS "Nº Empresa",
                                    OPCH."CardCode" AS "Nº Fornecedor",
		                            OPCH."DocEntry" AS "Nº Interno",
                                    OPCH."DocNum" AS "Nº SAP",
		                            OPCH."Serial" AS "Nº NF",
		                            OPCH."CardName" AS "Fornecedor",
                                    PCH6."DueDate" AS "Data Vcto.",
                                    (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") as "Data Baixa",
		                            cast(PCH6."InstlmntID" as nvarchar) || '/' || cast((select count(*) from PCH6 aux where aux."DocEntry" = OPCH."DocEntry") as nvarchar) AS "Parcela",
		                            PCH6."InsTotal" AS "Valor Parcela",
		                            COALESCE((select sum("U_ValorDoDesconto")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N'), 0) as "Valor Desc.",
		                            COALESCE((select sum("U_ValorMulta")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N'), 0) as "Valor Multa",
		                            COALESCE((select sum("U_ValorDoJurosMora")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N'), 0) as "Valor Juros",
		                            PCH6."InsTotal" - PCH6."PaidToDate" AS "Total a Pagar",                                    
                                    PCH6."PaidToDate" +
                                    COALESCE((select sum("U_ValorDoDesconto")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N'), 0) +
		                            COALESCE((select sum("U_ValorMulta")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N'), 0) +
		                            COALESCE((select sum("U_ValorDoJurosMora")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N'), 0) AS "Valor Pago",
                                    PCH6."InsTotal" - PCH6."PaidToDate" AS "Valor Saldo",
                                    (SELECT "AcctName" FROM OACT WHERE "AcctCode" =
                                    (select max(case 
		                                    when "CashSum" > 0 then "CashAcct"
		                                    when "CreditSum" > 0 then (SELECT MAX(VPM3."CreditAcct") FROM VPM3 WHERE VPM3."DocNum" = OVPM."DocEntry")
                                            when "TrsfrSum" > 0 then "TrsfrAcct"
		                                    when "BoeSum" > 0 then "BoeAcc"
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N')) AS "Conta",
                                    (select max(case 
		                                    when "CashSum" > 0 then 'Dinheiro' 
		                                    when "CreditSum" > 0 then 'Cartão' 
		                                    when "TrsfrSum" > 0 then 'Transferência'
		                                    when "BoeSum" > 0 then 'Boleto'
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = OPCH."DocEntry"
                                    and VPM2."InvType" = 18
                                    and VPM2."InstId" = PCH6."InstlmntID"
                                    and "Canceled" = 'N') AS "Carteira"
                            from OPCH
                            inner join PCH6 on PCH6."DocEntry" = OPCH."DocEntry" 
                            inner join JDT1 on JDT1."TransId" = OPCH."TransId" and JDT1."SourceLine" = PCH6."InstlmntID"							                            
                            where OPCH."CANCELED" = 'N'                            
                            and ('' = '' or OPCH."Serial" = '')
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDueDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OPCH."DocDueDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") <= cast('1990-01-01' as date))
                            and (0 = 0 or PCH6."InsTotal" >= 0)
                            and (0 = 0 or PCH6."InsTotal" <= 0)
                            and (0 = 0 or 0 = OPCH."BPLId")
                            and ('' = '' or '' = OPCH."CardCode")
                            and ('' = '' or '' = OPCH."PeyMethod")
                            and ('N' = 'N' or 'N' = 'Y' or ('N' = 'Y' and exists (select OITR."CreateDate" from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID")))
                            and ('N' = 'N' or 'N' = 'Y' or ('N' = 'Y' and not exists (select OITR."CreateDate" from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID")))
                            union
                            select  'N' AS "Check",
                                    CASE ODPO."BPLId" WHEN 1 THEN 'M' ELSE 'F' END AS "Filial",
		                            'ADT' AS "Tipo Doc",
                                    ODPO."BPLId" AS "Nº Empresa",
                                    ODPO."CardCode" AS "Nº Fornecedor",
		                            ODPO."DocEntry" AS "Nº Interno",
                                    ODPO."DocNum" AS "Nº SAP",
		                            0 AS "Nº NF",
		                            ODPO."CardName" AS "Fornecedor",
                                    DPO6."DueDate" AS "Data Vcto.",
                                    (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") as "Data Baixa",
		                            cast(DPO6."InstlmntID" as nvarchar) || '/' || cast((select count(*) from DPO6 aux where aux."DocEntry" = ODPO."DocEntry") as nvarchar) AS "Parcela",
		                            DPO6."InsTotal" AS "Valor Parcela",
                                    COALESCE((select sum("U_ValorDoDesconto")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N'), 0) AS "Valor Desc.",
		                            COALESCE((select sum("U_ValorMulta")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N'), 0) AS "Valor Multa",
		                            COALESCE((select sum("U_ValorDoJurosMora")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N'), 0) as "Valor Juros",
		                            DPO6."InsTotal" - DPO6."PaidToDate" AS "Total a Pagar",                                    
                                    DPO6."PaidToDate" +
                                    COALESCE((select sum("U_ValorDoDesconto")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N'), 0) +
		                            COALESCE((select sum("U_ValorMulta")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N'), 0) +
		                            COALESCE((select sum("U_ValorDoJurosMora")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N'), 0) AS "Valor Pago",
                                    DPO6."InsTotal" - DPO6."PaidToDate" AS "Valor Saldo",
                                    (SELECT "AcctName" FROM OACT WHERE "AcctCode" =
                                    (select max(case 
		                                    when "CashSum" > 0 then "CashAcct"
		                                    when "CreditSum" > 0 then (SELECT MAX(VPM3."CreditAcct") FROM VPM3 WHERE VPM3."DocNum" = OVPM."DocEntry")
                                            when "TrsfrSum" > 0 then "TrsfrAcct"
		                                    when "BoeSum" > 0 then "BoeAcc"
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N')) AS "Conta",
                                     (select max(case 
		                                    when "CashSum" > 0 then 'Dinheiro' 
		                                    when "CreditSum" > 0 then 'Cartão' 
		                                    when "TrsfrSum" > 0 then 'Transferência'
		                                    when "BoeSum" > 0 then 'Boleto'
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = ODPO."DocEntry"
                                    and VPM2."InvType" = 204
                                    and VPM2."InstId" = DPO6."InstlmntID"
                                    and "Canceled" = 'N') AS "Carteira"                                   
                            from ODPO
                            inner join DPO6 on DPO6."DocEntry" = ODPO."DocEntry"
                            inner join JDT1 on JDT1."TransId" = ODPO."TransId" and JDT1."SourceLine" = DPO6."InstlmntID"                            
                            where ODPO."CANCELED" = 'N'
                            and ('' = '')
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDueDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(ODPO."DocDueDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") <= cast('1990-01-01' as date))
                            and (0 = 0 or DPO6."InsTotal" >= 0)
                            and (0 = 0 or DPO6."InsTotal" <= 0)
                            and (0 = 0 or 0 = ODPO."BPLId")
                            and ('' = '' or '' = ODPO."CardCode")
                            and ('' = '' or '' = ODPO."PeyMethod")
                            and ('N' = 'N' or 'N' = 'Y' or ('N' = 'Y' and exists (select OITR."CreateDate" from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID")))
                            and ('N' = 'N' or 'N' = 'Y' or ('N' = 'Y' and not exists (select OITR."CreateDate" from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID")))
                            union
                            select  'N' AS "Check",
                                    CASE JDT1."BPLId" WHEN 1 THEN 'M' ELSE 'F' END AS "Filial",
		                            'LC' AS "Tipo Doc",
		                            JDT1."BPLId" AS "Nº Empresa",
                                    OCRD."CardCode" AS "Nº Fornecedor",
                                    OJDT."TransId" AS "Nº Interno",
                                    OJDT."TransId" AS "Nº SAP",
		                            OJDT."U_NDocFin" AS "Nº NF",
		                            OCRD."CardName" AS "Fornecedor",
                                    JDT1."DueDate" AS "Data Vcto.",
                                    (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") as "Data Baixa",
		                            cast((JDT1."Line_ID" + 1) as varchar(10)) || '/' || cast((select count(*) from JDT1 TX where TX."TransId" = JDT1."TransId" and TX."ShortName" LIKE 'FOR%') as varchar(10)) as "Parcela",
		                            JDT1."Credit" AS "Valor Parcela",
		                            COALESCE((select sum("U_ValorDoDesconto")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = 30
                                    and VPM2."InvType" = 30
                                    and VPM2."InstId" = JDT1."Line_ID" + 1
                                    and "Canceled" = 'N'), 0) AS "Valor Desc.",
		                            COALESCE((select sum("U_ValorMulta")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = JDT1."TransId"
                                    and VPM2."InvType" = 30
                                    and VPM2."InstId" = JDT1."Line_ID" + 1
                                    and "Canceled" = 'N'), 0) AS "Valor Multa",
		                            COALESCE((select sum("U_ValorDoJurosMora")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = JDT1."TransId"
                                    and VPM2."InvType" = 30
                                    and VPM2."InstId" = JDT1."Line_ID" + 1
                                    and "Canceled" = 'N'), 0) AS "Valor Juros",
		                            JDT1."BalDueCred" AS "Total A Pagar",
                                    (JDT1."Credit" - JDT1."BalDueCred") +
                                    COALESCE((select sum("U_ValorDoDesconto")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = 30
                                    and VPM2."InvType" = 30
                                    and VPM2."InstId" = JDT1."Line_ID" + 1
                                    and "Canceled" = 'N'), 0) +
		                            COALESCE((select sum("U_ValorMulta")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = JDT1."TransId"
                                    and VPM2."InvType" = 30
                                    and VPM2."InstId" = JDT1."Line_ID" + 1
                                    and "Canceled" = 'N'), 0) +
		                            COALESCE((select sum("U_ValorDoJurosMora")
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = JDT1."TransId"
                                    and VPM2."InvType" = 30
                                    and VPM2."InstId" = JDT1."Line_ID" + 1
                                    and "Canceled" = 'N'), 0) AS "Valor Pago",
                                    JDT1."BalDueCred" AS "Valor Saldo",
                                    (SELECT "AcctName" FROM OACT WHERE "AcctCode" =
                                    (select max(case 
		                                    when "CashSum" > 0 then "CashAcct"
		                                    when "CreditSum" > 0 then (SELECT MAX(VPM3."CreditAcct") FROM VPM3 WHERE VPM3."DocNum" = OVPM."DocEntry")
                                            when "TrsfrSum" > 0 then "TrsfrAcct"
		                                    when "BoeSum" > 0 then "BoeAcc"
		                                    end)
                                    from VPM2 
                                    left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                    where VPM2."DocEntry" = JDT1."TransId"
                                    and VPM2."InvType" = 30
                                    and VPM2."InstId" = JDT1."Line_ID" + 1
                                    and "Canceled" = 'N')) AS "Conta",
                                    (select max(case 
		                                when "CashSum" > 0 then 'Dinheiro' 
		                                when "CreditSum" > 0 then 'Cartão' 
		                                when "TrsfrSum" > 0 then 'Transferência'
		                                when "BoeSum" > 0 then 'Boleto'
		                                end) 
                                   from VPM2 
                                   left join OVPM on OVPM."DocEntry" = VPM2."DocNum" 
                                   where VPM2."DocEntry" = JDT1."TransId"
                                   and VPM2."InvType" = 30
                                   and VPM2."InstId" = JDT1."Line_ID" + 1
                                   and "Canceled" = 'N') AS "Carteira"
                            from JDT1
                            inner join OJDT on OJDT."TransId" = JDT1."TransId"
                            inner join OCRD on OCRD."CardCode" = JDT1."ShortName"
							--left join OPYM on OPYM."PayMethCod" = OJDT."U_FPagFin"
                            where OJDT."TransType" <> 18
                            and OJDT."TransType" <> 204
                            and JDT1."Credit" > 0
                            --and "MthDate" is null
                            and JDT1."ShortName" LIKE 'FOR%'
                            and OJDT."StornoToTr" IS NULL
                            and not exists (select * from OJDT aux where aux."StornoToTr" = OJDT."TransId")
                            and ('' = '' or OJDT."U_NDocFin" = '')
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."RefDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."RefDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."DueDate" as date) >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or cast(OJDT."DueDate" as date) <= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") >= cast('1990-01-01' as date))
                            and (cast('1990-01-01' as date) = cast('1990-01-01' as date) or (select max(OITR."CreateDate") from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID") <= cast('1990-01-01' as date))
                            and (0 = 0 or JDT1."Credit" >= 0)
                            and (0 = 0 or JDT1."Credit" <= 0)
                            and (0 = 0 or 0 = JDT1."BPLId")
                            and ('' = '' or '' = JDT1."ShortName")
                            and ('' = '' or '' = OJDT."U_FPagFin")
                            and ('N' = 'N' or 'N' = 'Y' or ('N' = 'Y' and exists (select OITR."CreateDate" from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID")))
                            and ('N' = 'N' or 'N' = 'Y' or ('N' = 'Y' and not exists (select OITR."CreateDate" from ITR1 inner join OITR on OITR."ReconNum" = ITR1."ReconNum" where OITR."Canceled" = 'N' and ITR1."TransId" = JDT1."TransId" and ITR1."TransRowId" = JDT1."Line_ID")))