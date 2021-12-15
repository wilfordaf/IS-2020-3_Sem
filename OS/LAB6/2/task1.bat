mkdir "C:/LAB6/2/temp"
net share tempResoure=%~dp0temp
net use * \\%computername%\%tempResoure%
