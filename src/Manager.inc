/*
 * main.asm
 *
 *  Created: 08.10.2014 21:54:56
 *   Author: AB
 */ 
 .include "common.inc"
 .include "memory.inc"
 .include "object.inc"

 .equ TASK_HEADER_SIZE = WORD_SIZE ; stack size
 .equ MAX_TASKS = 10
 .equ TASKS_TABLE_SIZE = MAX_TASKS * WORD_SIZE;

.CSEG
; interrupts

jmp _RESET

.ORG INT_VECTORS_SIZE

_RESET:
				_MINIT
				_MALLOC 100
_maintask:
.DSEG