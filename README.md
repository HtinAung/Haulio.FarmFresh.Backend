# Haulio.FarmFresh.Backend



Here what i have finished:
- Customer API = Used to get products, purchased and also view order histories
- Store API = Used to create store, manage store, add products, update products, delete products and view other people's order histories
- IdentityServer4 = Used to authenticate and authorize the flows
- Ocelot API Gateway = Acts as api gateway for both api endpoints
- Customer Web App = The UI for Customer API.
- (not yet implement) => Store Web App

I also have created default account for testing:
- Customer Account: (ghulamcyber@hotmail.com, password: @Future30)
-  Store Account: (raraanjani@gmail.com, password: @Future30)

The stack i used:
- Apis: .NET Core 5 
- IdentityServer4: .NET Core 5
- Api Gateway: Ocelot .NET Core 5
- Web App (Customer): ASP.NET Core 5 with JQuery and Bootstrap
- Database: SQL Server
- EF Strategy: Code First
- Storage: Azure Blob Storage

## Startup Project Configuration

![Startup](https://raw.githubusercontent.com/mirzaevolution/Haulio.FarmFresh.Backend/master/Screenshots/2022-01-25_09h49_22.png)

## DB Location
Here's the bacpac file that can be used to restore DB (if you don't want to run migration script).
[DB Bacpac.](https://github.com/mirzaevolution/Haulio.FarmFresh.Backend/tree/master/DB%20BackUp)




## Architecture

![enter image description here](https://raw.githubusercontent.com/mirzaevolution/Haulio.FarmFresh.Backend/master/Screenshots/2022-01-25_09h54_011.png)


## Storage & Logging
The storage used for uploading product image in Store API uses Azure Blob Storage. This storage is also the place for holding the log data (information and error) for the applications. You can see the blob storage connection string in the appsettings.json (in real life, i don't put sensitive informations in appsettings.json).

## API Swagger
### NB: To test the swagger apis, please click the Authorize button, click checklist for the scope and for login, please use account info above. And if you want to logout, please click the authorize button again, click logout and the go to identity server 4 and click logout (you don't need to do this if you use web app - it will be automatically logout from all sessions)


![enter image description here](https://raw.githubusercontent.com/mirzaevolution/Haulio.FarmFresh.Backend/master/Screenshots/2022-01-25_09h50_021.png)

![enter image description here](https://github.com/mirzaevolution/Haulio.FarmFresh.Backend/blob/master/Screenshots/2022-01-25_09h50_061.png?raw=true)

## Identity Server
![enter image description here](https://raw.githubusercontent.com/mirzaevolution/Haulio.FarmFresh.Backend/master/Screenshots/2022-01-25_09h50_151.png)


## Web App
![enter image description here](https://raw.githubusercontent.com/mirzaevolution/Haulio.FarmFresh.Backend/master/Screenshots/2022-01-25_09h46_04.png)


![enter image description here](https://raw.githubusercontent.com/mirzaevolution/Haulio.FarmFresh.Backend/master/Screenshots/2022-01-25_09h46_14.png)

![enter image description here](https://raw.githubusercontent.com/mirzaevolution/Haulio.FarmFresh.Backend/master/Screenshots/2022-01-25_09h48_33.png)



## NB: All Apis are working properly but i haven't finished all the functionalities for the Web App either the Customer Web App or Store Web App
