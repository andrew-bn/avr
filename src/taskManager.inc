/*
 * taskList.inc
 *
 *  Created: 21.10.2014 0:05:47
 *   Author: AB
 */ 
 #ifndef _TASKMANAGER_INC_
 #define _TASKMANAGER_INC_
 .include "list.inc"
 .include "listItem.inc"
 .include "task.inc"

 taskManager: CLASS list, list_size

 taskManager_ctor: 
	BEGIN_TINY
		THIS list_ctor 
	END_TINY

 taskManager_nextTask: ; A - pointer to task
		movw thisH:thisL, PH:PL
		FIELD_WLD list_head, AH, AL
 
 taskManager_nextTask_check:

		;if task_counter == 0 then run task
		GETW AH:AL, task_counter
		tst RH
		brne taskManager_nextTask_next
		tst RL
		breq taskManager_nextTask_runTask

taskManager_nextTask_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		brne taskManager_nextTask_check
		tst RL
		brne taskManager_nextTask_check

		FIELD_WLD list_head, AH, AL; load head again
		rjmp taskManager_nextTask_check

		taskManager_nextTask_runTask:; A - task pointer

		GETW AH:AL, task_stack
		out SPL, RL
		out SPH, RH
		GETW AH:AL, task_cmdPointer
		movw ZH:ZL, RH:RL
		movw PH:PL, AH:AL

		ijmp

#endif
