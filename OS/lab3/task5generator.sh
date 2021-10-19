#!/bin/bash

while :
do
	read line
	echo "$line" > pipe

	if [[ "$line" == "QUIT" ]];
	then
		echo "Finish!"
		exit 0
	fi

	if [[ ! "$line" =~ [0-9]+ ]] && [ "$line" != "+" ] && [ "$line" != "*" ]
	then
		echo "Error command: generator"
		exit 1
	fi
done
