using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    public Text hpText;
    public EnemyHealthManager enemyHp;
    public Image hpSprite;

    void Start()
    {
        enemyHp = GetComponent<EnemyHealthManager>();
    }

    void Update()
    {
        hpText.text = $"HP: {enemyHp.enemyCurrentHealth} / {enemyHp.enemyMaxHealth}";

        if(enemyHp.enemyCurrentHealth <= 0)
        {
            hpText.text = "";
        }

        var getFullHP = enemyHp.GetFullHp();

        float getValue = (float)enemyHp.enemyCurrentHealth / (float)getFullHP;


        if (getValue >= 1)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/HpbarEnemy");
        }

        else if (getValue >= 0.90f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/10%");
        }

        else if (getValue >= 0.85f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/17%");
        }

        else if (getValue >= 0.8f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/20%");
        }

        else if (getValue >= 0.75f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/25%");
        }

        else if (getValue >= 0.5f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/50%");
        }

        else if (getValue >= 0.4f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/60%");
        }

        else if (getValue >= 0.25f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/75%");
        }

        else if (getValue >= 0.15f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/85%");
        }

        else if (getValue >= 0.1f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/90%");
        }

        else if (getValue >= 0.05f)
        {
            hpSprite.sprite = Resources.Load<Sprite>("HpBarEnemy/95%");
        }
    }
}