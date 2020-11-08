#! zsh

sed -i "" -e "s/BackEndApiTemplateTest/$1Test/" -e "s/BackEndApiTemplate/$1/" .gitignore
sed -i "" "s/BackEndApiTemplateTest/$1Test/" BackEndApiTemplateTest/BackEndApiTemplateTest.csproj
sed -i "" "s/backendapitemplate/$1/" BackEndApiTemplate/BackEndApiTemplate.csproj
sed -i "" "s/BackEndApiTemplateTest/$1Test/" BackEndApiTemplateTest/UnitTest1.cs
sed -i "" "s/BackEndApiTemplate/$1/" BackEndApiTemplate/Startup.cs  BackEndApiTemplate/Program.cs BackEndApiTemplate/Controllers/HelloController.cs
sed -i "" "s/BackEndApiTemplate/$1/g" BackEndApiTemplateTest/BackEndApiTemplateTest.csproj
sed -i "" "s/BackEndApiTemplateTest/$1Test/g" BackEndApiTemplate.sln
sed -i "" "s/BackEndApiTemplate/$1/g" BackEndApiTemplate.sln

mv BackEndApiTemplateTest/BackEndApiTemplateTest.csproj BackEndApiTemplateTest/$1Test.csproj
mv BackEndApiTemplate/BackEndApiTemplate.csproj BackEndApiTemplate/$1.csproj
mv BackEndApiTemplateTest $1Test
mv BackEndApiTemplate $1
mv BackEndApiTemplate.sln $1.sln
# mv .vs/backendapitemplate .vs/$1

rm README.md
rm rename.sh
git add .
! git commit -m "chore: rename project to $1"
