using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUnhideButton : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    [SerializeField] private bool _isHidden = false;
    [SerializeField] private Transform _panel;
    [SerializeField] private float _offset;
    private float _panelTopPosition;
    private float _panelDownPosition;

    private void Start()
    {
        _panelTopPosition = _panel.transform.position.y;
        _panelDownPosition = _panel.transform.position.y - _offset;
    }

    public void ChangeState()
    {
        _isHidden = !_isHidden;
        if(_isHidden == true)
        {
            _animation.Play("hideButtonRotateDown");
            _panel.transform.position = new Vector3(_panel.transform.position.x, _panelDownPosition, _panel.transform.position.z);
        }
        else
        {
            _animation.Play("hideButtonRotateUp");
            _panel.transform.position = new Vector3(_panel.transform.position.x, _panelTopPosition, _panel.transform.position.z);
        }
    }
}
