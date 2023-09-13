using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Power Up IDs
    // 0 : Triple Shot 
    // 1 : Speed
    // 2 : Shield
    [SerializeField]
    private int _powerupId;

    [SerializeField]
    private AudioClip _audioClip;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.3)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null )
            {
                if (_powerupId == 0) {
                    player.TripleShotActive();
                }else if (_powerupId == 1) {
                    player.SpeedActive();
                }else
                {
                    player.ShieldActive();
                }
            }


            Destroy(this.gameObject);
        }
    }
}
