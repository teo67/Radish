dotnet build
rm -r -f Radish
mkdir Radish
cd Radish
mkdir bin
mkdir lib
cd ..
dotnet publish -c Release -r $1 --self-contained false -o $PWD/Radish/bin
cp -R $PWD/Radish-Standard-Library/ $PWD/Radish/lib