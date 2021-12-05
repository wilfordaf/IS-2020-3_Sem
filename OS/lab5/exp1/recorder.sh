#!/bin/bash

parameters="/home/user/OS/lab5/exp1/parameters.log"
./mem.sh&pid=$!

rm $parameters

while :
do
	echo "--------------------------" >> $parameters
	date +"%T" >> $parameters
	echo -e "\nSystem parameters:" >> $parameters
	top -b -n 1 | head -4 | tail -n +4 >> $parameters
	top -b -n 1 | head -5 | tail -n +5 >> $parameters
	echo -e "\nmem.sh parameters:" >> $parameters
	top -p "$pid" -b -n 1 | head -8 | tail -n +8 >> $parameters
	echo -e "\nTop 5 processes parameters:" >> $parameters
	top -b -n 1 | head -12 | tail -5 >> $parameters
	sleep 1
done
