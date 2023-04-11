using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDir : MonoBehaviour
{
    public float worldHeighthalf = 33.8f;
    public float worldWeighthalf = 54;
    public Vector2 worldCentre = new Vector2(30,7.4f);
    private SpriteRenderer _playerRenderer;
    private void Awake()
    {
        _playerRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeShadow());
    }
    

    IEnumerator ChangeShadow()
    {
        while (true)
        {
            _playerRenderer.material.SetFloat("_xOffSet", 2.5f*(transform.position.x - worldCentre.x) / worldWeighthalf);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
