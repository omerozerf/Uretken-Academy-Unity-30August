using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiCreate : MonoBehaviour
{
    [SerializeField] private Transform[] _aiCreateTransformArray;
    [SerializeField] private Ai _ai;
    [SerializeField] private float _delay;


    private void Start()
    {
        StartCoroutine(CreateCoroutine());
        StartCoroutine(ChangeDelay());
    }


    private IEnumerator ChangeDelay()
    {
        while (true)
        {
            _delay = _delay - (_delay / 10);

            yield return new WaitForSeconds(3f);
        }
    }


    private IEnumerator CreateCoroutine()
    {
        while (true)
        {
            Create();

            yield return new WaitForSeconds(_delay);
        }
    }

    private void Create()
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
