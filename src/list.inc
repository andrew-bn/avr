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

 .equ _head = _object_size
 .equ _tail = _head + pointerSize
 .equ _list_size = _tail + pointerSize

 list: CLASS object, _list_size
 list_ctor:
		BEGIN_TINY
			clr AL
			FIELD_WST _head, AL, AL
			FIELD_WST _tail, AL, AL
			OVERRIDE object_destroy, list_destructor
		END_TINY

 list_destructor:
	BEGIN_SMALL
		
		FIELD_WLD _head, AH, AL

		list_destructor_test:
		tst AH
		brne list_destructor_item
		tst AL
		breq list_destructor_this

		list_destructor_item:
		XCALL AH:AL, listItem_getNext
		movw BH:BL, RH:RL
		DEL AH:AL
		movw AH:AL, BH:BL

		rjmp list_destructor_test

		list_destructor_this:

		THIS object_destructor

	END_SMALL

 list_addData:
	.equ list_addData_data = 0
	BEGIN_SMALL

				PARAM_WLD list_addData_data, AH, AL
				FIELD_WLD _tail, BH, BL

				NEW listItem
				movw CH:CL, RH:RL
				XCALLW CH:CL, listItem_setData, AH, AL
				XCALLW CH:CL, listItem_setPrev, BH, BL

				; if this is the first element then set Tail and Head
				tst BH
				brne list_addData_setNext
				tst BL
				brne list_addData_setNext

				FIELD_WST _tail, CH, CL
				FIELD_WST _head, CH, CL

				rjmp list_addData_exit

	list_addData_setNext:
				; set next for Tail
				XCALLW BH:BL, listItem_setNext, CH, CL

	list_addData_exit:
	END_SMALL

 list_removeData:
 .equ list_removeData_data = 0; B - current data; C-current item; A - data to find; D - temp
	BEGIN
							PARAM_WLD list_removeData_data, AH, AL
							FIELD_WLD _head, CH, CL

		list_removeData_check:	
							
							XCALL CH:CL, listItem_getData
							movw BH:BL, RH:RL

							cp AH, BH
							brne list_removeData_next
							cp AL, BL
							brne list_removeData_next

							rjmp list_removeData_remove

		list_removeData_next:
							
							XCALL CH:CL, listItem_getNext
							; in R we have reference to item
							movw CH:CL, RH:RL
							tst CL
							brne list_removeData_check
							tst CH
							brne list_removeData_check
							rjmp list_removeData_exit

		list_removeData_remove: 

							XCALL CH:CL, listItem_getPrev
							movw AH:AL, CH:CL
							XCALL CH:CL, listItem_getNext
							movw BH:BL, CH:CL

							XCALLW AH:AL, listItem_setNext, BH, BL
							XCALLW BH:BL, listItem_setPrev, AH, AL

							VCALL CH:CL, object_destroy

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
							subi BL, -1
							sbci BH, 0

							cp AH, BH
							brne list_getItem_next
							cp AL, BL
							brne list_getItem_next

							movw RH:RL, CH:CL

							rjmp list_getItem_exit

		list_getItem_next:
							
							XCALL CH:CL, listItem_getNext
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
							subi BL, -1
							sbci BH, 0

							cp AH, BH
							brne list_getData_next
							cp AL, BL
							brne list_getData_next

							XCALL CH:CL, listItem_getData
							; in R we have reference to data
							rjmp list_getData_exit

		list_getData_next:
							
							XCALL CH:CL, listItem_getNext
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
