using UnityEngine;

public class MoltenSpear : MonoBehaviour
{

    public Vector3 vectorPos;

    private MagicNumber magicNumber;
    public Enemy target;
    private void Awake()
    {
        magicNumber = GetComponent<MagicNumber>();
    }
    public void Init(Enemy target)
    {
        this.target = target;
    }

    public void EnemyAttack()
    {
        float damage = GameManager.instance.statManager.attack * GameManager.instance.magicManager.magicInfo[magicNumber.magicNumber].damagePer;
        target.EnemyDamaged(damage, 2);
    }

    private void Update()
    {
        transform.position = target.transform.position + vectorPos;

    }
}
