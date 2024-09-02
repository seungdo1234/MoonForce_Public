using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MagicName {Explosion , Inferno , WaterBlast , RockMeteor, Leaf, Jack, Instant, FireBall, EarthSpike, HellSpear, Shovel, MagicBall, Hoe, RockThrow, WindCutter, Tornado, FireExplosion, ElectricShock, ChargeExplosion}
public class MagicManager : MonoBehaviour
{
    public SkillCoolTimeUI coolTimeUI;
    public Magic[] magicInfo;


    // 풀 담당을 하는 리스트들
    private List<GameObject>[] pools;
    private Player player;

    private void Awake()
    {
       // EnemyPrefabs의 길이 만큼 리스트 크기 초기화
        // Pool를 담는 배열 초기화
        pools = new List<GameObject>[magicInfo.Length];

        // 배열 안에 들어있는 각각의 리스트들도 초기화
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


        // 스테이지를 클리어했기때문에 활성화된 마법들을 비활성화 시킴 
        for(int i = 0; i<magicInfo.Length; i++)
        {
            foreach (GameObject item in pools[i])
            {
                if (item.activeSelf)
                {
                    if(i > 6) // 6이하 마법들은 레전드리 마법이기때문에 사이즈 업 X
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

        // 선택한 풀의 놀고(비활성화 된) 있는 게임오브젝트 접근 -> 발견하면 select 변수에 할당
        // 이미 생성한 Enemy가 죽었을 때 Destroy하지 않고 재활용
        foreach (GameObject item in pools[index])
        {
            // 내용물 오브젝트가 비활성화(대기 상태)인지 확인
            if (!item.activeSelf)
            {
                // 놀고 있는 게임오브젝트 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 만약 못찾았다면 -> 새롭게 생성하고 select 변수에 할당
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

            if (timer > coolTime) // 쿨타임이 찼을 때
            {
                if (!player.scanner.nearestTarget[0] && !magicInfo[magicNumber].isNonTarget) // 주변이 ENemy가 없고 타겟이 필요한 스킬이라면 continue
                {

                    yield return null; // 대기 상태로 전환
                    continue;
                }
                timer = 0; // 있다면 쿨타임 초기화

                if (magicInfo[magicNumber].isFlying) 
                {
                    Fire(magicNumber); // 파이어볼 발사 
                }
                else 
                {
                    SpawnMagic(magicNumber);
                }

                coolTimeUI.CoolTimeStart(slotNum);
            }
            yield return null; // 반복 
        }
    }

    

    private void Fire(int magicNumber)
    {


        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // Magic 생성
            Transform magic = Get(magicNumber).transform;


            magic.position = player.transform.position; // Magict의 위치


            if (magicNumber == (int)MagicName.WindCutter) // 윈드 컷터
            {
                //  초기화
                magic.localRotation = Quaternion.identity;


                // Bullet의 각도 구하기
                Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;


                magic.Rotate(rotVec + new Vector3(0, 0, 90));

                magic.GetComponent<WindCutter>().Init();
                continue;
            }


            MagicSizeUp(magic, magicNumber);

            Vector3 targetPos = player.scanner.nearestTarget[0].position;
            Vector3 dir = targetPos - player.transform.position;
            dir = dir.normalized; // 정규화

                                                // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
            magic.rotation = Quaternion.FromToRotation(Vector3.right, dir); // Enemy 방향으로 bullet 회전

            if (magicNumber == (int)MagicName.FireBall) // 파이어볼
            {
                magic.GetComponent<Bullet>().Init(0, magicInfo[magicNumber].penetration, dir, 11.5f);
            }
            else if (magicNumber == (int)MagicName.Hoe) // 괭이
            {
                magic.GetComponent<Hoe>().Init(dir);
            }
            else if (magicNumber == (int)MagicName.RockThrow) // 돌 던지기
            {
                magic.GetComponent<RockThrow>().Init(dir);
            }
        }

    }

    private void SpawnMagic(int magicNumber)
    {

        int length = 0;

        for (int i = 0; i < player.scanner.nearestTarget.Length; i++) // 타겟 정하기
        {
            if (player.scanner.nearestTarget[i] == null)
            {
                break;
            }
            length++;
        }

        int spawnCount = Mathf.Min(length, magicInfo[magicNumber].magicCount); // 마법 몇개를 스폰할건지 정함.

        List<int> availableIndices = new List<int>(); // 중복된 Enemy 제거하귀 위한 리스트 선언

        for (int i = 0; i < length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex); // 이미 선택된 Enemy를 리스트에서 제거

            Transform magic = Get(magicNumber).transform;
            magic.position = player.scanner.nearestTarget[selectedIndex].transform.position;
            MagicSizeUp(magic, magicNumber);
            if (magicNumber == (int)MagicName.HellSpear) // 지옥의 창
            {
                Enemy enemy = player.scanner.nearestTarget[selectedIndex].GetComponent<Enemy>();
                magic.GetComponent<MoltenSpear>().Init(enemy);
            }
        }
    }

    private void MagicSizeUp(Transform magic , int magicNumber) // 마법 크기 Up
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

        // 쿨타임 구하기
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
        // 공격 속도 단계 만큼 삽의 회전 속도 ++
        int rotationSpeed = 90 + (magicInfo[magicNumber].magicRateStep * 20);
        GameManager.instance.player.rotationBody.GetComponent<RotationWeapon>().rotationSpeed = rotationSpeed;
        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet의 부모를 MagicManager에서  Player의 RotationBody로 바꾸기 위해 Transform으로 저장
            Transform bullet;

            bullet = Get(magicNumber).transform;
            bullet.parent = GameManager.instance.player.rotationBody;


            //  초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;


            // Bullet의 각도 구하기
            Vector3 rotVec = Vector3.forward * 360 * i / magicInfo[magicNumber].magicCount;
   
            bullet.Rotate(rotVec);


            // 플레이어로 부터 일정 거리를 떨어뜨림
            bullet.Translate(bullet.up * 1f, Space.World);

        }
    }
    private void MagicBallSpawn(int magicNumber)
    {
        float lerpTime = 1.5f - (0.25f * magicInfo[magicNumber].magicRateStep);
        magicInfo[magicNumber].magicEffect.GetComponent<MagicBall>().lerpTime = lerpTime;

        for (int i = 0; i < magicInfo[magicNumber].magicCount; i++)
        {
            // bullet의 부모를 MagicManager에서  Player의 RotationBody로 바꾸기 위해 Transform으로 저장

            Transform bullet = Get(magicNumber).transform;

            bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        }
    }
}
