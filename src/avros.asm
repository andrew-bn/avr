/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */
 .listmac
.include "main.asm"

.CSEG
.dw 100			// main task stack size
main: jmp main	// main task
jmp main
jmp main
jmp main
