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

 .equ listItem_next = _object_size
 .equ listItem_prev = listItem_next + pointerSize
 .equ _listItem_size = listItem_prev + pointerSize

 listItem: CLASS object, _listItem_size
	listItem_ctor:
		BEGIN_TINY
					clr AL
					FIELD_WST listItem_next, AL,AL
					FIELD_WST listItem_prev, AL,AL
		END_TINY

	listItem_setNext: SETTERW listItem_next
	listItem_getNext: GETTERW listItem_next
	
	listItem_setPrev: SETTERW listItem_prev
	listItem_getPrev: GETTERW listItem_prev


 #endif /* _LISTITEM_INC_ */
