using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �ٸ� ��ũ��Ʈ������ ���� �����ϱ� ���� GameManager �ν��Ͻ� ȭ
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
    public int availablePoint; // ���������� ���������� ���� ������ �ø� �� �ִ� ����Ʈ
    public ItemAttribute attribute; // Player�� ���� �Ӽ�
    public int gold; // ���

    [Header("# Stage Data")]
    public bool playTutorial; // Ʃ�丮�� �÷��� ���� ��
    public bool gameStop; // ������ ������ �� true
    public bool isStage; // ���������� ���� �� �϶� True
    public bool isRedMoon; // ���� �ð��ȿ� Enemy�� ���� ������ �� Enemy�� �������� ���� ���� ������\
    public bool redMoonEffect; // ����� ������ ��
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
        // �ڱ� �ڽ����� �ʱ�ȭ
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
    public void GameLobby() // �κ�� �� �� (�װų�, ���� â���� ���ų�)
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
    public void LobbyGo() // �κ�� ����
    {
        guide.PageInit(); // ���̵� ������ �ʱ�ȭ
        level = 0; // ���� 0
        mainMenu.LobbyUI();
        player.transform.position = Vector3.zero;
        background.SetActive(true);
        hud.SetActive(false);
        // ������ ������ ���̽� �ʱ�ȭ
        ItemDatabase.instance.ItemReset();
        map.MapReset();
        pool.PoolingReset(); // Ǯ�� ����
        magicManager.MagicActiveCancel();
        player.gameObject.SetActive(false);
        AudioManager.instance.PlayBgm((int)Bgm.Main);

        if (redMoonEffect)
        {
            redMoon.Lobby();
            redMoonEffect = false;
        }

    }
    public void PoolingReset() // ������Ʈ Ǯ�� ����
    {
        magicManager.PoolingReset();
        pool.PoolingReset();
    }
    private IEnumerator StageClear() // �������� Ŭ����
    {
        if(enforce.enforceInfo[(int)EnforceName.HealthRecoveryUp].curLevel > 0) // ü�� ȸ�� ��ȭ�� �ߴٸ�
        {
            statManager.PlayerHealthRecovery();
        }

        if(enforce.enforceInfo[(int)EnforceName.GoldUp].curLevel != 0) // ��� ȹƯ�� ��ȭ �ߴٸ�
        {
            enforce.ExtraGold();
        }

        background.SetActive(true);
        isStage = false;
        gameStop = true;
        if (isRedMoon) // ���� ���� ���ö��� ��
        {
            redMoon.RedMoonEnd();
            isRedMoon = false;
        }
        redMoonEffect = false;
        joy.StageClear();
        hud.SetActive(false);
        pool.PoolingReset();
        pause.gameObject.SetActive(false); // ���� ��ư ��Ȱ��ȭ
        AudioManager.instance.EndBgm(); // Bgm ����
        demeterOn = false;

        PlayerPrefs.SetInt("GemStone", PlayerPrefs.GetInt("GemStone") + 1 ); // �������� Ŭ���� �� ���Ŵ� ������ + 1
        yield return new WaitForSeconds(1f);

        magicManager.PoolingReset();

        if (level >= maxLevel) // ������ ��������
        {
            GameEnd(0);
        }
        else
        {

            AudioManager.instance.PlayBgm((int)Bgm.Victory - 1); // �¸� Bgm ���
            yield return new WaitForSeconds(2f);
            availablePoint++;
            level++;
            nextStageBtn.LevelText(level); // ���� �������� ��ư �ؽ�Ʈ ����
            enemyMaxNum += 50;
            EnemyManager.instance.EnemyLevelUp();
            clearReward.SetActive(true); // Ŭ���� ���� On
            player.gameObject.SetActive(false); // �÷��̾� ��Ȱ��ȭ
            shop.ShopReset(); // ���� �ʱ�ȭ
            if (statManager.essenceOn) // �̹� ���������� ������ Ȱ��ȭ �ƴٸ�
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

        if (!redMoonEffect && curGameTime <= redMoon.lerpTime) // ȭ���� �������� ��
        {
            redMoon.RedMoonStart();
            redMoonEffect = true;
        }
        else if (curGameTime <= 0) // ���� �� On
        {
            curGameTime = 0;
            isRedMoon = true;
        }

    }


    // �������� (0. ���� Ŭ����   1. ���ӿ���)
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

    // ���� ��ŸƮ
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

    // ���� ������
    public void GameQuit()
    {
        AudioManager.instance.SelectSfx();
        Application.Quit();
    }
}
