using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] GameManager gameManager;
    [SerializeField] float scrollSpeed = 10f;
    [SerializeField] float minSize = 1f;
    [SerializeField] float maxSize = 5f;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float kbScroll = Input.GetAxis("Zoom");
        scrollSpeed = gameManager.GetScrollSpeed();
        playerCam.orthographicSize -= scroll * scrollSpeed * Time.deltaTime; //scroll the camera
        playerCam.orthographicSize -= kbScroll * scrollSpeed * Time.deltaTime;
        if (playerCam.orthographicSize < minSize)
            playerCam.orthographicSize = minSize;
        if (playerCam.orthographicSize > maxSize)
            playerCam.orthographicSize = maxSize;
        transform.rotation = Quaternion.Euler(Vector3.zero); //set the camera's rotation to zero (it's parented to the player ShipController so we don't want it to rotate if the player does)
    }
}
