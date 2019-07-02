document.onload = function () {

  let type = "WebGL";
  if (!PIXI.utils.isWebGLSupported()) {
    type = "canvas";
  }

  PIXI.utils.sayHello(type);

  const app = new PIXI.Application({
    autoResize: true,
    resolution: devicePixelRatio
  });
  document.body.appendChild(app.view);

  app.renderer.backgroundColor = 0x061639;

  app.renderer.autoResize = true;
  app.renderer.resize(512, 512);

  app.renderer.view.style.position = "absolute";
  app.renderer.view.style.display = "block";
  app.renderer.autoResize = true;
  app.renderer.resize(window.innerWidth, window.innerHeight);



}