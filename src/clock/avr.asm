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
	GET DH:DL, clkdata_seconds
	ldi AL, 4
	out PORTD, AL
	mov al,rl
	out porta, al
	andi al, 0x0f

	cpi al, 0
	brne case1
	ldi al, 0x3f out portb, al
	rjmp case_default

	case1: cpi al,1
	brne case2
	ldi al, 0x06 out portb, al
	rjmp case_default

	case2: cpi al,2
	brne case3
	ldi al, 0x5b out portb, al
	rjmp case_default

	case3: cpi al,3
	brne case4
	ldi al, 0x4f out portb, al
	rjmp case_default

	case4: cpi al,4
	brne case5
	ldi al, 0x66 out portb, al
	rjmp case_default

	case5: cpi al,5
	brne case6
	ldi al, 0x6d out portb, al
	rjmp case_default

	case6: cpi al,6
	brne case7
	ldi al, 0x7d out portb, al
	rjmp case_default

	case7: cpi al,7
	brne case8
	ldi al, 0x07 out portb, al
	rjmp case_default

	case8: cpi al,8
	brne case9
	ldi al, 0x7f out portb, al
	rjmp case_default

	case9: ldi al, 0x6f out portb, al
	case_default:
	/*GET DH:DL, clkdata_hours
	out PORTA, RL
	GET DH:DL, clkdata_minutes
	out PORTB, RL*/
	

	WAIT 500
	rjmp startClock_next

ret

