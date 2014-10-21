/*
 * task.inc
 *
 *  Created: 19.10.2014 0:07:19
 *   Author: AB
 */ 
 #ifndef _TASK_INC_
 #define _TASK_INC_

 .include "object.inc"
 .include "listItem.inc"
 .CSEG
 ; STATE
 .set task_Running = 0x1
 .set _tast_Idle = 0x0
 .set task_Wait = 0x2
 .set _task_Int = 0x3

 .equ task_Run = _listItem_size
 .equ _task_stack = task_Run + pointerSize
 .equ task_state = _task_stack + pointerSize
 .equ task_counter = task_state + 1
 .equ _task_cmdPointer = task_state + 2
 .equ _task_stackMinSpace = _task_cmdPointer + pointerSize
 .equ _task_size = _task_stackMinSpace + 20

 task: CLASS listItem, _task_size
 	task_ctor:
		BEGIN_TINY
					THIS listItem_ctor

					clr AL
					FIELD_WST task_counter, AL,AL

					ldi AL, task_Running
					FIELD_ST task_state, AL

					ldi AL, low(task_start)
					ldi AH, high(task_start)
					FIELD_WST _task_cmdPointer, AH, AL

					call __object_size
					add RL, thisL
					adc RH, thisH
					ldi AL, low(1)
					sub RL, AL
					ldi AL, high(1)
					sbc RH, AL
					FIELD_WST _task_stack, RH, RL

		END_TINY

	task_getState: GETTER task_state
	task_getCounter: GETTERW task_counter
	task_setCounter: SETTERW task_counter
	tast_start:
	ret
 #endif
