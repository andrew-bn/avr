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
 
 .equ _task_offcet = _object_offcet+_object_size
 .equ task_Run = _task_offcet
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
