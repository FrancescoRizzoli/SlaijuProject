using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core;
using System;
using UnityEngine.Audio;
using Utility;

namespace Grid
{
    public class GridControls : MonoBehaviour
    {
        Vector2Int emptyVector = new Vector2Int(-1, -1);
        CellExchange exchange;
        PlayerInput playerInput;
        [SerializeField]
        GridPositionController positionController;
        GridComponent gridComponent;
        [SerializeField]
        Vector2Int currentSelectedCell = new Vector2Int(-1, -1);
        [SerializeField]
        List<BaseCell> interchangebleCells;
        private bool isExchanging = false;

        [Header("Audio")]
        [SerializeField]
        AudioMixerGroup sfxGroup;
        [SerializeField]
        AudioClip selectAudio;
        [SerializeField]
        AudioClip deselectAudio;
        [SerializeField]
        AudioClip MoveCell;

        int move = 0;
        public delegate void PlayerMOves(int moveNumber);
        public event PlayerMOves onPlayerMoves;


        private void OnValidate()
        {
        }

        private void Awake()
        {
            if (gridComponent == null)
            {
                if (positionController == null)
                {
                    // Debug.LogError("Position controller not Assigned");
                }
                if (positionController.GetComponent<GridComponent>() != null)
                    gridComponent = positionController.GetComponent<GridComponent>();
            }
            if (playerInput == null)
                playerInput = this.GetComponent<PlayerInput>();
            exchange = this.GetComponent<CellExchange>();
        }

        private void OnEnable()
        {
            playerInput.onObjectClicked += HandleSelected;
        }

        private void OnDisable()
        {
            playerInput.onObjectClicked -= HandleSelected;
        }

        private void HandleSelected(GameObject ob)
        {
            if (isExchanging || ob.tag == "Button")
                return;
            Vector2Int inputPosition = GetWorldPosition(ob.transform.position);
            CheckPosition(inputPosition).Forget();
        }

        private void Update()
        {
            if (positionController.currentGridPosition == currentSelectedCell && currentSelectedCell != emptyVector && positionController.init)
            {
                Debug.Log(currentSelectedCell);
                CellDeselect();
            }
        }

        public async UniTaskVoid CheckPosition(Vector2Int position)
        {
            BaseCell inputCell = gridComponent.gridArray[position.x, position.y];

            if (inputCell.ID == CellID.CurveRoad || inputCell.ID == CellID.StraightRoad)
            {
                if (currentSelectedCell == position)
                {
                    AudioManager.instance.PlayAudioClip(selectAudio, sfxGroup);
                    CellDeselect();

                }
                else if (currentSelectedCell != emptyVector)
                {
                    AudioManager.instance.PlayAudioClip(deselectAudio, sfxGroup);
                    Debug.Log("different");

                    CellDeselect();
                    currentSelectedCell = position;
                    CellSelect();
                }
                else
                {
                    AudioManager.instance.PlayAudioClip(deselectAudio, sfxGroup);
                    currentSelectedCell = position;
                    CellSelect();
                }
            }
            else if (interchangebleCells == null || interchangebleCells.Count == 0)
            {
                return;
            }
            else if (inputCell.ID == CellID.Empty && interchangebleCells.Contains(inputCell))
            {
                Debug.Log("Exchange cell " + inputCell + " with " + gridComponent.gridArray[currentSelectedCell.x, currentSelectedCell.y]);
                isExchanging = true;
                move++;
                onPlayerMoves?.Invoke(move);
                AudioManager.instance.PlayAudioClip(MoveCell, sfxGroup);
                await exchange.Exchange(inputCell, gridComponent.gridArray[currentSelectedCell.x, currentSelectedCell.y]);
                AudioManager.instance.PlayAudioClip(MoveCell, sfxGroup);
                UpdateGridAfterExchange(position, GetWorldPosition(inputCell.transform.position));
                isExchanging = false;
                if (currentSelectedCell != emptyVector)
                {
                    CellDeselect();
                    gridComponent.gridArray[position.x, position.y].SelectDeselect();
                }
            }
        }

        private void CellSelect()
        {
            
          
            gridComponent.gridArray[currentSelectedCell.x, currentSelectedCell.y].SelectDeselect();
            Vector2Int currentPos = Vector2Int.FloorToInt(currentSelectedCell);
            SelectCrossCells(currentPos);
            viewCrossSelection();
        }

        private void CellDeselect()
        {


            try
            {

                gridComponent.gridArray[currentSelectedCell.x, currentSelectedCell.y].SelectDeselect();
                Debug.Log(currentSelectedCell + "deselect");
            }
            catch (IndexOutOfRangeException e)
            {

                Debug.LogWarning($"Index out of range: {e.Message}");
            }
            catch (Exception e)
            {
                // Handle any other exceptions
                Debug.LogError($"An error occurred: {e.Message}");
            }
            
            if (gridComponent.gridArray[currentSelectedCell.x, currentSelectedCell.y].ID == CellID.Empty)
                gridComponent.gridArray[currentSelectedCell.x, currentSelectedCell.y].SelectDeselect();
            currentSelectedCell = emptyVector;
            viewCrossSelection();
            interchangebleCells.Clear();
        }

        private void SelectCrossCells(Vector2Int center)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (var direction in directions)
            {
                Vector2Int pos = center;
                while (true)
                {
                    pos += direction;
                   
                    if (!gridComponent.IsValidPosition(pos) || gridComponent.gridArray[pos.x, pos.y].ID != CellID.Empty)
                        break;

                    interchangebleCells.Add(gridComponent.gridArray[pos.x, pos.y]);

                }
            }

        }
        private void viewCrossSelection()
        {
            if (interchangebleCells.Count > 0)
            {
                foreach (BaseCell cell in interchangebleCells)
                    cell.SelectDeselect();
            }
        }

        public void ResetCrossSelect()
        {
            if (currentSelectedCell == emptyVector || isExchanging) 
                return;

           
            viewCrossSelection();
            interchangebleCells.Clear();
            
            SelectCrossCells(currentSelectedCell);
            //Debug.Log(interchangebleCells.Count);
            viewCrossSelection();


        }

        private Vector2Int GetWorldPosition(Vector3 position)
        {
            return gridComponent.WorldToGridPosition(position);
        }

        private void UpdateGridAfterExchange(Vector2Int pos1, Vector2Int pos2)
        {
            BaseCell temp = gridComponent.gridArray[pos1.x, pos1.y];
            gridComponent.gridArray[pos1.x, pos1.y] = gridComponent.gridArray[pos2.x, pos2.y];
            gridComponent.gridArray[pos2.x, pos2.y] = temp;
        }
    }
}
