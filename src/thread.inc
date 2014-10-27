/*
 * thread.inc
 *
 *  Created: 19.10.2014 0:07:19
 *   Author: AB
 */ 
 #ifndef _THREAD_INC_
 #define _THREAD_INC_

 .include "object.inc"
 .include "listItem.inc"

 .CSEG
 ; STATE
 .set thread_Running = 0x1
 .set thread_Wait = 0x2

 .equ thread_Run = _listItem_size
 .equ thread_stack = thread_Run + pointerSize
 .equ thread_state = thread_stack + pointerSize
 .equ thread_counter = thread_state + 1
 .equ thread_cmdPointer = thread_counter + 2
 .equ thread_startParameter = thread_cmdPointer + pointerSize
 .equ thread_stackMinSpace = thread_startParameter + pointerSize
 .equ _thread_size = thread_stackMinSpace + 40

 thread: CLASS listItem, _thread_size
 	thread_ctor:
		BEGIN_TINY
					THIS listItem_ctor

					movw AH:AL, thisH:thisL
					FIELD_WST thread_startParameter, AH, AL

					ldi AL, 1
					ldi AH, 0
					FIELD_WST thread_counter, AH, AL

					ldi AL, thread_Running
					FIELD_ST thread_state, AL

					ldi AL, low(_thread_start)
					ldi AH, high(_thread_start)
					FIELD_WST thread_cmdPointer, AH, AL

					call __object_size
					add RL, thisL
					adc RH, thisH
					ldi AL, low(1)
					sub RL, AL
					ldi AL, high(1)
					sbc RH, AL
					FIELD_WST thread_stack, RH, RL

					XCLI
					XCALLW QH:QL, list_add, RH, RL
					XSEI
		END_TINY

	thread_getState: GETTER thread_state
	thread_getCounter: GETTERW thread_counter
	thread_setCounter: SETTERW thread_counter

	thread_start:
		.equ thread_start_methodPointer = 0
		.equ thread_start_parameter = pointerSize
		BEGIN_TINY
			PARAM_WLD thread_start_methodPointer, AH, AL ; //load pointer to 
			FIELD_WST thread_Run, AH, AL

			PARAM_WLD thread_start_parameter, AH, AL ; //load start parameter
			FIELD_WST thread_startParameter, AH, AL

			ldi AL, thread_Wait
			FIELD_ST thread_state, AL
		END_TINY

	_thread_start:

		push PL
		push PH
		movw thisH:thisL, PH:PL
		FIELD_WLD thread_Run, AH, AL
		FIELD_WLD thread_Run, BH, BL
		movw PH:PL, BH:BL
		movw ZH:ZL, AH:AL

	
		icall

		pop AH
		pop AL

		XCLI
		XCALLW QH:QL, list_remove, AH, AL
		XSEI

		XCALL QH:QL, threadManager_checkAndRun

	ret

 #endif

