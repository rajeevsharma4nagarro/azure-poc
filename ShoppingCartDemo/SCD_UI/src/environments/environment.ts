export const environment = {
  production: false,

  
  /*local without API gateway

  authApiUrl: 'http://localhost:7002/api',
  productApiUrl: 'http://localhost:7003/api',
  cartApiUrl: 'http://localhost:7004/api',
  orderApiUrl: 'http://localhost:7005/api'
  */

  /*local with api gateway
  
  authApiUrl: 'http://localhost:7002/api',
  productApiUrl: 'http://localhost:7000/api',
  cartApiUrl: 'http://localhost:7000/api',
  orderApiUrl: 'http://localhost:7000/api'
  */

  authApiUrl: 'https://scd-as-auth-f9h3btb6e8gfbuah.centralindia-01.azurewebsites.net/api', 
  productApiUrl: 'http://localhost:7000/api',
  cartApiUrl: 'http://localhost:7000/api',
  orderApiUrl: 'http://localhost:7000/api'
};
