/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */
 .listmac
 .include "kernel.inc"
 
.CSEG

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



