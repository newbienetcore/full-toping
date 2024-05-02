import express from 'express';
import mongoose from 'mongoose';

class ObjectIdValidationMiddleware {
  verifyObjectId(req: express.Request, res: express.Response, next: express.NextFunction) {
    if (!mongoose.Types.ObjectId.isValid(req.params.id || req.body.id))
      return res.status(404).json({ error: 'Invalid Id' });

    next();
  }
}

export default new ObjectIdValidationMiddleware();