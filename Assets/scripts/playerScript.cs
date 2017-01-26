using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{
    [Header("Movement")]
    private float _mH;
    private float _speedWalk = 10;
    private float _speed = 10;
    private float _timeToMoving;
    public bool _moving;
    private Rigidbody2D _rb;

    [Header("Jump")]
    private float _jumpForce = 20;
    private float _recoilJump = 5;
    private float _delayJump;
    private float _maxDelayJump = 5;
    private float _maxVelFalling;
    [SerializeField] private float _limitVelFalling;
    private bool _doubleSpaceActivated;

    [Header("Shoot")]
    private float _delayShooting;
    private float _maxDelayShooting = 18;
    private int _maxShootCadence = 8;
    private bool _ammo;
    [SerializeField] private int _shootCadence = 8;
    public GameObject _prefabBullet;
    public List<GameObject> _bulletsGameObjects;

    [Header("Scripts")]
    public cameraShake _cameraShakeScript;

    [Header("Graphics")]
    private bool _facingRight;
    public Transform _graphicsTransform;
    public SpriteRenderer _graphicsColor;

    [Header("Checkers")]
    public Transform _groundChecker;
    public LayerMask _groundMask;
    public Vector2 _groundSize;
    public bool _isGrounded;

    public Transform _wallChecker;
    public LayerMask _wallMask;
    public Vector2 _wallSize;
    public bool _isTouchingWall;

    [Header("Animations")]
    public Animator _anim;
    public Animator _animSmoke;

    [Header("Distance & score")]
    private float _distance;
    private float _currentDistance;
    private int _distancePoints;
    public int _distanceCounter;
    public int _distanceLives;
    public float _score;
    [HideInInspector] public float _score2;
    public float _highScore;

    [Header("Health")]
    [SerializeField] private int _lives;
    [SerializeField] bool _damageAgain;
    private float _timeToDamage;
    private float _timeDamageFeedback;
    public List<GameObject> _livesGameObjects;
    private bool damageFeedback;

    [Header("Text")]
    public TextMesh _distanceText;
    public TextMesh _scoreText;
    public TextMesh _highScoreText;
    public GameObject _youDead;

    void Start ()
    {
        _rb = GetComponent<Rigidbody2D>();
        _facingRight = false;
        _doubleSpaceActivated = false;
        _moving = true;
        _ammo = false;
        damageFeedback = false;
        _distanceLives = 200;

        PlayerPrefs.GetFloat("scorePref");
        _highScore = PlayerPrefs.GetFloat("scorePref");
        _graphicsColor = GetComponentInChildren<SpriteRenderer>();
    }
	
	void Update ()
    {
        if (Input.GetKey("escape"))
        {
            PlayerPrefs.DeleteKey("scorePref");
            Application.Quit();

        }
        else
        {
            PlayerPrefs.SetFloat("scorePref", _highScore);
        }

        if(damageFeedback) {
            _timeDamageFeedback++;
            if(_timeDamageFeedback > 0) {
                _graphicsColor.color = new Color(255, 0, 0, 0);
            }
            if(_timeDamageFeedback > 5) {
                _graphicsColor.color = new Color(255, 0, 0, 255);
            }
            if(_timeDamageFeedback > 10) {
                _graphicsColor.color = new Color(255, 0, 0, 0);
            }
            if(_timeDamageFeedback > 15) {
                _graphicsColor.color = new Color(255, 0, 0, 255);
            }
            if(_timeDamageFeedback > 20) {
                _graphicsColor.color = new Color(255, 255, 255, 255);
                _timeDamageFeedback = 0;
                damageFeedback = false;
            }
        }

        ///max velocity falling
        if (_rb.velocity.y < _limitVelFalling) _maxVelFalling = _limitVelFalling;
        else _maxVelFalling = _rb.velocity.y;

        ///DISTANCE & SCORE LOGIC
        _distancePoints = (int)-_distance;
        _score = _distancePoints / 4 + _score2;
        if (_highScore <= _score) _highScore = _score;

        ///SHOW ON CANVAS
        _distanceText.text = "" + _distancePoints;
        _scoreText.text = "" + _score;
        _highScoreText.text = "" + _highScore;

        ///GET 1 LIVE EACH 500 METERS (DISTANCE)
        _distanceCounter = _distancePoints;
        if (_distanceCounter >= _distanceLives)
        {
            GetLive();
            _distanceLives += 200;
        }

        ///MOVEMENT LOGIC & ANIMATION
        _mH = Input.GetAxisRaw("Horizontal");
        if(_moving) _rb.velocity = new Vector2(_mH * _speed, _maxVelFalling);

        if (_mH != 0) _anim.SetBool("run", true);
        else _anim.SetBool("run", false);

        ///CHECK PLAYER ALIVE
        if (_graphicsTransform.gameObject.activeSelf == true) SetSpeed();
        else
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }

        ///CHECK IS MOVING
        if (!_moving)
        {
            _timeToMoving++;
            if (_timeToMoving >= 10)
            {
                _timeToMoving = 0;
                _moving = true;
            }
        }

        ///CHECK PLAYER RECEIVE DAMAGE AGAIN
        if (!_damageAgain)
        {
            _timeToDamage++;
            if (_timeToDamage >= 50)
            {
                _timeToDamage = 0;
                _damageAgain = true;
            }
        }

        ///CHECK PLAYER DEATH
        if(_lives == 0)
        {
            Death();
        }

        if(_ammo)
        {
            foreach(GameObject obj in _bulletsGameObjects)
            {
                obj.SetActive(true);
            }
        }

        ///MAX AMMO
        if (_shootCadence >= _maxShootCadence) _shootCadence = _maxShootCadence;

        ///CHECK IS GROUNDED
        if (_isGrounded)
        {
            ///DETECT FIRST SPACE & JUMP
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                _delayJump++;
                if (_delayJump >= _maxDelayJump)
                {
                    _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
                    _delayJump = 0;
                    _anim.SetTrigger("jump");
                }
            }
            _anim.SetBool("fall", false);
        }
        else
        {
            if (_distance > transform.position.y) _distance = transform.position.y;

            ///DETECT SPACE AGAIN TO SHOOT
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                _doubleSpaceActivated = true;
                _ammo = false;
                _delayShooting = _maxDelayShooting;
            }

            ///SHOOT
            if (Input.GetKey(KeyCode.Space) && _shootCadence > 0 && _doubleSpaceActivated)
            {
                _delayShooting++;
                if(_delayShooting >= _maxDelayShooting)
                {
                    _anim.SetTrigger("shot");
                    _shootCadence--;
                    _bulletsGameObjects[_shootCadence].SetActive(false);

                    _rb.velocity = new Vector2(_rb.velocity.x, 0);
                    _rb.AddForce(new Vector2(0, _recoilJump), ForceMode2D.Impulse);

                    _cameraShakeScript.CameraShake();

                    GameObject bullet02 = (GameObject)Instantiate(_prefabBullet);
                    bullet02.transform.position = transform.position;

                    _delayShooting = 0;
                }
            }
            if(Input.GetMouseButton(0) && _shootCadence > 0 && _doubleSpaceActivated) {
                _delayShooting++;
                if(_delayShooting >= _maxDelayShooting) {
                    _anim.SetTrigger("shot");
                    _shootCadence--;
                    _bulletsGameObjects[_shootCadence].SetActive(false);

                    _rb.velocity = new Vector2(_rb.velocity.x, 0);
                    _rb.AddForce(new Vector2(0, _recoilJump), ForceMode2D.Impulse);

                    _cameraShakeScript.CameraShake();

                    GameObject bullet02 = (GameObject)Instantiate(_prefabBullet);
                    bullet02.transform.position = transform.position;

                    _delayShooting = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) && _shootCadence > 0 || Input.GetMouseButtonDown(0))
            {
                _delayShooting++;
                if (_delayShooting >= _maxDelayShooting)
                {
                    _anim.SetTrigger("shot");
                    _shootCadence--;
                    _bulletsGameObjects[_shootCadence].SetActive(false);

                    _rb.velocity = new Vector2(_rb.velocity.x, 0);
                    _rb.AddForce(new Vector2(0, _recoilJump), ForceMode2D.Impulse);

                    _cameraShakeScript.CameraShake();

                    GameObject bullet02 = (GameObject)Instantiate(_prefabBullet);
                    bullet02.transform.position = transform.position;

                    _delayShooting = 0;
                }
            }
            
            _anim.SetBool("fall", true);
        }

        if (_facingRight && _mH < 0) Flip();
        else if (!_facingRight && _mH > 0) Flip();
    }

    void FixedUpdate()
    {
        Collider2D hitCollider;
        Vector2 pointA;
        Vector2 pointB;

        ///CHECKGROUND
        pointA = new Vector2(_groundChecker.position.x - _groundSize.x / 2, _groundChecker.position.y - _groundSize.y / 2);
        pointB = new Vector2(_groundChecker.position.x + _groundSize.x / 2, _groundChecker.position.y + _groundSize.y / 2);
        hitCollider = Physics2D.OverlapArea(pointA, pointB, _groundMask);

        if (hitCollider != null) _isGrounded = true;
        else _isGrounded = false;

        ///CHECKWALL
        pointA = new Vector2(_wallChecker.position.x - _wallSize.x / 2, _wallChecker.position.y - _wallSize.y / 2);
        pointB = new Vector2(_wallChecker.position.x + _wallSize.x / 2, _wallChecker.position.y + _wallSize.y / 2);
        hitCollider = Physics2D.OverlapArea(pointA, pointB, _wallMask);

        if (hitCollider != null) _isTouchingWall = true;
        else _isTouchingWall = false;
    }

    void GetLive()
    {
        if (_lives <= 5)
        {
            _lives++;
            _livesGameObjects[_lives - 1].SetActive(true);
        }

    }

    void LoseLive()
    {
        damageFeedback = true;

        _lives--;
        _livesGameObjects[_lives].SetActive(false);
    }

    void GetAmmo()
    {
        _shootCadence = _maxShootCadence;
        _ammo = true;
        _animSmoke.SetTrigger("smoke");
    }

    void Flip()
    {
        Vector3 newScale = _graphicsTransform.localScale;
        newScale.x *= -1;
        _graphicsTransform.localScale = newScale;
        _facingRight = !_facingRight;
    }

    void SetSpeed()
    {
        if (_isTouchingWall) _speed = 0;
        else _speed = _speedWalk;
    }

    void Death()
    {
        _youDead.SetActive(true);
        _graphicsTransform.gameObject.SetActive(false);
        _rb.isKinematic = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundChecker.position, _groundSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_wallChecker.position, _wallSize);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "ground" || other.gameObject.tag == "destroyable_ground")
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            if (!_isGrounded)GetAmmo();
            _doubleSpaceActivated = false;
        }
        if (other.gameObject.tag == "enemy_ghost")
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            _anim.SetBool("fall", false);
            _anim.SetTrigger("jump");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy_spike" || other.gameObject.tag == "enemy")
        {
            if (_damageAgain)
            {
                LoseLive();
                if (!_facingRight) _rb.AddForce(new Vector2(20, 0), ForceMode2D.Impulse);
                else _rb.AddForce(new Vector2(-20, 0), ForceMode2D.Impulse);
                _moving = false;
                _damageAgain = false;
            }
        }
    }
    /*
    void OnGUI()
    {
        // Calculate player transform screen position
        Vector3 _position = Camera.main.WorldToScreenPoint(transform.position);
        Rect _rect = new Rect(_position.x - 50, _position.y - 200, 120, 20);

        // Create Text
        string _text = "";
        _text += "Player.Shoot: " + _shootCadence + "\n";

        GUI.TextArea(_rect, _text);//TextArea: rectangle button. Label: only text.
    }
    */
}
