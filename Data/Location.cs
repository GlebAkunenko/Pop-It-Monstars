using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Location
{
    public int Level { get; set; } = 1;
    public bool Moving { get; set; }
    public string Name { get; set; }
    public int AllStars { get; set; }
    public byte[] Stars { get; set; } = new byte[100];
    public bool FirstOpen { get; set; } = true;

    public SerializingVector SerPointPos { get; set; }
    public SerializingVector SerCamPos { get; set; }
    public SerializingVector SerReviewPos { get; set; }

    [NonSerialized]
    private Vector3 pointPos;
    [NonSerialized]
    private Vector3 camPos;
    [NonSerialized]
    private Vector3 reviewPos = new Vector3(0, -100);

    public Vector3 PointPos { get => pointPos; set => pointPos = value; }
    public Vector3 CamPos { get => camPos; set => camPos = value; }
    public Vector3 ReviewPos { get => reviewPos; set => reviewPos = value; }

    public void SetVectoresAfterDeserializing()
    {
        PointPos = SerPointPos.ToVector3();
        CamPos = SerCamPos.ToVector3();
        ReviewPos = SerReviewPos.ToVector3();
    }

    public void SetSerializeVectoresBeforeSerializing()
    {
        SerReviewPos = new SerializingVector(ReviewPos);
        SerCamPos = new SerializingVector(CamPos);
        SerPointPos = new SerializingVector(PointPos);
    }

    public Location(string name)
    {
        Name = name;
    }

    public void UpdateAllStars()
    {
        AllStars = 0;
        foreach (byte s in Stars)
            AllStars += s;
    }

    public static Location Forest
    {
        get { return new Location("Forest"); }
    }
}
