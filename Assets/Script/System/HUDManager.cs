using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class HUDManager : MonoBehaviour
{
    // Aps content
    public List<SpriteRenderer> spriteRenderersContent;

    public TMP_Text textDialogueUI, textContent, textMood, textHour;
    public Image fillEnergy, fillAudio;
    [SerializeField] public RawImage rawImageLike, rawImageAudio;

    public static HUDManager Instance;

    public Texture2D textureLike, textureLikeActive, textureAudio, textureAudioMute;

    bool isFullScreen = false;

    [SerializeField] CanvasGroup CG_audio, CG_pause, CG_topBar, CG_bottomBar, CG_like;
    [SerializeField] Animator animatorAudioMenu, animatorPauseMenu, animatorTopBottomBar;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
    
        isFullScreen = false;
    }

    void Update()
    {
        PauseGameUI();
    }

    public CanvasGroup GetCGLIke()
    {
        return CG_like;
    }

    public void ResetTextureLike()
    {
        rawImageLike.texture = textureLike;
    }

    public void SetTextureLike()
    {
        rawImageLike.texture = textureLikeActive;
    }

    public void SetTextMood(int moodValue)
    {
        string mood;
        // if()
        if (moodValue >= 80) mood = "Happy";
        else if (moodValue >= 60) mood = "Neutral";
        else if (moodValue >= 40) mood = "Upset";
        else if (moodValue >= 20) mood = "Sad";
        else
            mood = "Broken";


        textMood.text = $"Mood : {mood}";
    }


    // lets make functionality for game
    public void PauseGameUI()
    {

    }

    public void UnPauseGameUI()
    {

    }

    public void ChangeScreenUI()
    {
        isFullScreen = !isFullScreen;

        if (isFullScreen == false) // ada item2
        {
            // set full

            CG_bottomBar.DOFade(1, 0.4f);
            CG_topBar.DOFade(1, 0.4f).OnComplete(() =>
            {
                CG_bottomBar.interactable = true;
                CG_bottomBar.blocksRaycasts = true;

                CG_topBar.interactable = true;
                CG_topBar.blocksRaycasts = true;
            });
        }
        else // gada item2
        {

            CG_bottomBar.DOFade(0, 0.4f);
            CG_topBar.DOFade(0, 0.4f).OnComplete(() =>
            {
                CG_bottomBar.interactable = false;
                CG_bottomBar.blocksRaycasts = false;

                CG_topBar.interactable = false;
                CG_topBar.blocksRaycasts = false;
            });
        }
    }

    public void ToggleMuteMusic()
    {
        if (PlayerPrefs.GetInt("isMuteMusic", 0) == 0) // kalo mute
        {
            AudioManager.Instance.SetMute(AudioManager.Instance.aS_music);
            PlayerPrefs.SetInt("isMuteMusic", 1);
            MuteMusicUI(true);
        }
        else // kalo bersuara
        {
            AudioManager.Instance.SetMute(AudioManager.Instance.aS_music, false);
            PlayerPrefs.SetInt("isMuteMusic", 0);
            MuteMusicUI(false);
        }
    }

    public void MuteMusicUI(bool isMute)
    {
        if (isMute) rawImageAudio.texture = textureAudioMute;
        else rawImageAudio.texture = textureAudio;
    }

    public void SnapUI()
    {

    }

}
