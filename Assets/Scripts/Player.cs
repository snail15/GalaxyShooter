using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed;
    private float _boostedSpeed = 10.5f;
    [SerializeField] private float _defaultSpeed = 5.5f;
    [SerializeField] private GameObject _laser;
    [SerializeField] private GameObject _tripleShots;
    [SerializeField] private GameObject _shieldVisual;
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;

    private bool _isDead;
    [SerializeField] private bool _isTripleShotEnabled;
    [SerializeField] private float _tripleShotDuration = 5.0f;

    [SerializeField] private bool _isSpeedPowerUpEnabled;
    [SerializeField] private float _speedPowerUpDuration = 5.5f;

    [SerializeField] private bool _isShieldPowerUpEnabled;
    [SerializeField] private float _shieldPowerUpDuration = 6.0f;

    private SpawnManager _spawnManager;
    
    // Start is called before the first frame update
    void Start()
    {
        SwitchShieldVisual(_isShieldPowerUpEnabled);        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if(!_spawnManager) Debug.LogError("SpawnManager is null");
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovements();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) Fire();
      
    }

    private void CalculateMovements()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _speed = _isSpeedPowerUpEnabled ? _boostedSpeed : _defaultSpeed; 

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5f, 0), 0);

        if (transform.position.x > 11.3f || transform.position.x < -11.3f)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
    }

    private void Fire()
    {
        _canFire = Time.time + _fireRate;
        float offSet = _isTripleShotEnabled ? 0.1f : 1.2f;
        Instantiate(_isTripleShotEnabled ? _tripleShots : _laser, transform.position + new Vector3(0, offSet, 0),
            Quaternion.identity);
    }

    public void Damage()
    {
        if (_isShieldPowerUpEnabled)
        {
            _isShieldPowerUpEnabled = false;
            SwitchShieldVisual(false);
        }
        else
        {
            _lives -= 1;
            if (_lives < 1)
            {
                _isDead = true;
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
       
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void EnablePowerUp(Utils.PowerUpTypes powerUpType)
    {
        SwitchOn(powerUpType);
        StartCoroutine(ActivatePowerUp(powerUpType));
    }

    private void SwitchOn(Utils.PowerUpTypes powerUpType)
    {
        if (powerUpType == Utils.PowerUpTypes.TripleShot) _isTripleShotEnabled = true;
        else if (powerUpType == Utils.PowerUpTypes.Speed) _isSpeedPowerUpEnabled = true;
        else if (powerUpType == Utils.PowerUpTypes.Shields)
        {
            _isShieldPowerUpEnabled = true;
            SwitchShieldVisual(true);
        }
    }

    IEnumerator ActivatePowerUp(Utils.PowerUpTypes powerUpType)
    {
        float duration = GetPowerUpDuration(powerUpType);
        yield return new WaitForSeconds(duration);
        SwitchOff(powerUpType);
    }

    private void SwitchShieldVisual(bool makeVisible)
    {
        _shieldVisual.SetActive(makeVisible);
    }

    private void SwitchOff(Utils.PowerUpTypes powerUpType)
    {
        if (powerUpType == Utils.PowerUpTypes.TripleShot) _isTripleShotEnabled = false;
        else if (powerUpType == Utils.PowerUpTypes.Speed) _isSpeedPowerUpEnabled = false;
        else if (powerUpType == Utils.PowerUpTypes.Shields)
        {
            _isShieldPowerUpEnabled = false;
            SwitchShieldVisual(false);
        }
    }

    private float GetPowerUpDuration(Utils.PowerUpTypes powerUpType)
    {
        float duration = 0f;

        switch (powerUpType)
        {
            case Utils.PowerUpTypes.TripleShot:
                duration = _tripleShotDuration;
                break;
            case Utils.PowerUpTypes.Speed:
                duration = _speedPowerUpDuration;
                break;
            case Utils.PowerUpTypes.Shields:
                duration = _shieldPowerUpDuration;
                break;
        }

        return duration;
    }
}
