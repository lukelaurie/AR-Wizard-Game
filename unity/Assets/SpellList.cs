using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellList : MonoBehaviour
{
    private int[] spells = { 1, 0, 0, 0 };
    [SerializeField] private Button fireButton;
    [SerializeField] private Button lightButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Button rockButton;
    [SerializeField] private Button tempButton;
    void Start()
    {
        Button[] buttons = { lightButton, healButton, rockButton, tempButton };
        for (int i = 0; i < 4; i++)
        {
            if (spells[i] == 1)
            {
                buttons[i].interactable = true;
                buttons[i].GetComponent<Image>().enabled = false;
            }
            else
            {
                buttons[i].interactable = false;
                buttons[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
