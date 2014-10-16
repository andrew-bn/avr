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

 .def thisH = R15
 .def thisL = R14
 .def newH = R15
 .def newL = R14
 .def typeH = R15
 .def typeL = R14

 .equ metaSize = 4;

 .macro CLASS
	.dw @1
	.dw (@0 * 2)
 .endm

 .macro XCALL ; pointer to object; method
	movw PH:PL, @0:@1
	call @2
 .endm

 .macro XCALLW ; pointer to object; method; Wparam
	movw PH:PL, @0:@1
	push @3
	push @4
	call @2
 .endm

 .macro XCALLIW ; pointer to object; method; Wparam

	push AL
	ldi AL, high(@3)
	mov TH,AL
	ldi AL, low(@3)
	mov TL, AL
	pop AL

	movw PH:PL, @0:@1
	push TH
	push TL
	call @2

 .endm

 .macro NEW
		push R16
		ldi R16, low(2*@0)
		mov typeL, R16
		ldi R16, high(2*@0)
		mov typeH, R16
		pop R16
		call _object_new
 .endm

 .macro DEL
 .endm

 .macro PARAM_WLD; @ param offcet
	.if @0 <= 0x3E
		ldd @2, Y+@0
		ldd @1, Y+@0+1
	.else
		subi YL, low(-@0)
		sbci YH, high(-@0)
		ld @2, Y
		ldd @1, Y+1
		subi YL, low(@0)
		sbci YH, high(@0)
	.endif
 .endm

 .macro THIS_WLD; @0 - field offcet; @1 - regH,@2 - regL
	.if @0 <= 0x3E
		ldd @2, Z+@0
		ldd @1, Z+1+@0
	.else
		subi ZL, low(-@0)
		sbci ZH, high(-@0)
		ld @2, Z
		ldd @1, Z+1
		subi ZL, low(@0)
		sbci ZH, high(@0)
	.endif
 .endm

 .macro THIS_WST; @0 - field offcet; @1 - regH,@2 - regL
	.if @0 <= 0x3E
		std Z+@0, @2
		std Z+1+@0, @1
	.else
		subi ZL, low(-@0)
		sbci ZH, high(-@0)
		st Z, @2
		std Z+1, @1
		subi ZL, low(@0)
		sbci ZH, high(@0)
	.endif
 .endm

 .macro SETTER
		BEGIN_TINY
				PARAM_WLD 0x00, AH, AL
				THIS_WST @0, AH, AL
		END_TINY
 .endm

 .macro GETTER
		BEGIN_TINY
				THIS_WLD 0x00, AH, AL
				movw RH:RL,AH:AL
		END_TINY
 .endm

 .CSEG

 .equ _object_offcet = 0x0
 .equ _object_size = 0x00

 object: CLASS 0x00, _object_size

 object_ctor:			ret

 _object_new:
	.def tmpH = R17
	.def tmpL = R16
	.def sizeH = R19
	.def sizeL = R18

	BEGIN_SMALL

						clr sizeH
						clr sizeL

						movw tmpH:tmpL, typeH:typeL

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
						push sizeH
						push sizeL
						call memory_allocate
						pop sizeL
						pop sizeH

						;load ctor pointer
						movw ZH:ZL, typeH:typeL
						subi ZL, low(-metaSize)
						sbci ZH, high(-metaSize)
						lsr ZL
						lsr ZH
						brcc callCtor
						sbr ZL,0b10000000
						
callCtor:				

						movw PH:PL, RH:RL
						movw AH:AL, RH:RL
						icall
						movw RH:RL, AH:AL
	END_SMALL


 #endif  /* _OBJECT_INC_ */

 