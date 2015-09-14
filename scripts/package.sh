#!/bin/bash

version=${1}
bindle_version=${2}
release="target/GroupByInc.Api/Release/"

nuget pack GroupByInc.Api/GroupByInc.Api.nuspec \
 -Verbose \
 -NonInteractive \
 -IncludeReferencedProjects \
 -Version "${version}" \
 -OutputDirectory "target" \
 -BasePath "${release}" \
 -Properties "bindleVersion=${bindle_version}"
