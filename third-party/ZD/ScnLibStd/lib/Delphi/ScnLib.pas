// This is a part of the ZD Soft Screen Recorder SDK
// Copyright (C) 2005-2018; ZD Soft; all rights reserved.

(* Please add the following lines to the application manifest file:
<dependency>
  <dependentAssembly>
    <assemblyIdentity
      type="win32"
      name="ScnLib.DLLs"
      version="1.0.0.0" />
  </dependentAssembly>
</dependency>
*)

(* Example of getting an output string from an API:
var
	Path: array[0..260] of WideChar;
begin
	ScnLib_GetVideoPathW(Path);
end;
*)

Unit ScnLib;

interface

uses
	Winapi.Windows;

const
	POSITION_TOP_LEFT =      0;
	POSITION_TOP =           1;
	POSITION_TOP_RIGHT =     2;
	POSITION_RIGHT =         3;
	POSITION_BOTTOM_RIGHT =  4;
	POSITION_BOTTOM =        5;
	POSITION_BOTTOM_LEFT =   6;
	POSITION_LEFT =          7;
	POSITION_CENTER =        8;

procedure ScnLib_About(); stdcall; external 'ScnLib.dll';
function  ScnLib_SetLicenseA(const Name: PChar; const Email: PChar; const Key: PChar): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_SetLicenseW(const Name: PWideChar; const Email: PWideChar; const Key: PWideChar): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_GetErrorMessageA(ErrMsg: PChar); stdcall; external 'ScnLib.dll'; // ErrMsg >= 2048 Chars
procedure ScnLib_GetErrorMessageW(ErrMsg: PWideChar); stdcall; external 'ScnLib.dll'; // ErrMsg >= 2048 WideChars
function  ScnLib_CheckComponents(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_Initialize(): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_Uninitialize(); stdcall; external 'ScnLib.dll';
procedure ScnLib_SetVideoPathA(const Path: PChar); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetVideoPathA(Path: PChar; Saved: LongBool); stdcall; external 'ScnLib.dll'; // Path >= 260 Chars
procedure ScnLib_SetVideoPathW(const Path: PWideChar); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetVideoPathW(Path: PWideChar; Saved: LongBool); stdcall; external 'ScnLib.dll'; // Path >= 260 WideChars
procedure ScnLib_SetAudioPathA(const Path: PChar); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetAudioPathA(Path: PChar; Saved: LongBool); stdcall; external 'ScnLib.dll'; // Path >= 260 Chars
procedure ScnLib_SetAudioPathW(const Path: PWideChar); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetAudioPathW(Path: PWideChar; Saved: LongBool); stdcall; external 'ScnLib.dll'; // Path >= 260 WideChars
function  ScnLib_TakeScreenshotA(Path: PChar; left: Integer; top: Integer; right: Integer; bottom: Integer): LongBool; stdcall; external 'ScnLib.dll'; // Path >= 260 Chars
function  ScnLib_TakeScreenshotW(Path: PWideChar; left: Integer; top: Integer; right: Integer; bottom: Integer): LongBool; stdcall; external 'ScnLib.dll'; // Path >= 260 WideChars
procedure ScnLib_SetCaptureRegion(left: Integer; top: Integer; right: Integer; bottom: Integer); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetCaptureRegion(var left: Integer; var top: Integer; var right: Integer; var bottom: Integer); stdcall; external 'ScnLib.dll';
procedure ScnLib_EnableGPUAcceleration(Enable: LongBool); stdcall; external 'ScnLib.dll';
function  ScnLib_IsGPUAccelerationEnabled(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_StartRecording(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_PauseRecording(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_ResumeRecording(): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_StopRecording(); stdcall; external 'ScnLib.dll';
function  ScnLib_IsRecording(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_IsPaused(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_GetRecTime(): LongWord; stdcall; external 'ScnLib.dll';
procedure ScnLib_GetRecTimeA(Time: PChar); stdcall; external 'ScnLib.dll'; // Time >= 11 Chars
procedure ScnLib_GetRecTimeW(Time: PWideChar); stdcall; external 'ScnLib.dll'; // Time >= 11 WideChars
procedure ScnLib_ZoomInScreen(Ratio: Double); stdcall; external 'ScnLib.dll';
function  ScnLib_GetZoomRatio(): Double; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetZoomSpeed(Speed: Double); stdcall; external 'ScnLib.dll';
function  ScnLib_GetZoomSpeed(): Double; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetVideoResolution(Width: Integer; Height: Integer); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetVideoResolution(var Width: Integer; var Height: Integer); stdcall; external 'ScnLib.dll';
procedure ScnLib_SetVideoFrameRate(FPS: Double); stdcall; external 'ScnLib.dll';
function  ScnLib_GetVideoFrameRate(): Double; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetVideoKeyFrameInterval(Seconds: Double); stdcall; external 'ScnLib.dll';
function  ScnLib_GetVideoKeyFrameInterval(): Double; stdcall; external 'ScnLib.dll';
procedure ScnLib_EnableVideoVariableFrameRate(Enable: LongBool); stdcall; external 'ScnLib.dll';
function  ScnLib_IsVideoVariableFrameRateEnabled(): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetVideoQuality(CRF: Integer); stdcall; external 'ScnLib.dll';
function  ScnLib_GetVideoQuality(): Integer; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetVideoBitrate(Kbps: Integer); stdcall; external 'ScnLib.dll';
function  ScnLib_GetVideoBitrate(): Integer; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetAudioBitrate(Kbps: Integer); stdcall; external 'ScnLib.dll';
function  ScnLib_GetAudioBitrate(): Integer; stdcall; external 'ScnLib.dll';
function  ScnLib_GetAudioSourceDeviceCount(Playback: LongBool): Integer; stdcall; external 'ScnLib.dll';
function  ScnLib_GetAudioSourceDeviceA(Playback: LongBool; Index: Integer; Device: PChar): LongBool; stdcall; external 'ScnLib.dll'; // Device >= 260 Chars
function  ScnLib_GetAudioSourceDeviceW(Playback: LongBool; Index: Integer; Device: PWideChar): LongBool; stdcall; external 'ScnLib.dll'; // Device >= 260 WideChars
procedure ScnLib_SelectAudioSourceDevice(Playback: LongBool; Index: Integer); stdcall; external 'ScnLib.dll';
function  ScnLib_GetSelectedAudioSourceDevice(Playback: LongBool): Integer; stdcall; external 'ScnLib.dll';
procedure ScnLib_ConfigureAudioSourceDevices(Playback: LongBool); stdcall; external 'ScnLib.dll';
procedure ScnLib_RecordAudioSource(Playback: LongBool; Enable: LongBool); stdcall; external 'ScnLib.dll';
function  ScnLib_IsRecordAudioSource(Playback: LongBool): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetAudioSourceVolume(Playback: LongBool; Volume: Integer); stdcall; external 'ScnLib.dll';
function  ScnLib_GetAudioSourceVolume(Playback: LongBool): Integer; stdcall; external 'ScnLib.dll';
procedure ScnLib_MonitorVolumeLevel(Enable: LongBool); stdcall; external 'ScnLib.dll';
function  ScnLib_IsMonitoringVolumeLevel(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_GetAudioSourceVolumeLevel(Playback: LongBool): Integer; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetMicrophoneDelay(Milliseconds: Integer); stdcall; external 'ScnLib.dll';
function  ScnLib_GetMicrophoneDelay(): Integer; stdcall; external 'ScnLib.dll';
function  ScnLib_IsLogoVisible(): LongBool; stdcall; external 'ScnLib.dll';
function  ScnLib_SetLogoImageA(const Path: PChar): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_GetLogoImageA(Path: PChar); stdcall; external 'ScnLib.dll'; // Path >= 260 Chars
function  ScnLib_SetLogoImageW(const Path: PWideChar): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_GetLogoImageW(Path: PWideChar); stdcall; external 'ScnLib.dll'; // Path >= 260 WideChars
procedure ScnLib_UpdateLogoImage(const RGB: Pointer; Width: Integer; Height: Integer; BitCount: Integer); stdcall; external 'ScnLib.dll';
function  ScnLib_SetLogoTextA(const Text: PChar; const Font: tagLogFontA; Color: LongWord; Shadow: LongBool): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_GetLogoTextA(Text: PChar; var Font: tagLogFontA; var Color: LongWord; var Shadow: LongBool); stdcall; external 'ScnLib.dll'; // Text >= 1024 Chars
function  ScnLib_SetLogoTextW(const Text: PWideChar; const Font: tagLogFontW; Color: LongWord; Shadow: LongBool): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_GetLogoTextW(Text: PWideChar; var Font: tagLogFontW; var Color: LongWord; var Shadow: LongBool); stdcall; external 'ScnLib.dll'; // Text >= 1024 WideChars
procedure ScnLib_SetLogoPosition(Position: Integer; MarginX: Integer; MarginY: Integer); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetLogoPosition(var Position: Integer; var MarginX: Integer; var MarginY: Integer); stdcall; external 'ScnLib.dll';
procedure ScnLib_SetLogoOpacity(Opacity: Double); stdcall; external 'ScnLib.dll';
function  ScnLib_GetLogoOpacity(): Double; stdcall; external 'ScnLib.dll';
procedure ScnLib_RecordCursor(Enable: LongBool); stdcall; external 'ScnLib.dll';
function  ScnLib_IsRecordCursor(): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_SetCursorOriginalSize(Enable: LongBool); stdcall; external 'ScnLib.dll';
function  ScnLib_IsCursorOriginalSize(): LongBool; stdcall; external 'ScnLib.dll';
procedure ScnLib_AddCursorEffects(Highlight: LongBool; ClickEffects: LongBool; Track: LongBool; ClickSound: LongBool); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetCursorEffects(var Highlight: LongBool; var ClickEffects: LongBool; var Track: LongBool; var ClickSound: LongBool); stdcall; external 'ScnLib.dll';
procedure ScnLib_SetCursorEffectsColors(HighlightColor: LongWord; LeftClickColor: LongWord; RightClickColor: LongWord; TrackColor: LongWord); stdcall; external 'ScnLib.dll';
procedure ScnLib_GetCursorEffectsColors(var HighlightColor: LongWord; var LeftClickColor: LongWord; var RightClickColor: LongWord; var TrackColor: LongWord); stdcall; external 'ScnLib.dll';

implementation

end.