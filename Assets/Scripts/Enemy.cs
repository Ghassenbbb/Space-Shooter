using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    // Start is called before the first frame update

    private Player _player;

    private Animator _enemyAnimator;
    private AudioSource _audioSource;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if ( _player == null )
        {
            Debug.LogError("Player is Null!");
        }

        if (_enemyAnimator == null)
        {
            Debug.LogError("Enemy Animator is Null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed  * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            transform.position = new Vector3(Random.Range(-9.2f, 9.2f), 8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.transform.tag == "Dead")
            return;

        if (other.tag  == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null )
            {
                player.Damage();
            }

            _speed = 0;
            this.gameObject.transform.tag = "Dead";
            _enemyAnimator.SetTrigger("OnEnemyDeath");

            _audioSource.Play();

            Destroy(this.gameObject, 2.8f);

        }


        if (other.tag == "Laser")
        {
            _speed = 0;
            Destroy(other.gameObject);
            this.gameObject.transform.tag = "Dead";
            if (_player != null) _player.AddScore(5);
            _enemyAnimator.SetTrigger("OnEnemyDeath");

            _audioSource.Play();

            Destroy(this.gameObject, 2.8f);
        }

    }
}
