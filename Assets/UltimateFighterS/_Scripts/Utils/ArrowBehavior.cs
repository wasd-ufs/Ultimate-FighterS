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
        if(_playerIdentifier== 0){_arrowColor = Color.red;}
        if(_playerIdentifier== 1){_arrowColor = Color.blue;}
        if(_playerIdentifier== 2){_arrowColor = Color.yellow;}
        if(_playerIdentifier== 3){_arrowColor = Color.green;}
        
        _mySpriteRenderer.sprite = _spriteList[1];
        _mySpriteRenderer.color = _arrowColor;
        var fatherSprite = GetComponentInChildren<SpriteRenderer>();
        var spriteLength = fatherSprite.bounds.size.y;
        gameObject.transform.position = new Vector3(transform.position.x, (transform.position.y + spriteLength + 0.2f) , transform.position.z);
    
    }
    /// <summary>
    /// Atuliza a posição da sete (mantendo-a acima do player) e altera o seu sprite quando necessario.
    /// </summary>
    /// <author>JOÃO CARLOS</author>
    void Update()
    {   
        float distanceDiference = (transform.position - _camera.transform.position).z;
        _leftCameraPosition = _camera.ViewportToWorldPoint(new Vector3(0f, 0f, distanceDiference)).x;
        _rightCameraPosition = _camera.ViewportToWorldPoint(new Vector3(1f, 0f, distanceDiference)).x;
        
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
