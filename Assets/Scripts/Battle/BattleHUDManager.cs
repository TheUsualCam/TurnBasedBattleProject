using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class BattleHUDManager : MonoBehaviour
{
    public GameObject battlePanel;
    public GameObject notificationPanel;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI commandText;
    public TextMeshProUGUI escapeText;
    public TextMeshProUGUI mpWarningText;
    public TextMeshProUGUI itemWarningText;
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerMagicText;
    public List<TextMeshProUGUI> enemyNameList = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> enemyHealthList = new List<TextMeshProUGUI>();
    public List<GameObject> enemyButtons = new List<GameObject>();
    public List<GameObject> mainButtons = new List<GameObject>();
    public List<GameObject> magicButtons = new List<GameObject>();

    private PlayerBattle player;
    private List<GameObject> enemiesInBattle = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBattle").GetComponent<PlayerBattle>();
        battlePanel.SetActive(false);
        notificationPanel.SetActive(false);

        for (int i = 0; i < enemyButtons.Count; i++)
        {
            enemyButtons[i].SetActive(false);
        }

        for (int i = 0; i < magicButtons.Count; i++)
        {
            magicButtons[i].SetActive(false);
        }
    }

    public void ActivateHUD(List<GameObject> enemyList)
    {
        enemiesInBattle = enemyList;
        playerHealthText.SetText("HP :  " + player.GetCurrentHealth() + " / " + player.GetMaxHealth());
        playerMagicText.SetText("MP :  " + player.GetCurrentMagic() + " / " + player.GetMaxMagic());
        playerText.enabled = true;
        playerHealthText.enabled = true;
        playerMagicText.enabled = true;

        for (int i = 0; i < enemiesInBattle.Count; i++)
        {
            EnemyBattle enemy = enemiesInBattle[i].GetComponent<EnemyBattle>();
            enemyNameList[i].SetText(enemy.GetName());
            enemyHealthList[i].SetText("HP :  " + enemy.GetCurrentHealth() + " / " + enemy.GetMaxHealth());
            enemyNameList[i].enabled = true;
            enemyHealthList[i].enabled = true;
        }

        if(enemiesInBattle.Count < enemyNameList.Count)
        {
            for(int i = enemiesInBattle.Count; i < enemyNameList.Count; i++)
            {
                enemyNameList[i].enabled = false;
                enemyHealthList[i].enabled = false;
            }
        }

        for (int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].SetActive(true);
        }    

        battlePanel.SetActive(true);
    }

    public void DeactivateHUD()
    {
        battlePanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    public void DeactivateAllNotificationText()
    {
        escapeText.enabled = false;
        mpWarningText.enabled = false;
        itemWarningText.enabled = false;
        victoryText.enabled = false;
        gameOverText.enabled = false;
        turnText.enabled = false;
        commandText.enabled = false;
    }

    public void ActivateEscapeText()
    {
        DeactivateAllNotificationText();
        escapeText.enabled = true;
        notificationPanel.SetActive(true);
    }

    public void ActivateTurnNotification()
    {
        DeactivateAllNotificationText();
        turnText.enabled = true;
        commandText.enabled = true;
        notificationPanel.SetActive(true);
    }

    public void ActivateMPWarningText()
    {
        DeactivateAllNotificationText();
        mpWarningText.enabled = true;
        notificationPanel.SetActive(true);
    }

    public void ActivateItemWarningText()
    {
        DeactivateHUD();
        DeactivateAllNotificationText();
        itemWarningText.enabled = true;
        notificationPanel.SetActive(true);
        StartCoroutine(WaitBeforeDeactivateItemWarning());
    }

    public void ActivateVictoryText()
    {
        DeactivateHUD();
        DeactivateAllNotificationText();
        victoryText.enabled = true;
        notificationPanel.SetActive(true);
    }

    public void ActivateGameOver()
    {
        DeactivateHUD();
        DeactivateAllNotificationText();
        gameOverText.enabled = true;
        notificationPanel.SetActive(true);
    }

    public void DeactivateNotificationPanel()
    {
        notificationPanel.SetActive(false);
    }

    public void ActivateEnemyButtons()
    {
        playerText.enabled = false;
        playerHealthText.enabled = false;
        playerMagicText.enabled = false;

        for (int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].SetActive(false);
        }

        for(int i = 0; i < magicButtons.Count; i++)
        {
            magicButtons[i].SetActive(false);
        }

        for (int i = 0; i < enemiesInBattle.Count; i++)
        {
            enemyHealthList[i].enabled = false;
            enemyNameList[i].enabled = false;
            enemyButtons[i].SetActive(true);
            enemyButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = enemiesInBattle[i].GetComponent<EnemyBattle>().GetName();
        }
    }

    public void DeactivateEnemyButtons()
    {
        for(int i = 0; i < enemyButtons.Count; i++)
        {
            enemyButtons[i].SetActive(false);
        }
    }

    public void ActivateMagicButtons()
    {
        playerText.enabled = false;
        playerHealthText.enabled = false;
        playerMagicText.enabled = false;

        for (int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].SetActive(false);
        }

        for (int i = 0; i < enemiesInBattle.Count; i++)
        {
            enemyHealthList[i].enabled = false;
            enemyNameList[i].enabled = false;
        }

        for(int i = 0; i < magicButtons.Count; i++)
        {
            magicButtons[i].SetActive(true);
        }
    }

    public void DeactivateMagicButtons()
    {
        for(int i = 0; i < magicButtons.Count; i++)
        {
            magicButtons[i].SetActive(false);
        }
    }

    public void UpdateEnemyHealth(int enemyIndex)
    {
        EnemyBattle enemy = enemiesInBattle[enemyIndex].GetComponent<EnemyBattle>();

        if (enemy.GetCurrentHealth() <= 0)
        {
            enemyHealthList[enemyIndex].enabled = false;
            enemyNameList[enemyIndex].enabled = false;
            enemyButtons[enemyIndex].SetActive(false);
            Destroy(enemiesInBattle[enemyIndex]);
            enemiesInBattle.Remove(enemiesInBattle[enemyIndex]);
        }
        else
        {
            enemyHealthList[enemyIndex].SetText("HP :  " + enemy.GetCurrentHealth() + " / " + enemy.GetMaxHealth());
        }
    }

    public IEnumerator WaitBeforeDeactivateItemWarning()
    {
        yield return new WaitForSeconds(2.0f);
        DeactivateNotificationPanel();
        ActivateHUD(enemiesInBattle);
    }
}
