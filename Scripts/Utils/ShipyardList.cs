using System.Collections;
using System.Collections.Generic;

namespace GloryOfRitiria.Scripts.Utils;

// A collection of shipConstruction slots order in following manner:
// First empty slots (should only have a few at once), then building slots, then docked ships.
public class ShipyardList: IEnumerable<Shipyard>
{
    private List<Shipyard> _elements;
    private int _buildingEndIndex, _emptyEndIndex = 0;

    public ShipyardList()
    {
        _elements = new List<Shipyard>();
    }
    
    // Adds a slot to the list, conserving the order
    public void Add(Shipyard slot)
    {
        switch (slot.State)
        {
            case SlotState.Empty:
                _elements.Insert(_emptyEndIndex, slot);
                _emptyEndIndex++;
                _buildingEndIndex++;
                break;
            case SlotState.Busy:
                _elements.Insert(_buildingEndIndex, slot);
                _buildingEndIndex++;
                break;
        }
    }

    public IEnumerator<Shipyard> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}