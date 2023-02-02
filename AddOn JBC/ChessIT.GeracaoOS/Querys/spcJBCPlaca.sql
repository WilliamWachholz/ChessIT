set schema "CUTOVER_0211";
	
DROP PROCEDURE "spcJBCPlaca";

CREATE PROCEDURE "spcJBCPlaca"
(
	IN Placa nvarchar(5000)
)
--WITH ENCRYPTION 
	as BEGIN
		SELECT 
		T0."U_Placa" "Placa"
		,T0."U_UFPlaca" "UFPlaca"
		,'MOTORIOSTA' AS "MOTORISTA"
		,1 TARA
	FROM 
		"@VEICULOS" T0
	WHERE
		T0."U_Placa" = Placa;
END;

CALL "spcJBCPlaca"( 'AKA-0129');


		                                                        SELECT 
		                                                        T0."U_Placa" "Placa"
                                                                , T0."U_UFPlaca" "UFPlaca"
                                                                , 'MOTORIOSTA' AS "MOTORISTA"
                                                                , 1 TARA

                                                            FROM

                                                                "@VEICULOS" T0

                                                            WHERE

                                                                T0."U_Placa" = 'ASF-8660'
                            
