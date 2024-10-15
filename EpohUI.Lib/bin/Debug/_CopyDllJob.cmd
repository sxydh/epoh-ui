@echo off
chcp 65001
setlocal enabledelayedexpansion

set "watchFolder=%cd%"
set "targetFolder=%cd%\..\..\..\EpohUI.Core\bin\Debug\DLLs"

dir /b "%watchFolder%\*.dll" > _FileList.txt 2> nul

:loop
timeout /t 2 > nul

dir /b "%watchFolder%\*.dll" > _NewFileList.txt 2> nul

fc /b _FileList.txt _NewFileList.txt > nul
if errorlevel 1 (
    echo "文件夹发生变化，清空 target 文件夹并复制 DLL 文件。"

    echo "正在清空 %targetFolder%"
    del "%targetFolder%\*.dll" 2> nul

    echo "正在复制 DLL 文件到 %targetFolder%"
    copy "%watchFolder%\*.dll" "%targetFolder%\" 2> nul

    copy /y _NewFileList.txt _FileList.txt
)

goto loop