export interface User{
    id: number,
    username: string,
    email: string,
    lastLog?: Date,
    created: Date,
    chatsCount: number,
}
export interface Token {
    tokenContent: string,
}
export interface TokenContent{
    userId: number,
}
export interface UserChats {
    id: number,
    name: string,
}
export interface Login{
    email: string,
    password: string,
}
export interface Register{
    name: string,
    email: string,
    password: string,
    confirmedPassword: string,
}
