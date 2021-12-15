SC stop Dnscache
TIMEOUT /T 1
SC query type= service state= all | FIND "SERVICE_NAME" > servicesUpd
call task3.bat
SC start Dnscache

