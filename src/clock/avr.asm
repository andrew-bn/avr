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
	ldi AL, 0xFF
	out DDRA, AL
	out DDRB, AL
	out DDRD, AL
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
	startw startReadClock, CH, CL

	nextSecond:

		xcall ch:cl, clock_read

		movw AH:AL, RH:RL
		tst RH
		brne tw_error
		WAIT 500

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

startReadClock:
	movw CH:CL, PH:PL
	GETW CH:CL, clk_data
	movw DH:DL, RH:RL

	startClock_next:
	
	GET DH:DL, clkdata_hours
	out PORTA, RL
	GET DH:DL, clkdata_minutes
	out PORTB, RL
	GET DH:DL, clkdata_seconds
	out PORTD, RL

	WAIT 500
	rjmp startClock_next

ret

