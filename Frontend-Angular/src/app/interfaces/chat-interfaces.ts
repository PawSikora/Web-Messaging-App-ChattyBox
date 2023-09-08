import { Message } from "./message-interfaces";
import { User } from "./users-interfaces";

export interface Chat {
    name:string,
    chatId: number,
    users: User[],
    allMessages?: Message[],
}