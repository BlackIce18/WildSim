using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _panBorderThinkness = 10f;
    [SerializeField] private Vector2 _panLimit;

    [SerializeField] private float _scrollSpeed = 3f;
    [SerializeField] private float _minScrollY = 5f;
    [SerializeField] private float _maxScrollY = 50f;

    [SerializeField] private Vector2 _rotateLimit;
    public bool activatePanLimit = true;
    private float _currentSpeed;
    // Update is called once per frame
    private void Start()
    {
        _currentSpeed = _speed;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Vector3 position = transform.position;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        position.y -= scroll * _scrollSpeed * 100f * Time.deltaTime;
        transform.position = position;
        if (Input.GetKey("w") || ((Input.mousePosition.y >= Screen.height - _panBorderThinkness) && activatePanLimit))
        {
            //position.z += _speed * Time.deltaTime;
            transform.position += new Vector3(transform.forward.x * _currentSpeed * Time.deltaTime, 0, transform.forward.z * _currentSpeed * Time.deltaTime);
        }
        if(Input.GetKey("s") || ((Input.mousePosition.y <= _panBorderThinkness) && activatePanLimit))
        {

            //position.z -= _speed * Time.deltaTime;
            transform.position -= new Vector3(transform.forward.x * _currentSpeed * Time.deltaTime, 0, transform.forward.z * _currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey("d") || ((Input.mousePosition.x >= Screen.width - _panBorderThinkness) && activatePanLimit))
        {
            transform.Translate(Vector3.right * _currentSpeed * Time.deltaTime);
            //position.x += _speed * Time.deltaTime;
        }
        if (Input.GetKey("a") || ((Input.mousePosition.x <= _panBorderThinkness) && activatePanLimit))
        {
            transform.Translate(Vector3.left * _currentSpeed * Time.deltaTime);
            //position.x -= _speed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed = _speed * 2;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _currentSpeed = _speed / 2;
        }
        /*

        position.x = Mathf.Clamp(position.x, -_panLimit.x, _panLimit.x);
        position.y = Mathf.Clamp(position.y, _minScrollY, _maxScrollY);
        position.z = Mathf.Clamp(position.z, -_panLimit.y, _panLimit.y);

        transform.position = position;*/

    }

}
