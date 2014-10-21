/*
 * taskList.inc
 *
 *  Created: 21.10.2014 0:05:47
 *   Author: AB
 */ 
 #ifndef _TASKMANAGER_INC_
 #define _TASKMANAGER_INC_
 .include "list.inc"
 taskManager: CLASS list, 0

 taskManager_nextTask: ; A - pointer to task

		FIELD_WLD list_head, AH, AL
 
 taskManager_nextTask_check:

		;if task_counter == 0 then run task
		GETW AH:AL, task_counter
		tst RH
		sbic SREG, 7 ; if rh not 0 then skip tst
		tst RL
		sbic SREG, 7 ; if rl == 0 then runTask
		rjmp taskManager_nextTask_runTask

taskManager_nextTask_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		sbic SREG, 7 ; if rh not 0 then skip tst
		tst RL
		sbic SREG, 7 ; if rl not 0
		rjmp taskManager_nextTask_check

		FIELD_WLD list_head, AH, AL; load head again
		rjmp taskManager_nextTask_check
		ret
#endif
