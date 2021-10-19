#!/bin/bash

command="+"
buffer=1
echo "Working in addition mode"

tail -f pipe |
while :
do
	read line
	if [[ "$line" == "+" ]]
	then 
		command="+"
		echo "Switched mode to addition"
		
	elif [[ "$line" == "*" ]]
	then
		command="*"
		echo "Switched mode to multiplication"
	
	elif [[ "$line" == "QUIT" ]]
	then
		killall tail
		echo "Quit"
		exit 0
	
	elif [[ "$line" =~ [0-9]+ ]]
	then
		case $cmd in
			"+")
				buffer=$(echo "$buffer+$line" | bc)
				echo $buffer
			;;
			"*")
				buffer=$(echo "$buffer*$line" | bc)
				echo $buffer
			;;
		esac
		
	else
		killall tail
		echo "Error: handler"
		exit 1
	fi
done
done