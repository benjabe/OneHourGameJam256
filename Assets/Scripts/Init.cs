using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yeeter;

public class Init : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<LuaObjectComponent>().Load("Start");
    }
    private void Start()
    {
        InGameDebug.Log(File.ReadAllText(Path.Combine(
                Application.streamingAssetsPath, "Data", "Core", "Scripts", "Generated", "Bindings.generated.lua")));
        InGameDebug.Log("Unfortunately due to time constraints we did not have time to implement gravity properly. Luckily the devs left a handy set of tools to implement your own! Above's a list of commands and variables you can use. The most important ones are probably the ones found in Gravity");
        InGameDebug.Log("Unfortunately we were also left with too little time to document these functions, so you'll have to play around with it on your own.");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
