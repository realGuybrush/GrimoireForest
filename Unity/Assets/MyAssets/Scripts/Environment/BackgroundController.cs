using System.Collections.Generic;
using UnityEngine;

namespace MyAssets.Scripts.Environment {

    public class BackgroundController : MonoBehaviour {
        [SerializeField]
        private float minMovementCoefficient = 0.1f, maxMovementCoefficient = 1.0f;

        private List<BackgroundMovement> backgrounds = new List<BackgroundMovement>();

        public void Init(List<BackgroundMovement> Backgrounds) {//todo: add sky and moon as inputs
            var main = Camera.main;
            float speedDropDelta = (maxMovementCoefficient - minMovementCoefficient) / Backgrounds.Count;
            for (int i = 0; i < Backgrounds.Count; i++) {
                backgrounds.Add(Instantiate(Backgrounds[i], transform));
                backgrounds[i].Init(main, maxMovementCoefficient - i * speedDropDelta);
            }
        }

        public void ClearBackgrounds() {//todo: don't delete sky and moon here
            for (int i = transform.childCount - 1; i >= 0; i--) {
                Destroy(transform.GetChild(i));
            }
        }
    }

}
