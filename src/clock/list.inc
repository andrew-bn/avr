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

 .equ list_head = _object_size
 .equ list_tail = list_head + pointerSize
 .equ list_size = list_tail + pointerSize

 list: CLASS object, list_size
 list_ctor:
		BEGIN_TINY
			NEW listItem
			movw AH:AL, RH:RL
			FIELD_WST list_head, AH, AL
			FIELD_WST list_tail, AH, AL
			OVERRIDE object_destroy, list_destructor
		END_TINY
 list_getHead: GETTERW list_head
 list_destructor:
	BEGIN_SMALL
		
		FIELD_WLD list_head, AH, AL

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

 list_add: ; A - item pointer; B - tail pointer
	.equ list_add_item = 0
	BEGIN_SMALL

				PARAM_WLD list_add_item, AH, AL
				FIELD_WLD list_tail, BH, BL

				SETW AH:AL, listItem_prev, BH, BL

	list_add_setNext:
				; set next for Tail
				SETW BH:BL, listItem_next, AH, AL
				FIELD_WST list_tail, AH, AL

	END_SMALL

 list_remove:
 .equ list_remove_item = 0; C-current item; A - item to find; D - temp
	BEGIN
							PARAM_WLD list_remove_item, AH, AL
							FIELD_WLD list_head, CH, CL

		list_remove_check:
							cp AH, CH
							brne list_remove_next
							cp AL, CL
							brne list_remove_next

							GETW CH:CL, listItem_prev
							movw AH:AL, RH:RL
							GETW CH:CL, listItem_next
							movw BH:BL, RH:RL

							clr TH
							cp AH, TH
							brne list_remove_changePrev
							cp AL, TH
							breq list_remove_checkChangeNext

		list_remove_changePrev:
							SETW AH:AL, listItem_next, BH, BL

		list_remove_checkChangeNext:
							clr TH
							cp BH, TH
							brne list_remove_changeNext
							cp BL, TH
							breq list_remove_exit

		list_remove_changeNext:
							SETW BH:BL, listItem_prev, AH, AL

							rjmp list_remove_exit
		list_remove_next:
							GETW CH:CL, listItem_next
							; in R we have reference to item
							movw CH:CL, RH:RL
							tst CL
							brne list_remove_check
							tst CH
							brne list_remove_check
							rjmp list_remove_exit

		list_remove_exit:
	END

 list_get:
	.equ _index = 0; B - current index; C-current item; A - index to find; D - temp
	BEGIN
							PARAM_WLD _index, AH, AL
							FIELD_WLD list_head, CH, CL
							
							ldi BH, -1
							ldi BL, -1

		list_get_check:
							subi BL, -1
							sbci BH, 0

							cp AH, BH
							brne list_get_next
							cp AL, BL
							brne list_get_next

							movw RH:RL, CH:CL

							rjmp list_get_exit

		list_get_next:
							
							XCALL CH:CL, listItem_getNext
							; in R we have reference to item
							movw CH:CL, RH:RL
							tst CL
							brne list_get_check
							tst CH
							brne list_get_check
							; no item found
							clr RH
							clr RL
		list_get_exit: 
	END

 #endif /* _LIST_INC_ */
