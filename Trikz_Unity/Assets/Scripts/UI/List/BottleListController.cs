using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UIElements;

public class BottleListController
{
    // UXML template for list entries
    VisualTreeAsset _listElementTemplate;

    // UI element references
    ListView _bottleList;

    List<BottleData> _allBottles;

    public void InitializeBottleList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllBottles();

        // Store a reference to the template for the list entries
        _listElementTemplate = listElementTemplate;

        // Store a reference to the character list element
        _bottleList = root.Q<ListView>("bottle-list");

        // Store references to the selected character info elements
        //m_CharClassLabel = root.Q<Label>("character-class");
        //m_CharNameLabel = root.Q<Label>("character-name");
        //m_CharPortrait = root.Q<VisualElement>("character-portrait");

        FillBottleList();

        // Register to get a callback when an item is selected
        _bottleList.selectionChanged += OnBottleSelected;
    }

    void EnumerateAllBottles()
    {
        _allBottles = new List<BottleData>();
        _allBottles.AddRange(Resources.LoadAll<BottleData>("Bottles"));
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
        };

        // Set a fixed item height matching the height of the item provided in makeItem. 
        // For dynamic height, see the virtualizationMethod property.
        _bottleList.fixedItemHeight = 150;

        // Set the actual item's source list/array
        _bottleList.itemsSource = _allBottles;
    }

    void OnBottleSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedBottle = _bottleList.selectedItem as BottleData;

        // Handle none-selection (Escape to deselect everything)
        if (selectedBottle == null) {
            // Clear
            //m_CharClassLabel.text = "";
            //m_CharNameLabel.text = "";
            //m_CharPortrait.style.backgroundImage = null;

            return;
        }

        MenuEvents.BottleSelectionChanged(selectedBottle);
    }
}
