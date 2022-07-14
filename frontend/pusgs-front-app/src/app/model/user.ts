export interface UserForRegister {
    username: string;
    email?: string;
    password1: string;
    password2: string;
    name: string;
    surname: string;
    dateOfBirth: Date;
    address: string;
    userType: string;
    image: string;
}

export interface UserForLogin {
  email: string;
  password: string;
}

export interface User {
    username: string;
    email?: string;
    name: string;
    surname: string;
    dateOfBirth: Date;
    address: string;
    userType: string;
    image: string;
    isVerified: number;
}

export interface PasswordChange {
  currentPassword: string;
  newPassword: string;
}
