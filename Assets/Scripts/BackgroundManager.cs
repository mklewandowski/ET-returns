using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    Transform Player;

    [SerializeField]
    GameObject[] Tiles;

    float tileXOffset = 7.6f;
    float tileYOffset = 11.7f;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Player.position.x + "," + Player.position.y);
        int xIndex = (int)(Player.position.x / tileXOffset);
        int yIndex = (int)(Player.position.y / tileYOffset);
        // Debug.Log(xIndex + "," + yIndex);
        int xDir = Player.position.x >= xIndex * tileXOffset ? 1 : -1;
        int yDir = Player.position.y >= yIndex * tileYOffset ? 1 : -1;
        Tiles[0].transform.localPosition = new Vector3(xIndex * tileXOffset, yIndex * tileYOffset, Tiles[0].transform.localPosition.z);
        Tiles[1].transform.localPosition = new Vector3((xIndex + xDir) * tileXOffset, yIndex * tileYOffset, Tiles[1].transform.localPosition.z);
        Tiles[2].transform.localPosition = new Vector3(xIndex * tileXOffset, (yIndex + yDir) * tileYOffset, Tiles[0].transform.localPosition.z);
        Tiles[3].transform.localPosition = new Vector3((xIndex + xDir) * tileXOffset, (yIndex + yDir) * tileYOffset, Tiles[1].transform.localPosition.z);
    }
}
