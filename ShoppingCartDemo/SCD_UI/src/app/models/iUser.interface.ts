export interface iUserProfile {
  fullName: string;
  email: string;
  roleName: string | string[];
  address: string;
  city: string;
  state: string;
  zip: string;
}

export interface iLoggedInProfile extends iUserProfile  {
  id: string;
}
export interface iUser extends iLoggedInProfile {
  
  password: string;
  isActive: boolean;
}

export interface iRegistrationRequest extends iUserProfile { 
  password: string;
}
