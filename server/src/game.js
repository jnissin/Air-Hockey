import {
  getPlayer
} from './player';

const GAMES = {};

// send messages to the other player
function relayMessage(msg) {
  const game = GAMES[msg.gameId];

  if (msg.playerId === game.playerOne.id) {
    game.playerTwo.socket.send(msg);
  } else {
    game.playerOne.socket.send(msg);
  }
}

export function gameMove(msg) {
  relayMessage(msg);
}

export function gameFinish(msg) {
  relayMessage(msg);
  delete GAMES[msg.gameId];
}

export function gameStart(msg) {
  const id = msg.gameId;

  GAMES[id] = {
    id,
    playerOne: getPlayer(msg.playerOneId),
    playerTwo: getPlayer(msg.playerTwoId),
  };
}
