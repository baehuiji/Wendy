using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePuzzle_Controller : MonoBehaviour
{

    [SerializeField]
    private string oneImageSound = "FP_oneImage";

    [SerializeField]
    private string twoImage1Sound = "FP_twoImage1";

    [SerializeField]
    private string twoImage2Sound = "FP_twoImage1";

    [SerializeField]
    private string threeImageSound = "FP_threeImage";

    [SerializeField]
    private string fourImageSound = "FP_fourImage";



    private Vector3 MousePoint;
    Ray Mouse_ray;

    private bool state = false;

    //[SerializeField]
    //private LayerMask FramelayerMask;
    int FramelayerMask;
    //[SerializeField]
    //private LayerMask GearlayerMask;
    int GearlayerMask;

    private float range = 2.5f;

    RaycastHit hitInfo;

    public int activeGear_num = 0;

    private int preGear_gN = 8; //gN = gearNumber , 과거
    private int curGear_gN = 8; //gN = gearNumber , 현재
    private int preGear_mN = 0; //mN = matchNumber
    private int sequence = 1; //1, 2, 3, 4
    private int preGear_mT = 0; //mT = matchType

    Camera camera;

    FramePuzzle_ChangeCam fpCameraController;

    bool CamOn = false;

    private GearManager gearManager_script;

    private bool delayOn = false; //틀리거나 맞췄을때, 톱니가 다돌아갈때까지 기다림 

    public Subtitle_Controller subtitle_script;

    FramePuzzle_Enter puzzleEnter_script;

    public GameObject noEntry_obj;
    
    Drawer_FP _drawer_script;
    //public GameObject Flash_obj;
    //public GameObject _reward;

    public GameObject _frame_coll;


    void Start()
    {
        camera = GetComponent<Camera>();
        fpCameraController = GetComponent<FramePuzzle_ChangeCam>();

        FramelayerMask = (1 << LayerMask.NameToLayer("FramePuzzle"));
        GearlayerMask = 1 << LayerMask.NameToLayer("Gear");

        //gearManager_script = GameObject.FindObjectOfType(typeof(GearManager)); //X
        gearManager_script = GameObject.FindObjectOfType<GearManager>();

        //subtitle_script = GameObject.FindObjectOfType<Subtitle_Controller>();
        puzzleEnter_script = GameObject.FindObjectOfType<FramePuzzle_Enter>();

        _drawer_script = GameObject.FindObjectOfType<Drawer_FP>();
    }


    void Update()
    {
        TryAction();

        //if (CamOn)
        //Debug.DrawRay(Mouse_ray.origin, Mouse_ray.direction * range, Color.red, range);
    }

    void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //스크린 클릭
            Vector3 mouse = Input.mousePosition;
            mouse.z = camera.farClipPlane; // 카메라가 보는 방향과, 시야
            MousePoint = camera.ScreenToWorldPoint(mouse); //dir

            Vector3 p = Input.mousePosition;
            Mouse_ray = camera.ScreenPointToRay(Input.mousePosition);

            //프레임 클릭했을때, 기어 클릭 가능
            if (state)
            {
                ClickGear();
            }

            //프레임 클릭 상태 업데이트 # 오류가 있어서 액자프레임오브젝트에 스크립트 따로만듬
            ClickFrame();
        }
    }

    void ClickFrame()
    {
        // - 벽이 아닌지도 검사  ***** -> 이건 메인캠에서 검사

        // - 레이케스트
        if (Physics.Raycast(Mouse_ray, out hitInfo, range, FramelayerMask))
        {
            //CamOn = false;
            state = false;

            //퍼즐 카메라 off
            fpCameraController.change_Camera(false);
        }
    }

    public void set_state() //true
    {
        //CamOn = true;
        state = true;
    }

    void ClickGear()
    {
        if (sequence == 5 || delayOn)
            return;

        if (Physics.Raycast(Mouse_ray, out hitInfo, range, GearlayerMask))
        {
            Gear_Move gear_script = hitInfo.transform.GetComponent<Gear_Move>();

            // - 이미 클릭한 기어가 회전할때는 안함
            if (gear_script.get_inRotation())
            {
                return;
            }

            // - 기어가 회전중

            // - 기어가 클릭 가능한 상태일때 (회전X)
            if (gear_script.get_gearOn()) //기어상태변화와 상태반환
            {
                // - 기어타입이 같으면 리턴시킴
                //if (preGear_mT == gear_script.get_matchType())
                //{
                //    return;
                //}

                // - 기어 활성화
                ++activeGear_num;
                if (activeGear_num == 1)
                {
                    gear_script.gear_Active(true);

                    //백기어
                    gearManager_script.set_Active_backGears(true, true);
                }
                else if (activeGear_num >= 2)
                {
                    gear_script.gear_Active(false);

                    //백기어
                    gearManager_script.set_Active_backGears(true, false);
                }

                // - 두개의 기어 비교 (매치넘버가 같을때만) (이후에 매치넘버 순서대로하는것도 필요함)
                if (activeGear_num >= 2)
                {
                    //인덱스저장
                    curGear_gN = gear_script.get_gearNumber(); //톱니해제에 대한 회전 기다리기 위함

                    // - 자막시퀸스와 이전 매치넘버와 같을때,
                    if (sequence == preGear_mN)
                    {
                        // 현재 매치넘버와도 비교
                        int curGear_mN = gear_script.matchNumber;

                        if (curGear_mN == preGear_mN)
                        {
                            // - 맞다
                            sequence++; //이러면 추가다.
                            
                            
                            if(sequence == 2)
                            {
                                SoundManger.instance.PlaySound(oneImageSound);
                            }

                            if (sequence == 3)
                            {
                                SoundManger.instance.PlaySound(twoImage2Sound);
                                SoundManger.instance.PlaySound(twoImage1Sound);
                            }

                            if (sequence == 4)
                            {
                                SoundManger.instance.PlaySound(threeImageSound);

                            }

                            if (sequence == 5)
                            {
                                SoundManger.instance.PlaySound(fourImageSound);

                            }

                            //자막바꾸기 **************
                            //subtitle_script.change_text(sequence);

                            if (sequence >= 5)
                            {
                                puzzleEnter_script.set_puzzleEnd();
                                noEntry_obj.SetActive(false);

                                // 보상
                                _drawer_script.RewardActive();
                                //Flash_obj.SetActive(true);
                                //_reward.SetActive(true);
                            }

                            // - 톱니 해제 , 유예기간 지난후에 톱니가 해제됨.
                            delayOn = true;
                            gear_script.set_delayOn(true);

                            return; //뒤의 내용 수행 X
                        }
                    }
                    //else

                    // - 아니다
                    //톱니해제
                    delayOn = true;
                    gear_script.set_delayOn(true);
                }
                else
                {
                    //이전 기어의 넘버 저장
                    preGear_gN = gear_script.get_gearNumber();

                    //이전 기어의 매치넘버 저장, 자막 순서
                    preGear_mN = gear_script.matchNumber;

                    //이전 기어의 매치타입 저장, 이미지인지 사운드인지
                    preGear_mT = gear_script.get_matchType();
                }
            }
            else
            {
                // - 기어 해제
                --activeGear_num;
                init_();

                gear_script.gear_unActive();

                //백기어
                gearManager_script.set_Active_backGears(false, false);
            }
        }
    }

    private void init_() //두개 기어를 비교했을때, 정답이 아니면
    {
        preGear_gN = 8;
        preGear_mN = 0;
        preGear_mT = 0;
    }

    public void moveAfter()
    {
        delayOn = false;

        gearManager_script.set_delayOnGear(curGear_gN); //하나만 하기위해 ->근데 이거말고 다르게해야할듯, 근데 되긴함

        gearManager_script.set_unActiveGear(curGear_gN); //현재톱니
        gearManager_script.set_unActiveGear(preGear_gN); //이전톱니

        //백기어
        gearManager_script.set_Active_backGears(false, false);

        activeGear_num = 0;
        curGear_gN = 8;

        //자막
        subtitle_script.change_text(sequence);

        init_();
    }
}
