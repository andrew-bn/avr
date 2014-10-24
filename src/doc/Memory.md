
Offcet|Name           |Size(bytes)|Description
----------------------------------------------
0     |NextAddress    |2          | Next block address
2     |PrevAddress    |2          | Prev block address. if Prev == ramend, then block is free
4     |Block          |N          |

----------------
Macroses
FIELD_ LD - uses Z (this). no side effects
GETTER - uses GET,exptects P as pointer to obj
GET - @0 - pointer to obj, affects T, returns R

SETB - @0 pointer to obj, affects T

stack:
	Parameters
	RetAddress
	CommonRegisters+AddressingRegisters
	Variables

P - R14-15 - pointer (this or type pointers)
Q - R12-13 - pointer to task list
R - R10-11 - wreturn
S - R8-9 - temp
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



----------
 taskManager_checkAndRun: ; A - pointer to task

		FIELD_WLD list_head, AH, AL
 
 taskManager_checkAndRun_check:
		; if state== wait then decrement task_counter
		GET AH:AL, task_state
		ldi BL, task_Wait
		cp RL, BL
		brne taskManager_checkAndRun_next

		;if task_counter != 0 then decrement it
		GETW AH:AL, task_counter
		tst RH
		sbic SREG, 7 ; if rh not 0 then skip tst
		tst RL
		sbic SREG, 7 ; if rl not 0
		rjmp taskManager_checkAndRun_next

		crl BH
		ldi BL, 1
		sub RL, BL
		sbc RH, BH

		SETW AH:AL, task_counter, RH:RL

taskManager_checkAndRun_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		sbic SREG, 7 ; if rh not 0 then skip tst
		tst RL
		sbic SREG, 7 ; if rl not 0
		rjmp taskManager_checkAndRun_check

		FIELD_WLD list_head, AH, AL; load head again
		rjmp taskManager_checkAndRun_check
		ret