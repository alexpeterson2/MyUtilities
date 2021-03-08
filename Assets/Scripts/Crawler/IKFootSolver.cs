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

    private Ray _ray;

    float _footSpacing;
    public Vector3 OldPosition { get; private set; }
    public Vector3 CurrentPosition { get; private set; }
    public Vector3 NewPosition { get; private set; }

    Vector3 _oldNormal, _currentNormal, _newNormal;
    float _lerp;

    void Start()
    {
        _footSpacing = transform.localPosition.x;
        CurrentPosition = NewPosition = OldPosition = transform.position;
        _currentNormal = _newNormal = _oldNormal = transform.up;
        _lerp = 1;
    }

    void Update()
    {
        transform.position = CurrentPosition;
        transform.up = _currentNormal;

        CheckFootPosition();
        MoveFeet();
    }

    private void CheckFootPosition()
    {
        var ray = new Ray(_body.position + (_body.right * _footSpacing), Vector3.down);
        _ray = ray;

        if (Physics.Raycast(ray, out RaycastHit info, 10, _terrainLayer.value))
        {
            if (Vector3.Distance(NewPosition, info.point) > _stepDistance && !_otherFoot.IsMoving() && _lerp >= 1)
            {
                _lerp = 0;
                int direction = _body.InverseTransformPoint(info.point).z > _body.InverseTransformPoint(NewPosition).z ? 1 : -1;
                NewPosition = info.point + (_body.forward * _stepLength * direction) + footOffset;
                _newNormal = info.normal;
            }
        }
    }

    private void MoveFeet()
    {
        if (_lerp < 1)
        {
            var tempPosition = Vector3.Lerp(OldPosition, NewPosition, _lerp);
            tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;

            CurrentPosition = tempPosition;
            _currentNormal = Vector3.Lerp(_oldNormal, _newNormal, _lerp);
            _lerp += Time.deltaTime * _speed;
        }
        else
        {
            OldPosition = NewPosition;
            _oldNormal = _newNormal;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(NewPosition, 0.5f);
        Gizmos.DrawRay(_ray);
    }

    public bool IsMoving()
    {
        return _lerp < 1;
    }
}
