/*
 * Memory.inc
 *
 *  Created: 10/9/2014 7:04:17 PM
 *   Author: AB
 */ 
#ifndef _MEMORY_INC_
#define _MEMORY_INC_
.include "common.inc"
 /*
 input:
 0 - size
 
 uses:
 temp1-4

 output
 X - pointer to allocated object
 returns FFFF if no memory
 */
 .equ blockStatusBit = 7

 .MACRO _MINIT
				ser tempH								; set 0xFF
				ser tempL								; set 0xFF

				cbr tempH, blockStatusBit				; clean block status bit

				ldi XH, high(SRAM_START)
				ldi XL, low(SRAM_START)

				st X+, tempH
				st -X, tempL
 .ENDMACRO
 .MACRO _MALLOC

	.def curBlockH = R16
	.def curBlockL = R17
	
	.def headerH = R18
	.def headerL = R19

	.def memEndH = R20
	.def memEndL = R21

	.def sizeH = R22
	.def sizeL = R23

				ldi sizeH, high(@0)
				ldi sizeL, low(@0)

				ldi memEndH, high(RAMEND)
				ldi memEndL, low(RAMEND)

				;start from first memory block
				ldi curBlockH, high(SRAM_START)
				ldi curBlockL, low(SRAM_START)

				;load memory block header
checkBlock:		movw XH, curBlockH						; load pointer of current memory block to X register

				ld headerH,X+
				ld headerL,-X

				sbrs headerH, blockStatusBit			; if bit is 0
				rjmp tryAllocate						; then try to allocate memory

 jumpToNext:											; else jump to next memory block
				;jumping to next memory block
				cbr headerH, blockStatusBit				; clean block status bit

				add curBlockL, headerL					; increment memory block address
				adc curBlockH, headerH

				cp memEndL, curBlockL					; compare next block address with memoryEnd address
				cpc memEndH, curBlockH

				brcs noMemory							; if curBlock > memEnd (carry is 1)

				rjmp checkBlock							; check block
 
 tryAllocate:	cbr headerH, blockStatusBit				; clean block status bit

				cp headerL, sizeL						; compare block size with requested size
				cpc headerH, sizeH
				
				brcc allocate							; allocate memory if size<=blockSize (carry is 0)
				rjmp jumpToNext							; else jump to next block

 allocate:		sbr sizeH, blockStatusBit				; setup new header
				st X+, sizeH							; store new header
				st -X, sizeL

				sub headerL, sizeL						; setup freeBlock size
				sbc headerH, sizeH						

				tst headerH							; if new size is 0 then exit, else setup new free block
				brne setupFreeBlock
				tst headerL
				breq exit

setupFreeBlock:	add XL, headerL							; setup freeBlock header address
				adc XH, headerH

				cbr headerH, blockStatusBit

				st X+, headerH							; store freeBlock header
				st -X, headerL

				rjmp exit
 noMemory:
				ldi XH, 0xFF							;load NoMemory status
				ldi XL, 0xFF
 exit:

 .ENDMACRO

#endif  /* _MEMORY_INC_ */
