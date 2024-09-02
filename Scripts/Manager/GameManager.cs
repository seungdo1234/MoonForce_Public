using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 다른 스크립트에서도 쉽게 참조하기 위해 GameManager 인스턴스 화
    public static GameManager instance;
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public DamageTextPool damageTextPool;
    public MagicManager magicManager;
    public StatManager statManager;
    public CameraShake cameraShake;
    public Pause pause;
    public Inventory inventory;
    public RewardManager rewardManager;
    public Shop shop;
    public ShopManager shopManager;
    public NextStageBtn nextStageBtn;
    public Enchant enchant;
    public SkillCoolTimeUI coolTime;
    public Guide guide;
    public Enforce enforce;
    public MainMenu mainMenu;
    public Spawner spawner;

    [Header("# Player Data")]
    public int availablePoint; // 스테이지가 끝날때마다 스탯 레벨을 올릴 수 있는 포인트
    public ItemAttribute attribute; // Player의 무기 속성
    public int gold; // 골드

    [Header("# Stage Data")]
    public bool playTutorial; // 튜토리얼 플레이 중일 때
    public bool gameStop; // 게임이 멈췄을 때 true
    public bool isStage; // 스테이지를 진행 중 일때 True
    public bool isRedMoon; // 제한 시간안에 Enemy를 잡지 못했을 시 Enemy가 강해지는 붉은 달이 떠오름\
    public bool redMoonEffect; // 배경이 빨개질 때
    public float maxGameTime;
    public float curGameTime;
    public int kill;
    public int baseEnemyNum;
    public int enemyMaxNum;
    public int enemyCurNum = 9999;
    public int level;
    public int maxLevel;
    public int enemySpawnNumIncrese;
    public bool isResurrection;
    public bool demeterOn;



    [Header("# UI")]
    public GameObject clearReward;
    public GameObject hud;
    public RedMoonEffect redMoon;
    public GameObject gameOverObject;
    public GameObject gameClearObject;
    public GameObject background;
    public FloatingJoystick joy;
    public LoadingImage loadingImage;
    public float lobbyDelayTime;

    [Header("MainMenu")]
    public Map map;


    private void Awake()
    {
        // 자기 자신으로 초기화
        instance = this;
        Application.targetFrameRate = 60;
        Screen.SetResolution(720, 1280, true);
        enforce.EnforceLoad();
    }

    private void Start()
    {
        spawner = player.GetComponentInChildren<Spawner>();

    }
    private bool isClear()
    {
        if (kill == enemyMaxNum)
        {
            return true;
        }
        return false;
    }
    public void GameLobby() // 로비로 갈 때 (죽거나, 설정 창에서 가거나)
    {
        gameStop = true;
        loadingImage.Loading(0, 1);
        AudioManager.instance.EndBgm();
        StartCoroutine(LoadingTime());
    }
    private IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(loadingImage.lerpTime);

        LobbyGo();
        yield return new WaitForSeconds(lobbyDelayTime);
        loadingImage.Loading(1, 0);

        yield return new WaitForSeconds(loadingImage.lerpTime);
    }
    public void LobbyGo() // 로비로 가기
    {
        guide.PageInit(); // 가이드 페이지 초기화
        level = 0; // 레벨 0
        mainMenu.LobbyUI();
        player.transform.position = Vector3.zero;
        background.SetActive(true);
        hud.SetActive(false);
        // 아이템 데이터 베이스 초기화
        ItemDatabase.instance.ItemReset();
        map.MapReset();
        pool.PoolingReset(); // 풀링 리셋
        magicManager.MagicActiveCancel();
        player.gameObject.SetActive(false);
        AudioManager.instance.PlayBgm((int)Bgm.Main);

        if (redMoonEffect)
        {
            redMoon.Lobby();
            redMoonEffect = false;
        }

    }
    public void PoolingReset() // 오브젝트 풀링 리셋
    {
        magicManager.PoolingReset();
        pool.PoolingReset();
    }
    private IEnumerator StageClear() // 스테이지 클리어
    {
        if(enforce.enforceInfo[(int)EnforceName.HealthRecoveryUp].curLevel > 0) // 체력 회복 강화를 했다면
        {
            statManager.PlayerHealthRecovery();
        }

        if(enforce.enforceInfo[(int)EnforceName.GoldUp].curLevel != 0) // 골드 획특을 강화 했다면
        {
            enforce.ExtraGold();
        }

        background.SetActive(true);
        isStage = false;
        gameStop = true;
        if (isRedMoon) // 붉은 달이 떠올랐을 때
        {
            redMoon.RedMoonEnd();
            isRedMoon = false;
        }
        redMoonEffect = false;
        joy.StageClear();
        hud.SetActive(false);
        pool.PoolingReset();
        pause.gameObject.SetActive(false); // 퍼즈 버튼 비활성화
        AudioManager.instance.EndBgm(); // Bgm 끄기
        demeterOn = false;

        PlayerPrefs.SetInt("GemStone", PlayerPrefs.GetInt("GemStone") + 1 ); // 스테이지 클리어 시 떄매다 젬스톤 + 1
        yield return new WaitForSeconds(1f);

        magicManager.PoolingReset();

        if (level >= maxLevel) // 마지막 스테이지
        {
            GameEnd(0);
        }
        else
        {

            AudioManager.instance.PlayBgm((int)Bgm.Victory - 1); // 승리 Bgm 재생
            yield return new WaitForSeconds(2f);
            availablePoint++;
            level++;
            nextStageBtn.LevelText(level); // 다음 스테이지 버튼 텍스트 변경
            enemyMaxNum += 50;
            EnemyManager.instance.EnemyLevelUp();
            clearReward.SetActive(true); // 클리어 보상 On
            player.gameObject.SetActive(false); // 플레이어 비활성화
            shop.ShopReset(); // 상점 초기화
            if (statManager.essenceOn) // 이번 스테이지에 정수가 활성화 됐다면
            {
                statManager.EssenceOff();
            }
        }

        //    Time.timeScale = 0f;
    }
    public void NextStage()
    {
        hud.SetActive(true);
        isStage = true;
        player.gameObject.SetActive(true);
        gameStop = false;
        curGameTime = maxGameTime;
        enemyCurNum = 0;
        kill = 0;
        clearReward.SetActive(false);
        magicManager.StageStart();
        spawner.StageStart();
        AudioManager.instance.PlayBgm((int)Bgm.Stage);

    }
    private void Update()
    {
        if (gameStop || playTutorial)
        {
            return;
        }

        if (isClear())
        {
            StartCoroutine(StageClear());
            return;
        }

        if (isRedMoon)
        {
            return;
        }

        curGameTime -= Time.deltaTime;

        if (!redMoonEffect && curGameTime <= redMoon.lerpTime) // 화면이 빨개지는 중
        {
            redMoon.RedMoonStart();
            redMoonEffect = true;
        }
        else if (curGameTime <= 0) // 붉은 달 On
        {
            curGameTime = 0;
            isRedMoon = true;
        }

    }


    // 게임종료 (0. 게임 클리어   1. 게임오버)
    public void GameEnd(int endType)
    {
        if (endType == 0)
        {
            AudioManager.instance.PlayBgm((int)Bgm.GameClear - 1);
            gameClearObject.SetActive(true);
        }
        else
        {
            gameStop = true;
            joy.StageClear();
            hud.SetActive(false);
            pause.gameObject.SetActive(false);
            AudioManager.instance.PlayBgm((int)Bgm.GameOver);
            gameOverObject.SetActive(true);
            magicManager.PoolingReset();
        }

        level = 0;
        spawner.spawnPerLevelUp = 0;

    }

    // 게임 스타트
    public void GameStart()
    {
        availablePoint = 0;
        isResurrection = false;
        level = 0;
        gold = 0;
        enemyMaxNum = baseEnemyNum;
        demeterOn = false;
        AudioManager.instance.SelectSfx();
        EnemyManager.instance.EnemyReset();
        NextStage();
    }

    // 게임 나가기
    public void GameQuit()
    {
        AudioManager.instance.SelectSfx();
        Application.Quit();
    }
}
