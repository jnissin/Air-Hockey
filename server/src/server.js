/* eslint no-console: 0*/
import express from 'express';
import expressWs from 'express-ws';
import {
  gameMove,
  gameStart,
  gameFinish
} from './game';

import {
  playerConnect,
  playerDisconnect
} from './game';

const HANDLERS = {
  'game:move': gameMove,
  'game:start': gameStart,
  'game:finish': gameFinish,
  'player:connect': playerConnect,
  'player:disconnect': playerDisconnect,
};

const app = express();

expressWs(app);

app.ws('/play', (ws)=> {
  ws.on('message', (msg)=> {
    HANDLERS[msg.type](msg, ws);
  });
});

const server = app.listen(process.env.PORT || 3000, ()=> {
  const host = server.address().host;
  const port = server.address().port;

  console.log('Air Hockey app listening at http://%s:%s', host, port);
});

export {
  app,
  server
};
