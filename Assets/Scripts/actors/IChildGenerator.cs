using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChildGenerator {

    void SpawnChildrenOnHit();
    void SpawnChildrenOnDeath();
}
