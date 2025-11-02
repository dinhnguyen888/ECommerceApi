import { Channel, ConsumeMessage } from 'amqplib';
import { RabbitMqConnection } from './rabbitmqConnection';

export interface IMessageConsumer {
    consume<T>(queueName: string, onMessage: (message: T) => Promise<void> | void): Promise<void>;
}

export class RabbitMqConsumer implements IMessageConsumer {
    private channel: Channel;

    constructor(connection: RabbitMqConnection) {
        this.channel = connection.getChannel();
    }

    async consume<T>(queueName: string, onMessage: (message: T) => Promise<void> | void): Promise<void> {
        await this.channel.assertQueue(queueName, { durable: true });

        this.channel.consume(queueName, async (msg: ConsumeMessage | null) => {
            if (msg) {
                try {
                    const content = JSON.parse(msg.content.toString()) as T;
                    await onMessage(content);
                    this.channel.ack(msg);
                } catch (error) {
                    console.error('Loi xu ly message:', error);
                    this.channel.nack(msg, false, true);
                }
            }
        });
    }
}
