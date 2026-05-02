using Dynamite3D.RealIvy;
using UnityEngine;

public class VinesManager : MonoBehaviour
{
    [SerializeField] private IvyController[] ivyControllers;

    private Transform _playerTransform;
    private bool _vinesGenerated = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerTransform  = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
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
