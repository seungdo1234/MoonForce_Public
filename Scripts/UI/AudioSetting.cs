using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public bool isPlayingBgm;
    public bool isPlayingSfx;


    [Header("BgmButton")]
    public Image bgmBtnImage;
    public Sprite bgmOnSprite;
    public Sprite bgmOffSprite;
    [Header("SfxButton")]
    public Image sfxBtnImage;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;


    private AudioManager audioManager;
    private AudioSource bgmPlayer;
    private AudioSource[] sfxPlayers;
    private void Start()
    {
        audioManager = AudioManager.instance;

        AudioSource[] audios = AudioManager.instance.GetComponentsInChildren<AudioSource>();

        bgmPlayer = audios[0]; 

        sfxPlayers = new AudioSource[audioManager.channels];

        for (int i = 1; i < audios.Length; i++)
        {
            sfxPlayers[i - 1] = audios[i];
        }
    }

    public void BgmSetting() // 배경음 On/Off
    {
        audioManager.SelectSfx();
        if (isPlayingBgm)
        {
            bgmPlayer.volume = 0;
            isPlayingBgm = false;
            bgmBtnImage.sprite = bgmOffSprite;
        }
        else
        {
            bgmPlayer.volume = audioManager.bgmVolume;
            isPlayingBgm = true;
            bgmBtnImage.sprite = bgmOnSprite;
        }
    }
    public void SfxSetting() // 효과음 On/Off
    {
        audioManager.SelectSfx();
        if (isPlayingSfx)
        {
            for(int i = 0; i< sfxPlayers.Length; i++)
            {
                sfxPlayers[i].volume = 0;
            }
            isPlayingSfx = false;
            sfxBtnImage.sprite = sfxOffSprite;
        }
        else
        {
            for (int i = 0; i < sfxPlayers.Length; i++)
            {
               sfxPlayers[i].volume = audioManager.sfxVolume;
            }
            isPlayingSfx = true;
            sfxBtnImage.sprite = sfxOnSprite;
        }
    }
 
}
