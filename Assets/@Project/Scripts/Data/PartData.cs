using UnityEngine;

[System.Serializable]
public class PartData : IEntity
{    
    [SerializeField] private int dev_ID;
    [SerializeField] private string dev_Name;
    [SerializeField] private string prefab_Path;
    [SerializeField] private string sprite_Path;

    [SerializeField] private string display_Name;
    [TextArea]
    [SerializeField] private string display_Description;
    
    [SerializeField] private float armor;
    [SerializeField] private float weight;

    [Header("Lower")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float boosterPower;
    [SerializeField] private bool canJump;

    [Header("Upper")]
    [SerializeField] private float smoothRotation;

    [Header("Weapon")]
    [SerializeField] private string bulletPrefab_Path;

    [SerializeField] private float damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float coolDownTime;
    [SerializeField] private int projectilesPerShot;
    [SerializeField] private float shotErrorRange;
    [SerializeField] private int ammo;
    [SerializeField] private bool isReloadable;
    [SerializeField] private bool isSplash;

    public int Dev_ID => dev_ID;
    public string Dev_Name => dev_Name;
    public string Prefab_Path => prefab_Path;
    public string Sprite_Path => sprite_Path;
    public string Display_Name => display_Name;
    public string Display_Description=> display_Description;    

    public float Armor => armor;
    public float Weight => weight;

    public float Speed => speed;
    public float JumpPower => jumpPower;
    public float BoosterPower => boosterPower;
    public bool CanJump => canJump;

    public float SmoothRotation => smoothRotation;

    public string BulletPrefab_Path => bulletPrefab_Path;
    public float Damage => damage;
    public float BulletSpeed => bulletSpeed;
    public float FireRate => fireRate;
    public float CoolDownTime => coolDownTime;
    public int ProjectilesPerShot => projectilesPerShot;
    public float ShotErrorRange => shotErrorRange;
    public int Ammo => ammo;
    public bool IsReloadable => isReloadable;
    public bool IsSplash => isSplash;
}
