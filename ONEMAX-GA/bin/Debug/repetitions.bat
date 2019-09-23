@echo off

TITLE repetitions.bat

CALL :GET_POP_PARAMETERS
CALL :GET_MUT_PARAMETERS

SET /p repetitions=Input the number of repetitions: 

REM SET /a popLower=100
REM SET /a popUpper=500
REM SET /a popIncrement=100
REM SET /a mutLower=5
REM SET /a mutUpper=10
REM SET /a mutIncrement=1
REM SET /a repetitions=5

CALL :POP_LOOP

PAUSE
EXIT /B

REM ---------- SUBROUTINE DEFINITIONS ----------

:GET_POP_PARAMETERS
    ECHO ----- POPULATION SIZE -----
    SET /p popLower=Enter the lower bound: 
    SET /p popUpper=Enter the upper bound: 
    SET /p popIncrement=Enter the increment: 

    SETLOCAL
    SET /a modResult=(popUpper-popLower)%%popIncrement
    IF NOT %modResult% EQU 0 (
        ECHO Error: The increment does not evenly divide the boundaries
        CALL :GET_POP_PARAMETERS
    )
    ENDLOCAL
    ECHO.
    EXIT /B

:GET_MUT_PARAMETERS
    ECHO ----- MUTATION RATE -----
    ECHO Note: Enter mutation rates as hundredths of a percent (i.e. input '1' for 0.00001)
    SET /p mutLower=Enter the lower bound: 
    SET /p mutUpper=Enter the upper bound: 
    SET /p mutIncrement=Enter the increment: 

    SETLOCAL
    SET /a modResult=(mutUpper-mutLower)%%mutIncrement
    IF NOT %modResult% EQU 0 (
        ECHO Error: The increment does not evenly divide the boundaries
        CALL :GET_MUT_PARAMETERS
    )
    ENDLOCAL
    ECHO.
    EXIT /B

:POP_LOOP
    FOR /L %%x in (%popLower%, %popIncrement%, %popUpper%) DO (
        CALL :MUT_LOOP %%x
    )
    EXIT /B

:MUT_LOOP
    FOR /L %%y in (%mutLower%, %mutIncrement%, %mutUpper%) DO (
        SETLOCAL ENABLEDELAYEDEXPANSION
        
        SET mutPastPointStr=000%%y
        IF %%y GEQ 10 SET mutPastPointStr=00%%y
        IF %%y GEQ 100 SET mutPastPointStr=0%%y
        IF %%y GEQ 1000 SET mutPastPointStr=%%y

        SET popname=Popsize_%1_Mutrate_0.!mutPastPointStr!

        CALL :REP_LOOP %1
    )
    EXIT /B

:REP_LOOP
    FOR /L %%x IN (1, 1, %repetitions%) DO (
        CLS
        ECHO !popname!
        ECHO %%x/%repetitions%
        ONEMAX-Console.exe %1 30 0.!mutPastPointStr! 2 !popname!    
    )
    EXIT /B