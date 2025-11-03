import * as dotenv from 'dotenv';
import { createApp } from './app';

// Tai cac bien moi truong
dotenv.config();

const PORT = process.env.PORT || 3001;

async function startServer() {
  try {
    const app = await createApp();
    
    app.listen(PORT, () => {
      console.log(`CommerceService is running on port ${PORT}`);
    });
  } catch (error) {
    console.error('Failed to start server:', error);
    process.exit(1);
  }
}

startServer();

