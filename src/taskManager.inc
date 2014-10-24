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

 .equ taskManager_currentTask = list_size
 .equ taskManager_size = taskManager_currentTask + pointerSize

 taskManager: CLASS list, taskManager_size

 taskManager_ctor: 
	BEGIN_TINY
		THIS list_ctor 
	END_TINY

 taskManager_checkAndRun: ; A - pointer to task
		movw thisH:thisL, PH:PL
		FIELD_WLD list_head, AH, AL
 
 taskManager_checkAndRun_check:

		;if task_counter == 0 then run task
		XCLI
		GETW AH:AL, task_counter
		XSEI

		tst RH
		brne taskManager_checkAndRun_next
		tst RL
		breq taskManager_checkAndRun_runTask

taskManager_checkAndRun_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		brne taskManager_checkAndRun_check
		tst RL
		brne taskManager_checkAndRun_check

		FIELD_WLD list_head, AH, AL; load head again
		rjmp taskManager_checkAndRun_check

		taskManager_checkAndRun_runTask:; A - task pointer
		
		ldi BL, task_Running
		brid tm_cli_end
		set
		cli
		tm_cli_end:

		XCLI
		SET_ AH:AL, task_state, BL
		XSEI

		FIELD_WST taskManager_currentTask, AH, AL

		GETW AH:AL, task_stack
		out SPL, RL
		out SPH, RH
		GETW AH:AL, task_cmdPointer
		movw ZH:ZL, RH:RL
		movw PH:PL, AH:AL

		ijmp

	taskManager_switch:

		movw thisH:thisL, PH: PL
		FIELD_WLD taskManager_currentTask, AH, AL

		; save stack pointer
		in BL, SPL
		in BH, SPH

		SETW AH:AL, task_stack, BH, BL

		; save command pointer
		ldi BH, high(task_Manager_switch_exit)
		ldi BL, low(task_Manager_switch_exit)
		SETW AH:AL, task_cmdPointer, BH, BL

		jmp taskManager_checkAndRun
		task_Manager_switch_exit:
	ret

 taskManager_waitTimeout:
		movw thisH:thisL, PH: PL

		FIELD_WLD list_head, AH, AL
 
 taskManager_waitTimeout_check:
		; if state== wait then decrement task_counter
		GET AH:AL, task_state
		ldi BL, task_Wait
		cp RL, BL
		brne taskManager_waitTimeout_next

		;if task_counter != 0 then decrement it
		GETW AH:AL, task_counter
		tst RH
		brne taskManager_waitTimeout_decrement
		tst RL
		breq taskManager_waitTimeout_next

taskManager_waitTimeout_decrement:
		clr BH
		ldi BL, 1
		sub RL, BL
		sbc RH, BH

		SETW AH:AL, task_counter, RH, RL

taskManager_waitTimeout_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		brne taskManager_waitTimeout_check
		tst RL
		brne taskManager_waitTimeout_check

ret
#endif
