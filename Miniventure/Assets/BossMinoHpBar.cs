using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMinoHpBar : MonoBehaviour
{
    public Sprite emptyIcon;
    public Sprite normalIcon;
    public Image[] hpIcons = new Image[10];

    private MoveMino mino;
    private int hp;

    private void Start() 
    {
        mino = GameObject.Find("Boss Mino").GetComponent<MoveMino>();
    }

    private void Update() 
    {
        hp = mino.HP / 3;

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
