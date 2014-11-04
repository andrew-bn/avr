/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */
 .listmac
 .include "kernel.inc"
 
.CSEG
	;jmp thread2
	;cli
	START thread2
	START thread3
	;sei
	;jmp thread2
ret

thread2:
	ldi AL, 0xFF
	clr AH
	out DDRA, AL

	lbl:
	
	out PORTA, AL
	WAIT 1000
	out PORTA, AH
	WAIT 1000
		
	rjmp lbl

ret



thread3:
	ldi AL, 0xFF
	clr AH
	out DDRB, AL

	lbl2:
	
	out PORTB, AL
	WAIT 333
	out PORTB, AH
	WAIT 333
		
	rjmp lbl2

ret



