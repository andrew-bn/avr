/*
 * Memory.inc
 *
 *  Created: 10/9/2014 7:04:17 PM
 *   Author: AB
 */ 

.include "common.inc"
 /*
 input:
 0 - size
 
 uses:
 temp

 output
 X - pointer to allocated object
 returns FFFF if no memory
 */
 .equ blockStatusBit = 7
 .equ headerSize = 2

 .MACRO Memory_New

	.def curBlockH = tempH
	.def curBlockL = tempL
	
	.def headerH = temp2H
	.def headerL = temp2L

	.def memEndH = temp3H
	.def memEndL = temp3L

	.def sizeH = temp4H
	.def sizeL = temp4L

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
				ld headerL,X

				sbrs headerH, blockStatusBit			; if bit is 0
					rjmp tryAllocate					; then try to allocate memory

 jumpToNext:											; else jump to next memory block
				
				;jumping to next memory block
				cbr headerH, blockStatusBit				; clean block status bit

				add curBlockL, headerL					; increment memory block address
				adc curBlockH, headerH

				cp memEndL, curBlockL					; compare next block address with memoryEnd address
				cpc memEndH, curBlockH

				brcs noMemory							; if curBlock > memEnd (carry is 1)

				rjmp checkBlock							; check block
 
 tryAllocate:
				cbr headerH, blockStatusBit				; clean block status bit

				cp headerL, sizeL						; compare block size with requested size
				cpc headerH, sizeH
				
				brcc allocate							; allocate memory if size<=blockSize (carry is 0)
				rjmp jumpToNext							; else jump to next block

 allocate:		rjmp exit
 noMemory:
				ldi XH, 0xFF							;load NoMemory status
				ldi XL, 0xFF
 exit:

 .ENDMACRO
