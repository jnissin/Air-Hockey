##Quick start

1. Clone repository
2. Run `npm run-script babel-compile` to transpile from ES6 -> ES5
3. Run `npm start` to start the server in dev mode using pm2


## Development
All javascript files from src/ will be transpiled to dest/ using babel and should be used from there. The server is totally unrobust and makes no checks/guarantees for delivery. It is meant only as a quick hack to test connectivity between players and enable UI development.


## Server
The server at this point is a very simplistic message hub, either broadcasting messages to other peers or relaying messages from one player to another.

Connection are established using websocket to `/play`. After connection any of the following messages can be sent.

### Messages
__player:connect__
Sent to signal other players of a new available player to play against.
```
{
  type: 'player:connect',
  playerId: <new player id, should be e.g. UUID to roughly guarantee uniqueness for now>,
  playerName: <players display name>
}
```
__player:disconnect__
Sent to signal other players that a player has quit.
```
{
  type: 'player:disconnect',
  playerId: <disconnected players id>
}
```
__game:move__
Send a paddle move event to the other player
```
{
  type: 'game:move',
  gameId: <game id>
  playerId: <player who made the move>
  data: { ... move details ... }
}
```
__game:start__
Start a new game with other player
```
{
  type: 'game:start',
  gameId: <game id>,
  playerOneId: <player one id>,
  playerTwoId: <player two id>
}
```
__game:finish__
Sent when the other player disconnects
```
{
  type: 'game:finish',
  gameId: <game id>
}
```
