using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCharController : MonoBehaviour
{
    private Touch touch;

    private float speed = 0.01f ;
    // Start is called before the first frame update
    private Vector3 forward;
    private Vector3 right;
    void Start() {
        
        forward = Camera.main.transform.forward; // Set forward to equal the camera's forward vector
        forward.y = 0; // make sure y is 0
        forward = Vector3.Normalize(forward); // make sure the length of vector is set to a max of 1.0
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward; // set the right-facing vector to be facing right relative to the camera's forward vector
    }


    //needed this conversion because of isometric camera -> change of directions
    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0, 45.0f, 0);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                
                Vector3 touchPosition = new Vector3(
                    touch.deltaPosition.x,
                   0,
                   touch.deltaPosition.y );

                touchPosition = IsoVectorConvert((touchPosition));
                /*transform.position = IsoVectorConvert(new Vector3(
                    transform.position.x  + touch.deltaPosition.x * speed ,
                        transform.position.y ,
                    transform.position.z + touch.deltaPosition.y * speed));*/

                transform.position = transform.position + touchPosition * speed;


            }
        }
    }
}
