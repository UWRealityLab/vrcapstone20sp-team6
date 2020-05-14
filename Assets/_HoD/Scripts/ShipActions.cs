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
            if (anchorDown) {
                while (crank.quaternion.y < 270) {
                    crank.transform.Rotate(0, crank.quaternion.y + 5, 0);
                }
                anchor.transform.position.y = crank.transform.position.y;
                anchorDown = false;
            } else {
                while (crank.quaternion.y > 0) {
                    crank.transform.Rotate(0, crank.quaternion.y - 5, 0);
                }
                anchor.transform.position.y = crank.transform.position.y - 200;
                anchorDown = true;
            }
        }

        void Port()
        {

        }

        void Star()
        {

        }

    }
}