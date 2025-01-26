using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class VideoOption : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // 해상도 드롭다운
    public Toggle fullscreenToggle;    // 풀스크린 토글
    private List<Resolution> resolutions = new List<Resolution>();

    void Start()
    {
        InitUi();
    }

    void InitUi()
    {
        // 현재 시스템에서 지원하는 모든 해상도를 가져오기
        Resolution[] allResolutions = Screen.resolutions;
        HashSet<string> uniqueResolutions = new HashSet<string>();

        foreach (Resolution res in allResolutions)
        {
            // 16:9 비율인지 확인
            if (Mathf.Abs((float)res.width / res.height - 16f / 9f) < 0.01f)
            {
                string resolutionString = res.width + " x " + res.height;
                if (!uniqueResolutions.Contains(resolutionString))
                {
                    uniqueResolutions.Add(resolutionString);
                    resolutions.Add(res);
                }
            }
        }

        // Dropdown 초기화
        resolutionDropdown.ClearOptions();

        // Dropdown에 추가할 옵션 생성
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // 현재 해상도와 일치하는 옵션 저장
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Dropdown에 옵션 추가 및 현재 해상도로 설정
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Dropdown 변경 시 호출될 이벤트 연결
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // 풀스크린 토글 초기화 및 이벤트 연결
        fullscreenToggle.isOn = Screen.fullScreen; // 현재 풀스크린 상태로 초기화
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = resolutions[resolutionIndex];

        // 해상도 변경 및 풀스크린 여부 확인
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, false);

        // 풀스크린 상태 업데이트
        UpdateFullScreenToggle(selectedResolution);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (isFullScreen)
        {
            // 현재 해상도를 탐색하여 Dropdown에 반영
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    resolutionDropdown.value = i;
                    resolutionDropdown.RefreshShownValue();
                    break;
                }
            }
        }
    }

    private void UpdateFullScreenToggle(Resolution resolution)
    {
        // 현재 선택된 해상도가 시스템의 지원 풀스크린 해상도인지 확인
        bool isFullScreenResolution = false;

        foreach (Resolution res in Screen.resolutions)
        {
            if (res.width == resolution.width && res.height == resolution.height)
            {
                isFullScreenResolution = true;
                break;
            }
        }

        // 풀스크린 토글 업데이트
        fullscreenToggle.isOn = isFullScreenResolution && Screen.fullScreen;
    }
}
