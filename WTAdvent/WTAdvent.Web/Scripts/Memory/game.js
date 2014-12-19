'use strict';
/* Memory Game Models and Business Logic */

function Tile(title) {
  this.title = title;
  this.flipped = false;
}

Tile.prototype.flip = function ()
{
	this.flipped = !this.flipped;
}



function Game(tileNames) {
  var tileDeck = makeDeck(tileNames);

  this.grid = makeGrid(tileDeck);
  this.message = Game.MESSAGE_ONE_MORE;
  this.unmatchedPairs = tileNames.length;

  this.flipTile = function (tile)
  {
  	
  	if (tile.flipped)
  	{
  		document.getElementById('memory-message').innerHTML = this.message;
      return;
    }

    tile.flip();

    if (!this.firstPick || this.secondPick)
    {
		if (this.secondPick)
    	{
			this.firstPick.flip();
			this.secondPick.flip();
			this.firstPick = this.secondPick = undefined;
		}

		this.message = Game.MESSAGE_ONE_MORE;
		this.firstPick = tile;

    }
    else
    {
	   
    	if (this.firstPick.title === tile.title)
    	{
			this.unmatchedPairs--;
			this.message = (this.unmatchedPairs > 0) ? Game.MESSAGE_MATCH : Game.MESSAGE_WON;
			this.firstPick = this.secondPick = undefined;
    	}
    	else
    	{
			this.secondPick = tile;
			this.message = Game.MESSAGE_MISS;
		  }
    }

    document.getElementById('memory-message').innerHTML = this.message;
  }
}

Game.MESSAGE_CLICK = '<span>' + translations["game-info2"] + '</span>';
Game.MESSAGE_ONE_MORE = '<span>' + translations["game-info3"] + '</span>';
Game.MESSAGE_MISS = '<span>' + translations["game-info2"] + '</span>';
Game.MESSAGE_MATCH = '<span>' + translations["game-info4"] + '</span>';
Game.MESSAGE_WON = '<a href="javascript:void(0);" class="link-btn" onclick="location.reload(true);">' + translations["game-info1"] + '</a>';



/* Create an array with two of each tileName in it */
function makeDeck(tileNames) {
  var tileDeck = [];
  tileNames.forEach(function(name) {
    tileDeck.push(new Tile(name));
    tileDeck.push(new Tile(name));
  });

  return tileDeck;
}


function makeGrid(tileDeck) {
  var gridDimension = 4,
      grid = [];

  for (var row = 0; row < 2; row++) {
    grid[row] = [];
    for (var col = 0; col < gridDimension; col++) {
        grid[row][col] = removeRandomTile(tileDeck);
    }
  }

  return grid;
}


function removeRandomTile(tileDeck) {
  var i = Math.floor(Math.random()*tileDeck.length);
  return tileDeck.splice(i, 1)[0];
}

