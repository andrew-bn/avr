/*
 * clock.inc
 *
 *  Created: 03.12.2014 21:39:13
 *   Author: AB
 */ 

  #ifndef _CLOCK_INC_
 #define _CLOCK_INC_

 .include "clockData.inc"

 .CSEG
 .equ clk_write_address = 0b11010000
 .equ clk_read_address = 0b11010001

 .equ clk_data = _object_size
 .equ clk_size = clk_data + pointerSize

 clock: CLASS object, clk_size
 clock_ctor:
	BEGIN_TINY
		NEW clockData
		FIELD_WST clk_data, RH, RL
	END_TINY

 clock_write:
	BEGIN_SMALL

		FIELD_WLD clk_data, CH, CL

		; start
		ldi AH, 1
		call i2c_start
		mov AL, RL
		cpi AL, START_TRANSMITTED
		breq clock_write_start
		mov RH, AH
	end_small
		clock_write_start:

		; clock address
		inc AH
		xcallib thisH:thisL, i2c_send, clk_write_address //cpi r16, 0x18
		mov AL, RL
		cpi AL, 0x18
		breq clock_write_write_address
		cpi AL, 0x20
		breq clock_write_write_address
		mov RH, AH
	end_small
		clock_write_write_address:


		;call i2c_start
		// send clockaddress - write
		xcallib thisH:thisL, i2c_send, 0b11010000 //cpi r16, 0x18
		//send pointer address
		xcallib thisH:thisL, i2c_send, 0x00 //cpi r16, 0x28  
		//send data
		xcallib thisH:thisL, i2c_send, 0x00 //cpi r16, 0x28  
		call i2c_stop
	END_SMALL

 clock_read:
	BEGIN_SMALL
		FIELD_WLD clk_data, CH, CL
		; start
		ldi AH, 1
		call i2c_start
		mov AL, RL
		cpi AL, START_TRANSMITTED
		breq clock_read_start
		mov RH, AH
	end_small
		clock_read_start:

		; clock address
		inc AH;2
		xcallib thisH:thisL, i2c_send, clk_write_address //cpi r16, 0x18
		mov AL, RL
		cpi AL, 0x18
		breq clock_read_write_address
		cpi AL, 0x20
		breq clock_read_write_address
		mov RH, AH
	end_small
		clock_read_write_address:

		;send clock pointer 0x00
		inc AH;3
		xcallib thisH:thisL, i2c_send, 0x00
		mov AL, RL
		cpi AL, 0x28
		breq clock_read_pointer
		cpi AL, 0x30
		breq clock_read_pointer
		mov RH, AH
	end_small
		clock_read_pointer:

		;transmit repeated Start
		inc AH;4
		call i2c_start
		mov AL, RL
		cpi AL, 0x10
		breq clock_read_repeated
		mov RH, AH
	end_small
		clock_read_repeated:

		; clock address
		inc AH;5
		xcallib thisH:thisL, i2c_send, clk_read_address //cpi r16, 0x18
		mov AL, RL
		cpi AL, 0x40
		breq clock_read_read_address
		cpi AL, 0x48
		breq clock_read_read_address
		mov RH, AH
	end_small
		clock_read_read_address:

		;read seconds
		inc AH;6
		call i2c_read_ack
		mov AL, RL
		cpi AL, 0x50
		breq clock_read_seconds
		mov RH, AH
	end_small
		clock_read_seconds:
		mov BH, RH
		SETB CH:CL, clkdata_seconds, BH

		;read minutes
		inc AH;7
		call i2c_read
		mov AL, RL
		cpi AL, 0x58
		breq clock_read_minutes
		mov RH, AH
	end_small
		clock_read_minutes:
		mov BH, RH
		SETB CH:CL, clkdata_minutes, BH

		; stop i2c
		call i2c_stop

		clr RH
	END_SMALL

 #endif /* _CLOCK_INC_ */
