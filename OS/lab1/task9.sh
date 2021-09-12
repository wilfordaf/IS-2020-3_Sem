#!/bin/bash

file="/var/log/*.log"

cat $file | wc -l