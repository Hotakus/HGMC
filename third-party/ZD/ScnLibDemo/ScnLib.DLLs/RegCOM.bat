@echo off

echo Registering components... Administrator privilege is required.

if exist %windir%\SysWOW64\regsvr32.exe (
set regsvr32=%windir%\SysWOW64\regsvr32.exe
) else (
set regsvr32=%windir%\system32\regsvr32.exe
)

%regsvr32% /s "%~dp0ScnCap.ax"
%regsvr32% /s "%~dp0AudCap.ax"
%regsvr32% /s "%~dp0AacEnc.ax"
%regsvr32% /s "%~dp0AvcEnc.ax"
%regsvr32% /s "%~dp0Mp4Mux.ax"
%regsvr32% /s "%~dp0FlvMux.ax"
%regsvr32% /s "%~dp0Stream.ax"
%regsvr32% /s "%~dp0Webcam.ax"
%regsvr32% /s "%~dp0wavdest.ax"
%regsvr32% /s "%~dp0lame.ax"

@echo on