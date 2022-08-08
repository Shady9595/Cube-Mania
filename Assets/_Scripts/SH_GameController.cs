//Shady
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ColorScheme
{
    public Color PlayerColor;
    public Color BackgroundColor;
    public Color PlatformColor;
}//class end

[HideMonoScript]
public class SH_GameController : MonoBehaviour
{
    public static SH_GameController Instance {get; private set;}
    [Title("SH_GAME CONTROLLER", "SINGLETON", titleAlignment: TitleAlignments.Centered, false, true)]
    public Control Controls;
    public enum Control{PC, Mobile}
    [Space]
    [Title("Game Stats")]
    [SerializeField] int CurrentLevel  = 1;
    [SerializeField] int CubesObtained = 0;
    [SerializeField] int CubesToObtain = 0;

    [Space]
    [FoldoutGroup("Player References")]
    [SerializeField] Transform  Player           = null;
    [FoldoutGroup("Player References")]
    [SerializeField] GameObject TouchControls    = null;
    [FoldoutGroup("Player References")]
    [SerializeField] Camera     PlayerCamera     = null;
    [FoldoutGroup("Player References")]
    [SerializeField] Material   PlayerMaterial   = null;
    [FoldoutGroup("Player References")]
    [SerializeField] Transform  EndPoint         = null;

    [Space]
    [FoldoutGroup("Platform References")]
    [SerializeField] Transform  Platform         = null;
    [FoldoutGroup("Platform References")]
    [SerializeField] Material   PlatformMaterial = null;
    [FoldoutGroup("Platform References")]
    [SerializeField] float      PlatformSize     = 0.0f;
    [FoldoutGroup("Platform References")]
    [SerializeField] Transform  Destroyer        = null;

    [Space]
    [FoldoutGroup("Prefab References")]
    [SerializeField] GameObject CubePrefab  = null;
    [FoldoutGroup("Prefab References")]
    [SerializeField] GameObject EnemyPrefab = null;

    [Space]
    [FoldoutGroup("Color Schemes")]
    [InlineButton("ChangeColorScheme")]
    [SerializeField] ColorScheme[] ColorSchemes   = null;
    void ChangeColorScheme()
    {
        SetColorScheme();
    }//ChangeColorScheme() end

    [Space]
    [FoldoutGroup("UI References")]
    [SerializeField] CanvasGroup Fader            = null;
    [FoldoutGroup("UI References")]
    [SerializeField] Button PlayButton            = null;
    [FoldoutGroup("UI References")]
    [SerializeField] RectTransform LevelComplete  = null;
    [FoldoutGroup("UI References")]
    [SerializeField] RectTransform LevelFail      = null;
    [FoldoutGroup("UI References")]
    [SerializeField] GameObject StartTimer        = null;
    [FoldoutGroup("UI References")]
    [SerializeField] TMP_Text TimerText           = null;
    [FoldoutGroup("UI References")]
    [SerializeField] TMP_Text CollectText         = null;
    [FoldoutGroup("UI References")]
    [SerializeField] RectTransform UpperBar       = null;
    [FoldoutGroup("UI References")]
    [SerializeField] Image    CurrentLevelImage   = null;
    [FoldoutGroup("UI References")]
    [SerializeField] Image    NextLevelImage      = null;
    [FoldoutGroup("UI References")]
    [SerializeField] TMP_Text CurrentLevelText    = null;
    [FoldoutGroup("UI References")]
    [SerializeField] TMP_Text NextLevelText       = null;
    [FoldoutGroup("UI References")]
    [SerializeField] Image    FillBar             = null;
    [FoldoutGroup("UI References")]
    [SerializeField] Image    CubeImage           = null;
    [FoldoutGroup("UI References")]
    [SerializeField] TMP_Text CubesObtainedText   = null;
    [FoldoutGroup("UI References")]
    [SerializeField] TMP_Text CubesToObtainText   = null;

    [FoldoutGroup("Sound References")]
    [SerializeField] AudioSource BackgroundSource = null;
    [FoldoutGroup("Sound References")]
    [SerializeField] AudioSource SFXSource        = null;
    [FoldoutGroup("Sound References")]
    [SerializeField] AudioClip BackgroundMusic    = null;
    [FoldoutGroup("Sound References")]
    [SerializeField] AudioClip ClickSound         = null;
    [FoldoutGroup("Sound References")]
    [SerializeField] AudioClip GameWinSound       = null;
    [FoldoutGroup("Sound References")]
    [SerializeField] AudioClip GameOverSound      = null;
    [FoldoutGroup("Sound References")]
    [SerializeField] AudioClip CubeObtainSound    = null;
    [FoldoutGroup("Sound References")]
    [SerializeField] AudioClip ImpactSound        = null;

    //Private Variables
    private GameObject Enemies         = null;
    private GameObject Cubes           = null;
    private float      TotalDistance   = 0.0f; 
    private float      DistanceCovered = 0.0f;
    private float      ClampX          = 0.0f;
    private int        CubeCount       = 0;

    void Awake()
	{
		if(Instance)
        {
			DestroyImmediate (gameObject);
			return;
		}//if end
        else
        {
			Instance = this;
		}//else end
        if (SaveData.Instance == null)
        {
            SaveData.Instance = new SaveData();
            SaveSystem.LoadProgress();
        }//if end
	}//Awake() end

    void Start()
    {
        if(!Application.isEditor)
            Controls = Control.Mobile;
        if(Controls.Equals(Control.PC))
            TouchControls.SetActive(false);
        Application.targetFrameRate = 300;
        CurrentLevel = SaveData.Instance.Level;
        BackgroundSource.loop = true;
        BackgroundSource.clip = BackgroundMusic;
        BackgroundSource.Play();
        SetPlatform();
        SetColorScheme();
        SetUI();
        CubeCount = 0;
        if(CurrentLevel > 8)
            CubeCount = Random.Range(6, 8) * 5;
        else
            CubeCount    = CurrentLevel * 5;
    }//Start() end

    void SetPlatform()
    {
        Renderer Rend = Platform.GetComponent<Renderer>();
        PlatformSize  = Platform.localScale.z;
        int PlatformSizeIncrement = CurrentLevel;
        if(PlatformSizeIncrement > 8)
        {
            PlatformSizeIncrement = Random.Range(6, 8);
        }//if end
        for(int i=1 ; i<PlatformSizeIncrement ; i++)
        {
            PlatformSize += 100f;
        }//loop end
        Platform.localScale = new Vector3(Platform.localScale.x, Platform.localScale.y, PlatformSize);
        Player.gameObject.SetActive(false);
        Player.position     = new Vector3(Player.position.x, Player.position.y, Rend.bounds.min.z + 10f);
        Player.gameObject.SetActive(true);
        EndPoint.position   = new Vector3(EndPoint.position.x, EndPoint.position.y, Rend.bounds.max.z - 10f);
        TotalDistance = Vector3.Distance(Player.position, EndPoint.position);

        ClampX = (Platform.localScale.x/2) - 1;
        Player.GetComponent<PlayerController>().SetClampX(ClampX);

        Destroyer.position   = new Vector3(Platform.position.x, Platform.position.y - 20f, Platform.position.z);
        Destroyer.localScale = new Vector3(Platform.localScale.x + 50f, Platform.localScale.y, Platform.localScale.z);
    }//SetPlatform() end

    void SetColorScheme()
    {
        int SelectedColorScheme      = Random.Range(0, ColorSchemes.Length);
        PlayerCamera.backgroundColor = ColorSchemes[SelectedColorScheme].BackgroundColor;
        PlayerMaterial.color         = ColorSchemes[SelectedColorScheme].PlayerColor;
        PlatformMaterial.color       = ColorSchemes[SelectedColorScheme].PlatformColor;
        CurrentLevelImage.color      = ColorSchemes[SelectedColorScheme].PlatformColor;
        NextLevelImage.color         = ColorSchemes[SelectedColorScheme].PlatformColor;
        FillBar.color                = ColorSchemes[SelectedColorScheme].PlatformColor;
        // RenderSettings.fogColor      = ColorSchemes[SelectedColorScheme].BackgroundColor;
    }//SetColorScheme() end

    void MakeEnemies()
    {
        Enemies = new GameObject("Enemies");
        int DistInBetween = (int)TotalDistance / CubeCount;
        Vector3 Pos;
        for(int i=1 ; i<CubeCount ; i++)
        {
            Pos = new Vector3(Random.Range(-ClampX, ClampX), EnemyPrefab.transform.position.y, Player.position.z + (DistInBetween * i));
            GameObject Enemy = Instantiate(EnemyPrefab, Pos, Quaternion.identity, Enemies.transform);
        }//loop end
    }//MakeEnemies() end

    void MakeCubes()
    {
        Cubes = new GameObject("Cubes");
        int DistInBetween  = (int)TotalDistance / CubeCount;
        Vector3 Pos;
        for(int i=1 ; i<CubeCount ; i++)
        {
            Pos = new Vector3(Random.Range(-ClampX + 0.5f, ClampX - 0.5f), EnemyPrefab.transform.position.y, Player.position.z + (DistInBetween * i) + 20f);
            GameObject Cube = Instantiate(CubePrefab, Pos, Quaternion.identity, Cubes.transform);
        }//loop end
        CubesObtained = 0;
        CubesToObtain = CubeCount - 3;
        CubesObtainedText.text       = CubesObtained.ToString("00")/* + "/"*/;
        CubesToObtainText.text       = CubesToObtain.ToString("00");
        CollectText.text             = "COLLECT " + CubesToObtain.ToString("00") + " GREEN CUBES !";
    }//MakeCubes() end

    void SetUI()
    {
        PlayButton.onClick.RemoveAllListeners();
        PlayButton.onClick.AddListener(Play);
        PlayButton.gameObject.SetActive(true);
        LevelComplete.gameObject.SetActive(false);
        LevelFail.gameObject.SetActive(false);
        StartTimer.SetActive(false);
        UI_Manager.Instance.SetY(UpperBar);
        NextLevelText.text           = (CurrentLevel + 1).ToString();
        CurrentLevelText.text        = CurrentLevel.ToString();
        FillBar.fillAmount           = 0.0f;
        CubeImage.color              = CubePrefab.GetComponentInChildren<Renderer>().sharedMaterial.color;
        Fader.alpha = 0.0f;
        Fader.blocksRaycasts = false;
        Fader.interactable   = false;       
    }//SetColors() end

    void Play()
    {
        SFXSource.PlayOneShot(ClickSound);
        FadeIn(StartGame);
    }//Play() end

    void StartGame()
    {
        PlayButton.gameObject.SetActive(false);
        MakeEnemies();
        MakeCubes();
        StartCoroutine(StartGameTimer());
        FadeOut();
    }//StartGame() end

    void FadeIn(TweenCallback action)
    {
        Fader.DOFade(1.0f, 0.5f).OnComplete(action);
        Fader.blocksRaycasts = true;
        Fader.interactable   = true; 
    }//FadeIn() end

    void FadeOut()
    {
        Fader.DOFade(0.0f, 0.8f);
        Fader.blocksRaycasts = false;
        Fader.interactable   = false; 
    }//FadeOut() end

    private IEnumerator StartGameTimer()
    {
        UI_Manager.Instance.MoveY(UpperBar);
        int timer = 3;
        TimerText.text = timer.ToString();
        StartTimer.SetActive(true);
        while(timer != 0)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            timer--;
            TimerText.text = timer.ToString();
        }//loop end
        TimerText.text = "GO !";
        yield return new WaitForSecondsRealtime(0.5f);
        StartTimer.SetActive(false);
        Player.GetComponent<PlayerController>().Run();
    }//StartGameTimer() end

    public void GameComplete()
    {
        if(CubesObtained < CubesToObtain)
            GameLose();
        else
            GameWin();
    }//GameComplete()

    void GameWin()
    {
        SaveData.Instance.Level++;
        SaveSystem.SaveProgress();
        BackgroundSource.Stop();
        SFXSource.PlayOneShot(GameWinSound);
        UI_Manager.Instance.SetY(LevelComplete);
        LevelComplete.gameObject.SetActive(true);
        UI_Manager.Instance.MoveY(LevelComplete);
    }//GameLose() end

    public void GameLose()
    {
        BackgroundSource.Stop();
        SFXSource.PlayOneShot(GameOverSound);
        UI_Manager.Instance.SetY(LevelFail);
        LevelFail.gameObject.SetActive(true);
        UI_Manager.Instance.MoveY(LevelFail);
    }//GameLose() end
 
    void Update()
    {
        FillDistanceBar();
    }//Update() end

    void FillDistanceBar()
    {
        DistanceCovered = Vector3.Distance(Player.position, EndPoint.position);
        FillBar.fillAmount = 1.0f - (DistanceCovered / TotalDistance);
    }//FillDistanceBar() end

    public Transform GetPlayer()
    {
        return Player;
    }//GetPlayer() end

    public void CubeObtained()
    {
        CubesObtained++;
        SFXSource.PlayOneShot(CubeObtainSound);
        // if(CubesObtained > CubesToObtain)
        //     return;
        CubesObtainedText.text = CubesObtained.ToString("00")/* + "/"*/;
        if(DOTween.IsTweening(CubesObtainedText.transform))
            return;
        CubesObtainedText.transform.DOPunchScale(Vector3.one * 1.2f, 0.2f, 1, 1);
    }//CubeObtained() end

    public void EnemyImpact()
    {
        PlayerCamera.DOShakePosition(0.5f, 1.5f);
        SFXSource.PlayOneShot(ImpactSound);
    }//EnemyImpact() end

    public void ReloadSceneButton()
    {
        SFXSource.PlayOneShot(ClickSound);
        FadeIn(ReloadScene);
    }//ReloadSceneButton() end

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }//ReloadScene() end

}//class end