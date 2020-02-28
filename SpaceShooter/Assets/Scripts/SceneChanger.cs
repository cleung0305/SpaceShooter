using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string transportTo;
    public string musicName;
    public GameObject Player;
    public Vector3 playerSpawnPos;

    // Start is called before the first frame update
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    public void transportPlayer()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnterBossFight(4, transportTo, musicName, playerSpawnPos);
    }
}
