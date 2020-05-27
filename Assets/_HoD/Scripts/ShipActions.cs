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
        public int crankTurns;
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
        [SerializeField]
        public bool hoistDown;
        [SerializeField]
        public int cableScale;
        [SerializeField]
        public Transform hoistCable;
        [SerializeField]
        public Transform hoistPlatform;

        void Start()
        {
            DebugUIBuilder.instance.AddLabel("Ship Actions");

            DebugUIBuilder.instance.AddButton("Raise/Lower Sails", () => RaiseLowerSails());
            DebugUIBuilder.instance.AddButton("Weigh Anchor/Anchor", () => Anchor());
            DebugUIBuilder.instance.AddButton("Brace Portside", () => Port());
            DebugUIBuilder.instance.AddButton("Brace Starboard", () => Star());
            DebugUIBuilder.instance.AddButton("Brace Normal", () => Norm());
            DebugUIBuilder.instance.AddButton("Raise/Lower Hoist", () => Hoist());

            DebugUIBuilder.instance.Show();
        }

        void RaiseLowerSails()
        {
            sailsDown = !sailsDown;
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

        void Hoist()
        {
            hoistDown = !hoistDown;
        }

        void Update()
        {
            if (anchorDown && crankTurns < 30) {
                crank.transform.Rotate(0f, 3f, 0f);
                crankTurns++;
                // crank.transform.localEulerAngles = new Vector3(0f, crank.transform.rotation.y + 2f, 0f);
            } else if (!anchorDown && crankTurns > 0) {
                crank.transform.Rotate(0f, -3f, 0f);
                crankTurns--;
            }

            if (hoistDown) {
                if (hoistPlatform.transform.position.y > -0.1f) {
                    hoistPlatform.transform.Translate(0f, -0.01f, 0f);
                }
                if (cableScale < 90) {
                    cableScale++;
                    hoistCable.transform.localScale = new Vector3(0.06936289f, 7 * (7 * (cableScale / 90) / 7), 0.06936289f);
                }
            } else {
                if (hoistPlatform.transform.position.y < 0f) {
                    hoistPlatform.transform.Translate(0f, 0.01f, 0f);
                }
                if (cableScale > 1) {
                    cableScale--;
                    hoistCable.transform.localScale = new Vector3(0.06936289f, 7 * (7 * (cableScale / 90) / 7), 0.06936289f);
                }
            }

            if (sailsDown) {
                foreach (GameObject mainSail in mainSails) {
                    mainSail.active = true;
                    mainSail.SetActive(true);
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    mizzenSail.active = true;
                    mizzenSail.SetActive(true);
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
            } else {
                foreach (GameObject mainSail in mainSails) {
                    mainSail.active = false;
                    mainSail.SetActive(false);
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    mizzenSail.active = false;
                    mizzenSail.SetActive(false);
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
            }
        }

    }
}