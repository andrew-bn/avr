/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */
 .listmac
 .include "kernel.inc"
 
.CSEG
jmp avr_start
avr_start:

 NEW thread
 XCALLIWW RH:RL, thread_start, thread1, RH, RL

 call kernel_start

thread1:
		ldi AL, 0xFF
		clr AH
		out DDRA, AL

		emptythread_Run_:
			out PORTA, AL
			WAITI 1000 ; 
			out PORTA, AH
			WAITI 1000 ; 
			
		rjmp emptythread_Run_
	ret



