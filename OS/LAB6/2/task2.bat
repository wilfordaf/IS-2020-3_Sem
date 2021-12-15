forfiles /p "C:\Windows" /c "cmd /c if @fsize GEQ 1048576 copy @path %~dp0temp /Z"
