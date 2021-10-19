#!/bin/bash

./task6handler.sh & pid=$!
./task6generator.sh $pid