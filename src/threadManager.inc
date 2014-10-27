/*
 * threadList.inc
 *
 *  Created: 21.10.2014 0:05:47
 *   Author: AB
 */ 
 #ifndef _threadMANAGER_INC_
 #define _threadMANAGER_INC_
 .include "list.inc"
 .include "listItem.inc"
 .include "thread.inc"

 .macro WAITI ;@0- time to sleep
		XPUSH Ar+Br+Cr+Dr+Er+Xr+Zr+Yr
		
		movw thisH:thisL, QH:QL
		FIELD_WLD threadManager_currentthread, AH, AL
		movw thisH:thisL, AH:AL
		FIELD_WSTI thread_counter, @0
		XCALL QH:QL, threadManager_switch
		
		XPOP Ar+Br+Cr+Dr+Er+Xr+Zr+Yr
 .endm
 .equ threadManager_currentthread = list_size
 .equ threadManager_size = threadManager_currentthread + pointerSize

 threadManager: CLASS list, threadManager_size

 threadManager_ctor: 
	BEGIN_TINY
		THIS list_ctor 
	END_TINY

 threadManager_checkAndRun: ; A - pointer to thread
		
		movw thisH:thisL, PH:PL
		FIELD_WLD list_head, AH, AL
 
 threadManager_checkAndRun_check:

		;if thread_counter == 0 then run thread
		XCLI
		GETW AH:AL, thread_counter
		XSEI

		tst RH
		brne threadManager_checkAndRun_next
		tst RL
		breq threadManager_checkAndRun_runthread

threadManager_checkAndRun_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		brne threadManager_checkAndRun_check
		tst RL
		brne threadManager_checkAndRun_check

		FIELD_WLD list_head, AH, AL; load head again
		rjmp threadManager_checkAndRun_check

		threadManager_checkAndRun_runthread:; A - thread pointer
		
		ldi BL, thread_Running
		XCLI
		SETB AH:AL, thread_state, BL
		XSEI

		FIELD_WST threadManager_currentthread, AH, AL

		GETW AH:AL, thread_stack
		out SPL, RL
		out SPH, RH
		GETW AH:AL, thread_cmdPointer
		movw ZH:ZL, RH:RL
		movw PH:PL, AH:AL

		ijmp
/////////////
	threadManager_switch:

		movw thisH:thisL, PH: PL
		FIELD_WLD threadManager_currentthread, AH, AL

		; save stack pointer
		in BL, SPL
		in BH, SPH

		SETW AH:AL, thread_stack, BH, BL

		ldi BL, thread_Wait
		XCLI
		SETB AH:AL, thread_state, BL
		XSEI

		; save command pointer
		ldi BH, high(thread_Manager_switch_exit)
		ldi BL, low(thread_Manager_switch_exit)
		SETW AH:AL, thread_cmdPointer, BH, BL

		jmp threadManager_checkAndRun
		thread_Manager_switch_exit:
	ret

// int handler
 _threadManager_waitTimeout:
		push PH
		push PL
		push TH
		push TL
		push thisH
		push thisL
		push AH
		push AL
		push BH
		push BL
		push RH
		push RL
		in AL, SREG
		push AL

		movw thisH:thisL, PH: PL

		FIELD_WLD list_head, AH, AL
 
 threadManager_waitTimeout_check:
		; if state== wait then decrement thread_counter
		GET AH:AL, thread_state
		ldi BL, thread_Wait
		cp RL, BL
		brne threadManager_waitTimeout_next

		;if thread_counter != 0 then decrement it
		GETW AH:AL, thread_counter
		tst RH
		brne threadManager_waitTimeout_decrement
		tst RL
		breq threadManager_waitTimeout_next

threadManager_waitTimeout_decrement:
		clr BH
		ldi BL, 1
		sub RL, BL
		sbc RH, BH

		SETW AH:AL, thread_counter, RH, RL

threadManager_waitTimeout_next:
		GETW AH:AL, listItem_next
		movw AH:AL, RH:RL
		tst RH
		brne threadManager_waitTimeout_check
		tst RL
		brne threadManager_waitTimeout_check

		pop AL
		out SREG, AL
		pop RL
		pop RH
		pop BL
		pop BH
		pop AL
		pop AH
		pop thisL
		pop thisH
		pop TL
		pop TH
		pop PL
		pop PH

reti
#endif
