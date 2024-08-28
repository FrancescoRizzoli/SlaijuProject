using Architecture;
using Cysharp.Threading.Tasks;
using Grid;
using Grid.Cell;
using System;
using UnityEngine;
using Utility;

namespace Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterStateController : AStateController
    {
#if UNITY_EDITOR
        [SerializeField] private BaseCell cell = null;
#endif

        [SerializeField] private AState[] state = Array.Empty<AState>();
        [SerializeField] private AState initialState = null;
        public CharacterData characterData = null;
        [SerializeField] private CharacterView characterView = null;
        public Animator animator { get; private set; } = null;

        private AState currentState = null;
        public Vector3 currentCellEntranceSide = Vector3.zero;

        private const float COSINE_TOLERANCE = 0.71f; // approximation of cos(45°) = 0.707

        public delegate void CharacterEvent();
        public event CharacterEvent OnVictory = null;
        public event CharacterEvent OnDeath = null;

        private void Start()
        {
            animator = GetComponent<Animator>();
            Init();
        }

        private void Update()
        {
            if (currentState == null)
                return;

            currentState.Tick();


#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
                HandleCell(cell).Forget();
#endif
        }

        protected override void Init()
        {
            foreach (AState s in state)
            {
                s.OnChangeStateRequest += ChangeState;
                s.Init(this);
            }

            ChangeState(initialState).Forget();
        }

        protected override async UniTask ChangeState(AState state)
        {
            if (currentState == state)
                return;

            if (currentState != null)
            {
                AState closingState = currentState;
                currentState = null;
                await closingState.Exit();
            }

            await state.Enter();
            currentState = state;
        }

        public async UniTask HandleCell(BaseCell cell)
        {
            bool safe = CheckCellSafeSide(cell);
            Debug.Log(cell.ID);
            if (safe)
            {
                switch (cell.ID)
                {
                    case CellID.CurveRoad:
                        TurningState turningState = (TurningState)FindState<TurningState>();
                        turningState.target = ((CurveCell)cell).GetExitPosition(currentCellEntranceSide);
                        turningState.controlPoint = cell.transform.position;
                        await ChangeState(turningState);
                        break;
                    case CellID.Start:
                        ((StartCell)cell).HandleCharacterAppearence();
                        await UniTask.WaitForSeconds(characterData.waitTimeAtStart);
                        await ChangeState(FindState<WalkingState>());
                        break;
                    case CellID.StraightRoad:
                    case CellID.CrossRoad:
                        await ChangeState(FindState<WalkingState>());
                        break;
                    case CellID.Generator:
                    case CellID.City:
                        AttackingState attackingState = (AttackingState)FindState<AttackingState>();
                        attackingState.targetCell = (DestructibleCell)cell;
                        await ChangeState(attackingState);
                        break;
                    case CellID.Exit:
                        WinState winState = (WinState)FindState<WinState>();
                        winState.targetPosition = cell.transform.position;
                        await ChangeState(winState);
                        break;
                    default:
                        Debug.LogWarning("Cell is null");
                        break;
                }
            }
            else
                await ChangeState(FindState<DeathState>());
        }

        public void Warn(BaseCell cell)
        {

            if (CheckCellSafeSide(cell))
                return;

            characterView.ChangeView().Forget();

        }
        public void Warn(BaseCell cell, Vector3 predictedDirection)
        {
            Debug.Log("next direction" + predictedDirection);

            if (CheckCellSafeSide(cell, predictedDirection))
                return;

            characterView.ChangeView().Forget();

        }

        private AState FindState<T>() where T : AState
        {
            foreach (AState s in state)
                if (s.GetType() == typeof(T))
                    return s;

            return null;
        }

        private bool CheckCellSafeSide(BaseCell cell)
        {
            currentCellEntranceSide = GetCellEntranceSide(cell);

            if (cell.isSwitching)
                return false;


            foreach (Vector3 safeSide in cell.safeSide)
                if (currentCellEntranceSide == safeSide)
                    return true;

            return false;
        }

        private bool CheckCellSafeSide(BaseCell cell, Vector3 predictedDirection)
        {
           

            if (cell.isSwitching)
                return false;


            foreach (Vector3 safeSide in cell.safeSide)
            {
                Debug.Log(safeSide);
                if (predictedDirection == -safeSide)
                    return true;

            }

            return false;
        }


        private Vector3 GetCellEntranceSide(BaseCell cell)
        {
            if (Vector3.Dot(transform.forward, cell.transform.right) >= COSINE_TOLERANCE)
                return -cell.transform.right;

            if (Vector3.Dot(transform.forward, -cell.transform.right) >= COSINE_TOLERANCE)
                return cell.transform.right;

            if (Vector3.Dot(transform.forward, cell.transform.forward) >= COSINE_TOLERANCE)
                return -cell.transform.forward;

            return cell.transform.forward;
        }

        private void Victory() => OnVictory?.Invoke();

        private void Death() => OnDeath?.Invoke();

        private void DeathShake() => CameraShake.ShakeCinemachineTransform(characterData.cameraShakeDuration, characterData.cameraShakeMagnitude);

        public void SetCharacterSpeed(float speed)
        {
            ((WalkingState)FindState<WalkingState>()).movementSpeed = speed;
            ((TurningState)FindState<TurningState>()).SetTurningSpeed(speed);
            ((WinState)FindState<WinState>()).movementSpeed = speed;
        }

        private void DamageCell() => ((AttackingState)FindState<AttackingState>()).DamageTargetCell();

    }
}
