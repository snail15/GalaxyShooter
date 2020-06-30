using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player.IsDead()) Destroy(this.gameObject);
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -6f)
            {
                transform.position = new Vector3(Random.Range(-8f, 8f), 7, 0);
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(this.gameObject);

            Player player = other.transform.GetComponent<Player>();
            if (player)
            {
                other.transform.GetComponent<Player>().Damage();
            }
            
        }

        if(other.tag == "Laser")
        {  
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    
}
