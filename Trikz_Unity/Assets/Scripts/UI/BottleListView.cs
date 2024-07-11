using UnityEngine.UIElements;

public class BottleListView : UIView {

    private BottleListController _list;
    private VisualTreeAsset _listElementTemplate;
    private Button _mainMenuButton, _buyEquipButton;
    private BottleData _equippedBottle;
    
    public BottleListView(VisualElement topElement, VisualTreeAsset listElementTemplate) : base(topElement)
    {
        _list = new BottleListController();
        _listElementTemplate = listElementTemplate;
        _list.InitializeBottleList(_topElement.Q<ListView>(k_ListName), _listElementTemplate);
        MenuEvents.BottleSelectionChanged += (v) => _equippedBottle = v;
    }

    public const string k_ListName = "bottle-list";
    public const string k_MainMenuButtonName = "main-menu-button";
    public const string k_BuyEquipButton = "buy-equip-button";

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

    private void OnBuyEquipClicked(ClickEvent evt)
    {
        MenuEvents.BottleEquipped?.Invoke(_equippedBottle);
    }

    private void OnMainMenuClicked(ClickEvent evt)
    {
        GameManager.Instance.ChangeState(GameState.Menu);
        BottlePreviewManager.Instance.SetPreviewActive(false);
    }
}
