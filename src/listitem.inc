/*
 * array.inc
 *
 *  Created: 10/13/2014 5:26:53 PM
 *   Author: user
 */ 
 .include "object.inc"

 #ifndef _LISTITEM_INC_
 #define _LISTITEM_INC_

 .CSEG

 .equ _listItem_offcet = _object_offcet+_object_size
 .equ _next = _listItem_offcet
 .equ _prev = _next + pointerSize
 .equ _data = _prev + pointerSize
 .equ _listItem_size = _data + pointerSize

 listItem: CLASS object, _listItem_size
 listItem_ctor:
						_ENTER 0, 0

						clr R16
						THIS_WST _next, R16,R16
						THIS_WST _prev, R16,R16
						THIS_WST _data, R16,R16

						_LEAVE 0
						ret

 listItem_setNext:
	.equ p_next = 0
						_ENTER 1, 2

						PARAM_WLD p_next, R17, R16
						THIS_WST _next, R17, R16

						_LEAVE 0
						ret
 listItem_getNext:
						_ENTER 0, 0

						THIS_WST _next, R17, R16
						
						_LEAVE 0
						RETURN R17, R16


 #endif /* _LISTITEM_INC_ */
