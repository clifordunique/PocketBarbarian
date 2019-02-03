using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueItemSpriteStatic : MonoBehaviour, IMenueItemSprite {

    public enum MENUEITEM_TYPE {BACK_TO_GAME, RELOAD_SAVEPOINT, MAIN_MENUE, EXIT};

    public MENUEITEM_TYPE menueItemType;

    public Sprite enabledSprite;
    private Sprite disabledSprite;



    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        disabledSprite = sr.sprite;
    }
    

    public void Click() {
        switch (menueItemType) {
            case MENUEITEM_TYPE.BACK_TO_GAME:
                GameManager.GetInstance().CloseMenue();
                Debug.Log("Backtogame");
                break;
            case MENUEITEM_TYPE.RELOAD_SAVEPOINT:
                GameManager.GetInstance().ReloadLevel();
                Debug.Log("Reloadgame");
                break;
            case MENUEITEM_TYPE.MAIN_MENUE:
                GameManager.GetInstance().LoadMainMenue();
                Debug.Log("Loadmainmenug");
                break;
            case MENUEITEM_TYPE.EXIT:
                GameManager.GetInstance().ExitGame();
                Debug.Log("Exit");
                break;
            default: break;
        }
    }

    public float GetWidth() {
        return Utils.GetWidthFromSpriteGO(gameObject);
    }

    public void SetDisabled() {
        sr.sprite = disabledSprite;
    }

    public void SetEnabled() {
        sr.sprite = enabledSprite;
    }

}
