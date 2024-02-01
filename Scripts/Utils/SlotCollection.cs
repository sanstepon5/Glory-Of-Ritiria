using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GloryOfRitiria.Scripts.Utils;

// A collection of shipConstruction slots order in following manner:
// First full slots, then slots in progress, then empty slots, at the end always a single locked slot.
public class SlotCollection: IEnumerable<ShipConstructionSlot>
{
    private List<ShipConstructionSlot> _elements = new();
    private int _fullEndIndex, _buildingEndIndex, _emptyEndIndex = 0;

    public SlotCollection()
    {
        // By default has only a single locked slot
        _elements.Add(new ShipConstructionSlot());
    }
    
    // Adds a slot to the list, conserving the order
    public void Add(ShipConstructionSlot slot)
    {
        switch (slot.State)
        {
            case SlotState.Full:
                _elements.Insert(_fullEndIndex, slot);
                _fullEndIndex++;
                _buildingEndIndex++;
                _emptyEndIndex++;
                break;
            case SlotState.Building:
                _elements.Insert(_buildingEndIndex, slot);
                _buildingEndIndex++;
                _emptyEndIndex++;
                break;
            case SlotState.Empty:
                _elements.Insert(_emptyEndIndex, slot);
                _emptyEndIndex++;
                break;
            case SlotState.Locked:
                _elements.Insert(_elements.Count, slot);
                break;
        }
    }

    public IEnumerator<ShipConstructionSlot> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}