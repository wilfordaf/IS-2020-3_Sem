#!/bin/bash

home="/home/user"
lastBackup=$(ls $home | grep -E "^Backup" | sort -n | tail -1)

if [[ -z "$lastBackup" ]]
then
	echo "There are no backups in $home"
	exit 1
fi

lastBackupPath="$home/$lastBackup"

if [[ -e $home/restore ]]
then
	rm -rf $home/restore
fi

mkdir $home/restore

find "$lastBackupPath" -type f -print0 | while read -d $'\0' filePath
do
	file=$(echo "$filePath" | awk 'BEGIN{FS="/"}; {print $NF}')
	if [[ $file =~ "*[0-9]{4}-[0-9]{2}-[0-9]{2}$" ]]
	then continue
	fi

	cp "$lastBackupPath/$file" "$home/restore/$file"
	echo "Successfully copied $file"
done
