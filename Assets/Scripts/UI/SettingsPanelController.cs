using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject settingsMainContent;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject skinsPanel;

    private void Start()
    {
        ShowMainSettings();
    }

    public void OpenControlsPanel()
    {
        HideAllSubPanels();
        if (controlsPanel != null)
            controlsPanel.SetActive(true);
    }

    public void OpenSkinsPanel()
    {
        HideAllSubPanels();
        if (skinsPanel != null)
            skinsPanel.SetActive(true);
    }

    public void BackToSettingsMain()
    {
        ShowMainSettings();
    }

    private void ShowMainSettings()
    {
        HideAllSubPanels();
        if (settingsMainContent != null)
            settingsMainContent.SetActive(true);
    }

    private void HideAllSubPanels()
    {
        if (settingsMainContent != null) settingsMainContent.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (skinsPanel != null) skinsPanel.SetActive(false);
    }
}