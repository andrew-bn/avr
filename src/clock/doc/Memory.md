
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
Q - R12-13 - pointer to thread list
R - R10-11 - wreturn
S - R8-9 - temp
T - R6-7 - temp
MStat - R5
U - R4 
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
 threadManager_checkAndRun: ; A - pointer to thread

		FIELD_WLD list_head, AH, AL
 
 threadManager_checkAndRun_check:
		; if state== wait then decrement thread_counter
		GET AH:AL, thread_state
		ldi BL, thread_Wait
		cp RL, BL
		brne threadManager_checkAndRun_next

		;if thread_counter != 0 then decrement it
		GETW AH:AL, thread_counter
		tst RH
		sbic SREG, 7 ; if rh not 0 then skip tst
		tst RL
		sbic SREG, 7 ; if rl not 0
		rjmp threadManager_checkAndRun_next

		crl BH
		ldi BL, 1
		sub RL, BL
		sbc RH, BH

		SETW AH:AL, thread_counter, RH:RL

threadManager_checkAndRun_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		sbic SREG, 7 ; if rh not 0 then skip tst
		tst RL
		sbic SREG, 7 ; if rl not 0
		rjmp threadManager_checkAndRun_check

		FIELD_WLD list_head, AH, AL; load head again
		rjmp threadManager_checkAndRun_check
		ret




"ATmega32" instruction use summary:
.lds  :   0 .sts  :   0 adc   :   2 add   :   2 adiw  :   0 and   :   0 
andi  :   0 asr   :   0 bclr  :   0 bld   :   0 brbc  :   0 brbs  :   0 
brcc  :   1 brcs  :   3 break :   0 breq  :   7 brge  :   0 brhc  :   0 
brhs  :   0 brid  :   0 brie  :   0 brlo  :   0 brlt  :   0 brmi  :   0 
brne  :  26 brpl  :   0 brsh  :   0 brtc  :   0 brts  :   0 brvc  :   0 
brvs  :   0 bset  :   0 bst   :   0 CALL  :  29 cbi   :   0 cbr   :   0 
clc   :   0 clh   :   0 cli   :   8 cln   :   0 CLR   :  17 cls   :   0 
clt   :   0 clv   :   0 clz   :   0 com   :   0 cp    :  18 cpc   :   3 
cpi   :   2 cpse  :   0 DEC   :   9 eor   :   0 fmul  :   0 fmuls :   0 
fmulsu:   0 icall :   4 ijmp  :   1 IN    :  74 inc   :   9 jmp   :   2 
ld    :  18 ldd   :  78 LDI   :  62 lds   :   0 lpm   :   5 lsl   :   0 
lsr   :   2 mov   :  38 MOVW  : 162 mul   :   0 muls  :   0 mulsu :   0 
neg   :   0 nop   :   0 or    :   0 ori   :   0 OUT   :  37 pop   : 306 
PUSH  : 306 rcall :   0 ret   :  30 reti  :  20 RJMP  :  11 rol   :   0 
ror   :   0 sbc   :   2 SBCI  :  30 sbi   :   0 sbic  :   0 sbis  :   0 
sbiw  :   0 sbr   :   1 sbrc  :   8 sbrs  :   0 sec   :   0 seh   :   0 
sei   :   9 sen   :   0 ser   :   0 ses   :   0 set   :   0 sev   :   0 
sez   :   0 sleep :   0 spm   :   0 st    :  20 std   :  58 sts   :   0 
sub   :   2 SUBI  :  30 swap  :   0 tst   :  24 wdr   :   0 