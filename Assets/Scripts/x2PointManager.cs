using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class x2PointManager : MonoBehaviour
{
    public static x2PointManager instance;
    public GameObject[] x2_Colors;
    public int generateChance, randomIndex;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    // Start is called before the first frame update
    void Start()
    {
        randomIndex = -1;
    }

    public void x2Generate()
    {
        generateChance = Random.Range(0, x2_Colors.Length);
        if (generateChance != 0)
            return;
        randomIndex = Random.Range(0, x2_Colors.Length);
        if (x2_Colors[randomIndex].activeSelf)
        {
            randomIndex = -1;
            return;
        }
        x2_Colors[randomIndex].SetActive(true);
    }

    public void x2Disable(int idx)
    {
        x2_Colors[idx].SetActive(false);
    }

    public bool isX2Active(int idx)
    {
        return x2_Colors[idx].activeSelf;
    }
}
