import { each } from 'lodash';

const PLAYERS = {};

export function playerConnect(msg, ws) {
  PLAYERS[msg.playerId] = {
    name: msg.playerName,
    socket: ws,
  };

  // broadcast player connect
  each(PLAYERS, (otherPlayer) => {
    otherPlayer.socket.send(msg);
  });
}

export function playerDisconnect(msg) {
  const player = PLAYERS[msg.playerId];

  if (player) {
    delete PLAYERS[msg.playerId];

    // broadcast player disconnect
    each(PLAYERS, (otherPlayer) => {
      otherPlayer.socket.send(msg);
    });
  }
}

export function getPlayer(playerId) {
  return PLAYERS[playerId];
}
