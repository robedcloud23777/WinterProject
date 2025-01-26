using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class VideoOption : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // �ػ� ��Ӵٿ�
    public Toggle fullscreenToggle;    // Ǯ��ũ�� ���
    private List<Resolution> resolutions = new List<Resolution>();

    void Start()
    {
        InitUi();
    }

    void InitUi()
    {
        // ���� �ý��ۿ��� �����ϴ� ��� �ػ󵵸� ��������
        Resolution[] allResolutions = Screen.resolutions;
        HashSet<string> uniqueResolutions = new HashSet<string>();

        foreach (Resolution res in allResolutions)
        {
            // 16:9 �������� Ȯ��
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

        // Dropdown �ʱ�ȭ
        resolutionDropdown.ClearOptions();

        // Dropdown�� �߰��� �ɼ� ����
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // ���� �ػ󵵿� ��ġ�ϴ� �ɼ� ����
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Dropdown�� �ɼ� �߰� �� ���� �ػ󵵷� ����
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Dropdown ���� �� ȣ��� �̺�Ʈ ����
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Ǯ��ũ�� ��� �ʱ�ȭ �� �̺�Ʈ ����
        fullscreenToggle.isOn = Screen.fullScreen; // ���� Ǯ��ũ�� ���·� �ʱ�ȭ
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = resolutions[resolutionIndex];

        // �ػ� ���� �� Ǯ��ũ�� ���� Ȯ��
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, false);

        // Ǯ��ũ�� ���� ������Ʈ
        UpdateFullScreenToggle(selectedResolution);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (isFullScreen)
        {
            // ���� �ػ󵵸� Ž���Ͽ� Dropdown�� �ݿ�
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
        // ���� ���õ� �ػ󵵰� �ý����� ���� Ǯ��ũ�� �ػ����� Ȯ��
        bool isFullScreenResolution = false;

        foreach (Resolution res in Screen.resolutions)
        {
            if (res.width == resolution.width && res.height == resolution.height)
            {
                isFullScreenResolution = true;
                break;
            }
        }

        // Ǯ��ũ�� ��� ������Ʈ
        fullscreenToggle.isOn = isFullScreenResolution && Screen.fullScreen;
    }
}
