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
	rm -r $home/restore
fi

mkdir $home/restore

for file in $(ls $lastBackupPath | grep -E -v "[0-9]{4}-[0-9]{2}-[0-9]{2}$");
do
	cp "$lastBackupPath/$file" "$home/restore/$file"
	echo "Successfully copied $file"
done
