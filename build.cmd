@echo off
set Build="%SYSTEMDRIVE%\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MsBuild.exe"
if exist publish rd /s /q publish
%Build% "NET20/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET40/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET45/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET451/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET452/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET46/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET461/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET462/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET47/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET471/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET472/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
%Build% "NET48/Afx.Base/Afx.Base.csproj" /t:Rebuild /p:Configuration=Release
dotnet build "NETStandard2.0/Afx.Base/Afx.Base.csproj" -c Release
dotnet build "NETStandard2.1/Afx.Base/Afx.Base.csproj" -c Release
cd publish
del /q/s *.pdb
pause