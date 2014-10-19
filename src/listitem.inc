/*
 * array.inc
 *
 *  Created: 10/13/2014 5:26:53 PM
 *   Author: user
 */ 
 .include "object.inc"
 .include "list.inc"

 #ifndef _LISTITEM_INC_
 #define _LISTITEM_INC_

 .CSEG

 .equ _next = _object_size
 .equ _prev = _next + pointerSize
 .equ _listItem_size = _prev + pointerSize

 listItem: CLASS object, _listItem_size
	listItem_ctor:
		BEGIN_TINY
					clr AL
					FIELD_WST _next, AL,AL
					FIELD_WST _prev, AL,AL
		END_TINY

	listItem_setNext: SETTERW _next
	listItem_getNext: GETTERW _next
	
	listItem_setPrev: SETTERW _prev
	listItem_getPrev: GETTERW _prev


 #endif /* _LISTITEM_INC_ */
