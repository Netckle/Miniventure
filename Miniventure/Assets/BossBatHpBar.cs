using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBatHpBar : MonoBehaviour
{
    public Sprite emptyIcon;
    public Sprite normalIcon;
    public Image[] hpIcons = new Image[10];

    private BossBatMovement bat;
    private int hp;

    private void Start() 
    {
        bat = GameObject.Find("Bat Boss").GetComponent<BossBatMovement>();
    }

    private void Update() 
    {
        hp = bat.HP / 3;

        for (int i = 0; i < 10; ++i)
        {
            hpIcons[i].sprite = emptyIcon;
        }

        for (int i = 0; i < hp; ++i)
        {
            hpIcons[i].sprite = normalIcon;
        }
    }
}
