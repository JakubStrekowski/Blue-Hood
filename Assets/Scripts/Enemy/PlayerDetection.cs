using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private readonly int PLAYER_LAYER_ID = 6;

    [SerializeField]
    Collider2D _vision;

    Enemybehaviour _myBehaviour;
    LayerMask _playerLayerMask;


    private void Awake()
    {
        _myBehaviour = GetComponent<Enemybehaviour>();

        _playerLayerMask = LayerMask.GetMask(LayerMask.LayerToName(PLAYER_LAYER_ID));
    }

    private void Update()
    {
        if (_vision.IsTouchingLayers(_playerLayerMask))
        {
            _myBehaviour.playerInSight = true;
        }
        else
        {
            _myBehaviour.playerInSight = false;
        }
    }

}
