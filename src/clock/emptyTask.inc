/*
 * Emptythread.inc
 *
 *  Created: 19.10.2014 0:20:46
 *   Author: AB
 */ 

 #ifndef _EMPTYthread_INC
 #define _EMPTYthread_INC

 .include "object.inc"
 .include "thread.inc"
 .CSEG
	
 .equ _emptythread_size = _thread_size + 2

 emptythread: CLASS thread, _emptythread_size
	emptythread_ctor:
		BEGIN_TINY
			OVERRIDE thread_Run, emptythread_Run
			THIS thread_ctor
		END_TINY

	

 #endif
