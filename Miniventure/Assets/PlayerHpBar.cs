using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public Sprite emptyIcon;
    public Sprite normalIcon;
    public Image[] hpIcons = new Image[5];

    private PlayerMovement player;
    private int hp;

    private void Start() 
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void Update() 
    {
        hp = player.HP / 2;

        for (int i = 0; i < 5; ++i)
        {
            hpIcons[i].sprite = emptyIcon;
        }

        for (int i = 0; i < hp; ++i)
        {
            hpIcons[i].sprite = normalIcon;
        }
    }
}
