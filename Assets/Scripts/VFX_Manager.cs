using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : Singleton<VFX_Manager>
{
    public GameObject WoodHit;
    public GameObject KnifeClank;
    public GameObject ApplsCut;
    public GameObject CrackWood;

    private void PlayParticles(Vector2 vfxHitPosition, GameObject ParticlesPref)
    {
        GameObject vfx = Instantiate(ParticlesPref, vfxHitPosition, Quaternion.identity);

        ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
        if (ps == null)
        {
            ParticleSystem psChild = vfx.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(vfx, psChild.main.duration);
        }
        else
            Destroy(vfx, ps.main.duration);
    }

    public void PlayWoodHit(Vector2 vfxHitPosition)
    {
        PlayParticles(vfxHitPosition, WoodHit);
    }

    public void PlayClank(Vector2 vfxHitPosition)
    {
        PlayParticles(vfxHitPosition, KnifeClank);
    }

    public void PlayAppleCut(Vector2 vfxHitPosition)
    {
        PlayParticles(vfxHitPosition, ApplsCut);
    }

    public void PlayCrackWood(Vector2 vfxHitPosition)
    {
        PlayParticles(vfxHitPosition, CrackWood);
    }
}
