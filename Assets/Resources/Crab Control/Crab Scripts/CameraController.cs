using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject FollowObject;

    private Vector3 StartDifference;

    // Start is called before the first frame update
    void Start()
    {
        if(FollowObject != null)
        {
            StartDifference = transform.position - FollowObject.transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 newObjPos = FollowObject.transform.position;
        Vector3 newCamPos = newObjPos + StartDifference;

        Vector3 curCamPos = transform.position;

        float newCamLerpPosX = Mathf.Lerp(curCamPos.x, newCamPos.x, 0.1f);
        float newCamLerpPosY = Mathf.Lerp(curCamPos.y, newCamPos.y, 0.1f);
        float newCamLerpPosZ = Mathf.Lerp(curCamPos.z, newCamPos.z, 0.1f);

        transform.position = new Vector3(newCamLerpPosX, newCamLerpPosY, newCamLerpPosZ);
    }
}
