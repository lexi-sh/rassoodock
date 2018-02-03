#!/usr/bin/env bash

for dir in `ls -d *Tests.Unit*/`
do
    cd $dir && dotnet test
    cd ..
done
for dir in `ls -d *Tests.Integration*/`
do
    cd $dir && dotnet test
    cd ..
done