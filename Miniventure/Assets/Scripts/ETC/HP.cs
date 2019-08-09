using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum OBJTYPE
{
    PLAYER, SLIME, MINO, BAT
}

public class HP : MonoBehaviour
{
    public OBJTYPE type;

    public MoveSlimeDot slime;
    public PlayerMovement player;
    public MoveMino mino;
    public BossBatMovement batMovement;

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
            case OBJTYPE.MINO:
                UpdateMinoHPbar();
                break;
            case OBJTYPE.BAT:
                UpdateBatHPbar();
                break;

        }
    }

    void UpdatePlayerHPbar()
    {
        float HP = player.HP;
        HP_BAR.fillAmount = HP / player.maxHP;
        HP_TEXT.text = string.Format("HP {0} / {0}", HP, player.maxHP);       
    }

    void UpdateSlimeHPbar()
    {
        float HP = slime.HP;
        HP_BAR.fillAmount = HP / slime.maxHP;
        HP_TEXT.text = string.Format("HP {0} / {0}", HP, slime.maxHP);        
    }
    
    void UpdateMinoHPbar()
    {
        float HP = mino.HP;
        HP_BAR.fillAmount = HP / mino.maxHP;
        HP_TEXT.text = string.Format("HP {0} / {0}", HP, mino.maxHP);    
    }

    void UpdateBatHPbar()
    {
        float HP = batMovement.HP;
        HP_BAR.fillAmount = HP / batMovement.maxHP;
        HP_TEXT.text = string.Format("HP {0} / {0}", HP, batMovement.maxHP);    
    }
}
