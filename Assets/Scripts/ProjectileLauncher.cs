using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileLauncher : MonoBehaviour
{
    public float fireRate = 1f;
    public int damage = 50;
    [Space]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject barrel;
    
    private Camera _mainCamera;
    private Transform _mainCamTransform;

    private float _lastFireTime;
    
    private Controls _controls;
 
    void Start()
    {
        _mainCamera = Camera.main;
        _mainCamTransform = _mainCamera.transform;
        
        _controls = new Controls();
        _controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        AimingAndFiring();
    }
    
    
    private void AimingAndFiring()
    {
        
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);
            
        Vector3 fireDir = ray.direction;
         
        transform.rotation = Quaternion.LookRotation(fireDir, Vector3.up);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (Time.time > (1 / fireRate) + _lastFireTime)
            {
                GameObject projectileInstance = 
                    Instantiate(projectile, transform.position, Quaternion.LookRotation(fireDir, Vector3.up));
                projectileInstance.GetComponent<Projectile>().damage = damage;
                _lastFireTime = Time.time;
            }
        }
    }

    
    
}
