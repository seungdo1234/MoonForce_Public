using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Bgm { Main, Stage, MaintenanceRoom , Victory = 4 , GameOver , GameClear = 7 }
public enum Sfx { Dead, Hurt,  EnemyHit , ChestOpen = 4, Select , EnchantSuccess , EnchantFail , BuySell, BuySellFail, StatUp, Heal, Text }

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClip;
    public float bgmVolume;
    public AudioSource bgmPlayer;
    private AudioHighPassFilter bgmEffecter;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public AudioClip[] footStepClips; // 발자국 클립
    public float sfxVolume;
    public int channels; // 많은 효과음을 내기 위한 채널 시스템
    public AudioSource[] sfxPlayers; 
    private int channelIndex; // 채널 갯수 만큼 순회하도록 맨 마지막에 플레이 했던 SFX의 인덱스번호를 저장하는 변수


    private void Awake()
    {
        instance = this;
        Init();
    }
    private void Start()
    {
        PlayBgm(0);
    }
    private void Init()
    {
        // 배경음 플레이어 초기화
        // 오브젝트 생성
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
      //  bgmPlayer.playOnAwake = t; // 플레이 하자마자 시작 false
        bgmPlayer.loop = true; // 반복 true
        bgmPlayer.volume = bgmVolume; // 볼륨
        bgmEffecter = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].bypassListenerEffects = true; // 하이패스에 안 걸리게 함
        }

        sfxPlayers[1].clip = sfxClips[(int)Sfx.Select];

    }
    public void PlayBgm(int bgmNumber) // BGM 플레이 함수
    {
        if (!bgmPlayer.loop) // BgmPlayer의 반복이 켜져있다면 false
        {
            bgmPlayer.loop = true;
        }
        bgmPlayer.clip = bgmClip[bgmNumber];
        bgmPlayer.Play(); // Bgm 플레이

        // 해당 조건 Bgm은 오디오 소스가 두개 이므로 따로 조건을 두어 처리함
        if(bgmNumber == (int)Bgm.Victory - 1 || bgmNumber == (int)Bgm.GameClear - 1) 
        {
            StartCoroutine(Init_Loop_Bgm(bgmNumber));
        }
        else if(bgmNumber == (int)Bgm.GameOver)
        {
            bgmPlayer.loop = false;
        }
    }
    private IEnumerator Init_Loop_Bgm(int bgmNumber) 
    {
        bgmPlayer.loop = false;

        // Bgm의 초반부가 끝난다면 
       yield return new WaitForSeconds(bgmPlayer.clip.length); 

        // Loop
        bgmPlayer.clip = bgmClip[bgmNumber + 1];
        bgmPlayer.loop = true;
        bgmPlayer.Play();

    }
    public void EndBgm()
    {
        bgmPlayer.Stop();
    }
    public void EffectBgm(bool isPlay)

    {
        bgmEffecter.enabled = isPlay;
    }
    public void SelectSfx()
    {
        PlayerSfx(Sfx.Select);
    }
    public void PlayerSfx(Sfx sfx)
    {
        for (int i = 1; i < sfxPlayers.Length; i++) // 0번은 발자국 소리 채널이기 떄문에 1 ~ 15의 채널만 순회
        {
            // 예를들어 5번 인덱스를 마지막으로 사용했으면 6 7 8 9 10 1 2 3 4 5 이런식으로 순회하게 하기위한 계산임
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) // 해당 채널이 Play 중이라면
            {
                continue;
            }

            int randomIndex = 0;
            if (sfx == Sfx.EnemyHit)
            {
                randomIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randomIndex];
            sfxPlayers[loopIndex].Play();
            break; // 효과음이 빈 채널에서 재생 됐기 때문에 반드시 break로 반복문을 빠져나가야함
        }

    }
    public void FootStepSfxPlayer()
    {
        if (sfxPlayers[0].isPlaying)
        {
            return;
        }

        int randomIndex = Random.Range(0, 3);
        sfxPlayers[0].clip = footStepClips[randomIndex];
        sfxPlayers[0].Play();
    }
   
}