#!/bin/bash

option=""

while :
do
	echo "Choose an option: 
	1) open nano
	2) open vi
	3) open links
	4) exit"

	read option
	
	case "$option" in
		1 ) nano ;;
		2 ) vi ;;
		3 ) links ;;
		4 ) exit 0 ;;
	esac

done