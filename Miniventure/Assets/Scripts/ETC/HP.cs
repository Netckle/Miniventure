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
    //public TextMeshProUGUI HP_TEXT;

    public float offsetY;

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

    private void LateUpdate() 
    {
        Vector3 worldPos = new Vector3();

        switch(type)
        {
            case OBJTYPE.PLAYER:
                worldPos = player.transform.position + new Vector3(0, offsetY, 0);
                break;
            case OBJTYPE.SLIME:
                worldPos = slime.transform.position + new Vector3(0, offsetY, 0);
                break;
            case OBJTYPE.MINO:
                worldPos = mino.transform.position + new Vector3(0, offsetY, 0);
                break;
            case OBJTYPE.BAT:
                worldPos = batMovement.transform.position + new Vector3(0, offsetY, 0);
                break;
        }

        transform.position = Camera.main.WorldToScreenPoint(worldPos);
    }

    void UpdatePlayerHPbar()
    {
        float HP = player.HP;
        HP_BAR.fillAmount = HP / player.maxHP;
        //HP_TEXT.text = string.Format("HP {0} / {0}", HP, player.maxHP);       
    }

    void UpdateSlimeHPbar()
    {
        float HP = slime.HP;
        HP_BAR.fillAmount = HP / slime.maxHP;
        //HP_TEXT.text = string.Format("HP {0} / {0}", HP, slime.maxHP);        
    }
    
    void UpdateMinoHPbar()
    {
        float HP = mino.HP;
        HP_BAR.fillAmount = HP / mino.maxHP;
        //HP_TEXT.text = string.Format("HP {0} / {0}", HP, mino.maxHP);    
    }

    void UpdateBatHPbar()
    {
        float HP = batMovement.HP;
        HP_BAR.fillAmount = HP / batMovement.maxHP;
        //HP_TEXT.text = string.Format("HP {0} / {0}", HP, batMovement.maxHP);    
    }
}
