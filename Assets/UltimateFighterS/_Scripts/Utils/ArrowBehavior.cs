using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Controla o movimento da seta que fica acima do jogador e sua coloração.
///</summary>
public class ArrowBehavior : MonoBehaviour
{
    private int _playerIdentifier;
    private float _fatherPositionX;
    private float _leftCameraPosition;
    private float _rightCameraPosition;
    private Color _arrowColor = Color.white;
    [SerializeField] private IdComponent _idComponent;
    [SerializeField] private List<Sprite> _spriteList;
    [SerializeField] private SpriteRenderer _mySpriteRenderer;
    private Camera _camera;
    
    /// <summary>
    /// Pega o componente Camera na cena. 
    /// </summary>
    /// <author>Cubidev</author>
    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    
    /// <summary>
    /// Define a cor da seta correspodente ao playerIdentifier, o sprite inicial da seta, e coloca ela na posição relativa ao player. 
    /// </summary>
    /// <author>JOÃO CARLOS</author>
    void Start()
    {
        _playerIdentifier= _idComponent.id;
        Dictionary<int, Color> _playerColors = new Dictionary<int, Color>
        {
            { 0, Color.red },
            { 1, Color.blue },
            { 2, Color.yellow },
            { 3, Color.green }
        };
        _arrowColor = _playerColors.TryGetValue(_playerIdentifier, out Color color) ? color : _arrowColor;
        
        _mySpriteRenderer.sprite = _spriteList[1];
        _mySpriteRenderer.color = _arrowColor;
        SpriteRenderer _fatherSprite = GetComponentInChildren<SpriteRenderer>();
        float _fatherSpriteLength = _fatherSprite.bounds.size.y;
        gameObject.transform.position = new Vector3(transform.position.x, (transform.position.y + _fatherSpriteLength + 0.2f) , transform.position.z);
    
    }
    
    /// <summary>
    /// Atuliza a posição da sete (mantendo-a acima do player) e altera o seu sprite quando necessario.
    /// </summary>
    /// <author>JOÃO CARLOS</author>
    void Update()
    {   
        float _distanceDiference = (transform.position - _camera.transform.position).z;
        _leftCameraPosition = _camera.ViewportToWorldPoint(new Vector3(0f, 0f, _distanceDiference)).x;
        _rightCameraPosition = _camera.ViewportToWorldPoint(new Vector3(1f, 0f, _distanceDiference)).x;
        
        _fatherPositionX = transform.parent.position.x;

        if (_fatherPositionX <= _leftCameraPosition)
        {
            _mySpriteRenderer.sprite = _spriteList[_playerIdentifier * 3];
            gameObject.transform.position = new Vector3(_leftCameraPosition + 0.5f, gameObject.transform.position.y , gameObject.transform.position.z);
        }
        else if(_fatherPositionX >= _rightCameraPosition)
        {
            _mySpriteRenderer.sprite = _spriteList[2 + _playerIdentifier * 3];
            gameObject.transform.position = new Vector3(_rightCameraPosition - 0.5f, gameObject.transform.position.y , gameObject.transform.position.z);
        }
        else
        {
            _mySpriteRenderer.sprite = _spriteList[1 + _playerIdentifier * 3];
            gameObject.transform.position = new Vector3(_fatherPositionX, gameObject.transform.position.y , gameObject.transform.position.z);
        }
        
    }
}
