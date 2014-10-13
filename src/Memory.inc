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
 .set stackSize = 3
 .def temp = R16
					push XL
					push XH
					push temp

					ldi XH, high(SRAM_START)
					ldi XL, low(SRAM_START)

					/*clr temp
					st X+, temp*/

					; dw next block pointer (ramend)
					ldi temp, low(RAMEND)
					st X+, temp
					ldi temp, high(RAMEND)
					st X+, temp

					;next dw is prev==next - block is free
					ldi temp, low(RAMEND)
					st X+, temp
					ldi temp, high(RAMEND)
					st X+, temp

					pop temp
					pop XH
					pop XL

 ret

 memory_allocate:; dw size
 .set paramSize = 2
 .set stackSize = 14

 .def nextL = R18
 .def nextH = R19

 .def prevPrevL = R20
 .def prevPrevH = R21

 .def sizeL = R22
 .def sizeH = R23

 .def prevL = R16
 .def prevH = R17

 .def curL = R24
 .def curH = R25

				_STACK stackSize, 2
				_STACK_TO_Y stackSize + paramSize

				; load size from stack
				ld sizeL, Y
				ld sizeH, -Y

				; start from the begining but skip first byte (invalid pointer reference)
				ldi nextH, high(SRAM_START)
				ldi nextL, low(SRAM_START)

				clr curH
				clr curL

checkBlock:
				; if end of memory, then NoMemory error

				ldi prevPrevH, high(RAMEND)
				ldi prevPrevL, low(RAMEND)

				cp prevPrevL, nextL
				cpc prevPrevH, nextH

				brcs noMemory

				; goto current block
				movw XH:XL, nextH:nextL
				; store pointer to previous block
				movw prevPrevH:prevPrevL,curH:curL
				; store pointer to current block
				movw curH:curL, nextH:nextL
				
				; load pointers to next block and prev block
				ld nextL, X+
				ld nextH, X+
				ld prevL, X+
				ld prevH, X+

				; if prev == next then block is free

				cp prevL, nextL
				brne checkBlock
				cp prevH, nextH
				brne checkBlock

				; try to allocate
 tryAllocate:
				; calculate newNext pointer to X

				add XL, sizeL
				adc XH, sizeH

				; compare new pointer with next

				cp nextL, XL
				cpc nextH, XH
				
				; if X>next, then check next block else allocate
				brcs checkBlock

 allocate:
				; if next block is equal to next pointer then just set Prev equal to prevPrev
				subi nextL,low(headerSize)
				sbci nextH, high(headerSize)

				cp XL, nextL
				cpc XH, nextH

				brcs allocateNew

				movw XH:XL, curH:curL

				subi XL, low(-pointerSize)
				sbci XH, high(-pointerSize)

				st X+ , prevPrevL
				st X , prevPrevH
				movw XH:XL, curH:curL

				rjmp exit

 allocateNew:	
				subi nextL, low(-headerSize)
				sbci nextH, high(-headerSize)
				; set x.next = x.prev = next - free block
				st X+, nextL
				st X+, nextH
				st X+, nextL
				st X+, nextH

				; set Prev pointer equal to PrevPrev
				
				movw nextH:nextL, XH:XL
				subi nextL, low(headerSize)
				sbci nextH, high(headerSize)

				movw XH:XL, curH:curL
				st X+, nextL
				st X+, nextH

				st X+, prevPrevL
				st X, prevPrevH
				movw XH:XL, curH:curL

				rjmp exit
 noMemory:
				clr XH
				clr XL
 exit:
				subi XL, low(-headerSize)
				sbci XH, high(-headerSize)
				movw retH:retL, XH:XL

				_UNSTACK stackSize, 2
 ret

 memory_destroy: ; dw pointer
 .set paramSize = 2
 .set stackSize = 10+4

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

				_STACK stackSize,2
				_STACK_TO_Y stackSize + paramSize

				; load pointer from stack
				ld curL, Y
				ld curH, -Y

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

 destr_exit:	_UNSTACK stackSize,2

 ret
#endif  /* _MEMORY_INC_ */
