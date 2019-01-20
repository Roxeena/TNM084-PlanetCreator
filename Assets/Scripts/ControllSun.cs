using UnityEngine.UI;
using UnityEngine;

public class ControllSun : MonoBehaviour {

  //Object to controll parameters of
  public GameObject sun;

  //Renderer of the sun
  private Renderer render;

  //Values that can be changed in the shader
  /*************************************/
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

  [Range(0.0f, 0.2f)]
  public float gasSpeed = 0.1f;
  /**************************************/

  // ---- UI ---
  /**************************************/
  public Slider colorAmountSlider;
  public Slider colorFreqSlider;
  public Slider baseColorRatioSlider;
  public Slider emmisionAmountSlider;
  public Slider speedSlider;

  //Colors
  public Slider baseColorSliderR;
  public Slider baseColorSliderG;
  public Slider baseColorSliderB;

  public Slider spotColorSliderR;
  public Slider spotColorSliderG;
  public Slider spotColorSliderB;

  public Slider emisionColorSliderR;
  public Slider emisionColorSliderG;
  public Slider emisionColorSliderB;
  /**************************************/

  // Used for initialization
  void Start () {

    if (!sun) return;

    render = sun.GetComponent<Renderer>();
    render.material.shader = Shader.Find("Custom/Sun");

    //Get current values in shader
    baseColor = render.material.GetColor("_Color1");
    spotsColor = render.material.GetColor("_Color2");
    colorAmount = render.material.GetFloat("_ColorAmount");
    colorFreq = render.material.GetFloat("_ColorFreq");
    baseColorRatio = render.material.GetFloat("_Color1Ratio");
    emmision = render.material.GetColor("_ColorEmmision");
    emmisionAmount = render.material.GetFloat("_EmmisionAmount");
    gasSpeed = render.material.GetFloat("_Speed");

    //Set sliders to theses values
    colorAmountSlider.value = colorAmount;
    colorFreqSlider.value = colorFreq;
    baseColorRatioSlider.value = baseColorRatio;
    emmisionAmountSlider.value = emmisionAmount;
    speedSlider.value = gasSpeed;

    //Color sliders
    baseColorSliderR.value = baseColor.r;
    baseColorSliderG.value = baseColor.g;
    baseColorSliderB.value = baseColor.b;

    spotColorSliderR.value = spotsColor.r;
    spotColorSliderG.value = spotsColor.g;
    spotColorSliderB.value = spotsColor.b;

    emisionColorSliderR.value = emmision.r;
    emisionColorSliderG.value = emmision.g;
    emisionColorSliderB.value = emmision.b;
  }

  // Update is called once per frame
  void Update () {

    if (!sun) return;

    //Get new value in slider
    colorAmount = colorAmountSlider.value;
    colorFreq = colorFreqSlider.value;
    baseColorRatio = baseColorRatioSlider.value;
    emmisionAmount = emmisionAmountSlider.value;
    gasSpeed = speedSlider.value;

    //Color sliders
    baseColor.r = baseColorSliderR.value;
    baseColor.g = baseColorSliderG.value;
    baseColor.b = baseColorSliderB.value;

    spotsColor.r = spotColorSliderR.value;
    spotsColor.g = spotColorSliderG.value;
    spotsColor.b = spotColorSliderB.value;

    emmision.r = emisionColorSliderR.value;
    emmision.g = emisionColorSliderG.value;
    emmision.b = emisionColorSliderB.value;

    //Apply the slider values to the parameters in the shader
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
