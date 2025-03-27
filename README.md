# Employee Management System

## Database Schema 
- Employee Table
- Admin Table
- Timesheet Table
- Department Table
- Leave Table
### Database Relationships
- One-to-Many: Department (1) → (M) Employee
- One-to-Many: Employee (1) → (M) Timesheet
- Independent Entity: Admin has no direct relationship with other tables.

## User
- **Employee** 
- **Admin** 

### Employee 
- Login
- Reset Password
- Forgot Password
- View/Update Employee details
- Start/End Daily Timmer
- Add/View/Update Timesheet
- Total Logged Hours (week/month)
- Add/View Leave

### Admin 
- Login
- Reset Password
- Forgot Password
- Add Admin
- Add Department
- Add Employee
- View all Employee
- View/Update Employee details
- Activate/Deactivate Employee
- Add/View/Update/Delete Timesheet
- Export Timesheet as Excel
- Timesheet Analytics
- Total Logged Hours (week/month)
- View Pwnding Leave
- Approve/Reject Leave
- Get Employee on Leave
- Employee Leave Remaining

## Technical Architecture

- **Multi-Tiered Architecture:** Separates concerns into presentation, business, and data access layers to improve maintainability and scalability.
- **.NET Core API:** Ensures a performant, cross-platform solution.
- **Entity Framework (Code-First):** Simplifies database design and management while ensuring data integrity.
- **JWT Authentication:** Implements secure login and authorization for both employees and admins.
- **RESTful API Principles:** The API uses proper HTTP status codes and adheres to REST best practices.

## API Endpoints

This section provides an overview of the key API endpoints for the Employee Management System. The endpoints are grouped by functionality.

---

### 1. Authentication Endpoints

#### Login
- **Method:** POST  
- **URL:** `/api/Auth/login`  
- **Access:** Public  
- **Description:** Authenticates a user and returns a JWT token on successful login.  
- **Sample Request Body:**
  ```json
  {
    "email": "user@example.com",
    "password": "yourpassword"
  }
  ```
- **Responses:**
  - **200 OK:** Returns a JWT token.
  - **401 Unauthorized:** Invalid credentials.

#### Reset Password (Logged In)
- **Method:** PUT  
- **URL:** `/api/Auth/resetPassword`  
- **Access:** Authenticated  
- **Description:** Resets the password for the authenticated user.  
- **Sample Request Body:**
  ```json
  {
    "oldPassword": "oldPassword",
    "newPassword": "newPassword"
  }
  ```
- **Responses:**
  - **200 OK:** Password reset confirmation.
  - **400 Bad Request:** Password reset fail.
  - **401 Unauthorized:** Invalid credentials.
  - **500 Internal Server Error:** 

#### Forgot Password (Not Logged In)
- **Method:** PUT  
- **URL:** `/api/Auth/request-password-reset`  
- **Access:** Public  
- **Description:** Initiates a password reset for users who are not logged in.  
- **Sample Request Body:**
  ```json
  {
  "email": "m@gmail.com"
  }
  ```
- **Responses:**
  - **200 OK:** Instructions sent to email.
  - **404 NotFound:** User not found.
  - **500 Internal Server Error:** 

#### Verify Token & Reset Password
- **Method:** PUT  
- **URL:** `/api/Auth/reset-password`  
- **Access:** Public  
- **Description:** Verifies a one-time password (OTP) and, if valid, resets the password.  
- **Sample Request Body:**
  ```json
  {
    "token": "cd98b73f-393c-4d39-aed5-9ae63a543d60",
    "newPassword": "newPassword"
  }
  ```
- **Responses:**
  - **200 OK:** Password reset success.
  - **400 Bad Request:** Invalid Token.
---

### 2. Admin Endpoints
*All admin endpoints require authentication with an admin role.*

#### Add Admin
- **Method:** POST  
- **URL:** `/api/Admin/addAdmin`  
- **Description:** Adds a new admin user.  
- **Sample Request Body:**
  ```json
  {
    "firstName": "Meghal",
    "lastName": "Shah",
    "email": "m@gmail.com",
    "phoneNumber": "999999999",
    "password": "string"
  }
  ```
- **Responses:**
  - **201 Created:** Admin created successfully.
  - **400 Bad Request:** Invalid data.
  - **500 Internal Server Error:** 

#### Add Department
- **Method:** POST  
- **URL:** `/api/Admin/addDepartment`  
- **Description:** Add Department
- **Sample Request Body:**
  ```json
  {
    "departmentName": "IT"
  }
  ```
- **Responses:**
  - **201 Created:** Department created successfully.
  - **400 Bad Request:** Invalid data.
  - **500 Internal Server Error:** 

#### Add Employee
- **Method:** POST  
- **URL:** `/api/Admin/addEmployee`  
- **Description:** Add Employee.  
- **Sample Request Body:**
  ```json
  {
    "firstName": "Meghal",
    "lastName": "Shah",
    "email": "s@gmail.com",
    "password": "string",
    "dateofBirth": "2025-03-27",
    "phoneNumber": "999999999",
    "address": "Vadodara",
    "departmentID": 1,
    "techStack": ".NET"
  }
  ```
- **Responses:**
  - **200 OK:** Employee Created.
  - **400 Bad Request:** Invalid data.
  - **500 Internal Server Error:** 

#### Get All Employee Detail
- **Method:** GET  
- **URL:** `/api/Admin/viewAllEmployeeDetails`  
- **Description:** Retrieves detailed information about all employee.  
- **Responses:**
  - **200 OK:** All Employee details returned.
  - **404 NotFound:** User not found.
  - **500 Internal Server Error:** 

#### Get Employee Details
- **Method:** GET  
- **URL:** `/api/Admin/employeeDetails/{id}`  
- **Description:** Get Employee Details using id.  
- **Responses:**
  - **200 OK:** Employee details returned.
  - **400 Bad Request:** Invalid data.
  - **404 NotFound:** Employee not found.
  - **500 Internal Server Error:** 

#### Update Employee Details
- **Method:** PUT
- **URL:** `/api/Admin/updateEmployeeDetails/{id}`  
- **Description:** Update Employee Details.  
- **Sample Request Body:**
  ```json
  {
    "phoneNumber": "999999999",
    "techStack": ".NET",
    "address": "Vadodara"
  }
  ```
- **Responses:**
  - **200 OK:** Employee details updated.
  - **400 Bad Request:** Invalid data.
  - **404 NotFound:** Employee not found.
  - **500 Internal Server Error:** 

#### Deactivate Employee
- **Method:** DELETE 
- **URL:** `/api/Admin/deactivateEmployee/{id}`  
- **Description:** Deactivate Employee.  
- **Responses:**
  - **200 OK:** Employee Deactivated.
  - **400 Bad Request:** Invalid data.
  - **404 NotFound:** Employee not found.
  - **500 Internal Server Error:** 

#### Activate Employee
- **Method:** PUT
- **URL:** `/api/Admin/activateEmployee/{id}`  
- **Description:** Deactivate Employee.  
- **Responses:**
  - **200 OK:** Employee Activated.
  - **400 Bad Request:** Invalid data.
  - **404 NotFound:** Employee not found.
  - **500 Internal Server Error:** 
---

### 3. Employee Endpoints
*All endpoints require user authentication.*

#### View Employee Detail
- **Method:** GET  
- **URL:** `/api/Employee/employeeDetails`  
- **Description:** View Employee Detail.  
- **Sample Request Body:**
- **Responses:**
  - **200 OK:** Employee details sent.
  - **401 Unauthorized:** Invalid credentials.
  - **404 NotFound:** Employee not found.
  - **500 Internal Server Error:** 

#### Update Employee
- **Method:** PUT  
- **URL:** `/api/Employee/updateEmployeeDetails`  
- **Description:** Updates an existing employee’s profile.  
- **Sample Request Body:**
  ```json
  {
    "phoneNumber": "999999999",
    "techStack": ".NET",
    "address": "Vadodara"
  }
  ```
- **Responses:**
  - **200 OK:** Employee details updated.
  - **400 Bad Request:** Invalid data.
  - **404 NotFound:** Employee not found.
  - **500 Internal Server Error:** 
---

### 4. Timesheet Endpoints
*All endpoints require user authentication.*

#### Start Timmer
- **Method:** POST
- **URL:** `/api/Timesheet/employee/startTimer`  
- **Description:** Start Timmer.  
- **Responses:**
  - **200 OK:** Timmer started.
  - **400 Bad Request:** Invalid data.
  - **401 Unauthorized:** Invalid credentials.
  - **409 Conflict:** Timesheet already exist.
  - **500 Internal Server Error:** 

#### End Timmer
- **Method:** PUT  
- **URL:** `/api/Timesheet/employee/endTimer`  
- **Description:** End Timmer.  
  ```json
  {
    "description": "string"
  }
  ```
- **Responses:**
  - **200 OK:** Timmer end.
  - **400 Bad Request:** Invalid data.
  - **401 Unauthorized:** Invalid credentials.
  - **404 NotFound:** Timesheet doe not exist.
  - **500 Internal Server Error:** 

#### Add Timmesheet
- **Method:** POST  
- **URL:** `/api/Timesheet/employee/addTimesheet`  
- **Description:** Add Timmesheet.  
  ```json
  {
    "date": "2025-03-27",
    "startTime": "9:00",
    "endTime": "18:00",
    "description": "string"
  }
  ```
- **Responses:**
  - **200 OK:** Timmer added.
  - **400 Bad Request:** Invalid data/ timesheet exist.
  - **401 Unauthorized:** Invalid credentials.
  - **500 Internal Server Error:**

#### View Timmesheet
- **Method:** GET  
- **URL:** `/api/Timesheet/employee/viewTimesheet/{date}`  
- **Description:** View Timmesheet. 
- **Responses:**
  - **200 OK:** Timmesheet sent.
  - **401 Unauthorized:** Invalid credentials.
  - **404 NotFound:** Timesheet does not exist.
  - **500 Internal Server Error:**

#### View Timmesheet Pagination
- **Method:** GET  
- **URL:** `/api/Timesheet/employee/viewTimesheets`
- **Description:** View Timmesheet Pagination.   
- **Responses:**
  - **200 OK:** Timmesheet sent.
  - **401 Unauthorized:** Invalid credentials.
  - **404 NotFound:** Timesheet doe not exist.
  - **500 Internal Server Error:**

#### Update Timmesheet
- **Method:** PUT 
- **URL:** `/api/Timesheet/employee/updateTimesheet`
- **Description:** Update Timmesheet.  
  ```json
  {
    "date": "2025-03-27",
    "startTime": "9:00",
    "endTime": "18:00",
    "description": "string"
  }
  ```
- **Responses:**
  - **200 OK:** Timmesheet sent.
  - **400 Bad Request:** Invalid data/ timesheet exist.
  - **500 Internal Server Error:**

#### Add Timesheet
- **Method:** POST
- **URL:** `/api/Timesheet/admin/addTimesheet/{id}`
- **Description:** Add Timmesheet.  
  ```json
  {
    "date": "2025-03-27",
    "startTime": "9:00",
    "endTime": "18:00",
    "description": "string"
  }
  ```
- **Responses:**
  - **200 OK:** Timmer added.
  - **400 Bad Request:** Invalid data/ timesheet exist.
  - **404 NotFound:** Timesheet doe not exist.
  - **500 Internal Server Error:**

#### View Timesheet
- **Method:** GET
- **URL:** `/api/Timesheet/admin/viewTimesheet/{id}/{date}`
- **Description:** View Timmesheet.  
- **Responses:**
  - **200 OK:** Timmesheet sent.
  - **404 NotFound:** Timesheet does not exist.
  - **500 Internal Server Error:**

#### View Timesheet
- **Method:** GET
- **URL:** `/api/Timesheet/admin/viewTimesheets/{id}`
- **Description:** View Timmesheet Pagination.  
- **Responses:**
  - **200 OK:** Timmer added.
  - **400 Bad Request:** Invalid data/ timesheet exist.
  - **404 NotFound:** Timesheet doe not exist.
  - **500 Internal Server Error:**

#### Update Timesheet
- **Method:** PUT
- **URL:** `/api/Timesheet/admin/updateTimesheet/{id}`
- **Description:** Update Timmesheet.  
  ```json
  {
    "date": "2025-03-27",
    "startTime": "9:00",
    "endTime": "18:00",
    "description": "string"
  }
  ```
- **Responses:**
  - **200 OK:** Timmer added.
  - **400 Bad Request:** Invalid data/ timesheet exist.
  - **500 Internal Server Error:**

#### DELETE Timesheet
- **Method:** DELETE
- **URL:** `/api/Timesheet/admin/deleteTimesheet/{id}/{date}`
- **Description:** DELETE Timmesheet.  
- **Responses:**
  - **200 OK:** Timmer added.
  - **404 NotFound:** Timesheet doe not exist.  
  - **500 Internal Server Error:**
---
