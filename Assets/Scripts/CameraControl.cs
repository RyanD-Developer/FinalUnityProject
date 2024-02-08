using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{

    public Transform target;

    public BoxCollider2D bounds;

    private Vector3
        _min,
        _max;

    public bool isFollowing = true;

    public void cameraMovement()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        if (isFollowing)
        {
            x = target.position.x;

            y = target.position.y + 2.519385f;
        }

        var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);

        x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, _min.y + Camera.main.orthographicSize, _max.y - Camera.main.orthographicSize);

        transform.position = new Vector3(x, y, transform.position.z);
    }

    // Use this for initialization
    void Start()
    {
        _min = bounds.bounds.min;
        _max = bounds.bounds.max;
    }

    // Update is called once per frame
    void Update()
    {
        cameraMovement();
    }
}
