using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSphere : MonoBehaviour
{
    private OverworldHUDManager overworldHUD;

    // Start is called before the first frame update
    void Start()
    {
        overworldHUD = GameObject.Find("Overworld HUD Manager").GetComponent<OverworldHUDManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            overworldHUD.ActivateVictoryText();
            Destroy(this.gameObject);
        }
    }
}
