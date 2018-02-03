#!/usr/bin/env bash
for dir in PARENT/*
do
    cd $dir && dotnet test
done