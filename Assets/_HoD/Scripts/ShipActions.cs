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
        [SerializeField]
        public bool sailsDown;
        [SerializeField]
        public List<GameObject> mainSails;
        [SerializeField]
        public List<GameObject> mizzenSails;
        [SerializeField]
        public List<Transform> mastAndSails;

        void Start()
        {
            DebugUIBuilder.instance.AddLabel("Ship Actions");

            DebugUIBuilder.instance.AddButton("Raise/Lower Sails", () => RaiseLowerSails());
            DebugUIBuilder.instance.AddButton("Weigh Anchor/Anchor", () => Anchor());
            DebugUIBuilder.instance.AddButton("Brace Portside", () => Port());
            DebugUIBuilder.instance.AddButton("Brace Starboard", () => Star());
            DebugUIBuilder.instance.AddButton("Brace Normal", () => Norm());

            DebugUIBuilder.instance.Show();
        }

        void RaiseLowerSails()
        {
            sailsDown = !sailsDown;
            if (sailsDown) {
                foreach (GameObject mainSail in mainSails) {
                    // mainSail.active = true;
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    // mizzenSail.active = true;
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
            } else {
                foreach (GameObject mainSail in mainSails) {
                    // mainSail.active = false;
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    // mizzenSail.active = false;
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
            }
        }

        void Anchor()
        {
            anchorDown = !anchorDown;
        }

        void Port()
        {
            foreach (Transform mastOrSail in mastAndSails) {
                mastOrSail.transform.localEulerAngles = new Vector3(0f, -45f, 0f);
            }
        }

        void Star()
        {
            foreach (Transform mastOrSail in mastAndSails) {
                mastOrSail.transform.localEulerAngles = new Vector3(0f, 45f, 0f);
            }
        }

        void Norm()
        {
            foreach (Transform mastOrSail in mastAndSails) {
                mastOrSail.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

        void Update()
        {
            if (anchorDown && crank.transform.rotation.y < 270) {
                crank.transform.localEulerAngles = new Vector3(0f, crank.transform.rotation.y + 2, 0f);
            } else if (crank.transform.rotation.y > 0) {
                crank.transform.localEulerAngles = new Vector3(0f, crank.transform.rotation.y - 2, 0f);
            }
        }

    }
}