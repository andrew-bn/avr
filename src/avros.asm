/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */

.CSEG
 jmp start
.include "memory.inc"
.include "memoryTests.inc"
.include "listItem.inc"
 start:
					; setup stack
					ldi R16, low(RAMEND)
					out SPL, R16
					ldi R16, high(RAMEND)
					out SPH, R16

					call memory_init

					NEW listItem
					movw AH:AL, RH:RL
					XCALLIW AH, AL, listItem_setNext, 0x3456
					XCALL AH, AL, listItem_getNext
					DEL AH, AL

tmp2:				rjmp tmp2

					call memoryTests_run
					call memory_init

					; call memory_allocate
					ldi R16, low(12)
					push R16
					ldi R16, high(12)
					push R16

					call memory_allocate
					movw R19:R18, RH:RL
					call memory_allocate
					movw R21:R20, RH:RL
					call memory_allocate
					pop R16
					pop R16
					push R20
					push R21
					call memory_destroy;2
					pop R16
					pop R16
					push R18
					push R19
					call memory_destroy;1
					pop R16
					pop R16
					push RL
					push RH
					call memory_destroy;3
tmp:				rjmp tmp



