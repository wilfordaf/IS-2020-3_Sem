#!/bin/bash

buffer=1
cmd="+"

TERM()
{
	echo "Finished working"
	exit 0
}

USR1()
{
	cmd="+"
}

USR2()
{
	cmd="*"
}

trap 'TERM' SIGTERM
trap 'USR1' USR1
trap 'USR2' USR2

while :
do
	case "$cmd" in
		"+")
			buffer=$(echo "$buffer+2" | bc)
		;;
		"*")
			buffer=$(echo "$buffer*2" | bc)
		;;
	esac
	echo "$buffer"
	sleep 1
done