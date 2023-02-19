using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    GameObject mainButtons;
    [SerializeField]
    GameObject settingsInput, controls, backButton;
    [SerializeField]
    TMP_InputField durationText;
    [SerializeField]
    TMP_InputField respawnText;

    void Start()
    {
        durationText.text =  ConfigManager.Instance.GetMatchDuration().ToString();
        respawnText.text = ConfigManager.Instance.GetRespawnTime().ToString();
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackButton()
    {
        mainButtons.SetActive(true);
        controls.SetActive(false);
        settingsInput.SetActive(false);
        backButton.SetActive(false);
        mainButtons.transform.parent.gameObject.GetComponent<Image>().enabled = false;
    }

    public void OnGoToSettings()
    {
        mainButtons.SetActive(false);
        controls.SetActive(false);
        settingsInput.SetActive(true);
        backButton.SetActive(true);
        mainButtons.transform.parent.gameObject.GetComponent<Image>().enabled = true;
    }

    public void OnGoToControls()
    {
        mainButtons.SetActive(false);
        controls.SetActive(true);
        settingsInput.SetActive(false);
        backButton.SetActive(true);
        mainButtons.transform.parent.gameObject.GetComponent<Image>().enabled = true;
    }

    public void OnChangeDuration(string input)
    {
        durationText.text = ConfigManager.Instance.SetMatchDuration(input);      
    }

    public void OnChangeRespawn(string input)
    {
        respawnText.text = ConfigManager.Instance.SetRespawnTime(input);
    }

}
