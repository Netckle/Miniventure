using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlimeHpBar : MonoBehaviour
{
    public Sprite emptyIcon;
    public Sprite normalIcon;
    public Image[] hpIcons = new Image[10];

    private MoveSlimeDot slime;
    private int hp;

    private void Start() 
    {
        slime = GameObject.Find("Boss Slime").GetComponent<MoveSlimeDot>();
    }

    private void Update() 
    {
        hp = slime.HP / 3;

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
