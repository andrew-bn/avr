
Offcet|Name           |Size(bytes)|Description
----------------------------------------------
0     |NextAddress    |2          | Next block address
2     |PrevAddress    |2          | Prev block address. if Prev == ramend, then block is free
4     |Block          |N          |

----------------

stack:
	Parameters
	RetAddress
	CommonRegisters+AddressingRegisters
	Variables