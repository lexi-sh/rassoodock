#!/usr/bin/env bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
CD $DIR/../
dotnet build

find $DIR/../test -not -path '*.TestCommon*' -name '*.csproj' -print0 | xargs -L1 -0 -P 8 dotnet test --no-build

if [ "$?" !=  "0" ] ; then
    echo -e "######### TEST FAILED #########"
fi