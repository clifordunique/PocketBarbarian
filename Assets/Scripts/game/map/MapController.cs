using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public enum LOCATION { VILLAGE, DUNGEON, FOREST, GRAVEYARD, CAVE, MAGE, NA};

    public Location village;
    public Location dungeon;
    public Location forest;
    public Location graveyard;
    public Location cave;
    public Location mage;

    private Dictionary<LOCATION, Location> locationMap = new Dictionary<LOCATION, Location>();
    private LOCATION currentLocation = LOCATION.VILLAGE;

    // Start is called before the first frame update
    void Start()
    {
        locationMap.Add(LOCATION.VILLAGE, village);
        locationMap.Add(LOCATION.DUNGEON, dungeon);
        locationMap.Add(LOCATION.FOREST, forest);
        locationMap.Add(LOCATION.GRAVEYARD, graveyard);
        locationMap.Add(LOCATION.CAVE, cave);
        locationMap.Add(LOCATION.MAGE, mage);
    }

    // Update is called once per frame
    void Update()
    {
        LOCATION newLocation = LOCATION.NA;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            newLocation = locationMap[currentLocation].GetLocationLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            newLocation = locationMap[currentLocation].GetLocationRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            newLocation = locationMap[currentLocation].GetLocationUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            newLocation = locationMap[currentLocation].GetLocationDown();
        }

        if (newLocation != LOCATION.NA) {
            locationMap[currentLocation].DisablePointer();
            currentLocation = newLocation;
            locationMap[currentLocation].EnablePointer();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            locationMap[currentLocation].Mark();
        }
    }
}
