
	
DROP PROCEDURE "spcJBCPlaca";

CREATE PROCEDURE "spcJBCPlaca"
(
	IN Placa nvarchar(5000)
)
--WITH ENCRYPTION 
	as BEGIN
		SELECT 
		T0."U_Placa"
		,T0."U_UFPlaca"
		,'MOTORIOSTA' AS "MOTORISTA"
		,1 TARA
	FROM 
		"@VEICULOS" T0
	WHERE
		T0."U_Placa" = Placa;
END;

CALL "spcJBCPlaca"( 'AKA-0129');