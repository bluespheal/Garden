using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyExplode : MonoBehaviour
{

    public VisualEffect defeatVfx;

    public void PlayDefeatVFX()
    {
        GameManager.Instance.AudioManager.PlaySFX(1);
        defeatVfx.Play();
    }

}
