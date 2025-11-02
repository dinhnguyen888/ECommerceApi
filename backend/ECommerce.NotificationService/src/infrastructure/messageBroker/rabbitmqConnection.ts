import amqp, { Channel } from 'amqplib';
import * as dotenv from 'dotenv';

dotenv.config();

export class RabbitMqConnection {
    private connection: any = null;
    private channel: Channel | null = null;

    async connect(): Promise<void> {
        const hostName = process.env.RABBITMQ_HOST || 'localhost';
        const port = process.env.RABBITMQ_PORT || '5672';
        const userName = process.env.RABBITMQ_USERNAME || 'guest';
        const password = process.env.RABBITMQ_PASSWORD || 'guest';

        const url = `amqp://${userName}:${password}@${hostName}:${port}`;
        
        this.connection = await amqp.connect(url);
        this.channel = await this.connection.createChannel();
    }

    getChannel(): Channel {
        if (!this.channel) {
            throw new Error('RabbitMQ channel chua duoc khoi tao. Goi connect() truoc.');
        }
        return this.channel;
    }

    async close(): Promise<void> {
        if (this.channel) {
            await this.channel.close();
            this.channel = null;
        }
        if (this.connection) {
            await this.connection.close();
            this.connection = null;
        }
    }
}
