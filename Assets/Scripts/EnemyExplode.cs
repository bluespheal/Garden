using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyExplode : MonoBehaviour
{

    public VisualEffect defeatVfx;

    public void GiveBeans()
    {

    }

    public void PlayDefeatVFX()
    {
        defeatVfx.Play();
    }

}
