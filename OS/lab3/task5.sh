#!/bin/bash

mkfifo pipe
./task5handler.sh & ./task5generator.sh
rm pipe
