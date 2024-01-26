using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour
{
    [SerializeField]
    protected int beltItemId = -1;



    abstract public void SetBeltId(int id, int rot = 0);
    
    abstract public bool IsTransferAble(int id, int rot);

    // Belt�� ȣ��. indir �־��ָ��
    abstract public void EraseNextBelt(int rot);

}
