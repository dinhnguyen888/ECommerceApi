import * as dotenv from 'dotenv';

dotenv.config();

export interface JwtConfig {
  issuer: string;
  audience: string;
  secret: string;
}

export function getJwtConfig(): JwtConfig {
  return {
    issuer: process.env.JWT_ISSUER || 'ECommerce.AuthService',
    audience: process.env.JWT_AUDIENCE || 'ECommerce.Clients',
    secret: process.env.JWT_SECRET || 'MICROSERVICE_ECOMMERCE_SYSTEM_WITH_DOTNET_CORE_AND_NODE_JS'
  };
}

