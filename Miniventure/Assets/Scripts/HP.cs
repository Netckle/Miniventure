using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum OBJTYPE
{
    PLAYER, SLIME
}

public class HP : MonoBehaviour
{
    public OBJTYPE type;

    public MoveSlimeDot slime;
    public PlayerMovement player;

    public Image HP_BAR;
    public TextMeshProUGUI HP_TEXT;

    void Update()
    {
        switch(type)
        {
            case OBJTYPE.PLAYER:
                UpdatePlayerHPbar();
                break;
            case OBJTYPE.SLIME:
                UpdateSlimeHPbar();
                break;
        }
    }

    void UpdatePlayerHPbar()
    {
        float HP = player.health;
        HP_BAR.fillAmount = HP / player.maxHealth;
        HP_TEXT.text = string.Format("HP {0} / {0}", HP, player.maxHealth);       
    }

    void UpdateSlimeHPbar()
    {
        float HP = slime.HP;
        HP_BAR.fillAmount = HP / slime.maxHP;
        HP_TEXT.text = string.Format("HP {0} / {0}", HP, slime.maxHP);        
    }
}
