#!/bin/bash

version="${1}"

nuget push "target/GroupByInc.Api.${version}.nupkg" "${NUGET_API_KEY}"
