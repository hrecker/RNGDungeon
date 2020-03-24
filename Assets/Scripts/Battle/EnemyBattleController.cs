using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleController : MonoBehaviour
{
    private EnemyBattleStatus status;
    private EnemyRollGenerator rollGenerator;
    private RollResultGenerator rollResultGenerator;
    private Image enemySprite;

    //TODO load from resources eventually
    public Sprite batSprite;
    public Sprite slimeSprite;

    // Start is called before the first frame update
    void Awake()
    {
        status = GetComponent<EnemyBattleStatus>();
        rollGenerator = GetComponent<EnemyRollGenerator>();
        rollResultGenerator = GetComponent<RollResultGenerator>();
        enemySprite = GetComponent<Image>();

        if (CurrentLevel.currentEnemyName == "Bat")
        {
            status.maxHealth = 5;
            status.currentHealth = status.maxHealth;
            rollGenerator.minRoll = 1;
            rollGenerator.maxRoll = 3;
            enemySprite.sprite = batSprite;
        }
        else if (CurrentLevel.currentEnemyName == "Slime")
        {
            status.maxHealth = 4;
            status.currentHealth = status.maxHealth;
            rollGenerator.minRoll = 2;
            rollGenerator.maxRoll = 2;
            enemySprite.sprite = slimeSprite;
        }
    }
}
