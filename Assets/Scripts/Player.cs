using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.0f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _ShieldPrefab;
    [SerializeField]
    private GameObject _rightEngineDamage;
    [SerializeField]
    private GameObject _leftEngineDamage;
    [SerializeField]
    private GameObject _tripleShotlaserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private int _score = 0;

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _laserSoundClip;

    private UIManager _uiManager;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3 (0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _ShieldPrefab.SetActive(false);
        _rightEngineDamage.SetActive(false);
        _leftEngineDamage.SetActive(false);

        if (_spawnManager == null )
        {
            Debug.LogError("The spawn Manager is Null!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is Null!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Player Audio source is Null!");
        }else
        {
            _audioSource.clip = _laserSoundClip;
        }


        _uiManager.UpdateScore(_score);
        _uiManager.UpdateLives(_lives);

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, VerticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);


        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.2f, 9.2f), Mathf.Clamp(transform.position.y, -4, 6), 0);
    }

    void FireLaser()
    {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive) {
                Vector3 laserSpawnPosition = transform.position + new Vector3(-0.5f, 0, 0);
                Instantiate(_tripleShotlaserPrefab, laserSpawnPosition, Quaternion.identity);
            } else
            {
                Vector3 laserSpawnPosition = transform.position + new Vector3(0, 0.8f, 0);
                Instantiate(_laserPrefab, laserSpawnPosition, Quaternion.identity);
            }

            _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _ShieldPrefab.SetActive(false);
            _isShieldActive = false;
            return;
        }
        
        _lives--;

        _uiManager.UpdateLives(_lives);

        if (_lives == 2 )
        {
            _rightEngineDamage.SetActive(true);

        }else if ( _lives == 1 )
        {

            _leftEngineDamage.SetActive(true);

        }else if ( _lives < 1)
        {

            _spawnManager.onPlayerDeath();
            Destroy(this.gameObject);

        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        _isSpeedBoostActive = false;
    }


    public void ShieldActive()
    {
        _isShieldActive = true;

        _ShieldPrefab.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
