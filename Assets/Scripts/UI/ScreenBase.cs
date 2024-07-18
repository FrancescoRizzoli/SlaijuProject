using Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenBase : MonoBehaviour
    {
        protected Canvas canvas;
        [SerializeField]
        protected ScreenType screenType;
        [SerializeField]
        protected CanvasGroup canvasGroup;
        [SerializeField]
        protected bool disableCameraRendering = false;
        [SerializeField]
        protected int sortOrderModifier = 0;
        [Header("Animation")]
        [SerializeField]
        private RectTransform containerRectTransform;
        [SerializeField]
        private UIAnimation animationType;
        [SerializeField]
        private SimpleDirection slideDirection;
        [SerializeField]
        private float animationDuration;

        private Vector3 defaultPosition;

        protected Coroutine closingCoroutine;

        protected bool isOpen = false;

        protected virtual void Awake()
        {
            if (containerRectTransform != null)
                defaultPosition = containerRectTransform.anchoredPosition;
            canvas = GetComponent<Canvas>();
            UIManager.instance.SetScreenInScene(screenType, this);
        }
        protected virtual void OnDestroy()
        {

        }
        public virtual void OpenScreen(uint id)
        {
            if (isOpen)
                return;

            if (closingCoroutine != null)
                return;

            int sortingOrder = UIManager.instance.ScreenOpening(this, disableCameraRendering) + sortOrderModifier;
            if (sortingOrder == -1)
                return;
            if (containerRectTransform != null)
                containerRectTransform.anchoredPosition = defaultPosition;
            canvas.sortingOrder = sortingOrder;
            canvas.enabled = true;
            isOpen = true;
            playAnimation(true);
            Refresh();
        }
        /// <summary>
        /// method called buy uiManager to refresh all the value in the screen ad example the resource value in the HUD
        /// </summary>
        public virtual void Refresh()
        {

        }

        public virtual void CloseScreen()
        {
            if (!isOpen)
                return;

            //check if the screen was openend with animation
            if (animationType != UIAnimation.None)
                closingCoroutine = StartCoroutine(ClosingCoreRoutine());
            else
                CompleteClosing();

        }

        /// <summary>
        /// if canvas group is present can change the interactabe variable
        /// </summary>
        /// <param name="interactable"></param>
        public void SetInteractebale(bool interactable)
        {
            if (canvasGroup != null)
                canvasGroup.interactable = interactable;
        }

        public void ManageSelf(bool open)
        {
            UIManager.instance.ManageScreen(screenType, open);
        }

        private IEnumerator ClosingCoreRoutine()
        {
            UIManager.instance.HandleCamera(disableCameraRendering);
            //wait for the closing animation to complete before hide the screen
            playAnimation(false);
            yield return new WaitForSeconds(animationDuration);
            CompleteClosing();
        }
        protected virtual void CompleteClosing()
        {
            UIManager.instance.ScreenClosing(this, disableCameraRendering);
            isOpen = false;
            canvas.enabled = false;
            closingCoroutine = null;
        }
        private void playAnimation(bool open)
        {

            switch (animationType)
            {
                case UIAnimation.Slide:
                    StartCoroutine(AnimationCoreRoutine.Slide(containerRectTransform, slideDirection, animationDuration, open));
                    break;
                case UIAnimation.PopUp:
                    StartCoroutine(AnimationCoreRoutine.Scale(containerRectTransform, animationDuration, open));
                    break;
                default:
                    break;

            }
        }



    }
}
