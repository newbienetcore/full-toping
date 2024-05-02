import dotenv from 'dotenv';
const dotenvResult = dotenv.config();
if (dotenvResult.error) {
    throw dotenvResult.error;
}

import { mongoMigrateCli } from 'mongo-migrate-ts';

const DB_CONNECTION = process.env.DB_CONNECTION_STRING || '';
const DB_NAME = process.env.DB_NAME;

mongoMigrateCli({
  uri: DB_CONNECTION,
  database: DB_NAME,
  migrationsDir: __dirname,
  migrationsCollection: 'migrations_collection',
  options: {
    useNewUrlParser: true,
    useUnifiedTopology: true
  }
});