/*
 * Common.inc
 *
 *  Created: 10/9/2014 7:14:40 PM
 *   Author: user
 */ 

#ifndef _COMMON_INC_
#define _COMMON_INC_

 .def retH = R15
 .def retL = R14

 .macro _STACK_TO_Y

		in YH, SPH
		in YL, SPL

		subi YL, low(-(@0+2+@1+1));plus command pointer and sreg
		sbci YH, high(-(@0+2+@1+1))

 .endm

 .macro _PUSH_COMMON

	push R16
	in R16,SREG
	push R16

	.if @0 >= 2
		push R17
	.endif
	.if @0 >= 3
		push R18
	.endif
	.if @0 >= 4
		push R19
	.endif
	.if @0 >= 5
		push R20
	.endif
	.if @0 >= 6
		push R21
	.endif
	.if @0 >= 7
		push R22
	.endif
	.if @0 >= 8
		push R23
	.endif
	.if @0 >= 9
		push R24
	.endif
	.if @0 >= 10
		push R25
	.endif

		
	.if @1 >= 1
		push XL
		push XH
	.endif

	.if @1 >= 2
		push YL
		push YH
	.endif

	.if @1 >= 3
		push ZL
		push ZH
	.endif
 .endm

 .macro _POP_COMMON

	.if @1 >= 3
		pop ZH
		pop ZL
	.endif

	.if @1 >= 2
		pop YH
		pop YL
	.endif

	.if @1 >= 1
		pop XH
		pop XL
	.endif
	.if @0 >= 10
		pop R25
	.endif
	.if @0 >= 9
		pop R24
	.endif
	.if @0 >= 8
		pop R23
	.endif
	.if @0 >= 7
		pop R22
	.endif
	.if @0 >= 6
		pop R21
	.endif
	.if @0 >= 5
		pop R20
	.endif
	.if @0 >= 4
		pop R19
	.endif
	.if @0 >= 3
		pop R18
	.endif
	.if @0 >= 2
		pop R17
	.endif

	pop R16
	out SREG,R16
	pop R16

 .endm

#endif  /* _COMMON_INC_ */
