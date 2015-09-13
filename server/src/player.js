const PLAYERS = {};

export function playerConnect(msg, ws) {
  PLAYERS[msg.playerId] = {
    name: msg.playerName,
    socket: ws,
  };
}

export function playerDisconnect(msg) {
  delete PLAYERS[msg.playerId];
}

export function getPlayer(playerId) {
  return PLAYERS[playerId];
}
