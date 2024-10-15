@echo off
setlocal enabledelayedexpansion

:: 设置监控的文件夹
set "watchFolder=%cd%"
set "targetFolder=%cd%\..\..\..\EpohUI.Core\bin\Debug"

:: 创建一个初始的文件列表
dir /b "%watchFolder%\*.dll" > _FileList.txt

:loop
:: 等待文件变化
timeout /t 1 > nul

:: 生成新的文件列表
dir /b "%watchFolder%\*.dll" > _NewFileList.txt

:: 比较当前文件和新的文件列表
fc /b _FileList.txt _NewFileList.txt > nul
if errorlevel 1 (
    echo 文件夹发生变化，清空 target 文件夹并复制 DLL 文件。

    :: 清空 target 文件夹
    echo 正在清空 %targetFolder%
    del "%targetFolder%\*.dll"

    :: 复制 DLL 文件到 target 文件夹
    echo 正在复制 DLL 文件到 %targetFolder%
    copy "%watchFolder%\*.dll" "%targetFolder%\"

    :: 更新当前文件列表
    copy /y _NewFileList.txt _FileList.txt
)

goto loop