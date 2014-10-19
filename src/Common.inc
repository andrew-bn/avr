/*
 * Common.inc
 *
 *  Created: 10/9/2014 7:14:40 PM
 *   Author: user
 */ 

#ifndef _COMMON_INC_
#define _COMMON_INC_
 .def PH = R15
 .def PL = R14
 .def TH = R7
 .def TL = R6
 .def RH = R11
 .def RL = R10
 .def QH = R13
 .def QL = R12

 .equ Ar = 1
 .def AH = R17
 .def AL = R16
 .equ Br = 2
 .def BH = R19
 .def BL = R18
 .equ Cr = 4
 .def CH = R21
 .def CL = R20
 .equ Dr = 8
 .def DH = R23
 .def DL = R22
 .equ Er = 16
 .def EH = R25
 .def EL = R24
 .equ Xr = 32
 .equ Yr = 64
 .equ Zr = 128

 .equ pointerSize = 2

 .macro XPOP ; registers
	.if (@0 & Zr) == Zr
		pop ZH
		pop ZL
	.endif

	.if (@0 & Xr) == Xr
		pop XH
		pop XL
	.endif
	.if (@0 & Er) == Er
		pop EH
		pop EL
	.endif
	.if (@0 & Dr) == Dr
		pop DH
		pop DL
	.endif
	.if (@0 & Cr) == Cr
		pop CH
		pop CL
	.endif
	.if (@0 & Br) == Br
		pop BH
		pop BL
	.endif
	.if (@0 & Ar) == Ar
		pop AH
	.endif
	pop YH
	pop YL
	pop AL
	out SREG,AL
	pop AL
 .endm

 .macro XPUSH ; registers (R16, SREG, YH, YL are pushed always)
	push AL
	in AL,SREG
	push AL
	push YL
	push YH

	.if (@0 & Ar) == Ar
		push AH
	.endif
	.if (@0 & Br) == Br
		push BL
		push BH
	.endif
	.if (@0 & Cr) == Cr
		push CL
		push CH
	.endif
	.if (@0 & Dr) == Dr
		push DL
		push DH
	.endif
	.if (@0 & Er) == Er
		push EL
		push EH
	.endif
	.if (@0 & Xr) == Xr
		push XL
		push XH
	.endif
	.if (@0 & Zr) == Zr
		push ZL
		push ZH
	.endif

	; recalculate Y <= StackPointer
	in YH, SPH
	in YL, SPL
	subi YL, low (-(1+6+((@0 & Ar)==Ar)+((@0 & Br)==Br)*2+((@0 & Cr)==Cr)*2+((@0 & Dr)==Dr)*2+((@0 & Er)==Er)*2+((@0 & Xr)==Xr)*2+((@0 & Zr)==Zr)*2)); SREG, YH, YL, R16, CP
	sbci YH, high(-(1+6+((@0 & Ar)==Ar)+((@0 & Br)==Br)*2+((@0 & Cr)==Cr)*2+((@0 & Dr)==Dr)*2+((@0 & Er)==Er)*2+((@0 & Xr)==Xr)*2+((@0 & Zr)==Zr)*2)); SREG, YH, YL, R16, CP
 .endm

 .macro _STACK_TO_Y

		in YH, SPH
		in YL, SPL

		subi YL, low(-(@0+2+1));plus command pointer and sreg
		sbci YH, high(-(@0+2+1))

 .endm

 .macro _STACK ;@0 stacksize; @1 - memory pointers count

	push R16
	in R16,SREG
	push R16

	.if (@0-@1*2) >= 2
		push R17
	.endif
	.if (@0-@1*2) >= 3
		push R18
	.endif
	.if (@0-@1*2) >= 4
		push R19
	.endif
	.if (@0-@1*2) >= 5
		push R20
	.endif
	.if (@0-@1*2) >= 6
		push R21
	.endif
	.if (@0-@1*2) >= 7
		push R22
	.endif
	.if (@0-@1*2) >= 8
		push R23
	.endif
	.if (@0-@1*2) >= 9
		push R24
	.endif
	.if (@0-@1*2) >= 10
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

 .macro _UNSTACK;@0 stacksize; @1 - memory pointers count

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

	.if (@0-@1*2) >= 10
		pop R25
	.endif
	.if (@0-@1*2) >= 9
		pop R24
	.endif
	.if (@0-@1*2) >= 8
		pop R23
	.endif
	.if (@0-@1*2) >= 7
		pop R22
	.endif
	.if (@0-@1*2) >= 6
		pop R21
	.endif
	.if (@0-@1*2) >= 5
		pop R20
	.endif
	.if (@0-@1*2) >= 4
		pop R19
	.endif
	.if (@0-@1*2) >= 3
		pop R18
	.endif
	.if (@0-@1*2) >= 2
		pop R17
	.endif

	pop R16
	out SREG,R16
	pop R16

 .endm

 .macro BEGIN
	XPUSH Ar+Br+Cr+Dr+Er+Xr+Zr+Yr
	movw ZH:ZL, PH:PL
 .endm
 .macro END
	XPOP Ar+Br+Cr+Dr+Er+Xr+Zr+Yr
	ret
 .endm

 .macro BEGIN_SMALL
	XPUSH Ar+Br+Cr+Xr+Zr+Yr
	movw ZH:ZL, PH:PL
 .endm
 .macro END_SMALL
	XPOP Ar+Br+Cr+Xr+Zr+Yr
	ret
 .endm

 .macro BEGIN_TINY
	XPUSH Ar+Xr+Zr+Yr
	movw ZH:ZL, PH:PL
 .endm
 .macro END_TINY
	XPOP Ar+Xr+Zr+Yr
	ret
 .endm

#endif  /* _COMMON_INC_ */
