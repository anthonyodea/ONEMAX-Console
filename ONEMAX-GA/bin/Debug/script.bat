@echo off

SET /a popsize=400
SET /a mutrateUnits=0
SET /a mutrateTenths=0
SET /a mutrateHundredths=1

SET /a repetitions=500

SET popname=Popsize_%popsize%_Mutrate_%mutrateUnits%.%mutrateTenths%%mutrateHundredths%

FOR /L %%y IN (1, 1, %repetitions%) DO (
    CLS
    ECHO Progress: %%y/%repetitions%
    ONEMAX-Console.exe %popsize% 30 %mutrateUnits%.%mutrateTenths%%mutrateHundredths% 2 %popname%
)

PAUSE