#!/bin/bash

if [[ $# != 1 ]]
then
	echo "Invalid amount of arguments"
	exit 1
fi

home="/home/user"
trash="$home/.trash"
trashLog="$home/.trash.log"

if [[ ! -d $trash ]]
then
    echo "Directory does not exist"
    exit 1
fi

if [[ ! -f $trashLog ]]
then
    echo "Log file does not exist"
    exit 1
fi

invalidSymbols=$(echo $1 | sed 's/[a-z,0-9,_,-]*//')
invalidSymbolsLength=${#invalidSymbols}

if [ ! $invalidSymbolsLength -eq 0 ]
then
    echo "Invalid symbols in filename"
    exit 1
fi

for oldFilePath in $(grep "/$1" $trashLog | awk '{print $1}')
do
	newFilePath=$(grep "$oldFilePath" $trashLog | awk '{print $4}')
	read -p "$oldFilePath Do you want to restore this file? [y/N] " answer

	if [[ $answer != "y" ]]
	then
		continue
	fi

	restoreDirectory=$(echo $oldFilePath | awk 'BEGIN{FS=OFS="/"}; {$NF=""; print $0}')
	fileName=$(echo $oldFilePath | awk 'BEGIN{FS="/"}; {print $NF}')

	if [[ -d $restoreDirectory ]]
	then
		if [[ -f "$oldFilePath" ]] || [[ -d "$oldFilePath" ]]
		then
			read -p "File or directory with name $fileName already exists, enter new name: " newName
			ln "$newFilePath" "$restoreDirectory/$newName"
		else
			ln "$newFilePath" "$oldFilePath"
		fi
		rm "$newFilePath"
	else
		echo "Directory $restoreDirectory does not exist, $fileName will be restored in $home"
		if [[ -f "$home/$fileName" ]]
		then
			read -p "File with name $fileName already exists, enter new name: " newName
			ln "$newFilePath" "$home/$newName"
		else
			ln "$newFilePath" "$home/$fileName"
		fi
		rm "$newFilePath"
	fi

	sed -ie "s~.*$newFilePath.*~~" $trashLog
	sed -i "/^$/d" $trashLog
done
