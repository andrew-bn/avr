/*
 * clockData.inc
 *
 *  Created: 02.12.2014 22:36:12
 *   Author: AB
 */ 
 #ifndef _CLOCKDATA_INC_
 #define _CLOCKDATA_INC_
 .include "object.inc"
 .CSEG

 .equ clkdata_seconds = _object_size
 .equ clkdata_minutes = clkdata_seconds + 1
 .equ clkdata_hours = clkdata_minutes + 1
 .equ clkdata_error = clkdata_hours + 1
 .equ clkdata_size = clkdata_error + 1

 clockData: CLASS object, clkdata_size
 clkData_ctor:
	BEGIN_TINY
		clr AL
		FIELD_ST clkdata_seconds, AL
		FIELD_ST clkdata_minutes, AL
		FIELD_ST clkdata_hours, AL
		FIELD_ST clkdata_error, AL
	END_TINY

 #endif /* _CLOCKDATA_INC_ */
