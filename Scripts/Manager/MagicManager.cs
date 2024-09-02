using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MagicName {Explosion , Inferno , WaterBlast , RockMeteor, Leaf, Jack, Instant, FireBall, EarthSpike, HellSpear, Shovel, MagicBall, Hoe, RockThrow, WindCutter, Tornado, FireExplosion, ElectricShock, ChargeExplosion}
public class MagicManager : MonoBehaviour
{
    public SkillCoolTimeUI coolTimeUI;
    public Magic[] magicInfo;


    // Ǯ ����� �ϴ� ����Ʈ��
    private List<GameObject>[] pools;
    private Player player;

    private void Awake()
    {
       // EnemyPrefabs�� ���� ��ŭ ����Ʈ ũ�� �ʱ�ȭ
        // Pool�� ��� �迭 �ʱ�ȭ
        pools = new List<GameObject>[magicInfo.Length];

        // �迭 �ȿ� ����ִ� ������ ����Ʈ�鵵 �ʱ�ȭ
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    private void Start()
    {
        player = GameManager.instance.player;

      //  StageStart();
    }

    public void StageStart()
    {
        coolTimeUI.StageStart();

        int slotNum = 0;

        for (int i = 0; i < magicInfo.Length; i++)
        {
            if (magicInfo[i].isMagicActive)
            {

                GameManager.instance.magicManager.coolTimeUI.CoolTimeStart(slotNum);
                
                if (magicInfo[i].magicCoolTime == 0 || i == (int)MagicName.ChargeExplosion || i <= (int)MagicName.Instant)
                {
                    StartCoroutine( AlwaysPlayMagic(i, slotNum));
                }
                else
                {
                    StartCoroutine(StartCoolTimeMagic(i , slotNum));
                }
                slotNum++;
            }
        }
    }

    public void MagicActiveCancel()
    {
        for (int i = 0; i < magicInfo.Length; i++)
        {
            magicInfo[i].isMagicActive = false;
        }
    }
    public void PoolingReset()
    {
        StopAllCoroutines();


        // ���������� Ŭ�����߱⶧���� Ȱ��ȭ�� �������� ��Ȱ��ȭ ��Ŵ 
        for(int i = 0; i<magicInfo.Length; i++)
        {
            foreach (GameObject item in pools[i])
            {
                if (item.activeSelf)
                {
                    if(i > 6) // 6���� �������� �����帮 �����̱⶧���� ������ �� X
                    {
                        MagicNumber magic = item.GetComponent<MagicNumber>();
                        if (magic.isSizeUp)
                        {
                            magic.GetComponent<Transform>().localScale = magic.resetScale;
                            magic.isSizeUp = false;
                        }
                    }
                    item.SetActive(false);
                }
            }
        }

    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ���� -> �߰��ϸ� select ������ �Ҵ�
        // �̹� ������ Enemy�� �׾��� �� Destroy���� �ʰ� ��Ȱ��
        foreach (GameObject item in pools[index])
        {
            // ���빰 ������Ʈ�� ��Ȱ��ȭ(��� ����)���� Ȯ��
            if (!item.activeSelf)
            {
                // ��� �ִ� ���ӿ�����Ʈ select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ���� ��ã�Ҵٸ� -> ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(magicInfo[index].magicEffect, transform);

            pools[index].Add(select);
        }

        return select;
    }

    private IEnumerator StartCoolTimeMagic(int magicNumber, int slotNum)
    {

        float timer = 0;
        float coolTime = magicInfo[magicNumber].magicCoolTime - (magicInfo[magicNumber].magicCoolTime * GameManager.instance.enforce.enforceInfo[(int)EnforceName.MagicCoolTimeDown].curLevel * GameManager.instance.enforce.enforceInfo[(int)EnforceName.MagicCoolTimeDown].statIncrease);

        while (!GameManager.instance.gameStop)
        {
            if (timer <= coolTime)
            {
                timer += Time.deltaTime;
            }

            if (timer > coolTime) // ��Ÿ���� á�� ��
            {
                if (!player.scanner.nearestTarget[0] && !magicInfo[magicNumber].isNonTarget) // �ֺ��� ENemy�� ���� Ÿ���� �ʿ��� ��ų�̶�� continue
                {

                    yield return null; // ��� ���·� ��ȯ
                    continue;
                }
                timer = 0; // �ִٸ� ��Ÿ�� �ʱ�ȭ

                if (magicInfo[magicNumber].isFlying) 
                {
                    Fire(magicNumber); // ���̾ �߻� 
                }
                else 
                {
                    SpawnMagic(magicNumber);
                }

                coolTimeUI.CoolTimeStart(slotNum);
            }
            yield return null; // �ݺ� 
        }
    }

    

    private void Fire(int magicNumber)
    {


        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // Magic ����
            Transform magic = Get(magicNumber).transform;


            magic.position = player.transform.position; // Magict�� ��ġ


            if (magicNumber == (int)MagicName.WindCutter) // ���� ����
            {
                //  �ʱ�ȭ
                magic.localRotation = Quaternion.identity;


                // Bullet�� ���� ���ϱ�
                Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;


                magic.Rotate(rotVec + new Vector3(0, 0, 90));

                magic.GetComponent<WindCutter>().Init();
                continue;
            }


            MagicSizeUp(magic, magicNumber);

            Vector3 targetPos = player.scanner.nearestTarget[0].position;
            Vector3 dir = targetPos - player.transform.position;
            dir = dir.normalized; // ����ȭ

                                                // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
            magic.rotation = Quaternion.FromToRotation(Vector3.right, dir); // Enemy �������� bullet ȸ��

            if (magicNumber == (int)MagicName.FireBall) // ���̾
            {
                magic.GetComponent<Bullet>().Init(0, magicInfo[magicNumber].penetration, dir, 11.5f);
            }
            else if (magicNumber == (int)MagicName.Hoe) // ����
            {
                magic.GetComponent<Hoe>().Init(dir);
            }
            else if (magicNumber == (int)MagicName.RockThrow) // �� ������
            {
                magic.GetComponent<RockThrow>().Init(dir);
            }
        }

    }

    private void SpawnMagic(int magicNumber)
    {

        int length = 0;

        for (int i = 0; i < player.scanner.nearestTarget.Length; i++) // Ÿ�� ���ϱ�
        {
            if (player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            length++;
        }

        int spawnCount = Mathf.Min(length, magicInfo[magicNumber].magicCount); // ���� ��� �����Ұ��� ����.

        List<int> availableIndices = new List<int>(); // �ߺ��� Enemy �����ϱ� ���� ����Ʈ ����

        for (int i = 0; i < length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex); // �̹� ���õ� Enemy�� ����Ʈ���� ����

            Transform magic = Get(magicNumber).transform;
            magic.position = player.scanner.nearestTarget[selectedIndex].transform.position;
            MagicSizeUp(magic, magicNumber);
            if (magicNumber == (int)MagicName.HellSpear) // ������ â
            {
                Enemy enemy = player.scanner.nearestTarget[selectedIndex].GetComponent<Enemy>();
                magic.GetComponent<MoltenSpear>().Init(enemy);
            }
        }
    }

    private void MagicSizeUp(Transform magic , int magicNumber) // ���� ũ�� Up
    {
        if (!magic.GetComponent<MagicNumber>().isSizeUp && !magicInfo[magicNumber].magicCountIncrease && magicInfo[magicNumber].magicSizeStep != 0)
        {
            int magicSizeStep = magicInfo[magicNumber].magicSizeStep;
            magic.localScale = new Vector3(magic.localScale.x + (magicSizeStep * 0.25f), magic.localScale.y + (magicSizeStep * 0.25f), magic.localScale.z + (magicSizeStep * 0.25f));
            magic.GetComponent<MagicNumber>().isSizeUp = true;
        }
    }
    private IEnumerator AlwaysPlayMagic(int magicNumber, int slotNum)
    {
        yield return null;

        // ��Ÿ�� ���ϱ�
        float coolTime = Mathf.Max(0, magicInfo[magicNumber].magicCoolTime - (magicInfo[magicNumber].magicCoolTime * GameManager.instance.enforce.enforceInfo[(int)EnforceName.MagicCoolTimeDown].curLevel * GameManager.instance.enforce.enforceInfo[(int)EnforceName.MagicCoolTimeDown].statIncrease));
        switch (magicNumber)
        {
            case (int)MagicName.Inferno:
                InfernoSpawn(magicNumber , slotNum , coolTime);
                break;
            case (int)MagicName.WaterBlast:
                PoseidonSpawn(magicNumber , slotNum, coolTime);
                break;
            case (int)MagicName.RockMeteor:
                GaiaSpawn(magicNumber , slotNum, coolTime);
                break;
            case (int)MagicName.Leaf:
                LeafSpawn(magicNumber, slotNum, coolTime);
                break;
            case (int)MagicName.Jack:
                JackSpawn(magicNumber , slotNum, coolTime);
                break;
            case (int)MagicName.Shovel:
                ShovelSpawn(magicNumber);
                break;
            case (int)MagicName.MagicBall:
                MagicBallSpawn(magicNumber);
                break;
            case (int)MagicName.ChargeExplosion:
                ChargeExplosionSpawn(magicNumber , slotNum, coolTime);
                break;
        }


    }
    private void LeafSpawn(int magicNumber, int slotNum , float coolTime)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<Leaf>().Init(coolTime, slotNum);
    }
    private void JackSpawn(int magicNumber, int slotNum, float coolTime)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<Jack>().Init(coolTime, slotNum);
    }
    private void GaiaSpawn(int magicNumber, int slotNum, float coolTime)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<RockMeteor>().Init(coolTime, slotNum);
    }
    private void PoseidonSpawn(int magicNumber, int slotNum, float coolTime)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<Poseidon>().Init(coolTime, slotNum);
    }
    private void InfernoSpawn(int magicNumber, int slotNum, float coolTime)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<Inferno>().Init(coolTime, slotNum);
    }
    private void ChargeExplosionSpawn(int magicNumber, int slotNum, float coolTime)
    {
        GameObject magic = Get(magicNumber);

        magic.GetComponent<ChargeExplosion>().Init(coolTime, magicInfo[magicNumber].magicSizeStep , slotNum);
    }
    private void ShovelSpawn(int magicNumber)
    {
        // ���� �ӵ� �ܰ� ��ŭ ���� ȸ�� �ӵ� ++
        int rotationSpeed = 90 + (magicInfo[magicNumber].magicRateStep * 20);
        GameManager.instance.player.rotationBody.GetComponent<RotationWeapon>().rotationSpeed = rotationSpeed;
        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet�� �θ� MagicManager����  Player�� RotationBody�� �ٲٱ� ���� Transform���� ����
            Transform bullet;

            bullet = Get(magicNumber).transform;
            bullet.parent = GameManager.instance.player.rotationBody;


            //  �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;


            // Bullet�� ���� ���ϱ�
            Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;
   
            bullet.Rotate(rotVec);


            // �÷��̾�� ���� ���� �Ÿ��� ����߸�
            bullet.Translate(bullet.up * 1f, Space.World);

        }
    }
    private void MagicBallSpawn(int magicNumber)
    {
        float lerpTime = 1.5f - (0.25f * magicInfo[magicNumber].magicRateStep);
        magicInfo[magicNumber].magicEffect.GetComponent<MagicBall>().lerpTime = lerpTime;

        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet�� �θ� MagicManager����  Player�� RotationBody�� �ٲٱ� ���� Transform���� ����

            Transform bullet = Get(magicNumber).transform;

            bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        }
    }
}
