using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDimple : Popit
{
    public Type type;


    protected override void OnFull()
    {
        switch (type)
        {
            case Type.freeze:

                break;
            //case Type.heal:
            //    break;
            //case Type.damage:
            //    break;
            default:
                break;
        }
    }

    public enum Type
    {
        freeze
    }
}
