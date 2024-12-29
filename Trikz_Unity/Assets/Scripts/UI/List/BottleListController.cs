using System.Collections.Generic;
using UnityEngine.UIElements;
public class BottleListController : UIView
{
    // UXML template for list entries
    VisualTreeAsset _listElementTemplate;

    // UI element references
    ListView _bottleList;

    List<BottleData> _allBottles;

    BottleData _equippedBottle;
    
    public BottleListController(VisualElement topElement, VisualTreeAsset listElementTemplate, List<BottleData> bottleDataList) : base(topElement, false)
    {
        // Create list of all types of bottles
        _allBottles = bottleDataList;
        // Store a reference to the template for the list entries
        _listElementTemplate = listElementTemplate;
        // Fill the UI list with all bottles types
        FillBottleList();

        MenuEvents.BottleEquipped += OnBottleEquipped;
    }

    public override void Dispose()
    {
        base.Dispose();
        MenuEvents.BottleEquipped -= OnBottleEquipped;
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _bottleList = _topElement as ListView;
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        _bottleList.selectionChanged += OnBottleSelected;
    }

    protected override void UnRegisterCallbacks()
    {
        base.RegisterCallbacks();
        _bottleList.selectionChanged += OnBottleSelected;
    }

    

    void FillBottleList()
    {
        // Set up a make item function for a list entry
        _bottleList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = _listElementTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new BottleListElementController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        _bottleList.bindItem = (item, index) =>
        {
            (item.userData as BottleListElementController)?.SetBottleData(_allBottles[index]);
            if (_allBottles[index] == _equippedBottle) item.AddToClassList("BottleListElement--equipped");
            else item.RemoveFromClassList("BottleListElement--equipped");
        };

        // Set a fixed item height matching the height of the item provided in makeItem. 
        // For dynamic height, see the virtualizationMethod property.
        _bottleList.fixedItemHeight = 200;

        // Set the actual item's source list/array
        _bottleList.itemsSource = _allBottles;
    }

    void OnBottleSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedBottle = _bottleList.selectedItem as BottleData;

        MenuEvents.BottleSelectionChanged(selectedBottle);
    }

    void OnBottleEquipped(BottleData data)
    {
        _equippedBottle = data;
        _bottleList.Rebuild();
    }
}
