/*
 * object.inc
 *
 *  Created: 10/10/2014 12:48:39 PM
 *   Author: user
 */ 

 #ifndef _OBJECT_INC_
 #define _OBJECT_INC_

 .CSEG

 .equ _object_offcet = 0x0
 .equ object_next = 2
 .equ object_prev = 2
 .equ _object_size = object_next + object_prev

 object:
 object_size:			.dw _object_size
object_base:			.dw 0x0

 object_new:			ret										;ctor

 object__memalloc:		
				
 


 #endif  /* _OBJECT_INC_ */
 