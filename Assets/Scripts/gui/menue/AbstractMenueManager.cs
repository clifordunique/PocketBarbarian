using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMenueManager : MonoBehaviour
{
    public MenueItem[] menueItems;

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

    public void MenueItemSelected(MenueItem menueItem) {
        menueItemManager.MenueItemSelected(menueItem);
    }
}
