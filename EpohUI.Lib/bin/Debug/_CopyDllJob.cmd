@echo off
chcp 65001
setlocal enabledelayedexpansion

set "watchFolder=%cd%"
set "targetFolder=%cd%\..\..\..\EpohUI.Core\bin\Debug\DLLs"

mkdir %targetFolder%
pushd "%targetFolder%"
set "targetFolder=%cd%"
popd

dir /b "%watchFolder%\*.dll" > _FileList.txt 2> nul

:loop
timeout /t 2 > nul

dir /b "%watchFolder%\*.dll" > _NewFileList.txt 2> nul

fc /b _FileList.txt _NewFileList.txt > nul
if errorlevel 1 (
    echo "正在清空 %targetFolder%"
    mkdir %targetFolder%
    del "%targetFolder%\*.dll" 2> nul

    echo "正在复制 %watchFolder%"
    copy "%watchFolder%\*.dll" "%targetFolder%\" 2> nul

    copy /y _NewFileList.txt _FileList.txt
)

goto loop