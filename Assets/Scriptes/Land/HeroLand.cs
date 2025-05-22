using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshObstacle))]
public class HeroLand : Land, IClickable
{
    [SerializeField]
    private NavMeshObstacle navObstacle;

    [SerializeField]
    private Vector3 heroPosOffset = Vector3.zero;
    // head child에 붙혀서 의존성을 없앰
    [SerializeField]
    private Transform head;

    public void SetHero(int _idx)
    {
        Hero hero = CharacterManager.Instance.CreateHero(_idx);
        hero.transform.SetParent(head);
        // head는 쫌... land보다 위에 있어야 할듯...
        hero.transform.localPosition = Vector3.zero + heroPosOffset;
        hero.gameObject.layer = gameObject.layer;
    }
    public void RemoveHero(HeroData _heroData) 
    { 

    }

    public void OnClick()
    {
        // hero 정보창 
        if (head.childCount > 0)
        {
            UIManager.Instance.ShowUI<CharacterInfoUI>(UIPanelType.CharacterInfo);
        }
        // hero 생성창
        else
        {
            // 
        }
    }
}

