import { Channel } from 'amqplib';
import { RabbitMqConnection } from './rabbitmqConnection';

export interface IMessagePublisher {
    publishToQueue<T>(queueName: string, message: T): void;
}

export class RabbitMqPublisher implements IMessagePublisher {
    private channel: Channel;

    constructor(connection: RabbitMqConnection) {
        this.channel = connection.getChannel();
    }

    publishToQueue<T>(queueName: string, message: T): void {
        this.channel.assertQueue(queueName, { durable: true });
        const messageBuffer = Buffer.from(JSON.stringify(message));
        this.channel.sendToQueue(queueName, messageBuffer, { persistent: true });
    }
}

