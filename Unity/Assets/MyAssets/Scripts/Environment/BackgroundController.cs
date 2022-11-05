using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Scripts.Environment {

    public class BackgroundController : MonoBehaviour {
        [SerializeField]
        private float minMovementCoefficient = 0.1f, maxMovementCoefficient = 1.0f;

        [SerializeField]
        private List<BackgroundMovement> backgrounds;

        private void Awake() {
            var main = Camera.main;
            float speedDropDelta = (maxMovementCoefficient - minMovementCoefficient) / backgrounds.Count;
            for (int i = 0; i < backgrounds.Count; i++) {
                backgrounds[i].Init(main, maxMovementCoefficient - i * speedDropDelta);
            }
        }
    }

}
