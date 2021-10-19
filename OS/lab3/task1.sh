#!/bin/bash

test="/home/user/test"
report="/home/user/report"
datetime=$(date '+%F_%T')
date="/home/user/$datetime"

mkdir "$test" && echo "catalog was created succesfully" > "$report" ; touch "$datetime"
ping net_nikogo.ru > /dev/null 2>&1 || echo "$datetime host is unavailable" >> "$report" 