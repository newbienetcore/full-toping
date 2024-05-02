import express from 'express';

export abstract class CommonRoutesConfig {
  constructor(protected app: express.Application, private name: string) {
    this.app = app;
    this.name = name;
    this.configureRoutes();
  }

  getName() {
    return this.name;
  }

  abstract configureRoutes(): express.Application;
}