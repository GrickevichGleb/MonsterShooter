using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private TMP_Text levelPlate = null;
    [Space] 
    [SerializeField] private int startHealth;
    [SerializeField] private float startSpeed;
    [SerializeField] private float startWaitDelay;
    [SerializeField] private float statsBoostPerLevel;
    
    public Action<GameObject> OnMonsterDeath;

    private NavMeshAgent _nvAgent;
    
    //Start parameters
    private int _level = 1;
    
    public int _health = 1;
    public float _changeDestDelay;

    private bool _isWaiting;
    private void Awake()
    {
        _nvAgent = GetComponent<NavMeshAgent>();
    }
    

    void Update()
    {
        if (_health == 0)
        {
            OnMonsterDeath?.Invoke(gameObject);
            return;
        }
        
        if (IsArrived())
            StartCoroutine(MoveToNewDestination(_changeDestDelay));

        if (_nvAgent.remainingDistance > _nvAgent.stoppingDistance) return;
        _nvAgent.ResetPath();
    }
    
    
    public void TakeDamage(int damage)
    {
        _health = Mathf.Max(0, _health - damage);
        Debug.Log(gameObject.name + "got " + damage + " damage");
    }


    public void SetMonsterLevel(int level)
    {
        _health = startHealth;
        _nvAgent.speed = startSpeed;
        _changeDestDelay = startWaitDelay;

        for (int i = _level; i < level; i++)
        {
            _health += Mathf.RoundToInt(startHealth * statsBoostPerLevel);
            _nvAgent.speed += startSpeed * statsBoostPerLevel;
            _changeDestDelay -= startWaitDelay * statsBoostPerLevel;
        }
        
        levelPlate.text = level.ToString();
    }


    private void MoveToRandPoint()
    {
        _nvAgent.destination = GameController.instance.GetRandFieldPos();
    }

    private bool IsArrived()
    {
        if (_isWaiting)
            return false;
        
        if(!_nvAgent.hasPath)
            return true;
        if (_nvAgent.remainingDistance <= 0.1f && !_nvAgent.pathPending)
            return true;
        
        return false;
    }


    private IEnumerator MoveToNewDestination(float sec)
    {
        _isWaiting = true;
        
        yield return new WaitForSeconds(sec);
        MoveToRandPoint();

        _isWaiting = false;
    }
}
