import app from './../app';
import { expect } from 'chai';
import supertest from 'supertest';

describe('GET /ping', () => {
  it('should return pong', async () => {
    const result = await supertest(app).get('/ping');
    expect(result.statusCode).to.equal(200);
    expect(result.text).to.equal('pong');
  });
});