/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */
 .listmac
 .include "kernel.inc"

.CSEG
jmp str
 .include "clock.inc"
 .include "i2c.inc"
 str:
;transmit Start
/*	call i2c_start
	// send clockaddress - write
	xcallib thisH:thisL, i2c_send, 0b11010000 //cpi r16, 0x18
	//send pointer address
	xcallib thisH:thisL, i2c_send, 0x00 //cpi r16, 0x28  
	//send data
	xcallib thisH:thisL, i2c_send, 0x00 //cpi r16, 0x28  

	call i2c_stop*/
;-------------------------------------------
	;start startClock
	NEW clock
	movw CH:CL, RH:RL
	GETW CH:CL, clk_data
	movw DH:DL, RH:RL

	ldi AL, 0xFF
	out DDRA, AL
	out DDRB, AL
	out DDRD, AL
	ldi BL,1
	clr BH


	nextSecond:
	GET DH:DL, clkdata_minutes
	out PORTD, RL
	GET DH:DL, clkdata_seconds
	out PORTB, RL
	
	out PORTA, BL
	

	WAIT 333
	xcall ch:cl, clock_read

	movw AH:AL, RH:RL
	tst RH
	brne tw_error

	jmp nextSecond


ret
	tw_error:
	
	out PORTB, AL
	out PORTD, AH

	WAIT 1000

	clr TL
	out PORTB, TL
	out PORTD, TL
	out PORTA, TL
	WAIT 1000
	jmp tw_error
ret
startClock:
	
ret
startDone:
	ldi AL, 0xFF
	ldi AH, 0xFF
	out DDRA, AL
	out DDRB, AL

	ldi AL, 1
	lbl:
	
	out PORTA, BL
	out PORTB, BH
	WAIT 1000
	out PORTA, AH
	out PORTB, AH
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



