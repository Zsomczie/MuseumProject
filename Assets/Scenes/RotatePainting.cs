using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class RotatePainting : MonoBehaviour
{
    [SerializeField] GameObject painting;
    Camera cam;
    void Awake()
    {
        painting = this.gameObject;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        InspectPainting();


    }




    //InspectPainting();


    void InspectPainting() 
    {
        if (Input.touchCount>0)
        {
            if(Input.GetTouch(0).phase==TouchPhase.Moved) 
            {
                var Touch = Input.GetTouch(0);
                Vector3 relativePos = painting.transform.position-Camera.main.transform.position;
                var x = Touch.deltaPosition.y;
                var y = Touch.deltaPosition.x;
                Vector3 rotation = new Vector3 (x, y, 0);
                painting.transform.Rotate(cam.transform.rotation.eulerAngles,rotation.x);
            }
        }
    }
    public void Rotate(float rotateLeftRight, float rotateUpDown, bool isPlayer)
    {
        //useUpdate = false;

        //Unsure of how much below code changes outcome.
        float sensitivity = 0;
        if (isPlayer)
        {
            sensitivity = .5f;
        }
        else
        {
            sensitivity = .25f;
        }

        //Get Main camera in Use.
        Camera cam = Camera.main;
        //Gets the world vector space for cameras up vector 
        Vector3 relativeUp = cam.transform.TransformDirection(Vector3.up);
        //Gets world vector for space cameras right vector
        Vector3 relativeRight = cam.transform.TransformDirection(Vector3.right);

        //Turns relativeUp vector from world to objects local space
        Vector3 objectRelativeUp = transform.InverseTransformDirection(relativeUp);
        //Turns relativeRight vector from world to object local space
        Vector3 objectRelaviveRight = transform.InverseTransformDirection(relativeRight);

        //rotateBy = Quaternion.AngleAxis(rotateLeftRight / gameObject.transform.localScale.x * sensitivity, objectRelativeUp)
        //    * Quaternion.AngleAxis(-rotateUpDown / gameObject.transform.localScale.x * sensitivity, objectRelaviveRight);

        //newDeltaObtained = true;

    }
}
