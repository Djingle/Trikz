using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class BottleListView : UIView {

    private BottleListController _list;
    private BottleStatsController _stats;
    
    private Button _mainMenuButton, _buyEquipButton;

    private List<BottleData> _allBottles;
    private BottleData _selectedBottle;

    public const string k_ListName = "bottle-list";
    public const string k_StatsName = "active-bottle-stats";
    public const string k_MainMenuButtonName = "main-menu-button";
    public const string k_BuyEquipButton = "buy-equip-button";

    public BottleListView(VisualElement topElement, VisualTreeAsset listElementTemplate) : base(topElement)
    {
        EnumerateAllBottles();
        _list = new BottleListController(_topElement.Q<ListView>(k_ListName), listElementTemplate, _allBottles);
        _stats = new BottleStatsController(_topElement.Q<VisualElement>(k_StatsName), _allBottles);

        MenuEvents.BottleSelectionChanged += OnBottleSelectionChanged;
        MenuEvents.BottleEquipped += UpdateBuyEquipButton;
        MenuEvents.TransactionConfirmed += UpdateBuyEquipButton;
    }

    public override void Dispose()
    {
        base.Dispose();
        MenuEvents.BottleSelectionChanged -= OnBottleSelectionChanged;
        MenuEvents.BottleEquipped -= UpdateBuyEquipButton;
        MenuEvents.TransactionConfirmed -= UpdateBuyEquipButton;
    }


    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        _mainMenuButton = _topElement.Q<Button>(k_MainMenuButtonName);
        _buyEquipButton = _topElement.Q<Button>(k_BuyEquipButton);
    }

    protected override void RegisterCallbacks()
    {
        base.RegisterCallbacks();
        _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuClicked);
        _buyEquipButton.RegisterCallback<ClickEvent>(OnBuyEquipClicked);
    }

    protected override void UnRegisterCallbacks()
    {
        base.UnRegisterCallbacks();
        _mainMenuButton.UnregisterCallback<ClickEvent>(OnMainMenuClicked);
        _buyEquipButton.UnregisterCallback<ClickEvent>(OnBuyEquipClicked);
    }

    void EnumerateAllBottles()
    {
        _allBottles = new List<BottleData>();
        _allBottles.AddRange(Resources.LoadAll<BottleData>("Bottles"));
    }

    private void OnBottleSelectionChanged(BottleData data)
    {
        _selectedBottle = data;
        UpdateBuyEquipButton(_selectedBottle);
    }

    private void OnBuyEquipClicked(ClickEvent evt)
    {
        if (BottleDataHelper.IsBought(_selectedBottle)) // Bottle already bought
            MenuEvents.BottleEquipped?.Invoke(_selectedBottle);
        else if (_selectedBottle.Price <= GameManager.Instance.TotalPoints) { // Buy the bottle if player has enough points
            MenuEvents.BottleBought?.Invoke(_selectedBottle);
        }
    }

    private void OnMainMenuClicked(ClickEvent evt)
    {
        GameManager.Instance.ChangeState(GameState.Menu);
        BottlePreviewManager.Instance.SetPreviewActive(false);
    }

    private void UpdateBuyEquipButton(BottleData bottle)
    {
        if (BottleDataHelper.IsBought(bottle)) {
            _buyEquipButton.text = "Equip";
            _buyEquipButton.RemoveFromClassList("buy-equip-button--above-points");
        } 
        else {
            _buyEquipButton.text = bottle.Price.ToString();
            if (bottle.Price > GameManager.Instance.TotalPoints) _buyEquipButton.AddToClassList("buy-equip-button--above-points");
            else _buyEquipButton.RemoveFromClassList("buy-equip-button--above-points");
        }
    }
}
