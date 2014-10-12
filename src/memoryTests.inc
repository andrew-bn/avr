/*
 * memoryTests.inc
 *
 *  Created: 12.10.2014 21:52:03
 *   Author: AB
 */ 
  .macro _TEST_INT
		_TEST_BYTE low(@0)
		_TEST_BYTE high(@0)
 .endm
 .macro _TEST_BYTE
		ld R16, X+
		cpi R16, @0
		sbic SREG, 1

 .endm
 .CSEG
 memoryTests_run:
					call memory_init
	newBlock:		
					ldi R16,1; test number
					mov R1, R16
					; allocate 12 bytes of memory
					
					ldi R16, low(12)
					push R16
					ldi R16, high(12)
					push R16
					call memory_allocate
					pop R16
					pop R16

					ldi XL, low(SRAM_START)
					ldi XH, high(SRAM_START)
					_TEST_INT 0x70
					_TEST_INT 0x00

					ldi XL, low(SRAM_START+0x10)
					ldi XH, high(SRAM_START+0x10)
					_TEST_INT RAMEND
					_TEST_INT RAMEND
					
	create_block_inside_free_block:
					ldi R16,2; test number
					mov R1, R16
					; allocate 12 bytes of memory
					
					ldi R16, low(12)
					push R16
					ldi R16, high(12)
					push R16
					call memory_allocate
					pop R16
					pop R16

					ldi XL, low(SRAM_START)
					ldi XH, high(SRAM_START)
					_TEST_INT 0x70
					_TEST_INT 0x00

					ldi XL, low(SRAM_START+0x10)
					ldi XH, high(SRAM_START+0x10)
					_TEST_INT 0x80
					_TEST_INT 0x60

					ldi XL, low(SRAM_START+0x20)
					ldi XH, high(SRAM_START+0x20)
					_TEST_INT RAMEND
					_TEST_INT RAMEND

					rjmp testSuccess

 testFailed:		rjmp testFailed

 testSuccess:		ser R16
					mov R1,R16
					rjmp testSuccess
 ret


