#!/bin/bash

file="/var/log/anaconda/syslog"
info="/home/user/lab1/info.log"

sed -n '/ INFO.*/p' "$file" >"$info"