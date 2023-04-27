select 
	T1."ItemCode"
	,sum(T1."BatchQuantity") "Quantity"
from 
	"ClaudinoPicking" T0
	inner join "ClaudinoPickingItems" T1 on  T0.ID=T1.ID
where
	 T0."DocEntry"={0}
group by 
	T1."ItemCode"