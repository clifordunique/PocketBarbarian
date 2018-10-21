using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiSimpleCharacterBackgroundController : MonoBehaviour {

    public Sprite bgrCenter;
    public Sprite bgrLeft;
    public Sprite bgrRight;

    public Sprite bgrArrowDown;
    public Sprite bgrArrowUp;

    public int pixelSpaceX;
    public int pixelOverlapArrows;
    
    private GameObject bgrCenterGo;
    private GameObject bgrLeftGo;
    private GameObject bgrRightGo;

    private GameObject bgrArrowDownGo;
    //private GameObject bgrArrowUpGo;

    private float initSizeX;
    private bool init = false;

	// Use this for initialization
	void Start () {
        if (!init) {
            Init();            
        }
    }

    private void Init() {
        init = true;
        initSizeX = bgrCenter.bounds.size.x;

        bgrCenterGo = Utils.InstantiateSpriteGameObject(bgrCenter, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrLeftGo = Utils.InstantiateSpriteGameObject(bgrLeft, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrRightGo = Utils.InstantiateSpriteGameObject(bgrRight, Constants.SORTING_LAYER_DIALOGUE, 0, transform);

        //bgrArrowUpGo = Utils.InstantiateSpriteGameObject(bgrArrowUp, Constants.SORTING_LAYER_DIALOGUE, 1, transform);
        bgrArrowDownGo = Utils.InstantiateSpriteGameObject(bgrArrowDown, Constants.SORTING_LAYER_DIALOGUE, 1, transform);        
    }

    public void ResizeBackground(float size) {
        if (!init) {
            Init();
        }
        float newSize = size + ((pixelSpaceX * 2) / Constants.PPU);
        int pixelWidth = Utils.WorldunitToPixel(newSize);
        if (pixelWidth % 2 != 0) {
            pixelWidth++;
        }
        float newScale = (pixelWidth / Constants.PPU) / initSizeX;
        bgrCenterGo.transform.localScale = new Vector3(newScale, 1, 0);
        bgrCenterGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, 0));

        // rearrange bounds
        SpriteRenderer srCenter = bgrCenterGo.GetComponent<SpriteRenderer>();
        float extendsX = srCenter.bounds.extents.x;
        float extendsY = srCenter.bounds.extents.y;
        bgrLeftGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(-extendsX, 0));
        bgrRightGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(extendsX, 0));

        //tmp rearange arrows
        extendsY -= pixelOverlapArrows / Constants.PPU;
        //bgrArrowUpGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, extendsY));
        bgrArrowDownGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, -extendsY));
    }

    public Vector2 GetSize() {
        SpriteRenderer srCenter = bgrCenterGo.GetComponent<SpriteRenderer>();
        SpriteRenderer srLeft = bgrLeftGo.GetComponent<SpriteRenderer>();
        SpriteRenderer srArrow = bgrArrowDownGo.GetComponent<SpriteRenderer>();

        Vector2 size = new Vector2(srCenter.bounds.size.x + (2* srLeft.bounds.size.x), srCenter.bounds.size.y + srArrow.bounds.size.y);

        return size;
    }

    public void MoveArrow(float offsetX) {
        
        //bgrArrowUpGo.transform.localPosition = new Vector2(offsetX, bgrArrowUpGo.transform.localPosition.y);
        bgrArrowDownGo.transform.localPosition = new Vector2(offsetX, bgrArrowDownGo.transform.localPosition.y);
    }
}
