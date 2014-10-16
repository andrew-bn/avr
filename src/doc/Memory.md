
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

P - R14-15 - pointer (this or type pointers)
Q - R12-13
R - R10-11 - wreturn
S - R8-9
T - R6-7 - temp
U - R4-5
V - R2-3
W - R0-1

A - R16, R17
B - R18, R19
C - R20, R21
D - R22, R23
E - R24, R25

X - memory pointer R26, R27
Y - stack pointer R28, R29
Z - this pointer R30, R31

this, new, type - R12, R13
ret  - R14, R15
