using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameEvents : MonoBehaviour
{
    [SerializeField] private GameObject winBackground;
    [SerializeField] private GameObject loseBackground;
    [SerializeField] private GameObject deathBackground;
    [SerializeField] private GameObject spellPanel;

    void Update()
    {
        if (Input.GetKey(KeyCode.A)) 
        {
            partyLoseGame();
        }
        
        if (Input.GetKey(KeyCode.B)) 
        {
            playerDie();
        }
        
        if (Input.GetKey(KeyCode.C)) 
        {
            partyWinGame();
        }
    }

    private void partyLoseGame()
    {
        spellPanel.SetActive(false);
        loseBackground.SetActive(true);
    }
    
    private void playerDie()
    {
        spellPanel.SetActive(false);
        deathBackground.SetActive(true);
    }
    
    private void partyWinGame()
    {
        spellPanel.SetActive(false);
        winBackground.SetActive(true);
    }
}
