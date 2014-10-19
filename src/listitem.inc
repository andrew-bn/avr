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
 .equ _data = _prev + pointerSize
 .equ _listItem_size = _data + pointerSize

 listItem: CLASS object, _listItem_size
	listItem_ctor:
		BEGIN_TINY
					clr AL
					FIELD_WST _next, AL,AL
					FIELD_WST _prev, AL,AL
					FIELD_WST _data, AL,AL
		END_TINY

	listItem_setNext: SETTER _next
	listItem_getNext: GETTER _next
	
	listItem_setPrev: SETTER _prev
	listItem_getPrev: GETTER _prev

	listItem_setData: SETTER _data
	listItem_getData: GETTER _data


 #endif /* _LISTITEM_INC_ */
