$chocolatey = "C:\Chocolatey"
$libName = "rabbit"
$libfolder = "$chocolatey\lib\$libname"
$libBin = "$chocolatey\bin\rabbit.bat"
$currentPath =  $myinvocation.mycommand.path | split-path -parent
if(Test-Path $chocolatey)
{
    #create folder for library if not exists
    if(-not(Test-Path $libfolder))
    {
        new-item $libfolder -ItemType "Directory"
    }

    #copy files to library
    Copy-Item -Path $currentPath\Src\rabbit\bin\Debug\*.* -Destination $libfolder -Force

    #generat bat file for the library if not exist
    if(-not(Test-Path $libBin))
    {
    $bat = 
"@echo off
SET DIR=%~dp0%
`"%DIR%..\lib\$libName\rabbit.exe`" %*"
    
    $bat | Out-File $libBin -Force ascii
    }
    
}