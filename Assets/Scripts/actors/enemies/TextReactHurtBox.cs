using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReactHurtBox: HurtBox {

    public TextBox textBox;
    public string[] hurtText;
    public float showTextTime = 1F;
    private int pos = 0;

    // Start is called before the first frame update
    public override void Start() {
        base.Start();

    }

    public override void ReceiveHit(bool instakill, int damage, HitBox.DAMAGE_TYPE damageType, HitBox attackerActor, Vector3 hitSourcePosition, Collider2D collider2d) {
        base.ReceiveHit(instakill, damage, damageType, attackerActor, hitSourcePosition, collider2d);
        
        if (pos >= 0 && pos < hurtText.Length) {
            textBox.gameObject.SetActive(true);
            textBox.ShowTextBox(hurtText[pos]);
            pos++;
            StartCoroutine(DeactivateTextBox(pos));
        }        
    }

    IEnumerator DeactivateTextBox(int currentPos) {
        yield return new WaitForSeconds(showTextTime);
        if (pos == currentPos) {
            // prüfen ob sich in der Zwischenzeit ein neuer Hit ergeben hat
            textBox.gameObject.SetActive(false);
        }

    }
}
