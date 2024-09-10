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
        CrossRoad,
        LevelEditorEmpty
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

        public SelectDeselectView SelectDeselectView { get { return selectDeselectView; } }

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

        public void RecalculateSafeSides()
        {
            _safeSide.Clear();
            SetSafeSides();
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

        public void GetEnterDirection(Vector3 position)
        {
            Vector3 localPosition = transform.InverseTransformPoint(position);
            Debug.Log("local pos"+localPosition);
        }
        public Side GetEnterDirectionSide(Vector3 direction)
        {
            // Convert the direction to the local space of the cell
            Vector3 localDirection = transform.InverseTransformDirection(direction.normalized);

            // Determine which axis the local direction is closest to
            if (Mathf.Abs(localDirection.z) > Mathf.Abs(localDirection.x))
            {
                if (localDirection.z > 0)
                    return Side.forward;  // Forward
                else
                    return Side.backward; // Backward
            }
            else
            {
                if (localDirection.x > 0)
                    return Side.right;    // Right
                else
                    return Side.left;     // Left
            }
        }

        public virtual void OnCellExit() { }    // for flexibility purposes, the implementation is left to the children of this class; by default this does nothing

        
    }
}
