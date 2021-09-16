// This is a part of the ZD Soft Screen Recorder SDK
// Copyright (C) 2005-2018, ZD Soft, all rights reserved.

/* Please add the following lines to the application manifest file:
<dependency>
  <dependentAssembly>
    <assemblyIdentity
      type="win32"
      name="ScnLib.DLLs"
      version="1.0.0.0" />
  </dependentAssembly>
</dependency>
*/

using System;
using System.Text;
using System.Runtime.InteropServices;

/* Example of getting an output string from an API:
StringBuilder Path = new StringBuilder(260);
ZDSoft.SDK.ScnLib_GetVideoPathW(Path);
*/

namespace ZDSoft
{

	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
	public class LOGFONT
	{
		public int lfHeight;
		public int lfWidth;
		public int lfEscapement;
		public int lfOrientation;
		public int lfWeight;
		public byte lfItalic;
		public byte lfUnderline;
		public byte lfStrikeOut;
		public byte lfCharSet;
		public byte lfOutPrecision;
		public byte lfClipPrecision;
		public byte lfQuality;
		public byte lfPitchAndFamily;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
		public string lfFaceName;
	}

	public class SDK
	{

		public const int POSITION_TOP_LEFT =      0;
		public const int POSITION_TOP =           1;
		public const int POSITION_TOP_RIGHT =     2;
		public const int POSITION_RIGHT =         3;
		public const int POSITION_BOTTOM_RIGHT =  4;
		public const int POSITION_BOTTOM =        5;
		public const int POSITION_BOTTOM_LEFT =   6;
		public const int POSITION_LEFT =          7;
		public const int POSITION_CENTER =        8;

		public const int WEBCAM_VIEW_PADDING =    0;
		public const int WEBCAM_VIEW_CROPPING =   1;
		public const int WEBCAM_VIEW_STRETCHING = 2;

		[DllImport("ScnLib.dll")] public static extern void ScnLib_About();
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_SetLicenseW(string Name, string Email, string Key);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetErrorMessageW(StringBuilder ErrMsg); // ErrMsg >= 2048 Chars
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_CheckComponents();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_Initialize();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_Uninitialize();
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_LoadSettingsW(IntPtr RegKey, string SubKey);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_SaveSettingsW(IntPtr RegKey, string SubKey);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_DeleteSettingsW(IntPtr RegKey, string SubKey);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_ConfigureSettings(IntPtr ParentWnd);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_SetVideoPathW(string Path);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetVideoPathW(StringBuilder Path, bool Saved); // Path >= 260 Chars
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_SetAudioPathW(string Path);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetAudioPathW(StringBuilder Path, bool Saved); // Path >= 260 Chars
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_SetStreamingUrlW(string URL);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetStreamingUrlW(StringBuilder URL); // URL >= 2048 Chars
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_TakeScreenshotW(StringBuilder Path, int left, int top, int right, int bottom); // Path >= 260 Chars
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetCaptureRegion(int left, int top, int right, int bottom);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetCaptureRegion(ref int left, ref int top, ref int right, ref int bottom);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_SelectCaptureRegion(ref int left, ref int top, ref int right, ref int bottom);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_ShowCaptureRegionFrame(bool Enable);
		[DllImport("ScnLib.dll")] public static extern IntPtr ScnLib_GetCaptureRegionFrameWnd();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_EnableGPUAcceleration(bool Enable);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsGPUAccelerationEnabled();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_ShowCountdownBox(int Seconds);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_StartRecording();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_PauseRecording();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_ResumeRecording();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_StopRecording();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsRecording();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsPaused();
		[DllImport("ScnLib.dll")] public static extern uint ScnLib_GetRecTime();
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetRecTimeW(StringBuilder Time); // Time >= 11 Chars
		[DllImport("ScnLib.dll")] public static extern void ScnLib_ZoomInScreen(double Ratio);
		[DllImport("ScnLib.dll")] public static extern double ScnLib_GetZoomRatio();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetZoomSpeed(double Speed);
		[DllImport("ScnLib.dll")] public static extern double ScnLib_GetZoomSpeed();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_PreviewVideo(bool Enable, IntPtr Wnd, bool Padding, uint BkColor);
		[DllImport("ScnLib.dll")] public static extern IntPtr ScnLib_GetVideoPreviewWnd();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetVideoResolution(int Width, int Height);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetVideoResolution(ref int Width, ref int Height);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetVideoFrameRate(double FPS);
		[DllImport("ScnLib.dll")] public static extern double ScnLib_GetVideoFrameRate();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetVideoKeyFrameInterval(double Seconds);
		[DllImport("ScnLib.dll")] public static extern double ScnLib_GetVideoKeyFrameInterval();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_EnableVideoVariableFrameRate(bool Enable);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsVideoVariableFrameRateEnabled();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_ConfigureVideoCodec(IntPtr ParentWnd);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_SetVideoCodecExtraArgsW(string Args);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetVideoCodecExtraArgsW(StringBuilder Args); // Args >= 1024 Chars
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetVideoQuality(int CRF);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetVideoQuality();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetVideoBitrate(int Kbps);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetVideoBitrate();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetAudioBitrate(int Kbps);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetAudioBitrate();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetStreamingBitrate(int Kbps);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetStreamingBitrate();
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetAudioSourceDeviceCount(bool Playback);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_GetAudioSourceDeviceW(bool Playback, int Index, StringBuilder Device); // Device >= 260 Chars
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SelectAudioSourceDevice(bool Playback, int Index);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetSelectedAudioSourceDevice(bool Playback);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_ConfigureAudioSourceDevices(bool Playback);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_RecordAudioSource(bool Playback, bool Enable);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsRecordAudioSource(bool Playback);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetAudioSourceVolume(bool Playback, int Volume);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetAudioSourceVolume(bool Playback);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_MonitorVolumeLevel(bool Enable);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsMonitoringVolumeLevel();
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetAudioSourceVolumeLevel(bool Playback);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetMicrophoneDelay(int Milliseconds);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetMicrophoneDelay();
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetWebcamDeviceCount();
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_GetWebcamDeviceW(int Index, StringBuilder Device); // Device >= 260 Chars
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_SelectWebcamDevice(int Index);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetSelectedWebcamDevice();
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_PreviewWebcam(bool Enable, IntPtr Wnd, bool Padding, uint BkColor);
		[DllImport("ScnLib.dll")] public static extern IntPtr ScnLib_GetWebcamPreviewWnd();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_RecordWebcamOnly(bool Enable);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsRecordWebcamOnly();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_InputWebcamFrame(IntPtr RGB, int Width, int Height, int BitCount);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetWebcamResolution(int Width, int Height);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetWebcamResolution(ref int Width, ref int Height);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetWebcamDirection(bool Mirroring, bool Flipping);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetWebcamDirection(ref bool Mirroring, ref bool Flipping);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetWebcamViewMode(int ViewMode);
		[DllImport("ScnLib.dll")] public static extern int ScnLib_GetWebcamViewMode();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetWebcamPosition(int Position, int MarginX, int MarginY);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetWebcamPosition(ref int Position, ref int MarginX, ref int MarginY);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetWebcamViewSize(int ViewWidth, int ViewHeight);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetWebcamViewSize(ref int ViewWidth, ref int ViewHeight);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsLogoVisible();
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_SetLogoImageW(string Path);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetLogoImageW(StringBuilder Path); // Path >= 260 Chars
		[DllImport("ScnLib.dll")] public static extern void ScnLib_UpdateLogoImage(IntPtr RGB, int Width, int Height, int BitCount);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_SetLogoTextW(string Text, [MarshalAs(UnmanagedType.LPStruct)]LOGFONT Font, uint Color, bool Shadow);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern void ScnLib_GetLogoTextW(StringBuilder Text, [MarshalAs(UnmanagedType.LPStruct)]LOGFONT Font, ref uint Color, ref bool Shadow); // Text >= 1024 Chars
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetLogoPosition(int Position, int MarginX, int MarginY);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetLogoPosition(ref int Position, ref int MarginX, ref int MarginY);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetLogoOpacity(double Opacity);
		[DllImport("ScnLib.dll")] public static extern double ScnLib_GetLogoOpacity();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_RecordCursor(bool Enable);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsRecordCursor();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetCursorOriginalSize(bool Enable);
		[DllImport("ScnLib.dll")] public static extern bool ScnLib_IsCursorOriginalSize();
		[DllImport("ScnLib.dll")] public static extern void ScnLib_AddCursorEffects(bool Highlight, bool ClickEffects, bool Track, bool ClickSound);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetCursorEffects(ref bool Highlight, ref bool ClickEffects, ref bool Track, ref bool ClickSound);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_SetCursorEffectsColors(uint HighlightColor, uint LeftClickColor, uint RightClickColor, uint TrackColor);
		[DllImport("ScnLib.dll")] public static extern void ScnLib_GetCursorEffectsColors(ref uint HighlightColor, ref uint LeftClickColor, ref uint RightClickColor, ref uint TrackColor);
		[DllImport("ScnLib.dll", CharSet = CharSet.Unicode)] public static extern bool ScnLib_AddMP4BookmarkW(string Bookmark);  // Bookmark <= 1023 Chars

	}
}
