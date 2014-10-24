/*
 * Memory.inc
 *
 *  Created: 10/9/2014 7:04:17 PM
 *   Author: AB
 */ 
#ifndef _MEMORY_INC_
#define _MEMORY_INC_
 .include "common.inc"
 .equ headerSize = 4

 .CSEG
 memory_init:
	BEGIN_TINY
	XCLI
		ldi XH, high(SRAM_START)
		ldi XL, low(SRAM_START)

		; dw next block pointer (ramend)
		ldi AL, low(RAMEND)
		st X+, AL
		ldi AL, high(RAMEND)
		st X+, AL

		;next dw is prev==next - block is free
		ldi AL, low(RAMEND)
		st X+, AL
		ldi AL, high(RAMEND)
		st X+, AL
	XSEI
	END_TINY

 memory_allocate:; dw size
 // A - cur, B - next, C - prev, D -prev.prev, E - size
		XCLI
				BEGIN

				; load size from stack
				ld EL, Y+
				ld EH, Y

				; start from the begining but skip first byte (invalid pointer reference)
				ldi BH, high(SRAM_START)
				ldi BL, low(SRAM_START)

				clr AH
				clr AL

checkBlock:
				; if end of memory, then NoMemory error

				ldi DH, high(RAMEND)
				ldi DL, low(RAMEND)

				cp DL, BL
				cpc DH, BH

				brcs noMemory

				; goto current block
				movw XH:XL, BH:BL
				; store pointer to previous block
				movw DH:DL,AH:AL
				; store pointer to current block
				movw AH:AL, BH:BL
				
				; load pointers to next block and prev block
				ld BL, X+
				ld BH, X+
				ld CL, X+
				ld CH, X+

				; if prev == next then block is free

				cp CL, BL
				brne checkBlock
				cp CH, BH
				brne checkBlock

				; try to allocate
 tryAllocate:
				; calculate newNext pointer to X

				add XL, EL
				adc XH, EH

				; compare new pointer with next

				cp BL, XL
				cpc BH, XH
				
				; if X>next, then check next block else allocate
				brcs checkBlock

 allocate:
				; if next block is equal to next pointer then just set Prev equal to prevPrev
				subi BL,low(headerSize)
				sbci BH, high(headerSize)

				cp XL, BL
				cpc XH, BH

				brcs allocateNew

				movw XH:XL, AH:AL

				subi XL, low(-pointerSize)
				sbci XH, high(-pointerSize)

				st X+ , DL
				st X , DH
				movw XH:XL, AH:AL

				rjmp exit

 allocateNew:	
				subi BL, low(-headerSize)
				sbci BH, high(-headerSize)
				; set x.next = x.prev = next - free block
				st X+, BL
				st X+, BH
				st X+, BL
				st X+, BH

				; set Prev pointer equal to PrevPrev
				
				movw BH:BL, XH:XL
				subi BL, low(headerSize)
				sbci BH, high(headerSize)

				movw XH:XL, AH:AL
				st X+, BL
				st X+, BH

				st X+, DL
				st X, DH
				movw XH:XL, AH:AL

				rjmp exit
 noMemory:
				clr XH
				clr XL
 exit:
				subi XL, low(-headerSize)
				sbci XH, high(-headerSize)

				movw RH:RL, XH:XL
XSEI
	END

 memory_destroy:
 .def curH = R17
 .def curL = R16
 .def nextL = R18
 .def nextH = R19
 .def prevL = R20
 .def prevH = R21
 .def tempNextL = R22
 .def tempNextH = R23
 .def tempPrevL = R24
 .def tempPrevH = R25
	
	BEGIN
	XCLI
				; load pointer from stack
				movw curH:curL, PH:PL

				subi curL, low(headerSize)
				sbci curH, high(headerSize)

				; load pointers to Next and Prev
				movw XH:XL, curH:curL
				ld nextL, X+
				ld nextH, X+
				ld prevL, X+
				ld prevH, X+

				; load next.Next and next.Prev
				movw XH:XL, nextH:nextL
				ld tempNextL, X+
				ld tempNextH, X+
				ld tempPrevL, X+
				ld tempPrevH, X+

				; if next block is free, so cur.Next = next.Next, else checkPrev
				cp tempNextL, tempPrevL
				brne checkPrev
				cp tempNextH, tempPrevH
				brne checkPrev

				movw nextH:nextL, tempNextH:tempNextL

 checkPrev:

				; if begining of memory then free current else continue checking (checkPrev2)

				tst prevH
				brne checkPrev2
				tst prevL
				breq freeCur

				; load prev.Next and prev.Prev
checkPrev2:		movw XH:XL, prevH:prevL
				ld tempNextL, X+
				ld tempNextH, X+
				ld tempPrevL, X+
				ld tempPrevH, X+

				; if prev block is NOT free (prev.Next != prev.Prev), then freeCur, else prev.Next = prev.Prev = next
				cp tempNextL, tempPrevL
				brne freeCur
				cp tempNextH, tempPrevH
				brne freeCur

				movw curH:curL, prevH:prevL

freeCur:		movw XH:XL, curH:curL
				st X+, nextL
				st X+, nextH
				st X+, nextL
				st X+, nextH

				cpi nextH, high(RAMEND)
				brne editNext
				cpi nextL, low(RAMEND)
				breq destr_exit

editNext:		movw XH:XL, nextH:nextL
				subi XL, low(-pointerSize)
				sbci XH, high(-pointerSize)
				st X+, curL
				st X+, curH

 destr_exit:
 XSEI
END

#endif  /* _MEMORY_INC_ */
