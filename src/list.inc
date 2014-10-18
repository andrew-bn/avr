/*
 * list.inc
 *
 *  Created: 10/17/2014 4:38:59 PM
 *   Author: user
 */ 
 
 #ifndef _LIST_INC_
 #define _LIST_INC_
 .include "object.inc"
 .include "listItem.inc"
 .CSEG

 .equ _list_offcet = _object_offcet+_object_size
 .equ _head = _list_offcet
 .equ _tail = _head + pointerSize
 .equ _list_size = _tail + pointerSize

 list: CLASS object, _list_size

 list_addData:
	.equ list_addData_data = 0
	BEGIN_SMALL

		PARAM_WLD list_addData_data, AH, AL
		FIELD_WLD _tail, BH, BL

		NEW listItem
		movw CH:CL, RH:RL
		XCALLW CH, CL, listItem_setData, AH, AL
		XCALLW CH, CL, listItem_setPrev, BH, BL

		; set next for Tail
		XCALLW BH, BL, listItem_setNext, CH, CL
		
	END_SMALL

 list_removeData:
 .equ list_removeData_data = 0; B - current data; C-current item; A - data to find; D - temp
	BEGIN
							PARAM_WLD list_removeData_data, AH, AL
							FIELD_WLD _head, CH, CL

		list_removeData_check:	
							
							XCALL CH, CL, listItem_getData
							movw BH:BL, RH:RL

							cp AH, BH
							brne list_removeData_next
							cp AL, BL
							brne list_removeData_next

							rjmp list_removeData_remove

		list_removeData_next:
							
							XCALL CH, CL, listItem_getNext
							; in R we have reference to item
							movw CH:CL, RH:RL
							tst CL
							brne list_removeData_check
							tst CH
							brne list_removeData_check
							rjmp list_removeData_exit

		list_removeData_remove: 

							XCALL CH, CL, listItem_getPrev
							movw AH:AL, CH:CL
							XCALL CH, CL, listItem_getNext
							movw BH:BL, CH:CL

							XCALLW AH,AL, listItem_setNext, BH, BL
							XCALLW BH,BL, listItem_setPrev, AH, AL

							DEL CH, CL

		list_removeData_exit:
	END

 list_getItem:
	.equ _index = 0; B - current index; C-current item; A - index to find; D - temp
	BEGIN
							PARAM_WLD _index, AH, AL
							FIELD_WLD _head, CH, CL
							
							ldi BH, -1
							ldi BL, -1

		list_getItem_check:
							adiw BH:BL, 1
							cp AH, BH
							brne list_getItem_next
							cp AL, BL
							brne list_getItem_next

							movw RH:RL, CH:CL

							rjmp list_getItem_exit

		list_getItem_next:
							
							XCALL CH, CL, listItem_getNext
							; in R we have reference to item
							movw CH:CL, RH:RL
							tst CL
							brne list_getItem_check
							tst CH
							brne list_getItem_check
							; no item found
							clr RH
							clr RL
		list_getItem_exit: 
	END

 list_getData:
	.equ list_getData_index = 0; B - current index; C-current item; A - index to find; D - temp
	BEGIN
							PARAM_WLD list_getData_index, AH, AL
							FIELD_WLD _head, CH, CL
							
							ldi BH, -1
							ldi BL, -1

		list_getData_check:	
							adiw BH:BL, 1
							cp AH, BH
							brne list_getData_next
							cp AL, BL
							brne list_getData_next

							XCALL CH, CL, listItem_getData
							; in R we have reference to data
							rjmp list_getData_exit

		list_getData_next:
							
							XCALL CH, CL, listItem_getNext
							; in R we have reference to item
							movw CH:CL, RH:RL
							tst CL
							brne list_getData_check
							tst CH
							brne list_getData_check
							; no item found
							clr RH
							clr RL
		list_getData_exit: 

	END

 #endif /* _LIST_INC_ */
