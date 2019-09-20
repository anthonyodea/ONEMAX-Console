@echo off

TITLE script.bat

SET /a popsize=100
SET /a mutrateUnits=0
SET /a mutrateTenths=0
SET /a mutrateHundredths=0

SET /a repetitions=10

SET popname=Popsize_%popsize%_Mutrate_%mutrateUnits%.%mutrateTenths%%mutrateHundredths%

FOR /L %%y IN (1, 1, %repetitions%) DO (
    CLS
    ECHO Progress: %%y/%repetitions%
    ONEMAX-Console.exe %popsize% 30 %mutrateUnits%.%mutrateTenths%%mutrateHundredths% 2 %popname%
)

PAUSE