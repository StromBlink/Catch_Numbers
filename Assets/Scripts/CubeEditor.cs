using UnityEngine;
using System.Linq;

using System.Collections.Generic;
using TMPro;
using static Utilites;

public class CubeEditor : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;

    int counter;

    public void Start()
    {
        if (TryGetComponent<CollisionManager>(out CollisionManager collisionManager))
            MaterialClour();
        else
            meshRenderer.material.color = Color.gray;
        Catch();
    }
    public void Catch()
    {
        LevelScale();
        FindGameObjectInChildWithTag(gameObject, "CubeLevelText").GetComponent<TMP_Text>().text = gameObject.tag;
    }
    public void MaterialClour()
    {
        /* float randomnumber = Random.Range(0, 4);
        switch (randomnumber)
        {
            case 0: meshRenderer.material = material_1; break;
            case 1: meshRenderer.material = material_2; break;
            case 2: meshRenderer.material = material_3; break;
            case 3: meshRenderer.material = material_4; break;
        } */
        /* switch (gameObject.tag)
        {
            case "2": case "32": case "512": case "8192": case "128k": meshRenderer.material = material_1; break;
            case "4": case "64": case "1024": case "16k": case "256k": meshRenderer.material = material_2; break;
            case "8": case "128": case "2048": case "32k": case "512k": meshRenderer.material = material_3; break;
            case "16": case "256": case "4096": case "64k": case "1024k": meshRenderer.material = material_4; break;

        } */
    }
    public void LevelScale()
    {
        int tag = _CollisionManager.ToInt(transform);
        int number = 1;
        for (int i = 0; i < 20; i++)
        {
            number = number + number;
            if (number == tag)
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * (1f + i * .2f);
        }
        int number2 = 1;
        for (int i = 0; i < 20; i++)
        {
            number2 = number2 + number2;
            if (number2 == tag)
            {
                Color.RGBToHSV(meshRenderer.material.color, out float H, out float S, out float V);
                Color newcolor = Color.HSVToRGB(H, S, (V * (1f - i * .02f)));
                meshRenderer.material.color = newcolor;
                meshRenderer.material.SetColor("Shadow Color", newcolor);
            }

        }
    }
    public GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == tag)
                return t.GetChild(i).gameObject;

            counter++;
            if (counter < 3) FindGameObjectInChildWithTag(t.GetChild(i).gameObject, tag);
            return FindGameObjectInChildWithTag(t.GetChild(i).gameObject, tag);
        }
        counter = 0;
        return null;
    }


}
