using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Udomugo.OculusVRTutorial;

// Create menu of all scenes included in the build.
namespace Com.Udomugo.HoD
{
    public class ShipActions : MonoBehaviour
    {

        [SerializeField]
        public Transform crank;
        [SerializeField]
        public bool anchorDown;
        [SerializeField]
        public Transform anchor;

        void Start()
        {
            DebugUIBuilder.instance.AddLabel("Ship Actions");

            DebugUIBuilder.instance.AddButton("Raise/Lower Sails", () => RaiseLowerSails());
            DebugUIBuilder.instance.AddButton("Weigh Anchor/Anchor", () => Anchor());
            DebugUIBuilder.instance.AddButton("Brace Portside", () => Port());
            DebugUIBuilder.instance.AddButton("Brace Starboard", () => Star());

            DebugUIBuilder.instance.Show();
        }

        void RaiseLowerSails()
        {
            
        }

        void Anchor()
        {
            anchorDown = !anchorDown;
        }

        void Port()
        {

        }

        void Star()
        {

        }

        void Update()
        {
            if (anchorDown && crank.transform.rotation.y < 270) {
                crank.transform.localEulerAngles = new Vector3(0f, crank.transform.rotation.y + 5, 0f);
                anchor.transform.localPosition = new Vector3(anchor.transform.position.x, crank.transform.position.y, anchor.transform.position.z);
            } else if (crank.transform.rotation.y > 0) {
                crank.transform.localEulerAngles = new Vector3(0f, crank.transform.rotation.y - 5, 0f);
                anchor.transform.localPosition = new Vector3(anchor.transform.position.x, crank.transform.position.y - 200, anchor.transform.position.z);
            }
        }

    }
}