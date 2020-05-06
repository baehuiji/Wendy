using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitle_Controller : MonoBehaviour
{
    //Text subtitle;

    Image _subImg;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    //public Sprite sprite5;

    void Start()
    {
        //subtitle = GetComponent<Text>();
        _subImg = GetComponent<Image>();

        change_text(1);
    }

    void Update()
    {
        
    }

    public void change_text(int number)
    {
        switch (number)
        {
            case 1:
                //subtitle.text = "피터 팬은 새로 사귄 친구를 참 아꼈어." + System.Environment.NewLine + "친구들과 함께 네버랜드로 돌아갈 채비를 하는 중이었지." + System.Environment.NewLine + "아이들은 네버랜드로 가기 직전이 돼서야 부모님을 못 본다는 말에 무서워하기 시작했어.";
                _subImg.sprite = sprite1;
                break;
            case 2:
                //subtitle.text = "그 때 문이 열리는 소리와 함께 웬디가 제 발로 피터 팬 집에 들어온 거야.";
                _subImg.sprite = sprite2;
                break;
            case 3:
                //subtitle.text = "의도하지 않게 한 명의 친구가 더 생긴 피터팬은 나쁠 거 없잖아?" + System.Environment.NewLine + "두 팔 벌려 웬디를 환영했어.";
                _subImg.sprite = sprite3;
                break;
            case 4:
                //subtitle.text = "이제 곧 웬디도 피터 팬과 타이거 릴리, 존, 마이크를 따라 네버랜드로 가게 되겠지." + System.Environment.NewLine + "이 세상에서는 영원히 없어지는 거야.";
                _subImg.sprite = sprite4;
                break;
            default:
                //subtitle.text = "...";
                _subImg.enabled = false;
                break;
        }

    }
}
