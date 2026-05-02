using System;
using Dynamite3D.RealIvy;
using UnityEngine;

public class VinesManager : MonoBehaviour
{
    [SerializeField] private IvyController[] ivyControllers;

    private Transform _playerTransform;
    private bool _vinesGenerated = false;
    
    private RoomGenerator _generator;

    private void Start()
    {
         _generator = FindAnyObjectByType<RoomGenerator>();
    }

    public void AssignPlayer()
    {
        _playerTransform  = GameObject.FindAnyObjectByType<PlayerMovement>(FindObjectsInactive.Include).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerTransform != null)
            GenerateVines();
    }

    private void GenerateVines()
    {
        if (_vinesGenerated == false)
        {
            var distance = Vector3.Distance(transform.position,_playerTransform.position );
            if (distance <= 15f)
            {
                foreach (var ivy in ivyControllers)
                {
                    if(ivy.gameObject .activeSelf)
                     ivy.StartGrowth();
                }
                _vinesGenerated = true;
            }
        }
    }
}
