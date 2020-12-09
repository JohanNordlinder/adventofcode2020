#!/bin/bash
echo "Enter day number"
read DAY_NUMBER
sed 's/\${DAY_NUMBER}/'"${DAY_NUMBER}"'/' d0p1.cs.TEMPLATE > d${DAY_NUMBER}p1.cs
touch d_${DAY_NUMBER}.txt

echo "Enter number of test inputs"
read TEST_COUNT
for ((i = 1; i <= TEST_COUNT; i++))
do 
	touch d_${DAY_NUMBER}_t_${i}.txt
done
