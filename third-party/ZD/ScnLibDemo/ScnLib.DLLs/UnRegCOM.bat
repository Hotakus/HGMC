@echo off

echo Unregistering components... Administrator privilege is required.

if exist %windir%\SysWOW64\regsvr32.exe (
set regsvr32=%windir%\SysWOW64\regsvr32.exe
) else (
set regsvr32=%windir%\system32\regsvr32.exe
)

%regsvr32% /s /u "%~dp0ScnCap.ax"
%regsvr32% /s /u "%~dp0AudCap.ax"
%regsvr32% /s /u "%~dp0AacEnc.ax"
%regsvr32% /s /u "%~dp0AvcEnc.ax"
%regsvr32% /s /u "%~dp0Mp4Mux.ax"
%regsvr32% /s /u "%~dp0FlvMux.ax"
%regsvr32% /s /u "%~dp0Stream.ax"
%regsvr32% /s /u "%~dp0Webcam.ax"
%regsvr32% /s /u "%~dp0wavdest.ax"
%regsvr32% /s /u "%~dp0lame.ax"

@echo on