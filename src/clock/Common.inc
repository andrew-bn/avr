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
 .def SH = R9
 .def SL = R8
 .def TH = R7
 .def TL = R6
 .def RH = R11
 .def RL = R10
 .def QH = R13
 .def QL = R12
 .def MIStat = R5


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

	; recalculate Y <= StackPointer
	in YH, SPH
	in YL, SPL
	subi YL, low (-7)
	sbci YH, high(-7)


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

 .macro XCLI
	cli
	dec MIStat
 .endm

 .macro XSEI
	tst MIStat
	in TL, SREG
	sbrc TL, 1
	sei
	inc MIStat
 .endm
#endif  /* _COMMON_INC_ */
