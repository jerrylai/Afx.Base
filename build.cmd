@echo off
set Build="%SYSTEMDRIVE%\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MsBuild.exe"
if exist publish rd /s /q publish
%Build% "NET47/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET471/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET472/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET48/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
dotnet build "NETStandard2.0/Afx.Base/Afx.Base.csproj" -c Release
dotnet build "NETStandard2.1/Afx.Base/Afx.Base.csproj" -c Release
dotnet build "NET6.0/Afx.Base/Afx.Base.csproj" -c Release
cd publish
del /q/s *.pdb
pause