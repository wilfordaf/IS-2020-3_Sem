#!/bin/bash

out="/home/user/lab1/emails.lst"

grep "[[:alnum:]]\+@[.[:alnum:]]\+" /etc/* |
grep -o "[[:alnum:]]\+@[.[:alnum:]]\+" |
tr '\n' ',' >"$out"