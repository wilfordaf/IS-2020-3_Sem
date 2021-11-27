#!/bin/bash

home="/home/user"
report="$home/.backup-report"

todayDate=$(date "+%Y-%m-%d")
todayTime=$(date -d "$todayDate" "+%s")

lastBackup=$(ls $home | grep -E "^Backup-" | sort -n | tail -1)
lastBackupDate=$(echo $lastBackup | grep -E -o "[0-9]{4}-[0-9]{2}-[0-9]{2}$")
lastBackupTime=$(date -d "$lastBackupDate" +"%s")

timeDifference=$(echo "($todayTime - $lastBackupTime) / 60 / 60 / 24" | bc)

if [ "$timeDifference" -gt 7 ] || [[ -z "$lastBackup" ]]
then
	mkdir "$home/Backup-${todayDate}"
	echo "Created directory $home/Backup-${todayDate}" >> $report
	find "$home/source" -type f -print0 | while read -d $'\0' filePath
    do
		file=$(echo "$filePath" | awk 'BEGIN{FS="/"}; {print $NF}')
	    cp "$home/source/$file" "$home/Backup-${todayDate}"
		echo "Copied $file" >> $report
    done
	
else
	changes=""
	find "$home/source" -type f -print0 | while read -d $'\0' filePath
	do
		file=$(echo "$filePath" | awk 'BEGIN{FS="/"}; {print $NF}')
		if [[ ! -f "$home/$lastBackup/$file" ]]
		then
			cp "$home/source/$file" "$home/$lastBackup"
			changes="$changes\n$file was added"
		else
			sourceSize=$(wc -c "$home/source/$file" | awk '{print $1}')
			backupSize=$(wc -c "$home/$lastBackup/$file" | awk '{print $1}')
			sizeDifference=$(echo "$sourceSize - $backupSize" | bc)
			if [[ $sizeDifference != 0 ]]
			then
				mv "$home/$lastBackup/$file" "$home/$lastBackup/$file.${todayDate}"
				cp "$home/source/$file" "$home/$lastBackup"
				changes="$changes\n$file is new version of $file.${todayDate}"
			fi
		fi
	done
fi

changes=$(echo $changes | sed 's/^\\n//')
if [[ ! -z "$changes" ]]
then
	echo -e "$lastBackupDate Updated:\n${changes}" >> $report
fi
