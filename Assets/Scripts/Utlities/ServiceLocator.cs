using UnityEngine;

public class ServiceLocator : GenericMonoSingleton<ServiceLocator>
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private InteractionManager interactionManager;
    [SerializeField] private GlobalReference globalReference;
    [SerializeField] private SoundManager soundManager;
    public WeaponManager WeaponManager => weaponManager;
    public HUDManager HUDManager => hudManager;
    public InteractionManager InteractionManager => interactionManager;
    public GlobalReference GlobalReference => globalReference;
    public SoundManager SoundManager => soundManager;


    protected override void Awake()
    {
        base.Awake();
        weaponManager = new WeaponManager();
        hudManager = new HUDManager();
        interactionManager = new InteractionManager();
    }

    private void Start()
    {
        weaponManager.Initialize();
    }

    private void Update()
    {
        weaponManager.Update();
        hudManager.Update();
        interactionManager.Update();
    }
}