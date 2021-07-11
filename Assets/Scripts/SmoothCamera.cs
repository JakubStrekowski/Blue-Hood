using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public float dampTime = 0.25f;
    public Transform target;

    private Vector2 _velocity = Vector3.zero;

    Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector2 point = _camera.transform.position;
            Vector2 delta = (Vector2)target.position - point;
            Vector2 destination = (Vector2)transform.position + delta;

            Vector2 calculatedPosition = Vector2.SmoothDamp(transform.position, destination, ref _velocity, dampTime);
            transform.position = new Vector3(calculatedPosition.x, calculatedPosition.y, transform.position.z);
            
        }

    }
}