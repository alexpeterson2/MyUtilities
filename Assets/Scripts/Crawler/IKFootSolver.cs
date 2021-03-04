using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask _terrainLayer = default;
    [SerializeField] Transform _body = default;
    [SerializeField] IKFootSolver _otherFoot = default;
    [SerializeField] float _speed = 1;
    [SerializeField] float _stepDistance = 4;
    [SerializeField] float _stepLength = 4;
    [SerializeField] float _stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;

    float _footSpacing;
    Vector3 _oldPosition, _currentPosition, _newPosition;
    Vector3 _oldNormal, _currentNormal, _newNormal;
    float _lerp;

    void Start()
    {
        _footSpacing = transform.localPosition.x;
        _currentPosition = _newPosition = _oldPosition = transform.position;
        _currentNormal = _newNormal = _oldNormal = transform.up;
        _lerp = 1;
    }

    void Update()
    {
        transform.position = _currentPosition;
        transform.up = _currentNormal;

        CheckFootPosition();
        MoveFeet();
    }

    private void CheckFootPosition()
    {
        var ray = new Ray(_body.position + (_body.right * _footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, _terrainLayer.value))
        {
            if (Vector3.Distance(_newPosition, info.point) > _stepDistance && !_otherFoot.IsMoving() && _lerp >= 1)
            {
                _lerp = 0;
                int direction = _body.InverseTransformPoint(info.point).z > _body.InverseTransformPoint(_newPosition).z ? 1 : -1;
                _newPosition = info.point + (_body.forward * _stepLength * direction) + footOffset;
                _newNormal = info.normal;
            }
        }
    }

    private void MoveFeet()
    {
        if (_lerp < 1)
        {
            var tempPosition = Vector3.Lerp(_oldPosition, _newPosition, _lerp);
            tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;

            _currentPosition = tempPosition;
            _currentNormal = Vector3.Lerp(_oldNormal, _newNormal, _lerp);
            _lerp += Time.deltaTime * _speed;
        }
        else
        {
            _oldPosition = _newPosition;
            _oldNormal = _newNormal;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_newPosition, 0.5f);
    }

    public bool IsMoving()
    {
        return _lerp < 1;
    }
}
