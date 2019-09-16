@echo off

SET /a popsize=500
SET /a mutrateUnits=0
SET /a mutrateTenths=0
SET /a mutrateHundredths=1

SET popname=Popsize_%popsize%_Mutrate_%mutrateUnits%.%mutrateTenths%%mutrateHundredths%

FOR /L %%y IN (1, 1, 200) DO (
    ONEMAX-Console.exe %popsize% 30 %mutrateUnits%.%mutrateTenths%%mutrateHundredths% 2 %popname%
)

PAUSE