using Build1.PostMVC.Core;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.UI.Layers;
using UnityEngine;

public sealed class Root : UnityView
{
    [Header("App Layers"), SerializeField] public GameObject screens;

    [SerializeField] public GameObject systemCanvasPopups;
    [SerializeField] public GameObject systemCanvasHUD;
    [SerializeField] public GameObject systemCanvasScreensExpand;
    [SerializeField] public GameObject systemCanvasScreens;

    [SerializeField] public GameObject overlayCanvasPopups;
    [SerializeField] public GameObject overlayCanvasHUD;
    [SerializeField] public GameObject overlayCanvasScreens;

    [SerializeField] public GameObject gameCanvasPopups;
    [SerializeField] public GameObject gameCanvasHUDGeneral;

    [Inject] public IUILayersController UILayerController { get; set; }
    
    /*
     * Mono Behavior.
     */

    protected override void Awake()
    {
        base.Awake();

        PostMVC.Context()
               .AddExtension<UnityAppExtension>()
               .AddModule<RootModule>()
               .Start();

    }

    /*
     * PostMVC.
     */

    [PostConstruct]
    public void PostConstruct()
    {
        UILayerController.RegisterLayer(RootLayer.SystemPopups, systemCanvasPopups);
        UILayerController.RegisterLayer(RootLayer.SystemHUD, systemCanvasHUD);
        UILayerController.RegisterLayer(RootLayer.SystemScreensCanvasExpand, systemCanvasScreensExpand);
        UILayerController.RegisterLayer(RootLayer.SystemScreensCanvas, systemCanvasScreens);

        UILayerController.RegisterLayer(RootLayer.OverlayPopups, overlayCanvasPopups);
        UILayerController.RegisterLayer(RootLayer.OverlayHUD, overlayCanvasHUD);
        UILayerController.RegisterLayer(RootLayer.OverlayScreensCanvas, overlayCanvasScreens);

        UILayerController.RegisterLayer(RootLayer.GamePopups, gameCanvasPopups);
        UILayerController.RegisterLayer(RootLayer.GameHUDGeneral, gameCanvasHUDGeneral);

        UILayerController.RegisterLayer(RootLayer.Screens, screens);
    }
}