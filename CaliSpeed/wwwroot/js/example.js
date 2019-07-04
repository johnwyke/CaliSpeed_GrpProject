"use strict";

// builds connection with ExampleHub
var connection = new signalR.HubConnectionBuilder().withUrl("/ExampleHub").build();

// Disables send button until connection is established
document.getElementById("playButton").disabled = true;

// #region Starts connection, enables send button
connection
.start()
.then
(
    function ()
    {
        document.getElementById("playButton").disabled = false;
    }
);

// #endregion

// #region Listens for click, runs function to write "[user] played card"
document
    .getElementById("playButton")
    .addEventListener
    (
        "click",
        function (event)
        {
            var user = document.getElementById("playerName").value;
            connection.invoke("PlayCard", user);
        }
    );

// #endregion

// #region Writes "[user] played card"
connection.on
(
    "ReceiveCard",
    function (user)
    {
        var encodedMsg = user + " played a card";
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("gameUpdatesList").appendChild(li);
    }
);

// #endregion
