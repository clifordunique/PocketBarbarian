using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMenueManager : MonoBehaviour
{
    public enum MENUEITEM_TYPE { NEW_GAME, LOAD_GAME, RELOAD_SAVEPOINT, BACK_TO_GAME, MAIN_MENUE, OPTIONS, CREDITS, EXIT, NAN };
    public MenueItem[] menueItems;
    public bool menueInputEnabled = true;
    public QuestionManager questionManager;


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


    public void ShowQuestion() {
        menueInputEnabled = false;
        SetItemColliderActive(false);
        questionManager.gameObject.SetActive(true);
        questionManager.Init();
        StartCoroutine(WaitForAnswer());
    }

    IEnumerator WaitForAnswer() {
        while (questionManager.selectedAnswer == QuestionManager.ANSWER_TYPE.NAN) {
            yield return new WaitForEndOfFrame();
        }
        menueInputEnabled = true;
        // question selected
        if (questionManager.selectedAnswer == QuestionManager.ANSWER_TYPE.YES) {
            ExecuteSelectedMenueItem();
        }
        questionManager.gameObject.SetActive(false);
        SetItemColliderActive(true);
    }

    private void SetItemColliderActive(bool active) {
        foreach(MenueItem mi in menueItems) {
            mi.SetColliderActive(active);
        }
    }

    public abstract void MenueItemSelected(MENUEITEM_TYPE menueItemType);

    public abstract void ExecuteSelectedMenueItem();
    }
