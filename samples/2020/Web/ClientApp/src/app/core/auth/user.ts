export interface User {
    id: number;
    userName: string;
    firstName: string;
    lastName: string;
    password: string;
    token: string;
    role: string;
    tokenExpired: number;
    returnUrl: string;
}
