using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMenueManager : MonoBehaviour
{
    public enum MENUEITEM_TYPE { BACK_TO_GAME, RELOAD_SAVEPOINT, MAIN_MENUE, EXIT, NAN };
    public MenueItem[] menueItems;
    public bool menueInputEnabled = true;


    [HideInInspector]
    public MenueItemManager menueItemManager;


    private static AbstractMenueManager _instance;
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    public static AbstractMenueManager GetInstance() {
        return _instance;
    }

    public void MenueItemFocused(MenueItem menueItem) {
        menueItemManager.MenueItemFocused(menueItem);
    }

    public abstract void MenueItemSelected(MENUEITEM_TYPE menueItemType);
}
