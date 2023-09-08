export abstract class Message {
    id: number;
    chatId: number;
    senderId: number;
    timeStamp: Date;
    abstract messageType: string;
    constructor(id: number, chatId: number, senderId: number, timeStamp: Date, messageType: string) {
        this.id = id;
        this.chatId = chatId;
        this.senderId = senderId;
        this.timeStamp = timeStamp;
    }
}

export interface GetNewsestMessagesText {
    senderId: number,
    content: string,
    senderName:string;
}

export interface TextMessage extends Message {
    content: string,
}

export interface FileMessage extends Message {
    path: string,
    name: string,
    fileType: string,
}