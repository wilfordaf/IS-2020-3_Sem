#!/bin/bash

./task4loop.sh & pid1=$!
./task4loop.sh & pid2=$!
./task4loop.sh & pid3=$!

top -b -n 1 | head -10 | tail -3 | awk '{print $1 " " $9}'

niceness=$(top -b -n 1 | grep "$pid1" | awk '{print $9}')
cnt=0
condition=$(echo "$niceness > 10.0" | bc -l)

while [ "$condition" -eq 1 ]
do
	cnt=$(echo "$cnt+1" | bc)
	renice +$cnt "$pid1"
	niceness=$(top -b -n 1 | grep "$pid1" | awk '{print $9}')
	condition=$(echo "$niceness > 10.0" | bc -l)
done

kill "$pid3"

top -b -n 1 | head -10 | tail -3 | awk '{print $1 " " $9}'

kill "$pid1"
kill "$pid2"