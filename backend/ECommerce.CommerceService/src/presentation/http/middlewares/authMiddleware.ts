import { Request, Response, NextFunction } from 'express';
import { JwtValidator, DecodedToken } from '../../../infrastructure/auth/jwtValidator';

// Mo rong Express Request de them thong tin nguoi dung
declare global {
  namespace Express {
    interface Request {
      user?: DecodedToken;
    }
  }
}

const jwtValidator = new JwtValidator();

/**
 * Middleware de xac thuc JWT token
 * Token phai duoc gui trong header: Authorization: Bearer <token>
 */
export function authenticate(req: Request, res: Response, next: NextFunction): void {
  const authHeader = req.headers.authorization;

  if (!authHeader) {
    res.status(401).json({ error: 'Unauthorized: No token provided' });
    return;
  }

  const decoded = jwtValidator.validateToken(authHeader);

  if (!decoded) {
    res.status(401).json({ error: 'Unauthorized: Invalid or expired token' });
    return;
  }

  // Gan thong tin nguoi dung vao request
  req.user = decoded;
  next();
}

/**
 * Middleware de kiem tra role
 * @param allowedRoles - Mang cac role duoc phep truy cap (vi du: ['Admin'], ['Admin', 'Client'])
 */
export function authorize(...allowedRoles: string[]) {
  return (req: Request, res: Response, next: NextFunction): void => {
    if (!req.user) {
      res.status(401).json({ error: 'Unauthorized: User not authenticated' });
      return;
    }

    if (!allowedRoles.includes(req.user.role)) {
      res.status(403).json({ error: 'Forbidden: Insufficient permissions' });
      return;
    }

    next();
  };
}

/**
 * Ket hop authenticate va authorize trong mot middleware
 */
export function authenticateAndAuthorize(...allowedRoles: string[]) {
  return [
    authenticate,
    authorize(...allowedRoles)
  ];
}

