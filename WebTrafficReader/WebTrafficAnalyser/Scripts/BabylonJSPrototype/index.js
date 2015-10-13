///<reference path="scripts/babylon.max.js" />

var createScene = function (engine, canvas) {
    //BABYLON.SceneLoader.Load("Assets/Spaceship/", "spaceship.babylon", engine, function (scene) {

    //    //attach Camera
    //    scene.activeCamera.attachControl(canvas);

    //    // Register a render loop to repeatedly render the scene
    //    engine.runRenderLoop(function () {
    //        scene.render();
    //    });

    //}, null, null);

    //// Watch for browser/canvas resize events
    //window.addEventListener("resize", function () {
    //    engine.resize();
    //});

    // Now create a basic Babylon Scene object 
    var scene = new BABYLON.Scene(engine);

    // Change the scene background color to green.
    scene.clearColor = new BABYLON.Color3(0, 1, 0);

    // This creates and positions a free camera
    var camera = new BABYLON.FreeCamera("camera1", new BABYLON.Vector3(0, 5, -10), scene);

    // This targets the camera to scene origin
    camera.setTarget(BABYLON.Vector3.Zero());

    // This attaches the camera to the canvas
    //camera.attachControl(canvas, false);

    // This creates a light, aiming 0,1,0 - to the sky.
    var light = new BABYLON.HemisphericLight("light1", new BABYLON.Vector3(0, 1, 0), scene);

    // Dim the light a small amount
    light.intensity = .5;

    // Let's try our built-in 'ground' shape.  Params: name, width, depth, subdivisions, scene
    var ground = BABYLON.Mesh.CreateGround("ground1", 6, 10, 1, scene);

    // Leave this function
    return scene;
}