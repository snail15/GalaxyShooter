using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    [SerializeField] private float _spawnSpeed = 1.5f;
    [SerializeField] private float _triplePowerSpawnSpeed = 8f;
    [SerializeField] private float _speedPowerSpawnSpeed = 10f;
    [SerializeField] private float _shieldPowerSpawnSpeed = 9f;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _tripleShotPowerUp;
    [SerializeField] private GameObject _speedShotPowerUp;
    [SerializeField] private GameObject _shieldShotPowerUp;

    private bool _isPlayerDead;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine(_spawnSpeed, _enemy));
        StartCoroutine(SpawnRoutine(_triplePowerSpawnSpeed, _tripleShotPowerUp, 5f));
        StartCoroutine(SpawnRoutine(_speedPowerSpawnSpeed, _speedShotPowerUp, 6f));
        StartCoroutine(SpawnRoutine(_shieldPowerSpawnSpeed, _shieldShotPowerUp, 7f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnRoutine(float duration, GameObject spawnObject, float initialWait = 0f)
    {
        yield return new WaitForSeconds(initialWait);
        while (!_isPlayerDead)
        {
            Vector3 postToSpwan = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(spawnObject, postToSpwan, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(duration);
        }
    }

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}
