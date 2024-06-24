using UnityEngine;

public class ServiceLocator : GenericMonoSingleton<ServiceLocator>
{
    public WeaponManager WeaponManager { get; private set; }
    public HUDManager HUDManager { get; private set; }
    public InteractionManager InteractionManager { get; private set; }

    public GlobalReference GlobalReference;

    public SoundManager SoundManager;


    protected override void Awake()
    {
        base.Awake();
        WeaponManager = new WeaponManager();
        HUDManager = new HUDManager();
        InteractionManager = new InteractionManager();
    }

    private void Start()
    {
        WeaponManager.Initialize();
    }

    private void Update()
    {
        WeaponManager.Update();
        HUDManager.Update();
        InteractionManager.Update();
    }
}