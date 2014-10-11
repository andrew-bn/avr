/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */

.CSEG
 jmp start
.include "memory.inc"

 start:
					; setup stack
					ldi R16, low(RAMEND)
					out SPL, R16
					ldi R16, high(RAMEND)
					out SPH, R16

					call memory_init

					; call memory_allocate
					ldi R16, low(10)
					push R16
					ldi R16, high(10)
					push R16
					call memory_allocate 
					pop R16
					pop R16
					push retL
					push retH
					call memory_destroy

					
tmp:				rjmp tmp



