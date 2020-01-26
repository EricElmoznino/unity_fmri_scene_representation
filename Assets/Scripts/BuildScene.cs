using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class BuildScene : MonoBehaviour
{
    public string roomName;

    private GameObject room;

    void Start()
    {
        room = new GameObject(roomName);
        JObject roomData = LoadJsonRoom(roomName);
        PlaceSurfaces((JObject)roomData["floor"], (JObject)roomData["ceiling"], (JArray)roomData["walls"]);
        PlaceLights((JArray)roomData["lights"]);
    }

    private void PlaceSurfaces(JObject floor, JObject ceiling, JArray walls)
    {
        float planeScale = 5;
        GameObject surfaces = new GameObject("Surfaces");
        surfaces.transform.parent = room.transform;

        // Add floor
        GameObject f = GameObject.CreatePrimitive(PrimitiveType.Plane);
        f.transform.parent = surfaces.transform;
        f.name = "floor";
        f.transform.localPosition = new Vector3(
            (float)floor["centre"]["x"] * Constants.SCALE,
            (float)floor["centre"]["z"] * Constants.H_SCALE,
            (float)floor["centre"]["y"] * Constants.SCALE);
        f.transform.localScale = new Vector3(
            (float)floor["size"][0] / 2 / planeScale * Constants.SCALE,
            1,
            (float)floor["size"][1] / 2 / planeScale * Constants.SCALE);
        f.GetComponent<Renderer>().material = Resources.Load<Material>(
            "Materials/" + ((int)floor["type"]).ToString("D7"));


        // Add ceiling
        GameObject c = GameObject.CreatePrimitive(PrimitiveType.Plane);
        c.transform.parent = surfaces.transform;
        c.name = "ceiling";
        c.transform.localPosition = new Vector3(
            (float)ceiling["centre"]["x"] * Constants.SCALE,
            (float)ceiling["centre"]["z"] * Constants.H_SCALE,
            (float)ceiling["centre"]["y"] * Constants.SCALE);
        c.transform.localScale = new Vector3(
            (float)ceiling["size"][0] / 2 / planeScale * Constants.SCALE,
            1,
            (float)ceiling["size"][1] / 2 / planeScale * Constants.SCALE);
        c.transform.localRotation = Quaternion.Euler(180, 0, 0);
        c.GetComponent<Renderer>().material = Resources.Load<Material>(
            "Materials/" + ((int)ceiling["type"]).ToString("D7"));

        // Add walls
        int i = 0;
        foreach (JObject wall in walls)
        {
            GameObject w = GameObject.CreatePrimitive(PrimitiveType.Plane);
            w.transform.parent = surfaces.transform;
            w.name = "wall" + i.ToString();
            w.transform.localPosition = new Vector3(
                (float)wall["centre"]["x"] * Constants.SCALE,
                (float)wall["centre"]["z"] * Constants.H_SCALE,
                (float)wall["centre"]["y"] * Constants.SCALE);
            w.transform.localScale = new Vector3(
                (float)wall["size"][0] / 2 / planeScale * Constants.SCALE,
                1,
                (float)wall["size"][1] / 2 / planeScale * Constants.H_SCALE);
            if ((string)wall["normal"] == "BACK")
            {
                w.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
            else if ((string)wall["normal"] == "LEFT")
            {
                w.transform.localRotation = Quaternion.Euler(-90, 90, 0);
            }
            else if ((string)wall["normal"] == "RIGHT")
            {
                w.transform.localRotation = Quaternion.Euler(-90, -90, 0);
            }
            else
            {
                w.transform.localRotation = Quaternion.Euler(-90, 180, 0);
            }
            w.GetComponent<Renderer>().material = Resources.Load<Material>(
                "Materials/" + ((int)wall["type"]).ToString("D7"));
            i++;
        }
    }

    private void PlaceLights(JArray lights)
    {
        GameObject lightsObj = new GameObject("Lights");
        lightsObj.transform.parent = room.transform;

        int i = 0;
        foreach (JObject light in lights)
        {
            GameObject lightObj = new GameObject("light" + i.ToString());
            lightObj.transform.parent = lightsObj.transform;
            Light l = lightObj.AddComponent<Light>();
            l.range = 20;
            l.intensity = (float)light["intensity"];
            l.transform.localPosition = new Vector3(
                (float)light["location"]["x"] * Constants.SCALE,
                2.0f,
                (float)light["location"]["y"] * Constants.SCALE);
            i++;
        }
    }

    public JObject LoadJsonRoom(string roomName)
    {
        string jsonPath = Path.Combine("Assets", "RoomSpecs", roomName + ".json");
        string json = File.ReadAllText(jsonPath);
        JObject roomData = JsonConvert.DeserializeObject<JObject>(json);
        return roomData;
    }
}
