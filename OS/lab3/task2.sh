#!/bin/bash

report="/home/user/report"

at now +2 minutes <<ENDMARKER
./task1.sh
tail -f "$report"
ENDMARKER