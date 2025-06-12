# Mini-Account-Management-System

## Project Setup Guideline

1. **Clone the Repository**  
   `https://github.com/habibor-rahaman1010/Mini-Account-Management-System/tree/main`

2. **Create a Database in SQL Server Management Studio (SSMS)**  
   - Open SSMS.  
   - Create a new database and name it **AccountManagement** .

3. **Execute the Database Script**  
   - Open the file **/SqlScripts/DatabaseSetup.sql**.  
   - Copy its entire SQL content.  
   - In SSMS, select the **AccountManagement** database, paste the script into a new query window, and **Execute**.

4. **Run the EF-Core Migration for ASP.NET Identity**  
   - In Visual Studio’s **Package Manager Console** (or any terminal), run:  
   
     "dotnet ef database update -c "ApplicationDbContext" -p "Account.Management.Web""
   
   - This command adds the seven built-in ASP.NET Identity tables to the same database.

That’s it—run the project and navigate using the navigation bar on the first page.

---

### Roles

| Role | Username | Password | Can Access |
|------|----------|----------|------------|
| **Admin** | `user.admin@gmail.com` | `admin123` | **AddUser**, **AddRole**, **AssignAccess**, **AssignUserRole** |
| **Accountant** | `user.accountant@gmail.com` | `accountant123` | **ChartOfAccounts**, **VoucherList** |
| **Viewer** | Any self-registered user | *(chosen at signup)* | VoucherList |

Newly registered users are automatically placed in the **Viewer** role.

### 1. Registration Page  
![Landing Page](ScreenShots/Screenshot-1.png)

### 2. Validation Registration Page  
![Landing Page](ScreenShots/Screenshot-2.png)

### 3. Login Page  
![Landing Page](ScreenShots/Screenshot-3.png)

### 4. Validation Login Page  
![Landing Page](ScreenShots/Screenshot-4.png)

### 5. After Login Home page  
![Landing Page](ScreenShots/Screenshot-5.png)

### 6. Dashboard Page  
![Landing Page](ScreenShots/Screenshot-6.png)

### 7. Users List  
![Landing Page](ScreenShots/Screenshot-7.png)

### 8. user Edit  
![Landing Page](ScreenShots/Screenshot-8.png)

### 9. User Delete  
![Landing Page](ScreenShots/Screenshot-9.png)

### 10. Role Create Page  
![Landing Page](ScreenShots/Screenshot-10.png)

### 11. Role List  
![Landing Page](ScreenShots/Screenshot-11.png)

### 12. Edit Role  
![Landing Page](ScreenShots/Screenshot-12.png)

### 13. Change User Role 
![Landing Page](ScreenShots/Screenshot-13.png)

### 14. Create Chart Of Account  
![Landing Page](ScreenShots/Screenshot-14.png)

### 15. Chart Of Account  
![Landing Page](ScreenShots/Screenshot-15.png)

### 16. Update Chart Of Account  
![Landing Page](ScreenShots/Screenshot-16.png)

### 17. Voucher Type List
![Landing Page](ScreenShots/Screenshot-17.png)

### 18. Create Voucher Type  
![Landing Page](ScreenShots/Screenshot-18.png)

### 19. Create Voucher Type  
![Landing Page](ScreenShots/Screenshot-19.png)

### 20. Create Voucher Type  
![Landing Page](ScreenShots/Screenshot-20.png)

### 21. Create Voucher Type  
![Landing Page](ScreenShots/Screenshot-21.png)

### 22. Create Voucher Type  
![Landing Page](ScreenShots/Screenshot-22.png)

### 23. Create Voucher Type  
![Landing Page](ScreenShots/Screenshot-23.png)

### 24. Create Voucher Type  
![Landing Page](ScreenShots/Screenshot-24.png)
