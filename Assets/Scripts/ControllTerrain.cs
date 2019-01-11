using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ControllTerrain : MonoBehaviour {

  //Planet to controll parameters of
  public GameObject planet;

  //Renderer of the sun
  private Renderer render;

  //Values that can be changed in the shader
  /*************************************/
  [Range(0.0f, 1.0f)]
  public float terrainAmount = 0.4f;

  [Range(0.0f, 5.0f)]
  public float terrainFreq = 1.2f;

  public Color land1 = Color.green;
  public Color land2 = Color.grey;

  [Range(0.0f, 1.0f)]
  public float color1Ratio = 0.5f;

  public Color mountain = Color.white;
  public Color beach = Color.yellow;

  [Range(0.0f, 20.0f)]
  public float landColorFreq = 5.0f;
  /**************************************/

  // ---- UI ---
  /**************************************/
  public Slider amountSlider;
  public Slider freqSlider;
  public Slider colorRatioSlider;
  public Slider landFreqSlider;
  /**************************************/

  // Use this for initialization
  void Start () {

    if (!planet) return;

    render = planet.GetComponent<Renderer>();
    render.material.shader = Shader.Find("Custom/Terrain");

    terrainAmount = render.material.GetFloat("_Amount");
    terrainFreq = render.material.GetFloat("_Freq");
    land1 = render.material.GetColor("_LandColor1");
    land2 = render.material.GetColor("_LandColor2");
    color1Ratio = render.material.GetFloat("_Color1Ratio");
    mountain = render.material.GetColor("_MountColor");
    beach = render.material.GetColor("_BeachColor");
    landColorFreq = render.material.GetFloat("_ColorFreq");

    amountSlider.value = terrainAmount;
    freqSlider.value = terrainFreq;
    colorRatioSlider.value = color1Ratio;
    landFreqSlider.value = landColorFreq;
  }
	
	// Update is called once per frame
	void Update () {

    if (!planet) return;

    terrainAmount = amountSlider.value;
    terrainFreq = freqSlider.value;
    color1Ratio = colorRatioSlider.value;
    landColorFreq = landFreqSlider.value;

    render.material.SetFloat("_Amount", terrainAmount);
    render.material.SetFloat("_Freq", terrainFreq);
    render.material.SetColor("_LandColor1", land1);
    render.material.SetColor("_LandColor2", land2);
    render.material.SetFloat("_Color1Ratio", color1Ratio);
    render.material.SetColor("_MountColor", mountain);
    render.material.SetColor("_BeachColor", beach);
    render.material.SetFloat("_ColorFreq", landColorFreq);
  }
}
