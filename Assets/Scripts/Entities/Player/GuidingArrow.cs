using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingArrow : MonoBehaviour
{
    [SerializeField] private GameObject _pointer;
    [SerializeField] private float _deactivateDistance;
    [SerializeField] private bool _disableOnceDeactivated;

    [Header("Debug")]
    [SerializeField] private Transform _debugTarget;
    [SerializeField] private bool _debugSetTarget;
    [SerializeField] private bool _debugReset;

    public void Reset()
    {
        _pointer.SetActive(false);
        _targetObject = null;
    }
    public void SetPointing(Transform target)
    {
        _targetObject = target;
    }
    public void Enable(Transform target = null)
    {
        if (target != null) SetPointing(target);
        if (_targetObject == null) return;
        _pointer.SetActive(true);
    }
    void Update()
    {
        if (_debugSetTarget && _debugTarget != null)
        {
            _debugSetTarget = false;
            Enable(_debugTarget);
        }
        if (_debugReset)
        {
            _debugReset = false;
            Reset();
        }

        if (_targetObject == null) Reset();
        else {
            UpdatePointerDirection(_targetObject.position);
            if (Vector3.SqrMagnitude(transform.position - _targetObject.position) <= _deactivateDistance * _deactivateDistance)
            {
                if (_disableOnceDeactivated) Reset();
                else _pointer.SetActive(false);
            } else if (!_pointer.activeSelf && !_disableOnceDeactivated)
                _pointer.SetActive(true);
        }

    }

    private Transform _targetObject;
    private Transform _originalTransform;
    void UpdatePointerDirection(Vector3 position)
    {
        Vector3 upwardPointing = (position - transform.position).normalized;
        Quaternion pointerRotation = Quaternion.LookRotation(Vector3.up, upwardPointing);
        transform.rotation = pointerRotation;
    }
    void Awake()
    {
        _originalTransform = transform;
    }

    public static GuidingArrow Instance { get; private set; }
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        _pointer.SetActive(false);
    }
}
