using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        AudioManager.instance.Play("wood_hit");
    }
}
