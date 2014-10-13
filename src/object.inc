/*
 * object.inc
 *
 *  Created: 10/10/2014 12:48:39 PM
 *   Author: user
 */ 

 #ifndef _OBJECT_INC_
 #define _OBJECT_INC_
 .include "common.inc"
 .include "memory.inc"

 .def thisH = R13
 .def thisL = R12
 .def newH = R11
 .def newL = R10
 .equ metaSize = 4;
 .macro NEW
		_object_new
 .endm

 .macro RETURN 
	movw retH:retL, @0:@1
	ret
 .endm

 .macro PARAM_LD; @0 param offcet
		;ld Y+@0, @1
 .endm

 .macro PARAM_WLD; @ param offcet
	ldd @2, Z+@0
	ldd @1, Z+@0+1
 .endm

 .macro THIS_ST; @0 - field offcet; @1 - reg
	ldi R16, low(@1)
	st Y+@0, R16
 .endm

 .macro THIS_WST; @0 - field offcet; @1 - regH,@2 - regL
	ldd @2, Y+@0
	ldd @1,Y+@0+1
 .endm

 .macro _ENTER ; stacksize; paramsize
	_STACK @0+2, 3
	_STACK_TO_Y @0+2+@1
	movw ZH:ZL, thisH:thisL
 .endm

 .macro _LEAVE

	_UNSTACK @0+2, 3

 .endm

 .CSEG

 .equ _object_offcet = 0x0
 .equ _object_size = 0x00

 object:
 object_size:			.dw _object_size
 object_base:			.dw 0x0

 object_ctor:			ret
 _object_new:
	.def tmpH = R17
	.def tmpL = R16
	.def sizeH = R19
	.def sizeL = R18

						_STACK 4,3
						_STACK_TO_Y 2

						clr sizeH
						clr sizeL

						ld tmpL, Y
						ldd tmpH, Y+1

 calcSize:				movw ZH:ZL, tmpH:tmpL
						; load size to temp
						LPM tmpL, Z+
						LPM tmpH, Z+

						; increment size
						add sizeL, tmpL
						adc sizeH, tmpH

						;load next
						LPM tmpL, Z+
						LPM tmpH, Z+

						tst tmpH
						brne calcSize
						tst tmpL
						brne calcSize

						;allocate memory
						push sizeL
						push sizeH
						call memory_allocate
						pop R16
						pop R16

						movw newH:newL, XH:XL

						; call ctor
						ld tmpL, Y
						ldd tmpH, Y+1
						movw ZH:ZL, tmpH:tmpL

						subi ZL, low(-metaSize)
						sbci ZH, high(-metaSize)

						icall

						_UNSTACK 4,3
						ret
 #endif  /* _OBJECT_INC_ */
 