#!/bin/bash

if [[ $# != 1 ]]
then
	echo "Invalid amount of arguments"
	exit 1
fi

if [[ ! -f $1 ]]
then
    echo "File does no exist"
    exit 1
fi

if [[ ! -e "/home/user/.trash" ]]
then
    mkdir /home/user/.trash
fi

filecounter=$(find "/home/user/.trash" -type f -name "[0-9]*" | grep -o -E "[0-9]+" | sort -n | tail -1)
if [ -z $filecounter ];
then
    number=1
else
    number=$(echo "$filecounter+1" | bc)
fi

fileLink="$(readlink -f $1)"

ln "$fileLink" "/home/user/.trash/$number"
echo "$fileLink now is /home/user/.trash/$number" >> /home/user/.trash.log
rm $1