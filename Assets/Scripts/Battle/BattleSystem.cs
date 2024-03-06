using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    public Transform playerSpawn;
    public List<Transform> enemySpawns = new List<Transform>();
    private List<GameObject> enemiesInBattle = new List<GameObject>();
    private GameObject playerOverworld;
    private GameObject playerBattle;
    private GameObject enemyOverworld;
    private GameObject battleCamera;
    private GameObject playerCamera;
    private BattleHUDManager battleHUD;
    private Vector3 enemyOffset = new Vector3(5, 0, 5);
    private int attackingEnemy = 0;

    private void Start()
    {
        playerOverworld = GameObject.Find("PlayerOverworld");
        battleCamera = GameObject.Find("Battle Camera");
        battleCamera.SetActive(false);
        playerBattle = GameObject.Find("PlayerBattle");
        playerBattle.SetActive(false);
        playerCamera = GameObject.Find("Player Camera");
        battleHUD = GameObject.Find("Battle HUD").GetComponent<BattleHUDManager>();
    }

    public void StartBattle(GameObject currentEnemy, List<GameObject> enemyList)
    {
        playerOverworld.GetComponent<Player>().DisableMovement();

        enemyOverworld = currentEnemy;
        enemyOverworld.SetActive(false);
        playerOverworld.SetActive(false);
        playerBattle.SetActive(true);
        playerCamera.SetActive(false);
        battleCamera.SetActive(true);

        for(int i = 0; i < enemyList.Count; i++)
        {
            enemiesInBattle.Add(Instantiate(enemyList[i], enemySpawns[i].position, enemySpawns[i].rotation));
        }

        battleHUD.ActivateHUD(enemiesInBattle);
    }

    public void PlayerAttack(int enemyIndex)
    {
        battleHUD.DeactivateEnemyButtons();
        battleHUD.DeactivateHUD();
        battleHUD.turnText.SetText("Player Turn");
        PlayerBattle player = playerBattle.GetComponent<PlayerBattle>();

        if(player.GetCurrentMagic() < player.GetMPCost(player.attackType))
        {
            battleHUD.ActivateMPWarningText();
            StartCoroutine(WaitBeforeMainBattleScreen());
        }
        else
        {
            player.SetMagic(-player.GetMPCost(player.attackType));

            switch (player.attackType)
            {
                case PlayerBattle.AttackTypes.BASIC:
                    battleHUD.commandText.SetText("Attack");
                    break;
                case PlayerBattle.AttackTypes.FIRE_BALL:
                    battleHUD.commandText.SetText("Fireball");
                    break;
                case PlayerBattle.AttackTypes.HAIL_STORM:
                    battleHUD.commandText.SetText("Hailstorm");
                    break;
                case PlayerBattle.AttackTypes.LIGHTNING:
                    battleHUD.commandText.SetText("Lightning");
                    break;
                case PlayerBattle.AttackTypes.SOUL_BOMB:
                    battleHUD.commandText.SetText("Soul Bomb");
                    break;
                case PlayerBattle.AttackTypes.THANATOS:
                    battleHUD.commandText.SetText("Thanatos");
                    break;
                default:
                    battleHUD.commandText.SetText("Attack");
                    break;
            }

            battleHUD.ActivateTurnNotification();
            StartCoroutine(WaitBeforeEnemyDamage(enemyIndex));
        }
    }

    public void EnemyAttack()
    {
        if(attackingEnemy >= enemiesInBattle.Count)
        {
            attackingEnemy = 0;
            battleHUD.DeactivateNotificationPanel();
            battleHUD.ActivateHUD(enemiesInBattle);
        }
        else
        {
            battleHUD.DeactivateHUD();
            battleHUD.DeactivateNotificationPanel();
            battleHUD.turnText.SetText("Enemy " + (attackingEnemy + 1) + " Turn");
            EnemyBattle enemy = enemiesInBattle[attackingEnemy].GetComponent<EnemyBattle>();

            if (enemy.GetCurrentHealth() > enemy.GetMaxHealth() * 0.8f)
            {
                enemy.attackType = EnemyBattle.AttackTypes.BASIC;
            }
            else if (enemy.GetCurrentHealth() >= enemy.GetMaxHealth() * 0.5f)
            {
                enemy.attackType = (EnemyBattle.AttackTypes)Random.Range(0, 3);
            }
            else
            {
                enemy.attackType = (EnemyBattle.AttackTypes)Random.Range(1, 4);
            }

            switch (enemy.attackType)
            {
                case EnemyBattle.AttackTypes.BASIC:
                    battleHUD.commandText.SetText("Attack");
                    break;
                case EnemyBattle.AttackTypes.DARKNESS:
                    battleHUD.commandText.SetText("Darkness");
                    break;
                case EnemyBattle.AttackTypes.VOID:
                    battleHUD.commandText.SetText("Void");
                    break;
                case EnemyBattle.AttackTypes.TO_TELOS:
                    battleHUD.commandText.SetText("To Telos");
                    break;
                default:
                    battleHUD.commandText.SetText("Attack");
                    break;
            }

            battleHUD.ActivateTurnNotification();
            StartCoroutine(WaitBeforePlayerDamage(enemy));
        }
    }

    public void EscapeBattle()
    {
        battleHUD.DeactivateHUD();
        battleHUD.ActivateEscapeText();
        StartCoroutine(WaitBeforeExitBattle());
    }

    public void ExitBattle()
    {
        PlayerBattle player = playerBattle.GetComponent<PlayerBattle>();

        battleHUD.DeactivateNotificationPanel();
        battleCamera.SetActive(false);
        playerCamera.SetActive(true);
        player.SetHealth(player.GetMaxHealth() - player.GetCurrentHealth());
        player.SetMagic(player.GetMaxMagic() - player.GetCurrentMagic());
        playerBattle.SetActive(false);
        playerOverworld.SetActive(true);
        playerOverworld.GetComponent<Player>().ResetMovement();

        if (enemyOverworld != null)
        {
            enemyOverworld.transform.position += enemyOffset;
            enemyOverworld.SetActive(true);
            enemyOverworld.GetComponent<EnemyOverworldAI>().ResetBattleStarted();
        }

        for (int i = 0; i < enemiesInBattle.Count; i++)
        {
            Destroy(enemiesInBattle[i]);
        }

        enemiesInBattle.Clear();
    }

    public IEnumerator WaitBeforeExitBattle()
    {
        yield return new WaitForSeconds(2.0f);
        ExitBattle();
    }

    public IEnumerator WaitBeforeEnemyDamage(int enemyIndex)
    {
        yield return new WaitForSeconds(2.0f);
        enemiesInBattle[enemyIndex].GetComponent<EnemyBattle>().SetHealth(-playerBattle.GetComponent<PlayerBattle>().GetAttackDamage());
        battleHUD.UpdateEnemyHealth(enemyIndex);
        battleHUD.DeactivateNotificationPanel();

        if(enemiesInBattle.Count == 0)
        {
            battleHUD.ActivateVictoryText();
            Destroy(enemyOverworld);
            StartCoroutine(WaitBeforeExitBattle());
        }
        else
        {
            EnemyAttack();
        }
    }

    public IEnumerator WaitBeforePlayerDamage(EnemyBattle enemy)
    {
        yield return new WaitForSeconds(2.0f);
        PlayerBattle player = playerBattle.GetComponent<PlayerBattle>();
        player.SetHealth(-enemy.GetAttackDamage());

        if(player.GetCurrentHealth() <= 0)
        {
            player.SetHealth(-player.GetCurrentHealth());
            battleHUD.ActivateGameOver();
            StartCoroutine(WaitBeforeRestartApplication());
        }
        else
        {
            attackingEnemy++;
            EnemyAttack();
        }
    }

    public IEnumerator WaitBeforeMainBattleScreen()
    {
        yield return new WaitForSeconds(2.0f);
        battleHUD.DeactivateNotificationPanel();
        battleHUD.ActivateHUD(enemiesInBattle);
    }

    public IEnumerator WaitBeforeRestartApplication()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadSceneAsync(0);
    }
}
