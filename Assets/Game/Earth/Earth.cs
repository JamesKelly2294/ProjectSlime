using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    [Range(1.0f, 20.0f)]
    public float Radius;

    public Texture[] SeaLevelSprites;

    private GameObject _visuals;
    private GameResourceManager _rm;
    private int _cachedSeaLevel;

    // Start is called before the first frame update
    void Start()
    {
        _visuals = transform.Find("Visuals").gameObject;
        _visuals.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);

        _rm = FindObjectOfType<GameResourceManager>();

        _cachedSeaLevel = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rm != null && _rm.currentSeaLevels != _cachedSeaLevel)
        {
            _cachedSeaLevel = _rm.currentSeaLevels;
            var range = (_rm.maxSeaLevels - _rm.minSeaLevels);
            var increment = range / SeaLevelSprites.Length;
            var index = Mathf.Min(SeaLevelSprites.Length - 1, Mathf.Max(Mathf.RoundToInt(_cachedSeaLevel / increment), 0));
            _visuals.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", SeaLevelSprites[index]);
        }
    }
}
