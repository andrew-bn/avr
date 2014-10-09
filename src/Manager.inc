/*
 * main.asm
 *
 *  Created: 08.10.2014 21:54:56
 *   Author: AB
 */ 
 .include "common.inc"
  .include "memory.inc"

 .equ TASK_HEADER_SIZE = WORD_SIZE ; stack size
 .equ MAX_TASKS = 10
 .equ TASKS_TABLE_SIZE = MAX_TASKS * WORD_SIZE;


; MACROS
.MACRO Start
				LDI R16, LOW(@0) 
.ENDMACRO
 
.MACRO MemAlloc
				
.ENDMACRO

.CSEG
; interrupts

jmp _RESET

.ORG INT_VECTORS_SIZE

_RESET:			
				LDI tempH, high(TASKS_TABLE_SIZE)
				LDI tempL, low(TASKS_TABLE_SIZE)
				LDI XH, high(TasksTableSize)
				LDI XL, low(TasksTableSize)
				ST X+,tempL
				ST X, tempH

				Start _maintask + TASK_HEADER_SIZE
_maintask:
.DSEG
TasksTableSize:	.BYTE WORD_SIZE
Tasks:			.BYTE TASKS_TABLE_SIZE ; pointers to task objects