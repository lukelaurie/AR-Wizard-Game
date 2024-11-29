using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellStore : MonoBehaviour
{
    private int[] spells = { 1, 0, 0, 0 }; // List of player's unlocked spells e.g. [1,0,1,0]
    public Button fireball;
    public Button lightning;
    public GameObject l_Lock;
    public Button healing;
    public GameObject h_Lock;
    public Button temp1;
    public GameObject t1_Lock;
    public Button temp2;
    public GameObject t2_Lock;
    // Start is called before the first frame update
    void Start()
    {
        fireball.interactable = false;
        Setup();
    }

    private void Setup()
    {
        Button[] buttons = { lightning, healing, temp1, temp2 };
        GameObject[] panels = { l_Lock, h_Lock, t1_Lock, t2_Lock };
        for (int i = 0; i < 4; i++)
        {
            if (spells[i] != 1)
            {
                buttons[i].interactable = true;
                panels[i].gameObject.SetActive(true);
            }
            else
            {
                buttons[i].interactable = false;
                panels[i].gameObject.SetActive(false);
            }
        }

    }
}
