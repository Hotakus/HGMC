// This is a part of the ZD Soft Screen Recorder SDK
// Copyright (C) 2005-2018, ZD Soft, all rights reserved.

#ifndef __SCNLIB_H__
#define __SCNLIB_H__

#ifdef _MSC_VER
#pragma comment(linker, "/LARGEADDRESSAWARE")
#endif

#if _MSC_VER >= 1400 // Visual C++ 2005 or later
#pragma comment(linker, "/manifestdependency:\"type='win32' name='ScnLib.DLLs' version='1.0.0.0'\"")
#endif /* Equivalent of adding the following lines to the application manifest file:

<dependency>
  <dependentAssembly>
    <assemblyIdentity
      type="win32"
      name="ScnLib.DLLs"
      version="1.0.0.0" />
  </dependentAssembly>
</dependency>

You need to do this manually for Visual C++ 6.0 / 2002 / 2003 and C++Builder.*/

#include <tchar.h>
#include <windows.h>

#define SCNLIB_POSITION_TOP_LEFT        0
#define SCNLIB_POSITION_TOP             1
#define SCNLIB_POSITION_TOP_RIGHT       2
#define SCNLIB_POSITION_RIGHT           3
#define SCNLIB_POSITION_BOTTOM_RIGHT    4
#define SCNLIB_POSITION_BOTTOM          5
#define SCNLIB_POSITION_BOTTOM_LEFT     6
#define SCNLIB_POSITION_LEFT            7
#define SCNLIB_POSITION_CENTER          8

#define SCNLIB_WEBCAM_VIEW_PADDING      0
#define SCNLIB_WEBCAM_VIEW_CROPPING     1
#define SCNLIB_WEBCAM_VIEW_STRETCHING   2

#ifdef SCNLIB_EXPORT
#define SCNLIB_API __declspec(dllexport)
#else
#define SCNLIB_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

SCNLIB_API void __stdcall ScnLib_About(void);
SCNLIB_API BOOL __stdcall ScnLib_SetLicenseA(LPCSTR pcszName, LPCSTR pcszEmail, LPCSTR pcszKey);
SCNLIB_API BOOL __stdcall ScnLib_SetLicenseW(LPCWSTR pcwszName, LPCWSTR pcwszEmail, LPCWSTR pcwszKey);
SCNLIB_API void __stdcall ScnLib_GetErrorMessageA(LPSTR pszErrMsg); // pszErrMsg >= 2048 CHARs
SCNLIB_API void __stdcall ScnLib_GetErrorMessageW(LPWSTR pwszErrMsg); // pwszErrMsg >= 2048 WCHARs
SCNLIB_API BOOL __stdcall ScnLib_CheckComponents(void);
SCNLIB_API BOOL __stdcall ScnLib_Initialize(void);
SCNLIB_API void __stdcall ScnLib_Uninitialize(void);
SCNLIB_API void __stdcall ScnLib_SetVideoPathA(LPCSTR pcszPath);
SCNLIB_API void __stdcall ScnLib_GetVideoPathA(LPSTR pszPath, BOOL bSaved); // pszPath >= 260 CHARs
SCNLIB_API void __stdcall ScnLib_SetVideoPathW(LPCWSTR pcwszPath);
SCNLIB_API void __stdcall ScnLib_GetVideoPathW(LPWSTR pwszPath, BOOL bSaved); // pwszPath >= 260 WCHARs
SCNLIB_API void __stdcall ScnLib_SetAudioPathA(LPCSTR pcszPath);
SCNLIB_API void __stdcall ScnLib_GetAudioPathA(LPSTR pszPath, BOOL bSaved); // pszPath >= 260 CHARs
SCNLIB_API void __stdcall ScnLib_SetAudioPathW(LPCWSTR pcwszPath);
SCNLIB_API void __stdcall ScnLib_GetAudioPathW(LPWSTR pwszPath, BOOL bSaved); // pwszPath >= 260 WCHARs
SCNLIB_API void __stdcall ScnLib_SetStreamingUrlA(LPCSTR pcszURL);
SCNLIB_API void __stdcall ScnLib_GetStreamingUrlA(LPSTR pszURL); // pszURL >= 2048 CHARs
SCNLIB_API void __stdcall ScnLib_SetStreamingUrlW(LPCWSTR pcwszURL);
SCNLIB_API void __stdcall ScnLib_GetStreamingUrlW(LPWSTR pwszURL); // pwszURL >= 2048 WCHARs
SCNLIB_API BOOL __stdcall ScnLib_TakeScreenshotA(LPSTR pszPath, long left, long top, long right, long bottom); // pszPath >= 260 CHARs
SCNLIB_API BOOL __stdcall ScnLib_TakeScreenshotW(LPWSTR pwszPath, long left, long top, long right, long bottom); // pwszPath >= 260 WCHARs
SCNLIB_API void __stdcall ScnLib_SetCaptureRegion(long left, long top, long right, long bottom);
SCNLIB_API void __stdcall ScnLib_GetCaptureRegion(long *left, long *top, long *right, long *bottom);
SCNLIB_API BOOL __stdcall ScnLib_SelectCaptureRegion(long *left, long *top, long *right, long *bottom);
SCNLIB_API void __stdcall ScnLib_ShowCaptureRegionFrame(BOOL bEnable);
SCNLIB_API HWND __stdcall ScnLib_GetCaptureRegionFrameWnd(void);
SCNLIB_API void __stdcall ScnLib_EnableGPUAcceleration(BOOL bEnable);
SCNLIB_API BOOL __stdcall ScnLib_IsGPUAccelerationEnabled(void);
SCNLIB_API BOOL __stdcall ScnLib_ShowCountdownBox(int nSeconds);
SCNLIB_API BOOL __stdcall ScnLib_StartRecording(void);
SCNLIB_API BOOL __stdcall ScnLib_PauseRecording(void);
SCNLIB_API BOOL __stdcall ScnLib_ResumeRecording(void);
SCNLIB_API void __stdcall ScnLib_StopRecording(void);
SCNLIB_API BOOL __stdcall ScnLib_IsRecording(void);
SCNLIB_API BOOL __stdcall ScnLib_IsPaused(void);
SCNLIB_API DWORD __stdcall ScnLib_GetRecTime(void);
SCNLIB_API void __stdcall ScnLib_GetRecTimeA(LPSTR pszTime); // pszTime >= 11 CHARs
SCNLIB_API void __stdcall ScnLib_GetRecTimeW(LPWSTR pwszTime); // pwszTime >= 11 WCHARs
SCNLIB_API void __stdcall ScnLib_ZoomInScreen(double dRatio);
SCNLIB_API double __stdcall ScnLib_GetZoomRatio(void);
SCNLIB_API void __stdcall ScnLib_SetZoomSpeed(double dSpeed);
SCNLIB_API double __stdcall ScnLib_GetZoomSpeed(void);
SCNLIB_API BOOL __stdcall ScnLib_PreviewVideo(BOOL bEnable, HWND hWnd, BOOL bPadding, COLORREF crBkColor);
SCNLIB_API HWND __stdcall ScnLib_GetVideoPreviewWnd(void);
SCNLIB_API void __stdcall ScnLib_SetVideoResolution(int nWidth, int nHeight);
SCNLIB_API void __stdcall ScnLib_GetVideoResolution(int *nWidth, int *nHeight);
SCNLIB_API void __stdcall ScnLib_SetVideoFrameRate(double dFPS);
SCNLIB_API double __stdcall ScnLib_GetVideoFrameRate(void);
SCNLIB_API void __stdcall ScnLib_SetVideoKeyFrameInterval(double dSeconds);
SCNLIB_API double __stdcall ScnLib_GetVideoKeyFrameInterval(void);
SCNLIB_API void __stdcall ScnLib_EnableVideoVariableFrameRate(BOOL bEnable);
SCNLIB_API BOOL __stdcall ScnLib_IsVideoVariableFrameRateEnabled(void);
SCNLIB_API void __stdcall ScnLib_ConfigureVideoCodec(HWND hwndParent);
SCNLIB_API void __stdcall ScnLib_SetVideoQuality(int nCRF);
SCNLIB_API int __stdcall ScnLib_GetVideoQuality(void);
SCNLIB_API void __stdcall ScnLib_SetVideoBitrate(int nKbps);
SCNLIB_API int __stdcall ScnLib_GetVideoBitrate(void);
SCNLIB_API void __stdcall ScnLib_SetAudioBitrate(int nKbps);
SCNLIB_API int __stdcall ScnLib_GetAudioBitrate(void);
SCNLIB_API void __stdcall ScnLib_SetStreamingBitrate(int nKbps);
SCNLIB_API int __stdcall ScnLib_GetStreamingBitrate(void);
SCNLIB_API int __stdcall ScnLib_GetAudioSourceDeviceCount(BOOL bPlayback);
SCNLIB_API BOOL __stdcall ScnLib_GetAudioSourceDeviceA(BOOL bPlayback, int nIndex, LPSTR pszDevice); // pszDevice >= 260 CHARs
SCNLIB_API BOOL __stdcall ScnLib_GetAudioSourceDeviceW(BOOL bPlayback, int nIndex, LPWSTR pwszDevice); // pwszDevice >= 260 WCHARs
SCNLIB_API void __stdcall ScnLib_SelectAudioSourceDevice(BOOL bPlayback, int nIndex);
SCNLIB_API int __stdcall ScnLib_GetSelectedAudioSourceDevice(BOOL bPlayback);
SCNLIB_API void __stdcall ScnLib_ConfigureAudioSourceDevices(BOOL bPlayback);
SCNLIB_API void __stdcall ScnLib_RecordAudioSource(BOOL bPlayback, BOOL bEnable);
SCNLIB_API BOOL __stdcall ScnLib_IsRecordAudioSource(BOOL bPlayback);
SCNLIB_API void __stdcall ScnLib_SetAudioSourceVolume(BOOL bPlayback, int nVolume);
SCNLIB_API int __stdcall ScnLib_GetAudioSourceVolume(BOOL bPlayback);
SCNLIB_API void __stdcall ScnLib_MonitorVolumeLevel(BOOL bEnable);
SCNLIB_API BOOL __stdcall ScnLib_IsMonitoringVolumeLevel(void);
SCNLIB_API int __stdcall ScnLib_GetAudioSourceVolumeLevel(BOOL bPlayback);
SCNLIB_API void __stdcall ScnLib_SetMicrophoneDelay(int nMilliseconds);
SCNLIB_API int __stdcall ScnLib_GetMicrophoneDelay(void);
SCNLIB_API int __stdcall ScnLib_GetWebcamDeviceCount(void);
SCNLIB_API BOOL __stdcall ScnLib_GetWebcamDeviceA(int nIndex, LPSTR pszDevice); // pszDevice >= 260 CHARs
SCNLIB_API BOOL __stdcall ScnLib_GetWebcamDeviceW(int nIndex, LPWSTR pwszDevice); // pwszDevice >= 260 WCHARs
SCNLIB_API BOOL __stdcall ScnLib_SelectWebcamDevice(int nIndex);
SCNLIB_API int __stdcall ScnLib_GetSelectedWebcamDevice(void);
SCNLIB_API BOOL __stdcall ScnLib_PreviewWebcam(BOOL bEnable, HWND hWnd, BOOL bPadding, COLORREF crBkColor);
SCNLIB_API HWND __stdcall ScnLib_GetWebcamPreviewWnd(void);
SCNLIB_API void __stdcall ScnLib_RecordWebcamOnly(BOOL bEnable);
SCNLIB_API BOOL __stdcall ScnLib_IsRecordWebcamOnly(void);
SCNLIB_API void __stdcall ScnLib_InputWebcamFrame(const PVOID pRGB, int nWidth, int nHeight, int nBitCount);
SCNLIB_API void __stdcall ScnLib_SetWebcamResolution(int nWidth, int nHeight);
SCNLIB_API void __stdcall ScnLib_GetWebcamResolution(int *nWidth, int *nHeight);
SCNLIB_API void __stdcall ScnLib_SetWebcamDirection(BOOL bMirroring, BOOL bFlipping);
SCNLIB_API void __stdcall ScnLib_GetWebcamDirection(BOOL *bMirroring, BOOL *bFlipping);
SCNLIB_API void __stdcall ScnLib_SetWebcamViewMode(int nViewMode);
SCNLIB_API int __stdcall ScnLib_GetWebcamViewMode(void);
SCNLIB_API void __stdcall ScnLib_SetWebcamPosition(int nPosition, int nMarginX, int nMarginY);
SCNLIB_API void __stdcall ScnLib_GetWebcamPosition(int *nPosition, int *nMarginX, int *nMarginY);
SCNLIB_API void __stdcall ScnLib_SetWebcamViewSize(int nViewWidth, int nViewHeight);
SCNLIB_API void __stdcall ScnLib_GetWebcamViewSize(int *nViewWidth, int *nViewHeight);
SCNLIB_API BOOL __stdcall ScnLib_IsLogoVisible(void);
SCNLIB_API BOOL __stdcall ScnLib_SetLogoImageA(LPCSTR pcszPath);
SCNLIB_API void __stdcall ScnLib_GetLogoImageA(LPSTR pszPath); // pszPath >= 260 CHARs
SCNLIB_API BOOL __stdcall ScnLib_SetLogoImageW(LPCWSTR pcwszPath);
SCNLIB_API void __stdcall ScnLib_GetLogoImageW(LPWSTR pwszPath); // pwszPath >= 260 WCHARs
SCNLIB_API void __stdcall ScnLib_UpdateLogoImage(const PVOID pRGB, int nWidth, int nHeight, int nBitCount);
SCNLIB_API BOOL __stdcall ScnLib_SetLogoTextA(LPCSTR pcszText, const LOGFONTA *lfFont, COLORREF crColor, BOOL bShadow);
SCNLIB_API void __stdcall ScnLib_GetLogoTextA(LPSTR pszText, LOGFONTA *lfFont, COLORREF *crColor, BOOL *bShadow); // pszText >= 1024 CHARs
SCNLIB_API BOOL __stdcall ScnLib_SetLogoTextW(LPCWSTR pcwszText, const LOGFONTW *lfFont, COLORREF crColor, BOOL bShadow);
SCNLIB_API void __stdcall ScnLib_GetLogoTextW(LPWSTR pwszText, LOGFONTW *lfFont, COLORREF *crColor, BOOL *bShadow); // pwszText >= 1024 WCHARs
SCNLIB_API void __stdcall ScnLib_SetLogoPosition(int nPosition, int nMarginX, int nMarginY);
SCNLIB_API void __stdcall ScnLib_GetLogoPosition(int *nPosition, int *nMarginX, int *nMarginY);
SCNLIB_API void __stdcall ScnLib_SetLogoOpacity(double dOpacity);
SCNLIB_API double __stdcall ScnLib_GetLogoOpacity(void);
SCNLIB_API void __stdcall ScnLib_RecordCursor(BOOL bEnable);
SCNLIB_API BOOL __stdcall ScnLib_IsRecordCursor(void);
SCNLIB_API void __stdcall ScnLib_SetCursorOriginalSize(BOOL bEnable);
SCNLIB_API BOOL __stdcall ScnLib_IsCursorOriginalSize(void);
SCNLIB_API void __stdcall ScnLib_AddCursorEffects(BOOL bHighlight, BOOL bClickEffects, BOOL bTrack, BOOL bClickSound);
SCNLIB_API void __stdcall ScnLib_GetCursorEffects(BOOL *bHighlight, BOOL *bClickEffects, BOOL *bTrack, BOOL *bClickSound);
SCNLIB_API void __stdcall ScnLib_SetCursorEffectsColors(COLORREF crHighlight, COLORREF crLeftClick, COLORREF crRightClick, COLORREF crTrack);
SCNLIB_API void __stdcall ScnLib_GetCursorEffectsColors(COLORREF *crHighlight, COLORREF *crLeftClick, COLORREF *crRightClick, COLORREF *crTrack);

#ifdef __cplusplus
}
#endif

#endif //__SCNLIB_H__
