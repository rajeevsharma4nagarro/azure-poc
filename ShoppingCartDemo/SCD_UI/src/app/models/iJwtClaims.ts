export interface iJwtClaims {
  Name: string;
  sub: string;
  email: string;
  Address: string;
  City: string;
  State: string;
  Zip: string;
  FullName: string;
  RoleName: string | string[];
}
