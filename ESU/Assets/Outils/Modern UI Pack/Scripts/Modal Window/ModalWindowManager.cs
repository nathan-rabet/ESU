using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
    public class ModalWindowManager : MonoBehaviour
    {
        Animator mwAnimator;

        void Start()
        {
            mwAnimator = gameObject.GetComponent<Animator>();
        }

        public void OpenWindow()
        {
            mwAnimator.Play("Fade-in");
        }

        public void CloseWindow()
        {
            mwAnimator.Play("Fade-out");
        }
    }
}