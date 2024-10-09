using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private float _angularSpeed = 100.0f;
    [SerializeField] private Camera _targetCamera;

    [SerializeField] private Vector2 _rotationLimitX;

    void Start()
    {
        if (_targetCamera == null)
            _targetCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            float rotationX = _targetCamera.transform.localEulerAngles.x;
            float rotationY = _targetCamera.transform.localEulerAngles.y;

            rotationX += -Input.GetAxis("Mouse Y") * _angularSpeed * Time.deltaTime;
            rotationX = (rotationX > 180) ? rotationX - 360 : rotationX;
            rotationX = Mathf.Clamp(rotationX, _rotationLimitX.x, _rotationLimitX.y);

            rotationY += Input.GetAxis("Mouse X") * _angularSpeed * Time.deltaTime;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
        }
    }
}
