using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueItemManager
{
    private MenueItem[] menueItemArray;
    private int selectedIndex = -1;

    public MenueItemManager(MenueItem[] menueItemArray) {
        this.menueItemArray = menueItemArray;
    }

    public void DeselectAll() {
        foreach(MenueItem menueItem in menueItemArray) {
            menueItem.Deselect();
        }
    }

    public void MenueItemSelected(MenueItem menueItemNew) {
        int index = -1;
        int foundIndex = -1;
        foreach(MenueItem menueItem in menueItemArray) {
            index++;
            if (menueItem == menueItemNew) {
                foundIndex = index;
            }
        }
        if (foundIndex >= 0) {
            DeselectAll();
            selectedIndex = foundIndex;
            menueItemArray[selectedIndex].Select();
        }
    }

    public void NextMenueItem() {
        if (selectedIndex < 0 || (selectedIndex + 1) >= menueItemArray.Length) {
            selectedIndex = 0;
        } else {
            selectedIndex++;
        }
        DeselectAll();
        menueItemArray[selectedIndex].Select();
    }

    public void PreviousMenueItem() {
        if (selectedIndex <= 0) {
            selectedIndex = menueItemArray.Length - 1;
        } else {
            selectedIndex--;
        }
        DeselectAll();
        menueItemArray[selectedIndex].Select();
    }
}
