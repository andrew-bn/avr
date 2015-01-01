/*
 * i2c.inc
 *
 *  Created: 03.12.2014 21:48:22
 *   Author: AB
 */ 
   #ifndef _I2C_INC_
 #define _I2C_INC_

 .include "clockData.inc"
 .equ START_TRANSMITTED = 0x08
 i2c_stop:
		mov TL, AL
		ldi AL, (1<<TWINT)|(1<<TWEN)|(1<<TWSTO)
		out TWCR, AL 
		mov AL, TL
	ret
 i2c_send:
	BEGIN_TINY
		; clock write address
		PARAM_LD 0x00, AL
		out TWDR, AL 
		ldi AL, (1<<TWINT) | (1<<TWEN)
		out TWCR, AL

		i2c_send_wait:
		in AL,TWCR
		sbrs AL,TWINT
		rjmp i2c_send_wait

		in AL,TWSR
		andi AL, 0xF8

		mov RL, AL
	END_TINY
 i2c_read_ack:
		mov TL, AL

		ldi AL, (1<<TWINT) | (1<<TWEN) | (1<<TWEA)
		out TWCR, AL

		i2c_read_wait:
		in AL,TWCR
		sbrs AL,TWINT
		rjmp i2c_read_wait

		in AL,TWSR
		andi AL, 0xF8
		mov RL, AL

		in AL, TWDR
		mov RH, AL
		mov AL, TL
	ret
 i2c_read:
		mov TL, AL

		ldi AL, (1<<TWINT) | (1<<TWEN)
		out TWCR, AL

		i2c_read_ack_wait:
		in AL,TWCR
		sbrs AL,TWINT
		rjmp i2c_read_ack_wait

		in AL,TWSR
		andi AL, 0xF8
		mov RL, AL

		in AL, TWDR
		mov RH, AL
		mov AL, TL
	ret

 i2c_start:
		mov TL, AL
		clr RH
		clr RL
		ldi AL,(1<<TWINT)|(1<<TWSTA)|(1<<TWEN)
		out TWCR, AL

		i2c_start_wait:
		in AL, TWCR
		sbrs AL, TWINT
		rjmp i2c_start_wait

		in AL,TWSR
		andi AL, 0xF8
		mov RL, AL

		mov AL, TL
	ret
 .CSEG
 #endif /* _CLOCK_INC_ */
