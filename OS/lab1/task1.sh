#!/bin/bash

num1=$1
num2=$2
num3=$3
max="$num1"

if [[ "$max" -lt "$num2" ]];
then max="$num2"
fi

if [[ "$max" -lt "$num3" ]];
then max="$num3"
fi

echo "$max"