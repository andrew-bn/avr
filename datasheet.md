#AVR 

[Assembler][asm]

[Task definition][task-def]


##Task definition
##Assembler
###Macro
Dirrective    |Description  | Example|
--------------|-------------|--------|
comments      ||; or // or /**/
.include      |insert text from another file
.def          |processor resource synonim (e.g Port or Register)|.def myname = R0
.undef        |remove synonim definition|.undef myname
.equ          |integer or symbolic constant| .equ Const = 5
.CSEG         |code segment| 
.ORG          |shift segment start point| .ORG 0x00FF
.db           | bytes array| Const: .db 10, 11
.dw           |words array| Words: .dw 10,11,12
.dd           |double words array|
.dq           |two double words array|
.DSEG         |data segment|
.BYTE         |some data in datasegment|var1: .BYTE <length in bytes>
.EESEG        |EEPROM segment|
.MACRO        |macros|


[task-def]: #task-definition
[asm]: #assembler
