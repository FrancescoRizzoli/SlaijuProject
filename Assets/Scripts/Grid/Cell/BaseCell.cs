using Cysharp.Threading.Tasks;
using Grid.Cell;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public enum CellID
    {
        None,
        City,
        CurveRoad,
        Empty,
        Environment,
        Exit,
        Generator,
        Start,
        StraightRoad,
        CrossRoad
    }

    public class BaseCell : MonoBehaviour
    {
        public enum Side
        {
            left,
            right,
            forward,
            backward
        }

        [SerializeField] private CellID _ID = CellID.None;
        [SerializeField] private Side[] safeSides = Array.Empty<Side>();
        [SerializeField] private SelectDeselectView selectDeselectView = null;

        public bool isSwitching { get; set; } = false;
        public CellID ID { get { return _ID; } protected set { _ID = value; } }

        private List<Vector3> _safeSide = new List<Vector3>();
        public List<Vector3> safeSide 
        {
            get { return  _safeSide; }
        }

        private void Awake() => SetSafeSides();

        private void SetSafeSides()
        {
            foreach (Side side in safeSides)
            {
                switch (side)
                {
                    case Side.left:
                        _safeSide.Add(-transform.right);
                        break;
                    case Side.right:
                        _safeSide.Add(transform.right);
                        break;
                    case Side.forward:
                        _safeSide.Add(transform.forward);
                        break;
                    case Side.backward:
                        _safeSide.Add(-transform.forward);
                        break;
                }
            }
        }
        protected void EnableAllSafeSides()
        {
            safeSide.Clear();
            safeSide.Add(-transform.right);
            safeSide.Add(transform.right);
            safeSide.Add(transform.forward);
            safeSide.Add(-transform.forward);
        }

        public void SelectDeselect()
        {
            if (selectDeselectView != null)
                selectDeselectView.ChangeView().Forget();
        }
        public void PlayParticleExchange()
        {
            if (selectDeselectView != null)
                selectDeselectView.PlayParticleSystems().Forget();
        }
    }
}
