/*
 * task.inc
 *
 *  Created: 19.10.2014 0:07:19
 *   Author: AB
 */ 
 #ifndef _TASK_INC_
 #define _TASK_INC_

 .include "object.inc"
 .CSEG

 .equ task_Run = _object_size
 .equ _task_stack = task_Run + pointerSize
 .equ _task_state = _task_stack + pointerSize
 .equ _task_counter = _task_state + 1
 .equ _task_size = _task_counter + 2

 task: CLASS object, _task_size
 	task_ctor:
		BEGIN_TINY
					clr AL
					FIELD_WST task_Run, AL,AL
					FIELD_WST _task_stack, AL,AL
					FIELD_WST _task_counter, AL,AL
					FIELD_ST _task_counter, AL
		END_TINY
 #endif
