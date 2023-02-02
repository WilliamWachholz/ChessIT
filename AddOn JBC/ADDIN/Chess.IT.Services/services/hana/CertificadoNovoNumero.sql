select cast(max(cast(coalesce(case when T0."U_JBC_CERTI"='' then '0' else T0."U_JBC_CERTI" end ,'0') as int))+1 as nvarchar(50))
from
ORDR T0