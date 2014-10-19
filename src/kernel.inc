/*
 * main.asm
 *
 *  Created: 08.10.2014 21:54:56
 *   Author: AB
 */

.CSEG
; interrupts

jmp _RESET

.ORG INT_VECTORS_SIZE

_RESET: jmp _START

	.include "common.inc"
	.include "memory.inc"
	.include "object.inc"
	.include "list.inc"
	.include "listItem.inc"
	.include "task.inc"

_START: ; setup stack
		ldi R16, low(RAMEND)
		out SPL, R16
		ldi R16, high(RAMEND)
		out SPH, R16

		call memory_init

_maintask:
