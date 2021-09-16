' This is a part of the ZD Soft Screen Recorder SDK
' Copyright (C) 2005-2018, ZD Soft, all rights reserved.

' Please add the following lines to the application manifest file:
' <dependency>
'   <dependentAssembly>
'     <assemblyIdentity
'       type="win32"
'       name="ScnLib.DLLs"
'       version="1.0.0.0" />
'   </dependentAssembly>
' </dependency>

Imports System
Imports System.Text
Imports System.Runtime.InteropServices

' Example of getting an output string from an API:
' Dim Path As String = Space(260)
' ZDSoft.SDK.ScnLib_GetVideoPathW(Path)
' Path = Left(Path, InStr(Path, Chr(0)) - 1)

Namespace ZDSoft

	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
	Public Structure LOGFONT
		Public lfHeight As Int32
		Public lfWidth As Int32
		Public lfEscapement As Int32
		Public lfOrientation As Int32
		Public lfWeight As Int32
		Public lfItalic As Byte
		Public lfUnderline As Byte
		Public lfStrikeOut As Byte
		Public lfCharSet As Byte
		Public lfOutPrecision As Byte
		Public lfClipPrecision As Byte
		Public lfQuality As Byte
		Public lfPitchAndFamily As Byte
		<MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> _
		Public lfFaceName As String
	End Structure

	Public Class SDK

		Public Const POSITION_TOP_LEFT =      0
		Public Const POSITION_TOP =           1
		Public Const POSITION_TOP_RIGHT =     2
		Public Const POSITION_RIGHT =         3
		Public Const POSITION_BOTTOM_RIGHT =  4
		Public Const POSITION_BOTTOM =        5
		Public Const POSITION_BOTTOM_LEFT =   6
		Public Const POSITION_LEFT =          7
		Public Const POSITION_CENTER =        8

		Public Const WEBCAM_VIEW_PADDING =    0
		Public Const WEBCAM_VIEW_CROPPING =   1
		Public Const WEBCAM_VIEW_STRETCHING = 2

		Public Declare Sub ScnLib_About Lib "ScnLib.dll" ()
		Public Declare Unicode Function ScnLib_SetLicenseW Lib "ScnLib.dll" (ByVal Name As String, ByVal Email As String, ByVal Key As String) As Boolean
		Public Declare Unicode Sub ScnLib_GetErrorMessageW Lib "ScnLib.dll" (ByVal ErrMsg As String) ' ErrMsg >= 2048 Chars
		Public Declare Function ScnLib_CheckComponents Lib "ScnLib.dll" () As Boolean
		Public Declare Function ScnLib_Initialize Lib "ScnLib.dll" () As Boolean
		Public Declare Sub ScnLib_Uninitialize Lib "ScnLib.dll" ()
		Public Declare Unicode Sub ScnLib_SetVideoPathW Lib "ScnLib.dll" (ByVal Path As String)
		Public Declare Unicode Sub ScnLib_GetVideoPathW Lib "ScnLib.dll" (ByVal Path As String, ByVal Saved As Boolean) ' Path >= 260 Chars
		Public Declare Unicode Sub ScnLib_SetAudioPathW Lib "ScnLib.dll" (ByVal Path As String)
		Public Declare Unicode Sub ScnLib_GetAudioPathW Lib "ScnLib.dll" (ByVal Path As String, ByVal Saved As Boolean) ' Path >= 260 Chars
		Public Declare Unicode Sub ScnLib_SetStreamingUrlW Lib "ScnLib.dll" (ByVal URL As String)
		Public Declare Unicode Sub ScnLib_GetStreamingUrlW Lib "ScnLib.dll" (ByVal URL As String) ' URL >= 2048 Chars
		Public Declare Unicode Function ScnLib_TakeScreenshotW Lib "ScnLib.dll" (ByVal Path As String, ByVal left As Int32, ByVal top As Int32, ByVal right As Int32, ByVal bottom As Int32) As Boolean ' Path >= 260 Chars
		Public Declare Sub ScnLib_SetCaptureRegion Lib "ScnLib.dll" (ByVal left As Int32, ByVal top As Int32, ByVal right As Int32, ByVal bottom As Int32)
		Public Declare Sub ScnLib_GetCaptureRegion Lib "ScnLib.dll" (ByRef left As Int32, ByRef top As Int32, ByRef right As Int32, ByRef bottom As Int32)
		Public Declare Function ScnLib_SelectCaptureRegion Lib "ScnLib.dll" (ByRef left As Int32, ByRef top As Int32, ByRef right As Int32, ByRef bottom As Int32) As Boolean
		Public Declare Sub ScnLib_ShowCaptureRegionFrame Lib "ScnLib.dll" (ByVal Enable As Boolean)
		Public Declare Function ScnLib_GetCaptureRegionFrameWnd Lib "ScnLib.dll" () As IntPtr
		Public Declare Sub ScnLib_EnableGPUAcceleration Lib "ScnLib.dll" (ByVal Enable As Boolean)
		Public Declare Function ScnLib_IsGPUAccelerationEnabled Lib "ScnLib.dll" () As Boolean
		Public Declare Function ScnLib_ShowCountdownBox Lib "ScnLib.dll" (ByVal Seconds As Int32) As Boolean
		Public Declare Function ScnLib_StartRecording Lib "ScnLib.dll" () As Boolean
		Public Declare Function ScnLib_PauseRecording Lib "ScnLib.dll" () As Boolean
		Public Declare Function ScnLib_ResumeRecording Lib "ScnLib.dll" () As Boolean
		Public Declare Sub ScnLib_StopRecording Lib "ScnLib.dll" ()
		Public Declare Function ScnLib_IsRecording Lib "ScnLib.dll" () As Boolean
		Public Declare Function ScnLib_IsPaused Lib "ScnLib.dll" () As Boolean
		Public Declare Function ScnLib_GetRecTime Lib "ScnLib.dll" () As UInt32
		Public Declare Unicode Sub ScnLib_GetRecTimeW Lib "ScnLib.dll" (ByVal Time As String) ' Time >= 11 Chars
		Public Declare Sub ScnLib_ZoomInScreen Lib "ScnLib.dll" (ByVal Ratio As Double)
		Public Declare Function ScnLib_GetZoomRatio Lib "ScnLib.dll" () As Double
		Public Declare Sub ScnLib_SetZoomSpeed Lib "ScnLib.dll" (ByVal Speed As Double)
		Public Declare Function ScnLib_GetZoomSpeed Lib "ScnLib.dll" () As Double
		Public Declare Function ScnLib_PreviewVideo Lib "ScnLib.dll" (ByVal Enable As Boolean, ByVal Wnd As IntPtr, ByVal Padding As Boolean, ByVal BkColor As UInt32) As Boolean
		Public Declare Function ScnLib_GetVideoPreviewWnd Lib "ScnLib.dll" () As IntPtr
		Public Declare Sub ScnLib_SetVideoResolution Lib "ScnLib.dll" (ByVal Width As Int32, ByVal Height As Int32)
		Public Declare Sub ScnLib_GetVideoResolution Lib "ScnLib.dll" (ByRef Width As Int32, ByRef Height As Int32)
		Public Declare Sub ScnLib_SetVideoFrameRate Lib "ScnLib.dll" (ByVal FPS As Double)
		Public Declare Function ScnLib_GetVideoFrameRate Lib "ScnLib.dll" () As Double
		Public Declare Sub ScnLib_SetVideoKeyFrameInterval Lib "ScnLib.dll" (ByVal Seconds As Double)
		Public Declare Function ScnLib_GetVideoKeyFrameInterval Lib "ScnLib.dll" () As Double
		Public Declare Sub ScnLib_EnableVideoVariableFrameRate Lib "ScnLib.dll" (ByVal Enable As Boolean)
		Public Declare Function ScnLib_IsVideoVariableFrameRateEnabled Lib "ScnLib.dll" () As Boolean
		Public Declare Sub ScnLib_ConfigureVideoCodec Lib "ScnLib.dll" (ByVal ParentWnd As IntPtr)
		Public Declare Sub ScnLib_SetVideoQuality Lib "ScnLib.dll" (ByVal CRF As Int32)
		Public Declare Function ScnLib_GetVideoQuality Lib "ScnLib.dll" () As Int32
		Public Declare Sub ScnLib_SetVideoBitrate Lib "ScnLib.dll" (ByVal Kbps As Int32)
		Public Declare Function ScnLib_GetVideoBitrate Lib "ScnLib.dll" () As Int32
		Public Declare Sub ScnLib_SetAudioBitrate Lib "ScnLib.dll" (ByVal Kbps As Int32)
		Public Declare Function ScnLib_GetAudioBitrate Lib "ScnLib.dll" () As Int32
		Public Declare Sub ScnLib_SetStreamingBitrate Lib "ScnLib.dll" (ByVal Kbps As Int32)
		Public Declare Function ScnLib_GetStreamingBitrate Lib "ScnLib.dll" () As Int32
		Public Declare Function ScnLib_GetAudioSourceDeviceCount Lib "ScnLib.dll" (ByVal Playback As Boolean) As Int32
		Public Declare Unicode Function ScnLib_GetAudioSourceDeviceW Lib "ScnLib.dll" (ByVal Playback As Boolean, ByVal Index As Int32, ByVal Device As String) As Boolean ' Device >= 260 Chars
		Public Declare Sub ScnLib_SelectAudioSourceDevice Lib "ScnLib.dll" (ByVal Playback As Boolean, ByVal Index As Int32)
		Public Declare Function ScnLib_GetSelectedAudioSourceDevice Lib "ScnLib.dll" (ByVal Playback As Boolean) As Int32
		Public Declare Sub ScnLib_ConfigureAudioSourceDevices Lib "ScnLib.dll" (ByVal Playback As Boolean)
		Public Declare Sub ScnLib_RecordAudioSource Lib "ScnLib.dll" (ByVal Playback As Boolean, ByVal Enable As Boolean)
		Public Declare Function ScnLib_IsRecordAudioSource Lib "ScnLib.dll" (ByVal Playback As Boolean) As Boolean
		Public Declare Sub ScnLib_SetAudioSourceVolume Lib "ScnLib.dll" (ByVal Playback As Boolean, ByVal Volume As Int32)
		Public Declare Function ScnLib_GetAudioSourceVolume Lib "ScnLib.dll" (ByVal Playback As Boolean) As Int32
		Public Declare Sub ScnLib_MonitorVolumeLevel Lib "ScnLib.dll" (ByVal Enable As Boolean)
		Public Declare Function ScnLib_IsMonitoringVolumeLevel Lib "ScnLib.dll" () As Boolean
		Public Declare Function ScnLib_GetAudioSourceVolumeLevel Lib "ScnLib.dll" (ByVal Playback As Boolean) As Int32
		Public Declare Sub ScnLib_SetMicrophoneDelay Lib "ScnLib.dll" (ByVal Milliseconds As Int32)
		Public Declare Function ScnLib_GetMicrophoneDelay Lib "ScnLib.dll" () As Int32
		Public Declare Function ScnLib_GetWebcamDeviceCount Lib "ScnLib.dll" () As Int32
		Public Declare Unicode Function ScnLib_GetWebcamDeviceW Lib "ScnLib.dll" (ByVal Index As Int32, ByVal Device As String) As Boolean ' Device >= 260 Chars
		Public Declare Function ScnLib_SelectWebcamDevice Lib "ScnLib.dll" (ByVal Index As Int32) As Boolean
		Public Declare Function ScnLib_GetSelectedWebcamDevice Lib "ScnLib.dll" () As Int32
		Public Declare Function ScnLib_PreviewWebcam Lib "ScnLib.dll" (ByVal Enable As Boolean, ByVal Wnd As IntPtr, ByVal Padding As Boolean, ByVal BkColor As UInt32) As Boolean
		Public Declare Function ScnLib_GetWebcamPreviewWnd Lib "ScnLib.dll" () As IntPtr
		Public Declare Sub ScnLib_RecordWebcamOnly Lib "ScnLib.dll" (ByVal Enable As Boolean)
		Public Declare Function ScnLib_IsRecordWebcamOnly Lib "ScnLib.dll" () As Boolean
		Public Declare Sub ScnLib_InputWebcamFrame Lib "ScnLib.dll" (ByVal RGB As IntPtr, ByVal Width As Int32, ByVal Height As Int32, ByVal BitCount As Int32)
		Public Declare Sub ScnLib_SetWebcamResolution Lib "ScnLib.dll" (ByVal Width As Int32, ByVal Height As Int32)
		Public Declare Sub ScnLib_GetWebcamResolution Lib "ScnLib.dll" (ByRef Width As Int32, ByRef Height As Int32)
		Public Declare Sub ScnLib_SetWebcamDirection Lib "ScnLib.dll" (ByVal Mirroring As Boolean, ByVal Flipping As Boolean)
		Public Declare Sub ScnLib_GetWebcamDirection Lib "ScnLib.dll" (ByRef Mirroring As Boolean, ByRef Flipping As Boolean)
		Public Declare Sub ScnLib_SetWebcamViewMode Lib "ScnLib.dll" (ByVal ViewMode As Int32)
		Public Declare Function ScnLib_GetWebcamViewMode Lib "ScnLib.dll" () As Int32
		Public Declare Sub ScnLib_SetWebcamPosition Lib "ScnLib.dll" (ByVal Position As Int32, ByVal MarginX As Int32, ByVal MarginY As Int32)
		Public Declare Sub ScnLib_GetWebcamPosition Lib "ScnLib.dll" (ByRef Position As Int32, ByRef MarginX As Int32, ByRef MarginY As Int32)
		Public Declare Sub ScnLib_SetWebcamViewSize Lib "ScnLib.dll" (ByVal ViewWidth As Int32, ByVal ViewHeight As Int32)
		Public Declare Sub ScnLib_GetWebcamViewSize Lib "ScnLib.dll" (ByRef ViewWidth As Int32, ByRef ViewHeight As Int32)
		Public Declare Function ScnLib_IsLogoVisible Lib "ScnLib.dll" () As Boolean
		Public Declare Unicode Function ScnLib_SetLogoImageW Lib "ScnLib.dll" (ByVal Path As String) As Boolean
		Public Declare Unicode Sub ScnLib_GetLogoImageW Lib "ScnLib.dll" (ByVal Path As String) ' Path >= 260 Chars
		Public Declare Sub ScnLib_UpdateLogoImage Lib "ScnLib.dll" (ByVal RGB As IntPtr, ByVal Width As Int32, ByVal Height As Int32, ByVal BitCount As Int32)
		Public Declare Unicode Function ScnLib_SetLogoTextW Lib "ScnLib.dll" (ByVal Text As String, <MarshalAs(UnmanagedType.LPStruct)> Font As LOGFONT, ByVal Color As UInt32, ByVal Shadow As Boolean) As Boolean
		Public Declare Unicode Sub ScnLib_GetLogoTextW Lib "ScnLib.dll" (ByVal Text As String, <MarshalAs(UnmanagedType.LPStruct)> Font As LOGFONT, ByRef Color As UInt32, ByRef Shadow As Boolean) ' Text >= 1024 Chars
		Public Declare Sub ScnLib_SetLogoPosition Lib "ScnLib.dll" (ByVal Position As Int32, ByVal MarginX As Int32, ByVal MarginY As Int32)
		Public Declare Sub ScnLib_GetLogoPosition Lib "ScnLib.dll" (ByRef Position As Int32, ByRef MarginX As Int32, ByRef MarginY As Int32)
		Public Declare Sub ScnLib_SetLogoOpacity Lib "ScnLib.dll" (ByVal Opacity As Double)
		Public Declare Function ScnLib_GetLogoOpacity Lib "ScnLib.dll" () As Double
		Public Declare Sub ScnLib_RecordCursor Lib "ScnLib.dll" (ByVal Enable As Boolean)
		Public Declare Function ScnLib_IsRecordCursor Lib "ScnLib.dll" () As Boolean
		Public Declare Sub ScnLib_SetCursorOriginalSize Lib "ScnLib.dll" (ByVal Enable As Boolean)
		Public Declare Function ScnLib_IsCursorOriginalSize Lib "ScnLib.dll" () As Boolean
		Public Declare Sub ScnLib_AddCursorEffects Lib "ScnLib.dll" (ByVal Highlight As Boolean, ByVal ClickEffects As Boolean, ByVal Track As Boolean, ByVal ClickSound As Boolean)
		Public Declare Sub ScnLib_GetCursorEffects Lib "ScnLib.dll" (ByRef Highlight As Boolean, ByRef ClickEffects As Boolean, ByRef Track As Boolean, ByRef ClickSound As Boolean)
		Public Declare Sub ScnLib_SetCursorEffectsColors Lib "ScnLib.dll" (ByVal HighlightColor As UInt32, ByVal LeftClickColor As UInt32, ByVal RightClickColor As UInt32, ByVal TrackColor As UInt32)
		Public Declare Sub ScnLib_GetCursorEffectsColors Lib "ScnLib.dll" (ByRef HighlightColor As UInt32, ByRef LeftClickColor As UInt32, ByRef RightClickColor As UInt32, ByRef TrackColor As UInt32)

	End Class

End Namespace
