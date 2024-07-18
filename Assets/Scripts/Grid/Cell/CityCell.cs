using Grid.Cell;

namespace Grid
{
    public class CityCell : DestructibleCell
    {
#if UNITY_EDITOR
        private void OnValidate() => ID = CellID.City;
#endif
    }
}
