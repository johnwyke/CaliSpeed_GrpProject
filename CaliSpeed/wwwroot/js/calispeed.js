let cards = [{ name: "10_of_clubs", image: "images/10_of_clubs.png" },
{ name: "10_of_diamonds", image: "images/10_of_diamonds.png" },
{ name: "10_of_hearts", image: "images/10_of_hearts.png" },
{ name: "10_of_spades", image: "images/10_of_spades.png" },
{ name: "2_of_clubs", image: "images/2_of_clubs.png" },
{ name: "2_of_diamonds", image: "images/2_of_diamonds.png" },
{ name: "2_of_hearts", image: "images/2_of_hearts.png" },
{ name: "2_of_spades", image: "images/2_of_spades.png" },
{ name: "3_of_clubs", image: "images/3_of_clubs.png" },
{ name: "3_of_diamonds", image: "images/3_of_diamonds.png" },
{ name: "3_of_hearts", image: "images/3_of_hearts.png" },
{ name: "3_of_spades", image: "images/3_of_spades.png" },
{ name: "4_of_clubs", image: "images/4_of_clubs.png" },
{ name: "4_of_diamonds", image: "images/4_of_diamonds.png" },
{ name: "4_of_hearts", image: "images/4_of_hearts.png" },
{ name: "4_of_spades", image: "images/4_of_spades.png" },
{ name: "5_of_clubs", image: "images/5_of_clubs.png" },
{ name: "5_of_diamonds", image: "images/5_of_diamonds.png" },
{ name: "5_of_hearts", image: "images/5_of_hearts.png" },
{ name: "5_of_spades", image: "images/5_of_spades.png" },
{ name: "6_of_clubs", image: "images/6_of_clubs.png" },
{ name: "6_of_diamonds", image: "images/6_of_diamonds.png" },
{ name: "6_of_hearts", image: "images/6_of_hearts.png" },
{ name: "6_of_spades", image: "images/6_of_spades.png" },
{ name: "7_of_clubs", image: "images/7_of_clubs.png" },
{ name: "7_of_diamonds", image: "images/7_of_diamonds.png" },
{ name: "7_of_hearts", image: "images/7_of_hearts.png" },
{ name: "7_of_spades", image: "images/7_of_spades.png" },
{ name: "8_of_clubs", image: "images/8_of_clubs.png" },
{ name: "8_of_diamonds", image: "images/8_of_diamonds.png" },
{ name: "8_of_hearts", image: "images/8_of_hearts.png" },
{ name: "8_of_spades", image: "images/8_of_spades.png" },
{ name: "9_of_clubs", image: "images/9_of_clubs.png" },
{ name: "9_of_diamonds", image: "images/9_of_diamonds.png" },
{ name: "9_of_hearts", image: "images/9_of_hearts.png" },
{ name: "9_of_spades", image: "images/9_of_spades.png" },
{ name: "ace_of_clubs", image: "images/ace_of_clubs.png" },
{ name: "ace_of_diamonds", image: "images/ace_of_diamonds.png" },
{ name: "ace_of_hearts", image: "images/ace_of_hearts.png" },
{ name: "ace_of_spades", image: "images/ace_of_spades.png" },
{ name: "jack_of_clubs", image: "images/jack_of_clubs.png" },
{ name: "jack_of_diamonds", image: "images/jack_of_diamonds.png" },
{ name: "jack_of_hearts", image: "images/jack_of_hearts.png" },
{ name: "jack_of_spades", image: "images/jack_of_spades.png" },
{ name: "king_of_clubs", image: "images/king_of_clubs.png" },
{ name: "king_of_diamonds", image: "images/king_of_diamonds.png" },
{ name: "king_of_hearts", image: "images/king_of_hearts.png" },
{ name: "king_of_spades", image: "images/king_of_spades.png" },
{ name: "queen_of_clubs", image: "images/queen_of_clubs.png" },
{ name: "queen_of_diamonds", image: "images/queen_of_diamonds.png" },
{ name: "queen_of_hearts", image: "images/queen_of_hearts.png" },
{ name: "queen_of_spades", image: "images/queen_of_spades.png" }];

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
        width: 100,
        height: 100,
        autoDensity: true,
        resolution: devicePixelRatio
    });
    console.log(app);
    document.body.appendChild(app.view);

    app.renderer.backgroundColor = 0x061639;

    app.renderer.view.style.position = "absolute";
    app.renderer.view.style.display = "block";
    app.renderer.autoResize = true;
    app.renderer.resize(window.innerWidth, window.innerHeight);

    app.render();

    window.addEventListener("resize", function () {
        app.renderer.resize(window.innerWidth, window.innerHeight);
    });

    // Cache the sprites for each card

    PIXI.loader.add("images/card_back_red.png");
    // OPTIONAL: Use a sprite map instead of individual textures for the cards

    cards.forEach(card => {
        PIXI.Loader.shared.add(card.image);
    });

    PIXI.Loader.shared.load(setup);

    // setup is called when the images have finished loading
    function setup() {

        cardback = new PIXI.Sprite(PIXI.Loader.shared.resources["images/card_back_red.png"].texture);

        var ratio = cardback.width / cardback.height;
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

        //club = new PIXI.Sprite(PIXI.loader.resources["images/10_of_clubs.png"].texture);
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
                    console.log("Card was undefined :(");
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
<<<<<<< HEAD
    //app.renderer.resize(window.innerWidth, window.innerHeight);
    app.resize(window.innerWidth, window.innerHeight);
    adjustSpritesLocation();
    app.renderer.render(app.stage);
=======
  //app.renderer.resize(window.innerWidth, window.innerHeight);
  app.resize(window.innerWidth, window.innerHeight);
  app.renderer.render(app.stage);
  adjustSpritesLocation();
  app.renderer.render(app.stage);
  drawFrames = 10;
>>>>>>> master
};


function adjustSpritesLocation() {

    // move text
    message.x = app.renderer.width / 2;



<<<<<<< HEAD

    // move play field
    for (let i = 0; i < cardSprites.length; i++) {
        let row = Math.floor(i / 4);
        let column = i % 4;
        let sprite = cardSprites[i];
        let cardRatio = sprite.height / sprite.width;
        //console.log('sprite width:', sprite.width, 'screen width:', app.renderer.width);
        sprite.width = app.renderer.width * .2;
        if (sprite.width > 200) {
            sprite.width = 200;
        }
        if (sprite.height > app.renderer.height / 2.5) {
            console.log('Height override');
            sprite.height = app.renderer.height / 2.5;
            sprite.width = (1 - cardRatio) * sprite.height;
        }
        sprite.height = sprite.width * cardRatio;

        //console.log(sprite);
        sprite.position.set((column - 2) * sprite.width * 1.1 + app.renderer.width * .5 + sprite.width / 2,
            row * sprite.height * 1.1 + app.renderer.height * .24);
        //console.log('row:', row, 'column:', column, 'position', sprite.position);
    }

    if (cardSprites.length > 0) {
        cardback.width = cardSprites[0].width;
        cardback.height = cardSprites[0].height;
        resetCardBackLocation();
    }
=======
  // move play field
  for (let i = 0; i < cardSprites.length; i++) {
    let row = Math.floor(i / 4);
    let column = i % 4;
    let sprite = cardSprites[i];
    let cardRatio = 1.452; //calculated from sprite.height / sprite.width;
    console.log(cardRatio);
    //console.log('sprite width:', sprite.width, 'screen width:', app.renderer.width);
    sprite.width = app.renderer.width * .2;
    sprite.height = sprite.width * cardRatio;
    if (sprite.width > 200) {
      console.log('max width override');
      sprite.width = 200;
      sprite.height = sprite.width * cardRatio;
    }
    if (sprite.height > app.renderer.height / 2.75) {
      console.log('height override');
      sprite.height = app.renderer.height / 3;
      sprite.width = sprite.height / cardRatio;
    }
    
    //console.log(sprite);
    sprite.position.set((column - 2) * sprite.width * 1.1 + app.renderer.width * .5 + sprite.width / 2,
      row * sprite.height * 1.1 + app.renderer.height * .24);
    //console.log('row:', row, 'column:', column, 'position', sprite.position);
  }

  if (cardSprites.length > 0) {
    cardback.width = cardSprites[0].width;
    cardback.height = cardSprites[0].height;
    resetCardBackLocation();
  }
>>>>>>> master
}

function resetCardBackLocation() {
    cardback.x = app.renderer.width * .5;
    cardback.y = app.renderer.height - cardback.height * .25;
}

<<<<<<< HEAD
function animate() {

    requestAnimationFrame(animate);

    // render the stage
    //app.renderer.render(app.stage);
}

=======
>>>>>>> master
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

<<<<<<< HEAD
    for (let i = 0; i < cardSprites.length; i++) {
        let sprite = cardSprites[i];
        if (isSelectionCardHovering(sprite)) {
            let row = Math.floor(i / 4);
            let col = i % 4;
            sendPlay(row, col);
        }
=======
  for (let i = 0; i < cardSprites.length; i++) {
    let sprite = cardSprites[i];
    if (isSelectionCardHovering(sprite)) {
      let row = Math.floor(i / 4);
      let col = i % 4;
      sendPlay(row, col);
>>>>>>> master
    }


    // set the interaction data to null
    this.data = null;
}

function onDragMove() {
    if (this.dragging) {
        var newPosition = this.data.getLocalPosition(this.parent);
        this.position.x = newPosition.x;
        this.position.y = newPosition.y;

        for (let sprite of cardSprites) {
            if (isSelectionCardHovering(sprite)) {
                sprite.tint = 0x990000;
            } else {
                sprite.tint = 0xFFFFFF;
            }
        }
    }
}

var club = null;

var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();

connection.start().then
(
    function ()
    {
        console.log("Connection started");

        /* TESTING OTHER FUNCTIONS ON STARTUP */
        sendPlay(0, 1);
        getCardsList();
    }
);

// SEND PLAY -> RECEIVE PLAY RESULT
function sendPlay(row, column)
{
    connection.invoke("PlayCard", row, column/*, row2, column2*/);
    // why is there two rows and columns?
}

connection.on
(
    "ReceivePlayResult",
    function (result) {
        console.log("Play Result: ");
        if (result)
        {
            console.log("Success");
        }
        else
        {
            console.log("Fail");
        }
    }
)

// GET CARDS LIST -> RECEIVE CARDS LIST

function getCardsList()
{
    connection.invoke("GetCardsList");
}

connection.on
(
    "ReceiveCardsList",
    function (jsonCardsList)
    {
        //console.log(jsonCardsList);
        var cardsList = JSON.parse(jsonCardsList);
        //console.log(cardsList);

        if (cardsList[0][0]) {
            console.log("Received Cards: ");
            
            for (i = 0; i < cardsList[0].length; i++) {
                for (j = 0; j < cardsList[1].length; j++) {
                    console.log(cardsList[i][j].Face);
                }
            }
        }
        else {
            console.log("No Cards Received");
        }
    }
)

// CARD PLAYED EVENT (???????) -> RESPONSE (????????)

function 




//startSocket(); //NOT DEFINED... WHAT IS IT?