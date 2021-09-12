#!/bin/bash

string=""
answer=""

while [[ "$string" != "q" ]];
do
	answer="$answer$string"
	read string
done

echo "$answer"