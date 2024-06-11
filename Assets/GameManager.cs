using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericMonoSingleton<GameManager> 
{
    public SoundManager SoundManager;
    public WeaponManager WeaponManager;
    public HUDManager HUDManager;
    public AmmoManager AmmoManager;
    public IconManager IconManager;
    public GlobalReference GlobalReference;




}
