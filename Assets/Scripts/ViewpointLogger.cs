using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ViewpointLogger : MonoBehaviour
{
    public GameObject room;
    public float updateInterval = 1.0f;

    private string filePath;
    private float lastLogTime;
    private Transform player;
    private Transform cam;

    public void Start()
    {
        string time = DateTime.Now.ToString();
        filePath = Path.Combine("output_logs", time + ".txt");

        player = gameObject.transform;
        cam = gameObject.GetComponentInChildren<Camera>().transform;
    }

    public void Update()
    {
        if (Time.time - lastLogTime < updateInterval)
        {
            return;
        }

        lastLogTime = Time.time;
        string roomName = room.name;
        Vector3 loc = player.position - room.transform.position;
        loc = new Vector3(
            loc.x / Constants.SCALE,
            loc.z / Constants.SCALE,
            cam.position.y / Constants.H_SCALE);
        float rot = player.eulerAngles.y;
        rot = (rot + 270.0f) % 360.0f;
        if (rot > 180.0f)
        {
            rot = -(360.0f - rot);
        }
        rot = -rot;
        float horz = cam.eulerAngles.x;

        JObject log = new JObject(
            new JProperty("time", Time.time),
            new JProperty("room", roomName),
            new JProperty("viewpoint",
                new JObject(
                    new JProperty("location",
                        new JObject(
                            new JProperty("x", loc.x),
                            new JProperty("y", loc.y),
                            new JProperty("z", loc.z))),
                    new JProperty("rotation", rot),
                    new JProperty("horizon", horz))));

        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLineAsync(JsonConvert.SerializeObject(log));
        }
    }
}
