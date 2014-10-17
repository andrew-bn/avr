/*
 * list.inc
 *
 *  Created: 10/17/2014 4:38:59 PM
 *   Author: user
 */ 
 
 #ifndef _LIST_INC_
 #define _LIST_INC_

 .CSEG

 .equ _list_offcet = _object_offcet+_object_size
 .equ _head = _list_offcet
 .equ _tail = _head + pointerSize
 .equ _list_size = _tail + pointerSize

 list: CLASS object, _list_size

 list_addItem:
	.equ _data = 0
	BEGIN_SMALL

		PARAM_WLD _data, AH, AL
		FIELD_WLD _tail, BH, BL

		NEW listItem
		movw CH:CL, RH:RL
		XCALLW CH, CL, listItem_setData, AH, AL
		XCALLW CH, CL, listItem_setPrev, BH, BL

		; set next for Tail
		XCALLW BH, BL, listItem_setNext, CH, CL
		
	END_SMALL

 list_getItem:
	.equ _index = 0; BH:BL - current index; CH:CL-current item
	BEGIN
							PARAM_WLD _index, AH, AL
							FIELD_WLD _head, CH, CL
							
							ldi BH, 0
							ldi BL, 0

							cp AH, BH
							brne list_getItem_next
							cp AL, BL
							brne list_getItem_next

							XCALL CH, CL, listItem_getData, AH,AL
							rjmp list_getItem_exit
		list_getItem_next:
							
		list_getItem_exit: 
							
							movw RH:RL, AH:AL
	END

 #endif /* _LIST_INC_ */
