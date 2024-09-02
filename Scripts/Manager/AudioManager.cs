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
    public AudioClip[] footStepClips; // ���ڱ� Ŭ��
    public float sfxVolume;
    public int channels; // ���� ȿ������ ���� ���� ä�� �ý���
    public AudioSource[] sfxPlayers; 
    private int channelIndex; // ä�� ���� ��ŭ ��ȸ�ϵ��� �� �������� �÷��� �ߴ� SFX�� �ε�����ȣ�� �����ϴ� ����


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
        // ����� �÷��̾� �ʱ�ȭ
        // ������Ʈ ����
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
      //  bgmPlayer.playOnAwake = t; // �÷��� ���ڸ��� ���� false
        bgmPlayer.loop = true; // �ݺ� true
        bgmPlayer.volume = bgmVolume; // ����
        bgmEffecter = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].bypassListenerEffects = true; // �����н��� �� �ɸ��� ��
        }

        sfxPlayers[1].clip = sfxClips[(int)Sfx.Select];

    }
    public void PlayBgm(int bgmNumber) // BGM �÷��� �Լ�
    {
        if (!bgmPlayer.loop) // BgmPlayer�� �ݺ��� �����ִٸ� false
        {
            bgmPlayer.loop = true;
        }
        bgmPlayer.clip = bgmClip[bgmNumber];
        bgmPlayer.Play(); // Bgm �÷���

        // �ش� ���� Bgm�� ����� �ҽ��� �ΰ� �̹Ƿ� ���� ������ �ξ� ó����
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

        // Bgm�� �ʹݺΰ� �����ٸ� 
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
        for (int i = 1; i < sfxPlayers.Length; i++) // 0���� ���ڱ� �Ҹ� ä���̱� ������ 1 ~ 15�� ä�θ� ��ȸ
        {
            // ������� 5�� �ε����� ���������� ��������� 6 7 8 9 10 1 2 3 4 5 �̷������� ��ȸ�ϰ� �ϱ����� �����
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) // �ش� ä���� Play ���̶��
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
            break; // ȿ������ �� ä�ο��� ��� �Ʊ� ������ �ݵ�� break�� �ݺ����� ������������
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