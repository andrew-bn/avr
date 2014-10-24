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
 .set task_Wait = 0x2

 .equ task_Run = _listItem_size
 .equ task_stack = task_Run + pointerSize
 .equ task_state = task_stack + pointerSize
 .equ task_counter = task_state + 1
 .equ task_cmdPointer = task_counter + 2
 .equ task_stackMinSpace = task_cmdPointer + pointerSize
 .equ _task_size = task_stackMinSpace + 20

 task: CLASS listItem, _task_size
 	task_ctor:
		BEGIN_TINY
					THIS listItem_ctor

					ldi AL, 1
					ldi AH, 0
					FIELD_WST task_counter, AH, AL

					ldi AL, task_Wait
					FIELD_ST task_state, AL

					ldi AL, low(task_start)
					ldi AH, high(task_start)
					FIELD_WST task_cmdPointer, AH, AL

					call __object_size
					add RL, thisL
					adc RH, thisH
					ldi AL, low(1)
					sub RL, AL
					ldi AL, high(1)
					sbc RH, AL
					FIELD_WST task_stack, RH, RL

		END_TINY

	task_getState: GETTER task_state
	task_getCounter: GETTERW task_counter
	task_setCounter: SETTERW task_counter

	task_start:

		push PL
		push PH
		movw thisH:thisL, PH:PL
		FIELD_WLD task_Run, AH, AL

		movw ZH:ZL, AH:AL

	
		icall

		pop AH
		pop AL

		XCALLW QH:QL, list_remove, AH, AL
		XCALL QH:QL, taskManager_checkAndRun
	ret

 #endif

