/*
 * EmptyTask.inc
 *
 *  Created: 19.10.2014 0:20:46
 *   Author: AB
 */ 

 #ifndef _EMPTYTASK_INC
 #define _EMPTYTASK_INC

 .include "object.inc"
 .CSEG
	
 emptyTask: CLASS task, 100
	emptyTask_ctor:
		BEGIN_TINY
			THIS task_ctor
			OVERRIDE task_Run, emptyTask_Run
		END_TINY

	emptyTask_Run:
		BEGIN
		END
 #endif
