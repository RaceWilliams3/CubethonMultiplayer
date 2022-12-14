using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BarrierSpawn : MonoBehaviour
{
    public List<GameObject> moveableSections;
    private List<GameObject> frozenSections;

    private float spawnPos = 125;
    private float incrementDistance;

    private PlayerController spawnBasePlayer;

    void Start()
    {
        spawnBasePlayer = GameManager.instance.players[0];
        incrementDistance = moveableSections[0].transform.GetChild(0).GetComponent<Transform>().localScale.z;
        frozenSections = new List<GameObject>();
        spawnPos += incrementDistance;
    }


    void Update()
    {
        if (frozenSections.Count < 4)
        {
            GameObject newTile = moveableSections[Random.Range(0, moveableSections.Count - 1)];
            //Place new section
            PlaceNewSection(newTile);
            //Add to frozen sections and remove from moveable sections
        }

        if(spawnBasePlayer.GetComponent<Transform>().localPosition.z > frozenSections[0].GetComponent<Transform>().localPosition.z + incrementDistance * 2)
        {
            ThawTile(frozenSections[0]);
        }
    }

    void PlaceNewSection(GameObject newTile)
    {
        frozenSections.Add(newTile);
        newTile.GetComponent<Transform>().localPosition = new Vector3(0, 0, spawnPos);
        spawnPos += incrementDistance;
        moveableSections.Remove(newTile);
    }

    void ThawTile(GameObject oldTile)
    {
        frozenSections.Remove(oldTile);
        moveableSections.Add(oldTile);
    }
}
