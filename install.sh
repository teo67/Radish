./build.sh osx-x64
cp -R $PWD/Radish /usr/local
echo /usr/local/Radish/bin|sudo tee /etc/paths.d/radish;bash -l;echo $PATH
chmod +x /usr/local/Radish/bin/radish