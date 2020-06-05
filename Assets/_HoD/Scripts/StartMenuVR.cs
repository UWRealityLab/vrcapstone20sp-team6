﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Udomugo.OculusVRTutorial;

// Create menu of all scenes included in the build.
namespace Com.Udomugo.HoD
{
    public class StartMenuVR : MonoBehaviour
    {
        public OVROverlay overlay;
        public OVROverlay text;
        public OVRCameraRig vrRig;
        public LauncherVR launcher;

        void Start()
        {
            DebugUIBuilder.instance.AddLabel("Welcome to Hands on Deck");

            /*int n = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < n; ++i)
            {
                string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                var sceneIndex = i;
                DebugUIBuilder.instance.AddButton(Path.GetFileNameWithoutExtension(path), () => LoadScene(sceneIndex));
            }*/

            DebugUIBuilder.instance.AddButton("Connect to Server", () => Connect());

            DebugUIBuilder.instance.Show();
        }

        void LoadScene(int idx)
        {
            DebugUIBuilder.instance.Hide();
            Debug.Log("Load scene: " + idx);
            UnityEngine.SceneManagement.SceneManager.LoadScene(idx);
        }

        void Connect()
        {
            launcher.Connect();
        }


    }
}