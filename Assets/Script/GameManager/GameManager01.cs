using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager01 : MonoBehaviour
{
    [SerializeField] bool isPaused = false, isCanEvent = true, isCanDialog = true;
    [SerializeField] int id_env = 1;
    [SerializeField] int second = 0, hour, times = 0, mood = 100, eventTriggerDur = 60, vol_content = 8;
    [SerializeField] GameObject prefab_canvasText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // audio need
    public float hideDelay = 2f;
    private float timerAudioInteract;
    private bool isShowAudio = false, isFinished = false;

    public static GameManager01 Instance;
    List<GameObject> TextPopup = new List<GameObject>();

    [Header("== Level 2 Only ==")]
    [SerializeField] bool isLookToMe = false;
    [SerializeField] Animator animatorPenumpang;

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < 20; i++)
        {
            GameObject pref = Instantiate(prefab_canvasText);
            TextPopup.Add(pref);
            pref.SetActive(false);
        }
    }
    void Start()
    {
        mood = PlayerPrefs.GetInt("mood", 100);
        Debug.Log("GameManager01.Start() called");

        // InvokeRepeating("UpdateMood", 0f, 5f);
        InvokeRepeating("TimerUp", 1f, 1f);
        SetVolumeContent(1);

        if (HUDManager.Instance == null)
        {
            Debug.LogError("GameManager01.Start: HUDManager.Instance is null");
            return;
        }

        CanvasGroup bgCanvasGroup = HUDManager.Instance.GetCGBG();
        if (bgCanvasGroup == null)
        {
            Debug.LogError("GameManager01.Start: CG_bg (CanvasGroup) is null");
            return;
        }

        Debug.Log("GameManager01.Start: Starting fade animation");
        bgCanvasGroup.alpha = 1;

        StartCoroutine(WaitFor(3f, () =>
        {
            bgCanvasGroup.DOFade(0f, 3f);
        }));

    }

    public void SetIsCanDialog(bool val)
    {
        isCanDialog = val;
    }

    public bool GetIsCanDialog()
    {
        return isCanDialog;
    }

    void TimerUp()
    {
        second++;
        times++;
        // update text
        string hourText = hour < 10 ? $"0{hour}" : hour.ToString();
        string minuteText = second < 10 ? $"0{second}" : second.ToString();

        if (second == 60)
        {
            second = 0;
            hour++;
        }

        if (times >= eventTriggerDur)
            DoEvent();

        if (times >= 120 && !isFinished)
        {
            isFinished = true;
            //Do next bg
            Debug.Log("Next Load BG");

            PlayerPrefs.SetInt("mood", mood);
            PlayerPrefs.Save();

            HUDManager.Instance.GetCGBG().DOFade(1, 2f).OnComplete(() =>
            {
                SceneManager.LoadScene($"main_{id_env + 1}");
            });

        }
        HUDManager.Instance.textHour.text = $"{hourText}:{minuteText}";
    }

    void DoEvent()
    {
        if (!isCanEvent) return;
        isCanEvent = false;

        // do event
        if (id_env == 1)
        {
            isCanDialog = false;
            AudioManager.Instance.StopAudio(AudioManager.Instance.aS_dialogue);
            HUDManager.Instance.textDialogueUI.text = "";
        }

        if (id_env == 2)
        {

        }

        if (id_env == 3)
        {

        }

        if (id_env == 4)
        {
            // isCanDialog = false;
            AudioManager.Instance.StopAudio(AudioManager.Instance.aS_dialogue);
            HUDManager.Instance.textDialogueUI.text = "";

            HUDManager.Instance.animatorBgEnv.Play("event");
            HUDManager.Instance.ToggleMuteMusic();

            AudioManager.Instance.aS_event.Stop();
            AudioManager.Instance.aS_event.Play();

            StartCoroutine(WaitFor(4f, () =>
            {

                // if (PlayerPrefs.GetInt("isMuteMusic") == 0) // kalo mute)
                HUDManager.Instance.ToggleMuteMusic();
            }));


            // isCanDialog = true;
        }

        if (id_env == 5)
        {

        }

        if (id_env == 6)
        {

        }

    }


    GameObject GetTextPopup()
    {
        foreach (GameObject g in TextPopup)
        {
            if (!g.activeInHierarchy)
            {
                return g;
            }
        }

        return null;
    }

    IEnumerator WaitFor(float d, Action action)
    {
        yield return new WaitForSeconds(d);
        action?.Invoke();
    }

    public void SetMoodVal(int add)
    {
        if (add > 0) mood += add;
        else mood += add * 5;

        if (mood >= 100) mood = 100;
        if (mood <= 0) mood = 0;

        UpdateMood();
    }

    public void ShowTextPopup(string teks)
    {
        GameObject g = GetTextPopup();
        g.transform.position = new Vector3(UnityEngine.Random.Range(1.5f, 8), UnityEngine.Random.Range(-4.4f, 1), -3f);
        g.GetComponent<TextPopup>().SetText(teks);
        g.SetActive(true);
    }

    void UpdateMood()
    {
        HUDManager.Instance.SetTextMood(mood);
    }

    void Level2()
    {
        if (id_env != 2) return;

        if (vol_content >= 8 && !isLookToMe)
        {
            isLookToMe = true;
            animatorPenumpang.Play("look", 0, 0);
        }
        if (vol_content < 8 && isLookToMe)
        {
            isLookToMe = false;
            animatorPenumpang.Play("idle", 0, 0);
        }
    }

    void Level3()
    {
        if (id_env != 2) return;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            isPaused = !isPaused;

            if (!isPaused)
            {
                HUDManager.Instance.UnPauseGameUI();
            }
            else
            {
                HUDManager.Instance.PauseGameUI();
            }
        }

        EventPause();

        if (Input.GetKeyDown(KeyCode.F))
            HUDManager.Instance.ChangeScreenUI();

        if (Input.GetKeyDown(KeyCode.C))
            HUDManager.Instance.SnapUI();

        if (Input.GetKeyDown(KeyCode.M))
            HUDManager.Instance.ToggleMuteMusic();

        CheckAudioState();

        // Level2 dan Level 3

        Level2();
        Level3();
    }


    void EventPause()
    {
        if (!isPaused) return;
        if (Input.GetKeyDown(KeyCode.S))
            HUDManager.Instance.LoadSettingScene();
        if (Input.GetKeyDown(KeyCode.C))
            HUDManager.Instance.LoadControlScene();
        if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene("Home");
    }

    void ShowVolume()
    {
        if (!isShowAudio)
        {
            HUDManager.Instance.animatorAudioMenu.Play("show", 0, 0);
        }

        isShowAudio = true;
        timerAudioInteract = hideDelay; // langsung reset ketika dipanggil dari luar juga
    }

    void HideVolume()
    {
        HUDManager.Instance.animatorAudioMenu.Play("hide");
        isShowAudio = false;
    }

    void CheckAudioState()
    {
        // Cek input A atau D
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowVolume();
            SetVolumeContent(-1);

            string teksMood = "volume--";
            ShowTextPopup(teksMood);
            timerAudioInteract = hideDelay; // reset

            AudioManager.Instance.PlayHoverAudio();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowVolume();

            string teksMood = "volume++";
            if (vol_content >= 8) teksMood = "volume full";

            ShowTextPopup(teksMood);

            SetVolumeContent(1);

            timerAudioInteract = hideDelay; // reset
            AudioManager.Instance.PlayHoverAudio();
        }

        // Jika bar sedang tampil → mulai hitung mundur
        if (isShowAudio)
        {
            timerAudioInteract -= Time.deltaTime;

            if (timerAudioInteract <= 0)
                HideVolume();
        }
    }

    void SetVolumeContent(int newVal)
    {
        vol_content += newVal;

        // Clamp dulu biar aman
        if (vol_content > 8) vol_content = 8;
        if (vol_content < 0) vol_content = 0;

        // Guard: Check HUDManager.Instance and fillAudio before using
        if (HUDManager.Instance == null)
        {
            Debug.LogError("GameManager01.SetVolumeContent: HUDManager.Instance is null.");
            return;
        }

        if (HUDManager.Instance.fillAudio == null)
        {
            Debug.LogError("GameManager01.SetVolumeContent: HUDManager.Instance.fillAudio is null.");
            return;
        }

        // Update UI
        HUDManager.Instance.UpdateFillImage(HUDManager.Instance.fillAudio, vol_content);

        // Atur volume audio
        if (AudioManager.Instance == null)
        {
            Debug.LogError("GameManager01.SetVolumeContent: AudioManager.Instance is null.");
            return;
        }

        float maxVol = 1f;

        // Kalau vol_content = 0, biar gak crash → volume = 0
        if (vol_content == 0)
        {
            AudioManager.Instance.aS_content.volume = 0f;
        }
        else
        {
            Debug.LogWarning($"{maxVol * (vol_content / 8f)}");
            AudioManager.Instance.aS_content.volume = maxVol * (vol_content / 8f);
        }
    }

}
