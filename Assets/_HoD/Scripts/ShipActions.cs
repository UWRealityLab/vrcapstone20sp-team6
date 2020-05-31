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
        public List<Transform> masts;
        [SerializeField]
        public bool hoistDown;
        [SerializeField]
        public int cableScale;
        [SerializeField]
        public Transform hoistCable;
        [SerializeField]
        public Transform hoistPlatform;
        [SerializeField]
        public int braceDir;

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
            braceDir = 1;
        }

        void Star()
        {
            braceDir = 2;
        }

        void Norm()
        {
            braceDir = 0;
        }

        void Hoist()
        {
            hoistDown = !hoistDown;
        }

        void bracePort(Transform mastOrSail) {
            if ((int)mastOrSail.transform.localEulerAngles.y > 315 || (int)mastOrSail.transform.localEulerAngles.y == 0 || (int)mastOrSail.transform.localEulerAngles.y < 46) {
                mastOrSail.transform.Rotate(0, -3, 0);
            }
        }

        void braceStar(Transform mastOrSail) {
            if ((int)mastOrSail.transform.localEulerAngles.y < 45 || (int)mastOrSail.transform.localEulerAngles.y == 0 || (int)mastOrSail.transform.localEulerAngles.y > 314) {
                    mastOrSail.transform.Rotate(0, 3, 0);
            }
        }

        void braceNorm(Transform mastOrSail) {
            if ((int)mastOrSail.transform.localEulerAngles.y > 0 && (int)mastOrSail.transform.localEulerAngles.y < 180) {
                mastOrSail.transform.Rotate(0, -3, 0);
            } else if ((int)mastOrSail.transform.localEulerAngles.y > 180) {
                mastOrSail.transform.Rotate(0, 3, 0);
            }
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
                if (cableScale < 70) {
                    cableScale++;
                    hoistCable.transform.localScale = new Vector3(0.06936289f, 7f * (cableScale / 70f), 0.06936289f);
                    hoistPlatform.transform.Translate(0f, -0.045f, 0f);
                }
            } else {
                if (cableScale > 10) {
                    cableScale--;
                    hoistCable.transform.localScale = new Vector3(0.06936289f, 7f * (cableScale / 70f), 0.06936289f);
                    hoistPlatform.transform.Translate(0f, 0.045f, 0f);
                }
            }

            if (sailsDown) {
                foreach (GameObject mainSail in mainSails) {
                    mainSail.SetActive(true);
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    mizzenSail.SetActive(true);
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
            } else {
                foreach (GameObject mainSail in mainSails) {
                    mainSail.SetActive(false);
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    mizzenSail.SetActive(false);
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
            }

            if (braceDir == 1) {
                foreach (Transform mast in masts) {
                    bracePort(mast);
                }
                foreach (GameObject mainSail in mainSails) {
                    bracePort(mainSail.GetComponent<Transform>());
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    bracePort(mizzenSail.GetComponent<Transform>());
                }
            } else if (braceDir == 2) {
                foreach (Transform mast in masts) {
                    braceStar(mast);
                }
                foreach (GameObject mainSail in mainSails) {
                    braceStar(mainSail.GetComponent<Transform>());
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    braceStar(mizzenSail.GetComponent<Transform>());
                }
            } else {
                foreach (Transform mast in masts) {
                    braceNorm(mast);
                }
                foreach (GameObject mainSail in mainSails) {
                    braceNorm(mainSail.GetComponent<Transform>());
                }
                foreach (GameObject mizzenSail in mizzenSails) {
                    braceNorm(mizzenSail.GetComponent<Transform>());
                }
            }
        }

    }
}