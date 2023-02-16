using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class supCam : MonoBehaviour
{
    public focusLevel focusLevel;

    public List<GameObject> Players;

    public float DepthUpdateSpeed = 5f;
    public float AngleUpdateSpeed = 7f;
    public float PositionUpdateSpeed = 5f;

    public float DepthMax = -10f;
    public float DepthMin = -22f;

    public float AngleMax = 11f;
    public float AngleMin = 3f;

    private float CameraEulerX;
    private Vector3 CameraPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        Players.Add(focusLevel.gameObject);
    }
    public void changeMembers(GameObject[] players)
    {
        foreach (GameObject go in players)
        {
            Players.Add(go);
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateCameraLocation();
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 position = gameObject.transform.position;
        if (position != CameraPosition)
        {
            Vector3 newPosition = Vector3.zero;
            newPosition.x = Mathf.MoveTowards(position.x, CameraPosition.x, PositionUpdateSpeed * Time.deltaTime);
            newPosition.y = Mathf.MoveTowards(position.y, CameraPosition.y, PositionUpdateSpeed * Time.deltaTime);
            newPosition.z = Mathf.MoveTowards(position.z, CameraPosition.z, DepthUpdateSpeed * Time.deltaTime);
            gameObject.transform.position = newPosition;
        }

        Vector3 localEulerAngles = gameObject.transform.localEulerAngles;
        if(localEulerAngles.x != CameraEulerX)
        {
            Vector3 targetEulerAngles = new Vector3(CameraEulerX, localEulerAngles.y, localEulerAngles.z);
            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngles, targetEulerAngles, AngleUpdateSpeed*Time.deltaTime);
        }
    }
    private void CalculateCameraLocation()
    {
        Vector3 averageCenter = Vector3.zero;
        Vector3 totalPositions = Vector3.zero;
        Bounds playerBounds = new Bounds();

        for (int i=0; i<Players.Count; i++)
        {
            Vector3 playerPosition = Players[i].transform.position;

            if (!focusLevel.focusBounds.Contains(playerPosition))
            {
                float playerX = Mathf.Clamp(playerPosition.x, focusLevel.focusBounds.min.x, focusLevel.focusBounds.max.x);
                float playerY = Mathf.Clamp(playerPosition.y, focusLevel.focusBounds.min.y, focusLevel.focusBounds.max.y);
                float playerZ = Mathf.Clamp(playerPosition.z, focusLevel.focusBounds.min.z, focusLevel.focusBounds.max.z);
                playerPosition = new Vector3(playerX, playerY, playerZ);
            }

            totalPositions += playerPosition;
            playerBounds.Encapsulate(playerPosition);

        }

        averageCenter = (totalPositions / Players.Count);

        float extents = (playerBounds.extents.x + playerBounds.extents.y);
        float lerpPercent = Mathf.InverseLerp(0, (focusLevel.halfXBounds+focusLevel.halfYBounds)/2, extents);

        float depth = Mathf.Lerp(DepthMax, DepthMin, lerpPercent);
        float angle = Mathf.Lerp(AngleMax, AngleMin, lerpPercent);

        CameraEulerX = angle;
        CameraPosition = new Vector3(averageCenter.x, averageCenter.y, depth);
    }
}
