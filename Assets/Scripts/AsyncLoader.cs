using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AsyncLoader : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private TMP_Text Loading_Value;

    public void LoadAsyncSceneClick(string name)
    {
        LoadingPanel.SetActive(true);
        MainMenu.SetActive(false);

        StartCoroutine(LoadAsyncScene(name));
    }

    public void InGameLoadAsyncScene(string name)
    {
        LoadingPanel.SetActive(true);

        StartCoroutine(LoadAsyncScene(name));
    }

    IEnumerator LoadAsyncScene(string name)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(name);
        Cursor.lockState = CursorLockMode.Locked;
        while (!loadScene.isDone)
        {
            string progressValue = System.Math.Round(Mathf.Clamp01(loadScene.progress)*100, 0).ToString();
            Loading_Value.text = progressValue+"%";
            yield return null;
        }
    }
}
