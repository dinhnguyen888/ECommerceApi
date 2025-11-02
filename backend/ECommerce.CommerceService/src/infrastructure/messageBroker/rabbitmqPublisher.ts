import { Channel } from 'amqplib';
import { RabbitMqConnection } from './rabbitmqConnection';

export interface IMessagePublisher {
    publish<T>(exchange: string, routingKey: string, message: T): Promise<void>;
    publishToQueue<T>(queueName: string, message: T): Promise<void>;
}

export class RabbitMqPublisher implements IMessagePublisher {
    private channel: Channel;

    constructor(connection: RabbitMqConnection) {
        this.channel = connection.getChannel();
    }

    async publish<T>(exchange: string, routingKey: string, message: T): Promise<void> {
        await this.channel.assertExchange(exchange, 'topic', { durable: true });

        const content = Buffer.from(JSON.stringify(message));
        this.channel.publish(exchange, routingKey, content, { persistent: true });
    }

    async publishToQueue<T>(queueName: string, message: T): Promise<void> {
        await this.channel.assertQueue(queueName, { durable: true });

        const content = Buffer.from(JSON.stringify(message));
        this.channel.sendToQueue(queueName, content, { persistent: true });
    }
}
