using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Udomugo.OculusVRTutorial;
using Photon.Pun;

// Create menu of all scenes included in the build.
namespace Com.Udomugo.HoD
{
    public class ShipActions : MonoBehaviour
    {

        private PhotonView PV;
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

        public GameObject trim;
        void Start()
        {
            // foreach (GameObject sail in mainSails)
            // {
            //     sail.GetComponent<MeshRenderer>().enabled = false;
            // }
            // foreach (GameObject sail in mizzenSails)
            // {
            //     sail.GetComponent<MeshRenderer>().enabled = false;
            // }
            PV = GetComponent<PhotonView>();
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
            if (!PV.IsMine) { PV.RequestOwnership(); }
            sailsDown = !sailsDown;
            AudioManager.instance.Play("sail_deploy_1");
            AudioManager.instance.Play("sail_deploy_2");
            PV.RPC("RaiseLowerSailsRPC", RpcTarget.OthersBuffered, sailsDown);
        }

        void Anchor()
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            anchorDown = !anchorDown;
            PV.RPC("AnchorRPC", RpcTarget.OthersBuffered, anchorDown);
        }

        void Port()
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            braceDir = 1;
            PV.RPC("SetBraceDirRPC", RpcTarget.OthersBuffered, braceDir);
        }

        void Star()
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            braceDir = 2;
            PV.RPC("SetBraceDirRPC", RpcTarget.OthersBuffered, braceDir);
        }

        void Norm()
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            braceDir = 0;
            PV.RPC("SetBraceDirRPC", RpcTarget.OthersBuffered, braceDir);
        }

        void Hoist()
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            hoistDown = !hoistDown;
            PV.RPC("HoistRPC", RpcTarget.OthersBuffered, hoistDown);
        }

        void bracePort(Transform mastOrSail)
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            if ((int)mastOrSail.transform.localEulerAngles.y > 315 || (int)mastOrSail.transform.localEulerAngles.y == 0 || (int)mastOrSail.transform.localEulerAngles.y < 46)
            {
                mastOrSail.transform.Rotate(0, -3, 0);
                trim.transform.rotation = mastOrSail.transform.rotation;
                PV.RPC("BracePortRPC", RpcTarget.OthersBuffered, mastOrSail);
            }
        }

        void braceStar(Transform mastOrSail)
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            if ((int)mastOrSail.transform.localEulerAngles.y < 45 || (int)mastOrSail.transform.localEulerAngles.y == 0 || (int)mastOrSail.transform.localEulerAngles.y > 314)
            {
                mastOrSail.transform.Rotate(0, 3, 0);
                trim.transform.rotation = mastOrSail.transform.rotation;
                PV.RPC("BraceStarRPC", RpcTarget.OthersBuffered, mastOrSail);
            }
        }

        void braceNorm(Transform mastOrSail)
        {
            if (!PV.IsMine) { PV.RequestOwnership(); }
            if ((int)mastOrSail.transform.localEulerAngles.y > 0 && (int)mastOrSail.transform.localEulerAngles.y < 180)
            {
                mastOrSail.transform.Rotate(0, -3, 0);
                trim.transform.rotation = mastOrSail.transform.rotation;
                PV.RPC("BracePortRPC", RpcTarget.OthersBuffered, mastOrSail);
            }
            else if ((int)mastOrSail.transform.localEulerAngles.y > 180)
            {
                mastOrSail.transform.Rotate(0, 3, 0);
                trim.transform.rotation = mastOrSail.transform.rotation;
                PV.RPC("BraceStarRPC", RpcTarget.OthersBuffered, mastOrSail);
            }
        }

        void Update()
        {
            if (anchorDown && crankTurns < 30)
            {
                crank.transform.Rotate(0f, 3f, 0f);
                crankTurns++;
                AudioManager.instance.Play("anchor");
                // crank.transform.localEulerAngles = new Vector3(0f, crank.transform.rotation.y + 2f, 0f);
            }
            else if (!anchorDown && crankTurns > 0)
            {
                crank.transform.Rotate(0f, -3f, 0f);
                crankTurns--;
                AudioManager.instance.Play("anchor");
            }
            else
            {
                AudioManager.instance.Stop("anchor");
            }

            if (hoistDown)
            {
                if (cableScale < 70)
                {
                    AudioManager.instance.Play("hoist");
                    cableScale++;
                    hoistCable.transform.localScale = new Vector3(0.06936289f, 4.8f * (cableScale / 70f), 0.06936289f);
                    hoistPlatform.transform.Translate(0f, -0.042f, 0f);
                }
                else
                {
                    AudioManager.instance.Stop("hoist");
                }
            }
            else
            {
                if (cableScale > 10)
                {
                    AudioManager.instance.Play("hoist");
                    cableScale--;
                    hoistCable.transform.localScale = new Vector3(0.06936289f, 4.8f * (cableScale / 70f) + ((10 / cableScale) * 0.314f), 0.06936289f);
                    hoistPlatform.transform.Translate(0f, 0.042f, 0f);
                }
                else
                {
                    AudioManager.instance.Stop("hoist");
                }
            }

            if (sailsDown)
            {
                foreach (GameObject mainSail in mainSails)
                {
                    mainSail.SetActive(true);
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                foreach (GameObject mizzenSail in mizzenSails)
                {
                    mizzenSail.SetActive(true);
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
                AudioManager.instance.Play("sail_flap_1");
                AudioManager.instance.Play("sail_flap_2");
                if (!anchorDown)
                {
                    AudioManager.instance.Play("splash_calm");
                }
                else
                {
                    AudioManager.instance.Stop("splash_calm");
                }
            }
            else
            {
                foreach (GameObject mainSail in mainSails)
                {
                    mainSail.SetActive(false);
                    mainSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                foreach (GameObject mizzenSail in mizzenSails)
                {
                    mizzenSail.SetActive(false);
                    mizzenSail.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                AudioManager.instance.Stop("sail_flap_1");
                AudioManager.instance.Stop("sail_flap_2");
                AudioManager.instance.Stop("splash_calm");
            }
        }

        void FixedUpdate() {
            if (braceDir == 1)
            {
                foreach (Transform mast in masts)
                {
                    bracePort(mast);
                }
                foreach (GameObject mainSail in mainSails)
                {
                    bracePort(mainSail.GetComponent<Transform>());
                }
                foreach (GameObject mizzenSail in mizzenSails)
                {
                    bracePort(mizzenSail.GetComponent<Transform>());
                }
            }
            else if (braceDir == 2)
            {
                foreach (Transform mast in masts)
                {
                    braceStar(mast);
                }
                foreach (GameObject mainSail in mainSails)
                {
                    braceStar(mainSail.GetComponent<Transform>());
                }
                foreach (GameObject mizzenSail in mizzenSails)
                {
                    braceStar(mizzenSail.GetComponent<Transform>());
                }
            }
            else
            {
                foreach (Transform mast in masts)
                {
                    braceNorm(mast);
                }
                foreach (GameObject mainSail in mainSails)
                {
                    braceNorm(mainSail.GetComponent<Transform>());
                }
                foreach (GameObject mizzenSail in mizzenSails)
                {
                    braceNorm(mizzenSail.GetComponent<Transform>());
                }
            }
        }

        [PunRPC]
        void RaiseLowerSailsRPC(bool sailsPosition)
        {
            sailsDown = sailsPosition;
        }

        [PunRPC]
        void AnchorRPC(bool anchorPosition)
        {
            anchorDown = anchorPosition;
        }

        [PunRPC]
        void SetBraceDirRPC(int braceDirection)
        {
            braceDir = braceDirection;
        }

        [PunRPC]
        void HoistRPC(bool hoistPosition)
        {
            hoistDown = hoistPosition;
        }

        [PunRPC]
        void BracePortRPC(Transform mastOrSail)
        {
            mastOrSail.transform.Rotate(0, -3, 0);
        }

        [PunRPC]
        void BraceStarRPC(Transform mastOrSail)
        {
            mastOrSail.transform.Rotate(0, 3, 0);
        }
    }
}