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
 reti

 kernel_start:
	SETUP_SYSTIMER
	sei
	XCALL QH:QL, taskManager_nextTask
 ret

_RESET: ; setup stack
		ldi R16, low(RAMEND)
		out SPL, R16
		ldi R16, high(RAMEND)
		out SPH, R16

		call memory_init

		NEW taskManager
		movw QH:QL, RH:RL
_maintask:
