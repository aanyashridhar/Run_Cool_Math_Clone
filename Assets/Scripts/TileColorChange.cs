using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorChange : MonoBehaviour
{
    public Material material;
    public Color[] colors;

    private int currentColorIndex;
    public int CurrentColorIndex {
        get {return currentColorIndex;}
        set {currentColorIndex = value;}
    }

    private int targetColorIndex;
    public int TargetColorIndex {
        get {return targetColorIndex;}
        set {targetColorIndex = value;}
    }

    private float targetPoint;
    public float TargetPoint {
        get {return targetPoint;}
        set {targetPoint = value;}
    }

    private float duration;
    public float Duration {
        get {return duration;}
        set {duration = value;}
    }

    private bool transition;
    public bool Transition {
        get {return transition;}
        set {transition = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        material = new Material(material);
        GetComponent<Renderer>().material = material;

        Duration = 2;
        Transition = false;
        material.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        if (Transition) {
            TargetPoint += Time.deltaTime / Duration;
            Color color = Color.Lerp(colors[CurrentColorIndex], colors[TargetColorIndex], TargetPoint);
            material.color = color;
            material.SetColor("_EmissionColor", color);

            if (TargetPoint >= 1f) {
                Transition = false;
            }
        }
    }

    public void ColorChange(int index) {
        CurrentColorIndex = index;
        TargetColorIndex = index + 1;
        if (TargetColorIndex == colors.Length) {
            TargetColorIndex = 0;
        }

        Transition = true;
    }

    public void SetColor(int index)
    {
        CurrentColorIndex = index;
        TargetColorIndex = index + 1;
        if (TargetColorIndex == colors.Length) {
            TargetColorIndex = 0;
        }

        Color c = colors[CurrentColorIndex];
        material.color = c;
        material.SetColor("_EmissionColor", c);
    }
}
