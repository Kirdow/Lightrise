using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;

    private Camera _camera;

    private float _cameraY;

    public CameraBound Bound => new CameraBound(transform.position.y - _camera.orthographicSize, transform.position.y + _camera.orthographicSize);
    public float MaxBound { get; private set; }

    public void Reset()
    {
        _cameraY = 0;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _cameraY = transform.position.y;
    }

    private void Start()
    {
        _cameraY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float posY = playerTransform.position.y;
        if (posY < 0) posY = 0;
        if (posY > _cameraY) _cameraY = posY;
        transform.position = new Vector3(playerTransform.position.x, _cameraY, transform.position.z);
        float boundMax = Bound.max;
        if (boundMax > MaxBound) MaxBound = boundMax;
    }

    
}
