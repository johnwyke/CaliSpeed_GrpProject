let cards = [{ name: "1c", image: "images/1c.png" },
{ name: "1d", image: "images/1d.png" },
{ name: "1h", image: "images/1h.png" },
{ name: "1s", image: "images/1s.png" },
{ name: "2c", image: "images/2c.png" },
{ name: "2d", image: "images/2d.png" },
{ name: "2h", image: "images/2h.png" },
{ name: "2s", image: "images/2s.png" },
{ name: "3c", image: "images/3c.png" },
{ name: "3d", image: "images/3d.png" },
{ name: "3h", image: "images/3h.png" },
{ name: "3s", image: "images/3s.png" },
{ name: "4c", image: "images/4c.png" },
{ name: "4d", image: "images/4d.png" },
{ name: "4h", image: "images/4h.png" },
{ name: "4s", image: "images/4s.png" },
{ name: "5c", image: "images/5c.png" },
{ name: "5d", image: "images/5d.png" },
{ name: "5h", image: "images/5h.png" },
{ name: "5s", image: "images/5s.png" },
{ name: "6c", image: "images/6c.png" },
{ name: "6d", image: "images/6d.png" },
{ name: "6h", image: "images/6h.png" },
{ name: "6s", image: "images/6s.png" },
{ name: "7c", image: "images/7c.png" },
{ name: "7d", image: "images/7d.png" },
{ name: "7h", image: "images/7h.png" },
{ name: "7s", image: "images/7s.png" },
{ name: "8c", image: "images/8c.png" },
{ name: "8d", image: "images/8d.png" },
{ name: "8h", image: "images/8h.png" },
{ name: "8s", image: "images/8s.png" },
{ name: "9c", image: "images/9c.png" },
{ name: "9d", image: "images/9d.png" },
{ name: "9h", image: "images/9h.png" },
{ name: "9s", image: "images/9s.png" },
{ name: "10c", image: "images/10c.png" },
{ name: "10d", image: "images/10d.png" },
{ name: "10h", image: "images/10h.png" },
{ name: "10s", image: "images/10s.png" },
{ name: "11c", image: "images/11c.png" },
{ name: "11d", image: "images/11d.png" },
{ name: "11h", image: "images/11h.png" },
{ name: "11s", image: "images/11s.png" },
{ name: "12c", image: "images/12c.png" },
{ name: "12d", image: "images/12d.png" },
{ name: "12h", image: "images/12h.png" },
{ name: "12s", image: "images/12s.png" },
{ name: "13c", image: "images/13c.png" },
{ name: "13d", image: "images/13d.png" },
{ name: "13h", image: "images/13h.png" },
{ name: "13s", image: "images/13s.png" }];

var card_back = 'card_back_black.png';

function getRandomInt(max) {
    return Math.floor(Math.random() * Math.floor(max));
}

function generateSampleBoard() {
    var board = { player1cards: 22, player2cards: 22, cards: [] };
    for (var y = 0; y < 3; y++) {
        for (var x = 0; x < 2; x++) {
            board.cards[y][x] = cards[getRandomInt(52)];
        }
    }
}

// Create a map from the array for them fast reads.
var cardMap = new Map(cards.map(i => [i.name, i]));
var app = null;

let cardSprites = [];

var cardback = null;
var message = null;

window.onload = function () {
    let type = "WebGL";
    if (!PIXI.utils.isWebGLSupported()) {
        type = "canvas";
    }

    PIXI.utils.sayHello(type);

    app = new PIXI.Application({
        width: window.innerWidth,
        height: window.innerHeight,
        autoDensity: true,
        resolution: devicePixelRatio
    });
    
    document.body.appendChild(app.view);

  app.renderer.backgroundColor = 0x076324;

    app.renderer.view.style.position = "absolute";
    app.renderer.view.style.display = "block";
    app.renderer.autoResize = true;
    app.renderer.resize(window.innerWidth, window.innerHeight);

    app.render();

    window.addEventListener("resize", function () {
        app.renderer.resize(window.innerWidth, window.innerHeight);
    });

    // Cache the sprites for each card

    PIXI.Loader.shared.add("images/card_back_red.png");
    // OPTIONAL: Use a sprite map instead of individual textures for the cards

    cards.forEach(card => {
        PIXI.Loader.shared.add(card.image);
    });

    PIXI.Loader.shared.load(setup);

    // setup is called when the images have finished loading
    function setup() {

        cardback = new PIXI.Sprite(PIXI.Loader.shared.resources["images/card_back_red.png"].texture);

        ratio = cardback.width / cardback.height;
        cardback.height = app.screen.height * .25;
        cardback.width = cardback.height * ratio;
        cardback.interactive = true;
        cardback.buttonMode = true;
        cardback
            .on('mousedown', onDragStart)
            .on('touchstart', onDragStart)
            // events for drag end
            .on('mouseup', onDragEnd)
            .on('mouseupoutside', onDragEnd)
            .on('touchend', onDragEnd)
            .on('touchendoutside', onDragEnd)
            // events for drag move
            .on('mousemove', onDragMove)
            .on('touchmove', onDragMove);

        cardback.anchor.set(.5);
        cardback.zIndex = 100;

        cardback.position.set(app.screen.width * .2, app.screen.height * .5);

        //club = new PIXI.Sprite(PIXI.loader.resources["images/10c.png"].texture);

        //var board = generateSampleBoard();
        //app.stage.addChild(club);

        message = new PIXI.Text("California Speed");
        message.style.fill = "#FFFFFF";
        message.style.fontSize = 72;
        message.anchor.set(.5, 0);
        app.stage.addChild(message);

        for (let i = 0; i < 4; i++) {
            for (let j = 0; j < 2; j++) {

                // get a random card

                let card = cards[getRandomInt(51)];

                // create a sprite from the random card
                if (card !== undefined) {
                    var sprite = new PIXI.Sprite(PIXI.Loader.shared.resources[card.image].texture);
                    let cardRatio = sprite.width / sprite.height;

                    app.stage.addChild(sprite);
                    sprite.anchor.set(0.5);
                    cardSprites.push(sprite);
                } else {
                    console.error("Card was undefined :(");
                }
            }
        }
        app.stage.addChild(cardback);
        adjustSpritesLocation();
    }
};

var drawFrames = 0;
requestAnimationFrame(animate);

function isSelectionCardHovering(value) {
    var bounds = value.getBounds();
    // cardback
    return cardback.position.x >= bounds.left && cardback.position.x <= bounds.right
        && cardback.position.y >= bounds.top && cardback.position.y <= bounds.bottom;
}

function animate() {

  requestAnimationFrame(animate);

  if (drawFrames > 0) {
    app.resize(window.innerWidth, window.innerHeight);
    adjustSpritesLocation();
    app.renderer.render(app.stage);
    drawFrames--;
  }
}

window.onresize = function (event) {
  //app.renderer.resize(window.innerWidth, window.innerHeight);
  app.resize(window.innerWidth, window.innerHeight);
  app.renderer.resize(window.innerWidth, window.innerHeight);
  app.renderer.render(app.stage);
  adjustSpritesLocation();
  app.renderer.render(app.stage);
  drawFrames = 10;
};


function adjustSpritesLocation() {
  // Get the real width of the screen
  var screenWidth = app.renderer.view.style.width;
  var screenHeight = app.renderer.view.style.height;

  screenWidth = screenWidth.substr(0, screenWidth.length - 2);
  screenHeight = screenHeight.substr(0, screenHeight.length - 2);

  message.x = screenWidth / 2;
  message.style.fontSize = screenHeight / 13;

  for (let i = 0; i < cardSprites.length; i++) {
    let row = Math.floor(i / 4);
    let column = i % 4;
    let sprite = cardSprites[i];
    let cardRatio = 1.452; //calculated from sprite.height / sprite.width
    sprite.width = screenWidth * .2;
    sprite.height = sprite.width * cardRatio;
    if (sprite.width > 200) {
      sprite.width = 200;
      sprite.height = sprite.width * cardRatio;
    }
    if (sprite.height > screenHeight / 3.4) {
      sprite.height = screenHeight / 3.5;
      sprite.width = sprite.height / cardRatio;
    }
    
    sprite.position.set((column - 2) * sprite.width * 1.1 + screenWidth * .5 + sprite.width / 2,
      row * sprite.height * 1.1 + screenHeight * .24);
  }

  if (cardSprites.length > 0) {
    cardback.width = cardSprites[0].width;
    cardback.height = cardSprites[0].height;
    resetCardBackLocation(screenWidth, screenHeight);
  }
}

function resetCardBackLocation(screenWidth, screenHeight) {
    cardback.x = screenWidth * .5;
    cardback.y = screenHeight - cardback.height * .4;
}

function onDragStart(event) {
    // store a reference to the data
    // the reason for this is because of multitouch
    // we want to track the movement of this particular touch
    this.data = event.data;
    this.alpha = 0.5;
    this.dragging = true;
}

function onDragEnd() {
    this.alpha = 1;

    this.dragging = false;
    for (let i = 0; i < cardSprites.length; i++) {
        let sprite = cardSprites[i];
        if (isSelectionCardHovering(sprite)) {
            let row = Math.floor(i / 4);
            let col = i % 4;
            sendPlay(row, col);
        }
        // set the interaction data to null
        this.data = null;
    }
}

// Checks to see if the dragged card is over any of the sprites.
// If so, it will highlight it as red
function doCardHighlighting() {
  for (let sprite of cardSprites) {
    if (isSelectionCardHovering(sprite)) {
      sprite.tint = 0x990000;
    } else {
      sprite.tint = 0xFFFFFF;
    }
  }
}

// Called whenver the card is moved
function onDragMove() {
    if (this.dragging) {
        var newPosition = this.data.getLocalPosition(this.parent);
        this.position.x = newPosition.x;
        this.position.y = newPosition.y;

      doCardHighlighting();
    }
}

var club = null;


/**** INITIATE SIGNALR CONNECTION ****
 *  
 *  Creates connection to GameHub
 *  Once connected to hub, gets the cardsList, then does anything
 *  else we need it to do
 *  
 */

// Instantiate connection
var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();

// Once connected, run any desired startup code here
connection.start().then
(
    function ()
    {
        getCardsList();
    }
);




/**** GET CARDS LIST, RECEIVE CARDS LIST, and UPDATE CARDS ****
 * 
 * asks GameHub for cardsList
 * informs user / client that cardsList was received
 * listens for Game.cs to ask client to update display of cards
 *
 */

// Ask GameHub for cardsList
function getCardsList() {
    connection.invoke("GetCardsList");
}


// Note from Aaron: 
// Kameron, I think ReceiveCardsList and Update_AllCards are identical - 
// both should receive the cards that are in the play field- make them both
// ReceiveCardsList. The Events now live in the Room due to a technical limitation

// Once GameHub responds...
connection.on
  (
    "ReceiveCardsList",
    function (cardsList) {
      console.log(cardsList); //displays JS object cardsList

      if (cardsList) {
        console.log("Received cardsList. Waiting for Game.cs to update of cards display.");

      }
      else {
        console.log("No cardsList received.");
      }
    }
  );

// Once Game.cs tells client to update cardsList...

connection.on
  (
    "Update_AllCards",
    function (cards) {
      console.log(cards); //displays JS object cards

      //update a ton of stuff
      console.log("UPDATE NOW TO SHOW ALL NEW CARDS");
    }
  );


/**** SEND PLAY, RECEIVE PLAY RESULT, and UPDATE CARD ****
 * 
 * Asks GameHub to handle played card
 * Informs user / client that card was played successfully
 * Game.cs tells user / client to update display to show card
 * 
 */

// Ask GameHub to play a card.
function sendPlay(row, column)
{
    connection.invoke("PlayCard", row, column);
}

// Once GameHub responds...
connection.on
  (
    "ReceivePlayResult",
    function (result) {

      console.log("Received Play Result with status", result);

      if (result) {
        // TODO indicate a success somehow
      }
      else {
        // TODO indicate a failure somehow
      }
      adjustSpritesLocation();
      doCardHighlighting();
    }
  );

// Once Game.cs tells client to update display of new card
connection.on
  (
    "Update_NewCard",
    function (row, column, newCard) {
      var image = getCardImage(newCard).image;
      var newTexture = PIXI.Loader.shared.resources[image].texture;
      cardSprites[row * 4 + column].texture = newTexture;
    }
  );

connection.on
  (
    "PlayerWon",
    function (winMessage) {
      let txt = new PIXI.Text(winMessage, { fontSize: 20, stroke: 1 });
      txt.anchor.set(.5, 0);
      app.stage.add(txt);
    }
  );



/**** GET CARD IMAGE ****
 *
 * Receives a Game.cs Card object
 * Returns the corresponding JS object from the cards[] array
 * Suits are alphabetical: (0) clubs, (1) diamonds, (2) hearts, (3) spades
 */

function getCardImage(gameCard) {
    return cards[gameCard.face * 4 + gameCard.suit];
}