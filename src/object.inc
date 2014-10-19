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

 .equ metaSize = 4;

 .def thisH = R31
 .def thisL = R30

 .macro CLASS
	.dw @1
	.dw (@0 * 2)
 .endm
 .macro VCALL ; pointer to object; offcet of method pointer
	push ZH
	push ZL
	push AH
	push AL
	movw thisH:thisL, @0 ; load this
	FIELD_WLD @1, AH, AL
	movw PH:PL, thisH:thisL
	movw ZH:ZL, AH: AL
	pop AL
	pop AH
	icall
	pop ZL
	pop ZH
 .endm
 .macro THIS
	XCALL thisH:thisL, @0
 .endm
 .macro XCALL ; pointer to object; method
	movw PH:PL, @0
	call @1
 .endm

 .macro XCALLW ; pointer to object; method; Wparam
	movw PH:PL, @0
	push @2
	push @3
	call @1
	pop TL
	pop TL
 .endm

 .macro XCALLIW ; pointer to object; method; Wparam

	mov TL, AL
	ldi AL, high(@2)
	push AL
	ldi AL, low(@2)
	push AL
	mov AL, TL

	movw PH:PL, @0
	call @1
	pop TL
	pop TL
 .endm

 .macro NEW ; memory pointer

		mov TL, AL
		ldi AL, low(2*@0)
		mov PL, AL
		ldi AL, high(2*@0)
		mov PH, AL
		mov AL, TL

		call _object_new
 .endm
 .macro DEL ; memory pointer
	VCALL @0, object_destroy
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

 .macro FIELD_WLD; @0 - field offcet; @1 - regH,@2 - regL
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

 .macro FIELD_WST; @0 - field offcet; @1 - regH,@2 - regL
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
  .macro FIELD_WSTI; @0 - field offcet; @1 wconst
	mov TL, AL
	.if @0 <= 0x3E
		ldi AL, low(@1)
		std Z+@0, AL
		ldi AL, high(@1)
		std Z+1+@0, AL
	.else
		subi ZL, low(-@0)
		sbci ZH, high(-@0)
		ldi AL, low(@1)
		st Z, AL
		ldi AL, high(@1)
		std Z+1, AL
		subi ZL, low(@0)
		sbci ZH, high(@0)
	.endif
	mov AL, TL
 .endm
  .macro FIELD_ST; @0 - field offcet; @1 - regH,@2 - regL
	.if @0 <= 0x3E
		std Z+@0, @1
	.else
		subi ZL, low(-@0)
		sbci ZH, high(-@0)
		st Z, @1
		subi ZL, low(@0)
		sbci ZH, high(@0)
	.endif
 .endm
 .macro OVERRIDE
	FIELD_WSTI @0, @1
 .endm
 .macro SETTER
		BEGIN_TINY
				PARAM_WLD 0x00, AH, AL
				FIELD_WST @0, AH, AL
		END_TINY
 .endm

 .macro GETTER
		BEGIN_TINY
				FIELD_WLD @0, AH, AL
				movw RH:RL,AH:AL
		END_TINY
 .endm

 .CSEG

 .equ object_destroy = 0x0
 .equ _object_size = object_destroy+pointerSize

 object: CLASS 0x00, _object_size

 object_ctor:
	BEGIN_TINY
		OVERRIDE object_destroy, object_destructor
	END_TINY

 object_destructor:
		call memory_destroy
	ret

 _object_new:
	BEGIN_TINY
						movw ZH:ZL, PH:PL

						; load size to temp
						LPM AL, Z+
						LPM AH, Z+

						;allocate memory
						push AH
						push AL
						call memory_allocate
						pop AL
						pop AH

						;load ctor pointer
						movw ZH:ZL, PH:PL
						subi ZL, low(-metaSize)
						sbci ZH, high(-metaSize)
						lsr ZL
						lsr ZH
						brcc callCtor
						sbr ZL,0b10000000

callCtor:				
						
						movw PH:PL, RH:RL
						movw AH:AL, RH:RL
						XCALL AH:AL, object_ctor
						icall
						movw RH:RL, AH:AL
	END_TINY


 #endif  /* _OBJECT_INC_ */

 