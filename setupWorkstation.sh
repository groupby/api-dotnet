#!/bin/bash

# install mono
sudo apt-get update
sudo apt-get install -y mono-complete monodevelop nunit-console

# update certificates
mozroots --import --sync

# install nuget
wget https://nuget.org/nuget.exe
sudo mv nuget.exe /usr/bin/nuget.exe
echo "mono --runtime=v4.0 /usr/bin/nuget.exe \${@}" | sudo tee /usr/bin/nuget > /dev/null
sudo chmod +x /usr/bin/nuget
echo "nunit-console -framework=4.0 \${@}" | sudo tee /usr/bin/nunit-console4 > /dev/null
sudo chmod +x /usr/bin/nunit-console4
