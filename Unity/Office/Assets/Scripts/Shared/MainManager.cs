using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField] Animator ScreenTransition;
    public static MainManager Instance { get; private set; }

    public static bool GamePaused
    {
        get { return false; }
    }

    List<string> StartingScenes = new List<string>{
        Settings.InGameScreenName,
        Settings.HudScreenName};

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PatchNotes.GameBooted();
            DoKickBack();
        }
        else
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public static void CloseGame()
    {
        Logger.Log("Quitting");

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public static void DoKickBack()
    {
        Instance.StartCoroutine(Instance.KickBack());
    }

    IEnumerator KickBack()
    {
        Logger.Log("Doing kick back");
        yield return StartCoroutine(WaitForSetBlack(true));

        //start unloading scenes apart from boot
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name != Settings.BootScreenName)
            {
                //turn off all root objects in scenes so they stop doing updates
                var rootObjects = scene.GetRootGameObjects();
                foreach (var rootObject in rootObjects)
                {
                    rootObject.SetActive(false);
                }
                //unload scene
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        //wait until all scenes unloaded apart from boot
        bool allScenesUnloaded = false;
        while (!allScenesUnloaded)
        {
            allScenesUnloaded = true;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name != Settings.BootScreenName)
                {
                    allScenesUnloaded = false;
                    break;
                }
            }

            yield return null;
        }

        //start loading starting scenes
        foreach (var scene in StartingScenes)
        {
            AddScene(scene);
        }

        //wait until all StartingScenes loaded
        bool allScenesLoaded = false;
        while (!allScenesLoaded)
        {
            allScenesLoaded = true;
            foreach (var scene in StartingScenes)
            {
                if (!SceneManager.GetSceneByName(scene).isLoaded)
                {
                    allScenesLoaded = false;
                    break;
                }
            }

            yield return null;
        }

        SetShowBlack(false);
    }

    #region screen stuff

    public void TransToScreen(string screenTo, string sceneFrom = "")
    {
        StartCoroutine(TransToScreenCo(screenTo, sceneFrom));
    }

    IEnumerator TransToScreenCo(string screenTo, string sceneFrom)
    {
        yield return StartCoroutine(WaitForSetBlack(true));
        if (!string.IsNullOrEmpty(sceneFrom))
        {
            yield return StartCoroutine(SubtractSceneCo(sceneFrom));
        }

        yield return StartCoroutine(AddSceneCo(screenTo));
        SetShowBlack(false);
    }

    static void AddScene(string scene)
    {
        Instance.StartCoroutine(AddSceneCo(scene));
    }
    public static IEnumerator AddSceneCo(string scene)
    {
        if (!SceneManager.GetSceneByName(scene).isLoaded)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);

            while (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                yield return null;
            }

            //yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
    }

    public static void SubtractScene(string scene)
    {
        Instance.StartCoroutine(SubtractSceneCo(scene));
    }
    public static IEnumerator SubtractSceneCo(string scene)
    {
        if (SceneManager.GetSceneByName(scene).isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(scene);
        }
    }
    #endregion

    #region ScreenTransition

    bool IsAnimating
    {
        get
        {
            var stateInfo = ScreenTransition.GetCurrentAnimatorStateInfo(0);
            return !stateInfo.IsName("Black") && !stateInfo.IsName("Open");
        }
    }

    bool ShowingBlack;

    void SetShowBlack(bool showBlack)
    {
        ScreenTransition.SetBool("Open", !showBlack);

        Logger.Log($"setting show black to: {showBlack} was {ShowingBlack}");
        ShowingBlack = showBlack;

        ScreenTransition.speed = 1;
    }

    IEnumerator WaitForSetBlack(bool showBlack)
    {
        if (ShowingBlack == showBlack)
        {
            yield break;
        }

        SetShowBlack(showBlack);
        yield return null;

        do
        {
            yield return null;
        } while (IsAnimating);

        yield break;
    }
    #endregion
}
