using UnityEngine.UI;
using UnityEngine;

public class ControllWater : MonoBehaviour {

  //Planet to controll parameters of
  public GameObject planet;

  //Renderer of the sun
  private Renderer render;

  //Values that can be changed in the shader
  /*************************************/
  public Color water = Color.blue;
  public Color wave = Color.white;

  [Range(1.0f, 50.0f)]
  public float waveFreq = 20.0f;

  [Range(0.0f, 1.0f)]
  public float waveAmount = 0.5f;

  [Range(0.0f, 0.2f)]
  public float waterSpeed = 0.2f;
  /**************************************/

  // ---- UI ---
  /**************************************/
  public Slider sizeSlider;
  public Slider waveAmountSlider;
  public Slider waveFreqSlider;
  public Slider speedSlider;

  //Colors
  public Slider waterColorR;
  public Slider waterColorG;
  public Slider waterColorB;

  public Slider wavesColorR;
  public Slider wavesColorG;
  public Slider wavesColorB;
  /**************************************/

  //Send size to terrain controller to decide beach size
  private float waterSize; 

  // Used for initialization
  void Start () {

    if (!planet) return;

    render = planet.GetComponent<Renderer>();
    render.material.shader = Shader.Find("Custom/Water");

    //Get current values in shader
    water = render.material.GetColor("_WaterColor");
    wave = render.material.GetColor("_WaveColor");
    waveAmount = render.material.GetFloat("_WaveAmount");
    waveFreq = render.material.GetFloat("_WaveFreq");
    waterSpeed = render.material.GetFloat("_WaterSpeed");

    //Set sliders to theses values
    sizeSlider.value = planet.GetComponent<Transform>().localScale.x;
    waveAmountSlider.value = waveAmount;
    waveFreqSlider.value = waveFreq;
    speedSlider.value = waterSpeed;

    //Color sliders
    waterColorR.value = water.r;
    waterColorG.value = water.g;
    waterColorB.value = water.b;

    wavesColorR.value = wave.r;
    wavesColorG.value = wave.g;
    wavesColorB.value = wave.b;
  }

  // Update is called once per frame
  void Update () {

    if (!planet) return;

    //Get new value in slider
    waterSize = sizeSlider.value;
    waveAmount = waveAmountSlider.value;
    waveFreq = waveFreqSlider.value;
    waterSpeed = speedSlider.value;

    //Color sliders
    water.r = waterColorR.value;
    water.g = waterColorG.value;
    water.b = waterColorB.value;

    wave.r = wavesColorR.value;
    wave.g = wavesColorG.value;
    wave.b = wavesColorB.value;

    //Apply the slider values to the parameters in the shader
    planet.GetComponent<Transform>().localScale = new Vector3(waterSize, waterSize, waterSize);
    render.material.SetColor("_WaterColor", water);
    render.material.SetColor("_WaveColor", wave);
    render.material.SetFloat("_WaveAmount", waveAmount);
    render.material.SetFloat("_WaveFreq", waveFreq);
    render.material.SetFloat("_WaterSpeed", waterSpeed);
  }

  public float getWaterSize()
  {
    return waterSize;
  }
}
