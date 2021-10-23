using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Warrior : AI_Base
{
    public override void OnBreackEnter(Collider2D col){
        Destroy(col.gameObject);
        Helpers.AudioManager.instance.PlayClip("break", 0.2f);
    }

    public override void Raycasting(){
        base.Raycasting();
        if (CollBlock!= null && CollBlock.CompareTag(TAG.TAG_BREACK) && CollBlock.gameObject.layer!=13){
            direction *= -1;
        }
    }
}
