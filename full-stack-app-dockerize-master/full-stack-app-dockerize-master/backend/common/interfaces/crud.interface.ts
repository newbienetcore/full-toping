export interface CRUD<T, TCREATE, TUPDATE, TPATCH> {
  list: (limit: number, page: number) => Promise<T[]>;
  create: (resource: TCREATE) => Promise<TCREATE>;
  putById: (id: string, resource: TUPDATE) => Promise<TUPDATE>;
  getById: (id: string) => Promise<T | null>;
  deleteById: (id: string) => Promise<T | null>;
  patchById: (id: string, resource: TPATCH) => Promise<TPATCH | null>;
}