#!/bin/bash

file="/var/log/anaconda/X.log"
out="/home/user/lab1/full.log"

grep "] (WW)" "$file" | sed "s/(WW)/Warning:/" >"$out"
grep "] (II)" "$file" | sed "s/(II)/Information:/" >"$out"

cat "$out"