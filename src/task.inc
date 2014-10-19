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
 
 .set _task_Running = 0x1
 .set _tast_Idle = 0x0
 .set _task_Wait = 0x2
 .set _task_Int = 0x3

 .equ task_Run = _listItem_size
 .equ _task_stack = task_Run + pointerSize
 .equ _task_state = _task_stack + pointerSize
 .equ _task_counter = _task_state + 1
 .equ _task_size = _task_counter + 2

 task: CLASS listItem, _task_size
 	task_ctor:
		BEGIN_TINY
					THIS listItem_ctor
					clr AL
					FIELD_WST task_Run, AL,AL
					FIELD_WST _task_stack, AL,AL
					FIELD_WST _task_counter, AL,AL
					FIELD_ST _task_counter, AL
		END_TINY

	task_getState: GETTER _task_state
	task_getCounter: GETTERW _task_counter
	task_setCounter: SETTERW _task_counter
 #endif
