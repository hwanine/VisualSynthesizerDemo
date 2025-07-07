# VisualSynthesizerDemo

## 개요
VisualSynthesizerDemo는 DlibDotNet과 OpenCvSharp를 활용하여 이미지/비디오/웹캠에서 얼굴 및 코 영역을 실시간으로 검출하고, WPF UI에 시각적으로 표시하는 데모 애플리케이션.

---

## 지원 환경 및 플랫폼

- **프레임워크:** .NET Framework 4.8
- **플랫폼:** Windows 10/11, x64 
- **UI:** WPF (Windows Presentation Foundation)
---

## 주요 기능
- 이미지 파일에서 얼굴 및 코 영역 검출
- 비디오 파일에서 프레임 단위로 코 영역 실시간 검출
- 웹캠 실시간 코 검출
- 검출 결과 WPF UI에 실시간 시각화
---

## 사용법

### 실행 방법

- 빌드 패키지 실행
   - Release_Demo 폴더 내 VisualSynthesizerDemo.exe 파일 실행

- 직접 빌드 후 실행 or 빌드 패키지 실행
   - Visual Studio에서 솔루션 로드 후 빌드 및 실행(Release/Debug x64)
   

### 동작 방법

1. **이미지 파일 열기**
   - "Image/Video File Open" 버튼 클릭 → 이미지 선택 → 코 영역 자동 검출 및 표시
   - 지원 확장자: `.jpg`, `.jpeg`, `.png`, `.bmp`, `.gif`

2. **비디오 파일 열기**
   - "Image/Video File Open" 버튼 클릭 → 비디오 선택 → 프레임별 코 영역 실시간 검출 및 표시
   - 지원 확장자: `.mp4`, `.avi`, `.mov`, `.mkv`

3. **웹캠 사용**
   - 웹캠 연결
   - "Webcam Load" → "Webcam Start" 버튼 순서로 클릭  
   - 실시간 웹캠 영상에서 코 영역 검출
   - "Webcam Stop" 버튼으로 중단

5. **UI**
   - 검출된 코 영역은 빨간색 입체 원으로 표시됨

---
