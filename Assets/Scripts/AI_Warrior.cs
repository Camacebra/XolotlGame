using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Warrior : AI_Base
{
    public override void OnBreackEnter(Collider2D col){
        Destroy(col.gameObject);
    }

    public override void Raycasting(){
        base.Raycasting();
        if (hit &&  hit.collider.CompareTag(TAG.TAG_BREACK)){
            direction *= -1;
        }
    }
}
