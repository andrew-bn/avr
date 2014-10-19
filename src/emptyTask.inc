/*
 * EmptyTask.inc
 *
 *  Created: 19.10.2014 0:20:46
 *   Author: AB
 */ 

 #ifndef _EMPTYTASK_INC
 #define _EMPTYTASK_INC

 .include "object.inc"
 .include "task.inc"
 .CSEG
	
 .equ _emptyTask_size = _task_size+2
 emptyTask: CLASS task, _emptyTask_size
	emptyTask_ctor:
		BEGIN_TINY
			THIS task_ctor
			OVERRIDE task_Run, emptyTask_Run
		END_TINY

	emptyTask_Run:
		BEGIN
		END
 #endif
