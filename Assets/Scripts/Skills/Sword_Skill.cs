using System;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType SwordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] private int amountOfBounce;

    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Peirce Info")]
    [SerializeField] private int pierceAmount;

    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float maxTravelDistance;

    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;
    [SerializeField] private float hitCoolDown;

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;

    [FormerlySerializedAs("launchDir")] [SerializeField]
    private Vector2 launchForce;

    [SerializeField] private float swordGravity;

    private Vector2 finalDir;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;

    [SerializeField] private float spaceBeetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetUpGravity();
    }

    private void SetUpGravity()
    {
        if (SwordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        } else if (SwordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        } else if (SwordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    protected override void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBeetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject sword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = sword.GetComponent<Sword_Skill_Controller>();

        if (SwordType == SwordType.Bounce)
        {
            newSwordScript.SetUpBounce(true, amountOfBounce, bounceSpeed);
        } else if (SwordType == SwordType.Pierce)
        {
            newSwordScript.SetUpPierce(pierceAmount);
        } else if (SwordType == SwordType.Spin)
        {
            newSwordScript.SetUpSpin(true, maxTravelDistance, spinDuration, spinGravity, hitCoolDown);
        }


        newSwordScript.SetUpSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(sword);
        player.Skill.sword.DotsActive(false);
    }

    #region Aim Region

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position +
                           new Vector2(AimDirection().normalized.x * launchForce.x,
                               AimDirection().normalized.y * launchForce.y) * t + 0.5f *
                           (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }

    #endregion
}