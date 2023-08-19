using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiCreate : MonoBehaviour
{
    [SerializeField] private Transform[] _aiCreateTransformArray;
    [SerializeField] private Ai _ai;


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var randomIndex = Random.Range(0, _aiCreateTransformArray.Length);
            
            var ai = Instantiate(_ai, _aiCreateTransformArray[randomIndex].position, Quaternion.identity);
            int dir;
            
            if (randomIndex == 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            
            ai.SetDirection(dir);
            ai.ReverseLocalScaleX(dir);
        }
    }
}
