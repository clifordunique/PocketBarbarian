using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiSimpleCharacterBackgroundController : MonoBehaviour {

    public Sprite bgrTopLeft;
    public Sprite bgrLeft;
    public Sprite bgrBottomLeft;

    public Sprite bgrTopCenter;
    public Sprite bgrCenter;
    public Sprite bgrBottomCenter;

    public Sprite bgrTopRight;
    public Sprite bgrRight;
    public Sprite bgrBottomRight;


    public Sprite bgrArrowDown;
    public Sprite bgrArrowUp;

    public int pixelBorderX;
    public int pixelBorderY;
    public int pixelOverlapArrows;
    
    private GameObject bgrCenterGo;
    private GameObject bgrLeftGo;
    private GameObject bgrRightGo;

    private GameObject bgrTopCenterGo;
    private GameObject bgrTopLeftGo;
    private GameObject bgrTopRightGo;

    private GameObject bgrBottomCenterGo;
    private GameObject bgrBottomLeftGo;
    private GameObject bgrBottomRightGo;

    private GameObject bgrArrowDownGo;
    //private GameObject bgrArrowUpGo;

    private float initSizeX;
    private float initSizeY;
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
        initSizeY = bgrCenter.bounds.size.y;

        bgrCenterGo = Utils.InstantiateSpriteGameObject(bgrCenter, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrLeftGo = Utils.InstantiateSpriteGameObject(bgrLeft, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrRightGo = Utils.InstantiateSpriteGameObject(bgrRight, Constants.SORTING_LAYER_DIALOGUE, 0, transform);

        bgrTopCenterGo = Utils.InstantiateSpriteGameObject(bgrTopCenter, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrTopLeftGo = Utils.InstantiateSpriteGameObject(bgrTopLeft, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrTopRightGo = Utils.InstantiateSpriteGameObject(bgrTopRight, Constants.SORTING_LAYER_DIALOGUE, 0, transform);

        bgrBottomCenterGo = Utils.InstantiateSpriteGameObject(bgrBottomCenter, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrBottomLeftGo = Utils.InstantiateSpriteGameObject(bgrBottomLeft, Constants.SORTING_LAYER_DIALOGUE, 0, transform);
        bgrBottomRightGo = Utils.InstantiateSpriteGameObject(bgrBottomRight, Constants.SORTING_LAYER_DIALOGUE, 0, transform);

        //bgrArrowUpGo = Utils.InstantiateSpriteGameObject(bgrArrowUp, Constants.SORTING_LAYER_DIALOGUE, 1, transform);
        bgrArrowDownGo = Utils.InstantiateSpriteGameObject(bgrArrowDown, Constants.SORTING_LAYER_DIALOGUE, 1, transform);        
    }

    public void ResizeBackground(Vector2 size) {
        if (!init) {
            Init();
        }

        float newScaleX = CalculateScale(size.x, pixelBorderX, initSizeX);
        float newScaleY = CalculateScale(size.y, pixelBorderY, initSizeY);

        bgrCenterGo.transform.localScale = new Vector3(newScaleX, newScaleY, 0);
        bgrCenterGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, 0));

        SpriteRenderer srCenter = bgrCenterGo.GetComponent<SpriteRenderer>();
        float extendsX = srCenter.bounds.extents.x;
        float extendsY = srCenter.bounds.extents.y;

        bgrTopCenterGo.transform.localScale = new Vector3(newScaleX, 1, 0);
        bgrTopCenterGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, extendsY));
        bgrBottomCenterGo.transform.localScale = new Vector3(newScaleX, 1, 0);
        bgrBottomCenterGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, -extendsY));

        bgrTopLeftGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(-extendsX, extendsY));
        bgrTopLeftGo.transform.localScale = new Vector3(1, 1, 0);
        bgrLeftGo.transform.localScale = new Vector3(1, newScaleY, 0);
        bgrLeftGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(-extendsX, 0));
        bgrBottomLeftGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(-extendsX, -extendsY));
        bgrBottomLeftGo.transform.localScale = new Vector3(1, 1, 0);

        bgrTopRightGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(extendsX, extendsY));
        bgrTopRightGo.transform.localScale = new Vector3(1, 1, 0);
        bgrRightGo.transform.localScale = new Vector3(1, newScaleY, 0);
        bgrRightGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(extendsX, 0));
        bgrBottomRightGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(extendsX, -extendsY));
        bgrBottomRightGo.transform.localScale = new Vector3(1, 1, 0);

        SpriteRenderer srBottomCenter = bgrBottomCenterGo.GetComponent<SpriteRenderer>();
        float extendsBottomY = srBottomCenter.bounds.size.y;
        extendsBottomY += extendsY;
        extendsBottomY -= pixelOverlapArrows / Constants.PPU;
        //bgrArrowUpGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, extendsY));
        bgrArrowDownGo.transform.localPosition = Utils.MakePixelPerfect(new Vector2(0, -extendsBottomY));
    }

    public Vector2 GetSize() {
        SpriteRenderer srCenter = bgrCenterGo.GetComponent<SpriteRenderer>();
        SpriteRenderer srLeft = bgrLeftGo.GetComponent<SpriteRenderer>();
        SpriteRenderer srTopCenter = bgrTopCenterGo.GetComponent<SpriteRenderer>();
        SpriteRenderer srArrow = bgrArrowDownGo.GetComponent<SpriteRenderer>();

        Vector2 size = new Vector2(srCenter.bounds.size.x + (2* srLeft.bounds.size.x), srCenter.bounds.size.y + srTopCenter.bounds.size.y + srArrow.bounds.size.y);

        return size;
    }

    public void MoveArrowLeft(float offsetX) {
        
        if (bgrArrowDownGo.transform.localScale.x < 0) {
            offsetX = Mathf.Abs(offsetX);
        }
        bgrArrowDownGo.transform.localPosition = new Vector2(offsetX, bgrArrowDownGo.transform.localPosition.y);
    }

    public void MoveArrowRight(float offsetX) {
        
        if (bgrArrowDownGo.transform.localScale.x > 0) {
            offsetX = Mathf.Abs(offsetX);
        }
        bgrArrowDownGo.transform.localPosition = new Vector2(offsetX, bgrArrowDownGo.transform.localPosition.y);
    }

    private float CalculateScale(float size, int borderSpace, float initSize) {
        float newSizeX = size + ((borderSpace * 2) / Constants.PPU);
        int pixelWidth = Utils.WorldunitToPixel(newSizeX);
        if (pixelWidth % 2 != 0) {
            pixelWidth++;
        }
        return ((pixelWidth / Constants.PPU) / initSize);
    }
}
