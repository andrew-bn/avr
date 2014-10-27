/*
 * main.asm
 *
 *  Created: 08.10.2014 21:54:56
 *   Author: AB
 */
 .macro TASK_ADD; task type
	NEW @0
	
	XCLI
	XCALLW QH:QL, list_add, RH, RL
	XSEI
 .endm

.CSEG


; interrupts
.ORG $000  
rjmp _RESET 
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
.ORG OC0addr rjmp dispatcherTimer
;reti	
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

 dispatcherTimer: jmp _taskManager_waitTimeout


 kernel_start:
	.equ MainClock = 1000000
	.equ TimerDivider = MainClock/8/1000

	ldi AL,(1<<CTC0)|(1<<CS01);|(1<<CS00)
	out TCCR0,AL
	clr AL
	out TCNT0,AL

	ldi AL,low(TimerDivider)
	out OCR0,AL		

	ldi AL, 1<<OCIE0 
	out	TIMSK,AL

	ldi AL, 1<<OCF0
	out TIFR, AL


	clr MIStat
	inc MIStat
	sei
	
	XCALL QH:QL, taskManager_checkAndRun
 ret

_RESET: ; setup stack
		ldi R16, low(RAMEND)
		out SPL, R16
		ldi R16, high(RAMEND)
		out SPH, R16

		clr MIStat
		dec MIStat

		;aaa: jmp aaa
		call memory_init

		NEW taskManager
		movw QH:QL, RH:RL
_maintask:
