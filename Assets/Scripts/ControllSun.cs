using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllSun : MonoBehaviour {

  //Planet to controll parameters of
  public GameObject sun;

  //Renderer of the sun
  private Renderer render;

  //Values that can be changed in the shader
  /*************************************/
  [Range(0.0f, 0.2f)]
  public float amount = 0.01f;

  [Range(5.0f, 20.0f)]
  public float freq = 10.0f;

  public Color baseColor = Color.yellow;
  public Color spotsColor = Color.red;

  [Range(0.0f, 5.0f)]
  public float colorAmount = 1.0f;

  [Range(5.0f, 100.0f)]
  public float colorFreq = 10.0f;

  [Range(0.0f, 1.0f)]
  public float baseColorRatio = 0.5f;

  public Color emmision = Color.white;

  [Range(0.0f, 1.0f)]
  public float emmisionAmount = 0.9f;

  [Range(0.0f, 1.0f)]
  public float gasSpeed = 0.1f;
  /**************************************/

  // Use this for initialization
  void Start () {

    if (!sun) return;

    render = sun.GetComponent<Renderer>();
    render.material.shader = Shader.Find("Custom/Sun");

    amount = render.material.GetFloat("_Amount");
    freq = render.material.GetFloat("_Freq");
    baseColor = render.material.GetColor("_Color1");
    spotsColor = render.material.GetColor("_Color2");
    colorAmount = render.material.GetFloat("_ColorAmount");
    colorFreq = render.material.GetFloat("_ColorFreq");
    baseColorRatio = render.material.GetFloat("_Color1Ratio");
    emmision = render.material.GetColor("_ColorEmmision");
    emmisionAmount = render.material.GetFloat("_EmmisionAmount");
    gasSpeed = render.material.GetFloat("_Speed");
  }
	
	// Update is called once per frame
	void Update () {

    if (!sun) return;

    render.material.SetFloat("_Amount", amount);
    render.material.SetFloat("_Freq", freq);
    render.material.SetColor("_Color1", baseColor);
    render.material.SetColor("_Color2", spotsColor);
    render.material.SetFloat("_ColorAmount", colorAmount);
    render.material.SetFloat("_ColorFreq", colorFreq);
    render.material.SetFloat("_Color1Ratio", baseColorRatio);
    render.material.SetColor("_ColorEmmision", emmision);
    render.material.SetFloat("_EmmisionAmount", emmisionAmount);
    render.material.SetFloat("_Speed", gasSpeed);
  }
}
