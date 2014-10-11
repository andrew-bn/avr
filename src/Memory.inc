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
 .equ pointerSize = 2
 .CSEG
 memory_init:
 .set stackSize = 3
 .def temp = R16
					push XL
					push XH
					push temp

					ldi XH, high(SRAM_START)
					ldi XL, low(SRAM_START)

					; first byte is 0
					clr temp
					st X+, temp

					; next dw is next block pointer (ramend)
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

				_PUSH_COMMON stackSize-4, 2 ; push R16-R23, X, Y
				_STACK_TO_Y stackSize, paramSize

				; load size from stack
				ld sizeL, Y
				ld sizeH, -Y

				; start from the begining but skip first byte (invalid pointer reference)
				ldi nextH, high(SRAM_START+1)
				ldi nextL, low(SRAM_START+1)

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

				_POP_COMMON stackSize-4, 2, paramSize ; push R16-R23, X, Y
 ret

#endif  /* _MEMORY_INC_ */
