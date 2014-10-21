/*
 * avros.asm
 *
 *  Created: 08.10.2014 21:33:05
 *   Author: AB
 */
 .listmac
 .include "kernel.inc"
 
.CSEG
jmp avr_start
.include "emptyTask.inc"
avr_start:
 TASK_ADD emptyTask
 ;TASK_ADD emptyTask
 call kernel_start

tmp2:				rjmp tmp2



