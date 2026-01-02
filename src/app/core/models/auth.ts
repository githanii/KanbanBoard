
export interface loginRequest {
  username: string;
  password: string;

}
export interface registerRequest {
  username: string;
  Email: string;
  password: string;
}

export interface authStatus {
  token: string;
  expiresAt?: string;
}   
