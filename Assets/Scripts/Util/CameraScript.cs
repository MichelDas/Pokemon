using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] float minX, maxX, minY, maxY;

    [SerializeField] Transform target;

    [SerializeField] float followSpeed;

    float targetX, targetY;


    private void LateUpdate()
    {
        if (target)
        {
            targetX = target.position.x;
            targetY = target.position.y;

            targetX = Mathf.Clamp(targetX, minX, maxX);
            targetY = Mathf.Clamp(targetY, minY, maxY);

            Vector3 curPos = transform.position;

            curPos = Vector3.Lerp(curPos, new Vector3(targetX, targetY, curPos.z), followSpeed * Time.deltaTime);

            transform.position = curPos;
        }
    }


}
