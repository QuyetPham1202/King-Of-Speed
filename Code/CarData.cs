using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " New Car Data", menuName = "Car Data ", order = 51)]
public class CarData :ScriptableObject
{
    // Start is called before the first frame update
    public int index;
    [SerializeField]
    public int carUniqueID;
    [SerializeField]
    public Sprite carUISprite;
    [SerializeField]
    public GameObject carPrefab;
    public int CarUniqueID
    {
        get { return carUniqueID; }

    }
    public Sprite CarUISprite
    {
        get { return carUISprite; }
    }
    public GameObject CarPrefab
    { get { return carPrefab; } }

}
