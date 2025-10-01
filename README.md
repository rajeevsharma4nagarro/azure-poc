# Shopping Cart Applicattion

A multi-service user management application built with below technologies:
├── Angular 20.*.* (Frontend UI)
├── Ocelot ApiGateway to as single entry point for all Backend REST API
├── .net(8.0) Core AuthAPI (Backend REST API) + Azure SQL Database (featured with Microsoft Asp Net Core Identity)
├── .net(8.0) Core OrderAPI (Backend REST API) + Azure SQL Database
├── .net(8.0) Core CartAPI (Backend REST API) + Azure SQL Database
├── .net(8.0) Core OrderAPI (Backend REST API) + Azure SQL Database
├── .net Library as MessageBus to Send messages to Azure Message Bus queue 
└── Azure Function ServiceBusTrigger  to  read and process message from  azure message bus queue
	
# Project Structure
project-root/
├── AzureResources-Screen-Shots folder containing all needed screen shots
├── GitActionWorkflows folder containing all yaml file used  in github actions for azure deployment and also it copy code from git to azure devops  repo
├── ShoppingCartDemo is .net soltuion root folder
├── Project-Workflow.png high level workflow of shopping cart  solution
├── AzureResources-Screen-Shots\VisualDiagram-of-all-resources.jpg (All Azure Resource Visual  Diagram)
├── README.md	
└── ShoppingCart-Azure-POC-Basic.pdf detailed about  project

# Project Features

Frontend (Angular Tier)
├── Sign up: page (unique email id required, rest all fields are prefild  to save time)
├── Sign In: page with JWT token storage (prefilled with customer@nagarro.com / Pass@123 as login as customer, admin@nagarro.com / Pass@123 as admin )
├── Product Dashboard (MyShop): feature to add products in cart
├── View Cart: 
├── Checkout:
├── My Orders:
├── Authentication guard (CanActivate)
├── JWT Interceptor to append request header with Authorized attribute to secure API calls
└── Responsive styling with bootstrap

API Gateway
├── Upstream url will be http://localhost:4200
├── Gateway base url http://localhost:7000
└── Downstream url be
    ├── For Product: http://localhost:7003
    ├── For Cart: http://localhost:7004
    └── For Orders: http://localhost:7005

.Net Core Api (SCD.Services.AuthAPI)
├── Developed using .Net Core Web App (net 8.0)
├── Will be expose on http://localhost:4200
└── Key Action Methods:
    ├── Register
    └── Login

.Net Core Api (SCD.Services.ProductAPI)
├── Developed using .Net Core Web App (net 8.0)
├── Will be expose on http://localhost:7003
├── SQL database for data
├── Azure blob storage container for product images
└── Key Action Methods:
    ├── Get (get all products)
    ├── Get/{id} (get product by id)
    └── Post(Add products) for ADMIN role only

.Net Core Api (SCD.Services.CartAPI)
├── Developed using .Net Core Web App (net 8.0)
├── Will be expose on http://localhost:7004
├── SQL database for data
└── Key Action Methods:
    ├── CartUpsert (Add/Update Items in Cart)
    ├── RemoveCart (Delete items from cart)
    ├── GetCart/{UserId} (Get cart by userid)
    └── ClearCart (Clear cart)

.Net Core Api (SCD.Services.OrderAPI)
├── Developed using .Net Core Web App (net 8.0)
├── Will be expose on http://localhost:7005
├── SQL database for data
└── Key Action Methods:
    ├── CreateOrder (Place a order for cart items)
    ├── GetOrders/{Userid} (Get all orders list for logged in user)
    ├── GetPendingOrders(List all newly created orders) for ADMIN role only
    └── UpdateOrderStatus(Admin can approve/reject order) for ADMIN role only

.Net Library (SCD.MessageBus)
├── Utility library to send message in Azure bus Queue
└── This library is reference from CartApi and OrderApi send message in queue

Azure Function App (SCD.EmailProcessorFunction)
├── This is ServiceBusTrigger azure function to read messages from azure message bus queue
├── Once we receive any messages, we create a mail template (to, subject, body) and send message to Azure Logic App to send mail
└── Also send a new message to read BlobStorageQueue if admin as approved this order, so that next order can be set as Delivered status. 

# GitHub public repositories
├── https://github.com/rajeevsharma4nagarro/azure-poc
└── https://github.com/rajeevsharma4nagarro/azure-poc.git


# Project Setup on devlopment environment

Prerequisites
├── Angular CLI: 20
├── Node: 22
├── Package Manager: npm 10
├── Visual Studio 
└── SQL Server
Note: configurtation file's sample is added in Configuration-Sample/for all the apis need to append  there values

# Screenshots of the application in action
├── WebSite-pages/1-SCD-Register.png
├── WebSite-pages/2-SCD-Login.png
├── WebSite-pages/3-SCD-admin-dashboard.png
├── WebSite-pages/4-SCD-admin-add-product.png
├── WebSite-pages/5-SCD-admin-pending-orders.png
├── WebSite-pages/6-SCD-Dashboard-and-cart.png
├── WebSite-pages/7-SCD-checkout.png
└── WebSite-pages/8-SCD-customer-myorders.png

