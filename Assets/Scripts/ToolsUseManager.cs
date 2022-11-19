using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsUseManager : MonoBehaviour
{
    [SerializeField] Character _character;
    [SerializeField] GameObject toolWater;
    [SerializeField] GameObject toolAxe;
    [SerializeField] GameObject toolAirBlow;

    [SerializeField] ParticleSystem axeHitParticles;
    [SerializeField] AxeScript axeScript;
    [SerializeField] AirBlowArea airBlowArea;
    [SerializeField] GameObject blowParticles;
    [SerializeField] Collider airBlowTrigger;

    [SerializeField] MeshRenderer konwekaFilled_MR;
    [SerializeField] MeshRenderer konwekaEmpty_MR;
    [SerializeField] LayerMask slimeLayer;
    [SerializeField] float axeSlimeHitDistance = 4f;

    Animator _toolAnimator;

    Tool currentTool;

    public static System.Action<float> UsingWaterToolTick;
    public static System.Action<Vector3> AxeHit;
    public static System.Action AxeUsed;
    internal static System.Action<int> ToolSelected;
    bool axeCanHit = false;

    float waterToolPower = 60f;
    float waterLevel = 0f;
    float waterUsageSpeed = 8f;

    enum Tool
    {
        None,
        Water,
        Axe,
        AirBlow
    }

    private void OnEnable()
    {
        SlotsInteractor.RefillWater += RefillWaterTool;
    }

    private void OnDisable()
    {
        SlotsInteractor.RefillWater -= RefillWaterTool;
    }

    private void RefillWaterTool()
    {
        waterLevel = 100f;
        UIMGR.instance.UpdateWaterLevelToolSlider(waterLevel / 100f);
        UpdateKonwekaMesh();
    }

    // Start is called before the first frame update
    void Start()
    {
        _toolAnimator = GetComponent<Animator>();

        currentTool = Tool.None;
        HideAllTools();
        UIMGR.instance.UpdateWaterLevelToolSlider(waterLevel / 100f);
        UpdateKonwekaMesh();
    }

    private void UpdateKonwekaMesh()
    {
        if (waterLevel > 0f)
        {
            konwekaFilled_MR.enabled = true;
            konwekaEmpty_MR.enabled = false;
        }
        else
        {
            konwekaFilled_MR.enabled = false;
            konwekaEmpty_MR.enabled = true;
        }
    }

    private void HideAllTools()
    {
        toolWater.SetActive(false);
        toolAxe.SetActive(false);
        toolAirBlow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int toolChange = 0;

        if (Input.GetKeyDown("1"))
        {
            toolChange = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            toolChange = 2;
        }
        else if(Input.GetKeyDown("3"))
        {
            toolChange = 3;
        }

        if (InputMgr.instance.GetMouseScroll() > 0)
        {
            if (currentTool == Tool.Water)
                toolChange = 3;
            if (currentTool == Tool.Axe)
                toolChange = 1;
            if (currentTool == Tool.AirBlow)
                toolChange = 2;
            if (currentTool == Tool.None)
                toolChange = 3;
        }
        else if (InputMgr.instance.GetMouseScroll() < 0)
        {
            if (currentTool == Tool.Water)
                toolChange = 2;
            if (currentTool == Tool.Axe)
                toolChange = 3;
            if (currentTool == Tool.AirBlow)
                toolChange = 1;
            if (currentTool == Tool.None)
                toolChange = 1;
        }

        // tool change
        if (toolChange == 1)
        {
            if (currentTool == Tool.Water)
            {
                currentTool = Tool.None;
                ToolSelected?.Invoke(-1);
                HideAllTools();
                return;
            }

            currentTool = Tool.Water;

            HideAllTools();
            toolWater.SetActive(true);
            ToolSelected?.Invoke(0);
            _toolAnimator.Play("Idle AnyTool", 0, 0);
        }
        else if (toolChange == 2)
        {
            if (currentTool == Tool.Axe)
            {
                currentTool = Tool.None;
                ToolSelected?.Invoke(-1);
                HideAllTools();
                return;
            }

            currentTool = Tool.Axe;

            HideAllTools();
            toolAxe.SetActive(true);
            ToolSelected?.Invoke(1);
            _toolAnimator.Play("Idle AnyTool", 0, 0);
        }
        else if (toolChange == 3)
        {
            if (currentTool == Tool.AirBlow)
            {
                currentTool = Tool.None;
                ToolSelected?.Invoke(-1);
                HideAllTools();
                return;
            }

            currentTool = Tool.AirBlow;

            HideAllTools();
            toolAirBlow.SetActive(true);
            ToolSelected?.Invoke(2);
            _toolAnimator.Play("Idle AnyTool", 0, 0);
        }

        // Handle tools usage
        if (Input.GetButton("UseTool") && !UIMGR.instance.isMenuOverlay)
        {
            if (currentTool == Tool.Water && waterLevel > 0)
            {
                _toolAnimator.SetInteger("toolUsed", 1);
                UsingWaterToolTick?.Invoke(waterToolPower * Time.deltaTime);
                AudioMgr.instance.isUsingWater = true;
                AudioMgr.instance.isUsingAirBlow = false;
                waterLevel -= waterUsageSpeed * Time.deltaTime;
                UIMGR.instance.UpdateWaterLevelToolSlider(waterLevel / 100f);
                UpdateKonwekaMesh();
            }
            else if (currentTool == Tool.Axe)
            {
                _toolAnimator.SetInteger("toolUsed", 2);
                AudioMgr.instance.isUsingWater = false;
                AudioMgr.instance.isUsingAirBlow = false;
            }
            else if (currentTool == Tool.AirBlow)
            {
                _toolAnimator.SetInteger("toolUsed", 3);
                AudioMgr.instance.isUsingWater = false;
                AudioMgr.instance.isUsingAirBlow = true;
                airBlowArea.isBlowing = true;
                airBlowTrigger.enabled = true;
                blowParticles.SetActive(true);
            }
        }
        else
        {
            _toolAnimator.SetInteger("toolUsed", 0);
            airBlowArea.isBlowing = false;
            airBlowTrigger.enabled = false;
            blowParticles.SetActive(false);
            AudioMgr.instance.isUsingWater = false;
            AudioMgr.instance.isUsingAirBlow = false;
        }
    }

    private void EquipTool(Tool newTool)
    {
        
    }

    public void OnHitTree(GameObject other, Vector3 pos)
    {
        if (axeCanHit)
        {
            other.GetComponent<Tree>().OnHit(axeScript.axePower);
            Instantiate(axeHitParticles, pos, toolAxe.transform.rotation);
            axeCanHit = false;
            AxeHit?.Invoke(pos);
        }
    }

    public void EnteredAxeHittingPos()
    {
        axeCanHit = true;
        AxeUsed?.Invoke();        
    }

    public void AxeCheckForSlimeHits()
    {
        RaycastHit hit;
        Vector3 startPos = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;
        if (Physics.Raycast(startPos, dir, out hit, axeSlimeHitDistance, slimeLayer))
        {
            int power = 60;
            Vector3 forceVector = Camera.main.transform.forward * power;
            forceVector.y /= 5f;
            hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        }
    }

    public void ExitedAxeHittingPos()
    {
        axeCanHit = false;
    }
}
