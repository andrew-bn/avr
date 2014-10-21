/*
 * main.asm
 *
 *  Created: 08.10.2014 21:54:56
 *   Author: AB
 */
 .macro TASK_ADD; task type
	NEW @0
	XCALLW QH:QL, list_add, RH, RL
 .endm
 .macro SETUP_SYSTIMER

	.equ MainClock = 8000000
	.equ TimerDivider = MainClock/64/1000

	ldi AL,1<<CTC2|4<<CS20
	out TCCR2,AL
	clr AL
	out TCNT2,AL

	ldi AL,low(TimerDivider)
	out OCR2,AL		

	ldi AL, 1<<OCF2 
	out	TIMSK,AL

 .endm

.CSEG
; interrupts
.ORG $000  rjmp _RESET 
.ORG INT0addr
reti
.ORG INT1addr
reti
.ORG INT2addr
reti
.ORG OC2addr rjmp dispatcherTimer
.ORG OVF2addr
reti
.ORG ICP1addr
reti
.ORG OC1Aaddr
reti
.ORG OC1Baddr
reti
.ORG OVF1addr
reti
.ORG OC0addr
reti	
.ORG OVF0addr
reti
.ORG SPIaddr
reti	
.ORG URXCaddr
reti
.ORG UDREaddr
reti
.ORG UTXCaddr
reti
.ORG ADCCaddr
reti
.ORG ERDYaddr
reti
.ORG ACIaddr
reti	
.ORG TWIaddr
reti	
.ORG SPMRaddr
reti
.ORG INT_VECTORS_SIZE


	.include "common.inc"
	.include "memory.inc"
	.include "object.inc"
	.include "list.inc"
	.include "listItem.inc"
	.include "task.inc"
	.include "taskManager.inc"
 dispatcherTimer:
		push AL
		push AH
		push EH
		push EL
		in AL, SREG
		push AL

		XCALL QH:QL, list_getHead

	dispatcherTimer_check:
		tst RH
		brne dispatcherTimer_cnt
		tst RL
		breq dispatcherTimer_exit

	dispatcherTimer_cnt:
		movw AH:AL, RH:RL
		XCALL AH:AL, task_getState
		ldi EH, task_Wait
		cp RL, EH
		brne dispatcherTimer_next
		XCALL AH:AL, task_getCounter
		movw EH:EL, RH:RL

		tst EH
		brne dispatcherTimer_dec
		tst EL
		breq dispatcherTimer_next

	dispatcherTimer_dec:
		
		sbiw EH:EL, 1
		XCALLW AH:AL, task_setCounter, EH, EL

	dispatcherTimer_next:
		XCALL AH:AL, listItem_getNext

		rjmp dispatcherTimer_check

	dispatcherTimer_exit:

		pop AL
		out SREG, AL
		pop EL
		pop EH
		pop AH
		pop AL
	reti

_RESET: ; setup stack
		ldi R16, low(RAMEND)
		out SPL, R16
		ldi R16, high(RAMEND)
		out SPH, R16

		call memory_init

		NEW taskManager
		movw QH:QL, RH:RL
_maintask:
