import jwt from 'jsonwebtoken';
import { getJwtConfig } from '../config/jwt';

export interface TokenPayload {
  sub: string; // id nguoi dung
  unique_name: string; // ten nguoi dung
  email: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string; // Role claim tu .NET
  role?: string; // Fallback neu co claim role truc tiep
  exp: number;
  iat: number;
}

export interface DecodedToken {
  userId: string;
  userName: string;
  email: string;
  role: string;
}

export class JwtValidator {
  private config = getJwtConfig();

  validateToken(token: string): DecodedToken | null {
    try {
      // Loai bo tien to 'Bearer ' neu co
      const cleanToken = token.startsWith('Bearer ') ? token.substring(7) : token;

      const decoded = jwt.verify(cleanToken, this.config.secret, {
        issuer: this.config.issuer,
        audience: this.config.audience
      }) as TokenPayload;

      // Lay role tu claims - .NET su dung claim type cho role
      const role = decoded.role || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 'Client';

      return {
        userId: decoded.sub,
        userName: decoded.unique_name,
        email: decoded.email,
        role: role
      };
    } catch (error) {
      return null;
    }
  }

  requireRole(token: string, allowedRoles: string[]): DecodedToken | null {
    const decoded = this.validateToken(token);
    if (!decoded) {
      return null;
    }

    if (!allowedRoles.includes(decoded.role)) {
      return null;
    }

    return decoded;
  }
}

