using System.Collections.Generic;
using UnityEngine;

public class GameManager01 : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] int second, hour, mood = 100;
    [SerializeField] GameObject prefab_canvasText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GameManager01 Instance;
    List<GameObject> TextPopup = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < 40; i++)
        {
            GameObject pref = Instantiate(prefab_canvasText);
            TextPopup.Add(pref);
            pref.SetActive(false);
        }
    }
    void Start()
    {

        // InvokeRepeating("UpdateMood", 0f, 5f);
        InvokeRepeating("TimerUp", 1f, 1f);

    }

    void TimerUp()
    {
        second++;
        // update text
        string hourText = hour < 10 ? $"0{hour}" : hour.ToString();
        string minuteText = second < 10 ? $"0{second}" : second.ToString();

        HUDManager.Instance.textHour.text = $"{hourText}:{minuteText}";
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

    public void SetMoodVal(int add)
    {
        if (add > 0) mood += add;
        else mood += add * 8;

        if(mood >= 100) mood = 100;

        UpdateMood();
    }

    public void ShowTextPopup(string teks)
    {
        GameObject g = GetTextPopup();
        g.transform.position = new Vector3(Random.Range(1.5f, 8), Random.Range(-4.4f, 1), -3f);
        g.GetComponent<TextPopup>().SetText(teks);
        g.SetActive(true);
    }

    void UpdateMood()
    {
        HUDManager.Instance.SetTextMood(mood);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!isPaused)
            {
                HUDManager.Instance.PauseGameUI();
                Time.timeScale = 0;
            }
            else
            {
                HUDManager.Instance.UnPauseGameUI();
                Time.timeScale = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
            HUDManager.Instance.ChangeScreenUI();

        if (Input.GetKeyDown(KeyCode.C))
            HUDManager.Instance.SnapUI();

        if (Input.GetKeyDown(KeyCode.M))
            HUDManager.Instance.ToggleMuteMusic();

    }

}
