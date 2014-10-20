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
	BEGIN
		FIELD_WLD list_head, AH, AL

		;if task_counter == 0
		GETW AH:AL, task_counter
		tst RH
		sbic SREG, 7 ; if rh not 0 then skip tst
		tst RL
		sbic SREG, 7 ; if rl not 0
		rjmp taskManager_nextTask_next

		GET AH:AL, task_state
		; if task is running
		cpi RL, task_Running

taskManager_nextTask_next:


	END
#endif
