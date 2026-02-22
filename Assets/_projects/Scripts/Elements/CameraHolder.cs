using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform followObject;
    public float rotationOffset;

    public float smoothTime;


    private Vector3 _vel;

    private void FixedUpdate()
    {
        var followPost = 
        transform.position = Vector3.SmoothDamp(
            transform.position,
        followObject.position + 
        followObject.forward * rotationOffset, 
        ref _vel, smoothTime);
    }
}
