using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldHUDManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public TextMeshProUGUI victoryText;

    // Start is called before the first frame update
    void Start()
    {
        victoryText.enabled = false;
        StartCoroutine(WaitBeforeDeactivateIntroText());
    }

    public void ActivateVictoryText()
    {
        victoryText.enabled = true;
        StartCoroutine(WaitBeforeEndGame());
    }

    public IEnumerator WaitBeforeDeactivateIntroText()
    {
        yield return new WaitForSeconds(4.0f);
        introText.enabled = false;
    }

    public IEnumerator WaitBeforeEndGame()
    {
        yield return new WaitForSeconds(5.0f);
        Application.Quit();
    }
}
