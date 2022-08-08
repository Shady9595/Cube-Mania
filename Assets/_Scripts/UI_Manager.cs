using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[HideMonoScript]
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance {get; private set;}
    [Title("UI MANAGER", "SINGLETON", titleAlignment: TitleAlignments.Centered, false, true)]
    [SerializeField] float MoveDuration = 0.5f;

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
			// DontDestroyOnLoad(gameObject);
		}//else end
	}//Awake() end

    public void SetX(RectTransform Panel)
    {
        Panel.anchoredPosition = new Vector2(-Panel.anchoredPosition.x, Panel.anchoredPosition.y);
    }//SetX() end

    public void SetY(RectTransform Panel)
    {
        Panel.anchoredPosition = new Vector2(Panel.anchoredPosition.x, -Panel.anchoredPosition.y);
    }//SetX() end

    public void SetX(RectTransform Panel, float Position)
    {
        Panel.anchoredPosition = new Vector2(Position, Panel.anchoredPosition.y);
    }//SetX() end

    public void SetY(RectTransform Panel, float Position)
    {
        Panel.anchoredPosition = new Vector2(Panel.anchoredPosition.x, Position);
    }//SetX() end
    
    public void MoveX(RectTransform Panel)
    {
        Panel.DOAnchorPos3DX(-Panel.anchoredPosition.x, MoveDuration, false);
    }//MoveX() end

    public void MoveY(RectTransform Panel)
    {
        Panel.DOAnchorPos3DY(-Panel.anchoredPosition.y, MoveDuration, false);
    }//MoveX() end

    public void MoveX(RectTransform Panel, float Duration)
    {
        Panel.DOAnchorPos3DX(-Panel.anchoredPosition.x, Duration, false);
    }//MoveX() end

    public void MoveY(RectTransform Panel, float Duration)
    {
        Panel.DOAnchorPos3DY(-Panel.anchoredPosition.y, MoveDuration, false);
    }//MoveX() end
}//class end