import dotenv from 'dotenv';
const dotenvResult = dotenv.config();
if (dotenvResult.error) {
    throw dotenvResult.error;
}

import express from 'express';
import * as http from 'http';

import * as winston from 'winston';
import * as expressWinston from 'express-winston';
import cors from 'cors';
import { CommonRoutesConfig } from './common/common.routes.config';
import { CoursesRoutes } from './courses/course.routes.config';
import debug from 'debug';

const app: express.Application = express();
const server: http.Server = http.createServer(app);
const port = process.env.DEFAULT_PORT;
const routes: Array<CommonRoutesConfig> = [];
const debugLog: debug.IDebugger = debug('app');

/*
  Middleware registration
*/
// adding middleware to parse all incoming requests as JSON
app.use(express.json());
// adding middleware to allow cross-origin requests
app.use(cors());

// logging middleware configuration,
// which will automatically log all HTTP requests handled by Express.js
const loggerOptions: expressWinston.LoggerOptions = {
  transports: [new winston.transports.Console()],
  format: winston.format.combine(
    winston.format.json(),
    winston.format.prettyPrint(),
    winston.format.colorize({ all: true })
  ),
};

if (!process.env.DEBUG) {
  loggerOptions.meta = false; // when not debugging, log requests as one-liners
}

// initialize the logger with the above configuration
app.use(expressWinston.logger(loggerOptions));

/*
  Routes registration
*/
// Adding the CourseRoutes to our routes array,
// after sending the Express.js application object to have the routes added to our app!
routes.push(new CoursesRoutes(app));

// 2 simple routes to make sure everything is working properly
const runningMessage = `Server running at http://localhost:${port}`;
app.get('/', (req: express.Request, res: express.Response) => {
  res.status(200).send(runningMessage);
});
app.get('/ping', (req: express.Request, res: express.Response) => {
  res.status(200).send('pong');
});

export default server.listen(port, () => {
  routes.forEach((route: CommonRoutesConfig) => {
    debugLog(`Routes configured for ${route.getName()}`)
  });
  // logging when the server is done starting up
  console.log(runningMessage);
});
