# Technical Test: HR Medical Records Management System

## Objective
Develop a medical records management system for the HR department by implementing a RESTful API that allows management of employee medical records.

## Mandatory Technical Requirements
- .NET 8
- PostgreSQL **(local environment)**
- Entity Framework Core (Database First, Fluent API)
- AutoMapper
- Repository Pattern
- Service Pattern
- Swagger for API documentation
- FluentValidation

## Database Structure

### Database Name: **RRHH_DB**

### Table: T_MEDICAL_RECORD
```sql
CREATE TABLE T_MEDICAL_RECORD (
    MEDICAL_RECORD_ID SERIAL PRIMARY KEY,
    AUDIOMETRY VARCHAR(2),
    POSITION_CHANGE VARCHAR(2),
    MOTHER_DATA VARCHAR(2000),
    DIAGNOSIS VARCHAR(100),
    OTHER_FAMILY_DATA VARCHAR(2000),
    FATHER_DATA VARCHAR(2000),
    EXECUTE_MICROS VARCHAR(2),
    EXECUTE_EXTRA VARCHAR(2),
    VOICE_EVALUATION VARCHAR(2),
    DELETION_DATE DATE,
    CREATION_DATE DATE,
    MODIFICATION_DATE DATE,
    END_DATE DATE,
    START_DATE DATE,
    STATUS_ID INTEGER,
    MEDICAL_RECORD_TYPE_ID INTEGER,
    DISABILITY VARCHAR(2),
    MEDICAL_BOARD VARCHAR(200),
    DELETION_REASON VARCHAR(2000),
    OBSERVATIONS VARCHAR(2000),
    DISABILITY_PERCENTAGE NUMERIC(10),
    DELETED_BY VARCHAR(2000),
    CREATED_BY VARCHAR(2000),
    MODIFIED_BY VARCHAR(2000),
    AREA_CHANGE VARCHAR(2)
);

CREATE TABLE STATUS (
    STATUS_ID SERIAL PRIMARY KEY,
    NAME VARCHAR(100),
    DESCRIPTION VARCHAR(500)
);

CREATE TABLE MEDICAL_RECORD_TYPE (
    MEDICAL_RECORD_TYPE_ID SERIAL PRIMARY KEY,
    NAME VARCHAR(100),
    DESCRIPTION VARCHAR(500)
);

ALTER TABLE T_MEDICAL_RECORD
ADD CONSTRAINT FK_STATUS_ID_RECORD
FOREIGN KEY (STATUS_ID) REFERENCES STATUS(STATUS_ID);

ALTER TABLE T_MEDICAL_RECORD
ADD CONSTRAINT FK_MEDICAL_RECORD_TYPE
FOREIGN KEY (MEDICAL_RECORD_TYPE_ID) REFERENCES MEDICAL_RECORD_TYPE(MEDICAL_RECORD_TYPE_ID);

-- Initial test data
INSERT INTO STATUS (NAME, DESCRIPTION) VALUES 
('Active', 'Active medical record'),
('Inactive', 'Inactive medical record');

INSERT INTO MEDICAL_RECORD_TYPE (NAME, DESCRIPTION) VALUES 
('Regular', 'Regular medical record'),
('Special', 'Special medical record');
```

## Functional Requirements

### 1. Endpoints to Implement
- **GetFilterMedicalRecords**: List of medical records with pagination and filters. It should be possible to filter by STATUS_ID (optional), START_DATE (optional), END_DATE (optional), and MEDICAL_RECORD_TYPE_ID (optional).
    Page and PageSize are mandatory parameters.
- **GetMedicalRecordById**: Retrieve medical record by ID. Get a more detailed and formatted description of the medical record.
- **AddMedicalRecord**: Create new medical record. Mandatory fields must be validated and comply with validation rules.
- **UpdateMedicalRecord**: Update existing medical record. Mandatory fields must be validated and comply with validation rules.
- **DeleteMedicalRecord**: Delete medical record (logical deletion)

### 2. Required Validations

#### 2.1 Date Controls
- START_DATE cannot be later than END_DATE
- START_DATE cannot be a future date
- If END_DATE exists, it must be later than START_DATE
- CREATION_DATE is mandatory and must be auto-generated when inserting the record

#### 2.2 Required Fields
The following fields are mandatory:
- DIAGNOSIS
- START_DATE
- STATUS_ID
- MEDICAL_RECORD_TYPE_ID
- FILE_ID
- CREATED_BY

#### 2.3 Related Records Validation
- STATUS_ID must exist in the STATUS table
- MEDICAL_RECORD_TYPE_ID must exist in the MEDICAL_RECORD_TYPE table
- A record cannot be deleted without providing DELETION_REASON
- When creating or modifying a record, the status must be valid according to these rules:
  * Cannot assign 'Inactive' status when creating a new record
  * To change to 'Inactive' status, DELETION_REASON must be provided

#### 2.4 Maximum Length Validation
- DIAGNOSIS: maximum 100 characters
- MOTHER_DATA: maximum 2000 characters
- FATHER_DATA: maximum 2000 characters
- OTHER_FAMILY_DATA: maximum 2000 characters
- MEDICAL_BOARD: maximum 200 characters
- DELETION_REASON: maximum 2000 characters
- OBSERVATIONS: maximum 2000 characters
- Two-character fields (must be 'YES' or 'NO'):
  * AUDIOMETRY
  * POSITION_CHANGE
  * EXECUTE_MICROS
  * EXECUTE_EXTRA
  * VOICE_EVALUATION
  * DISABILITY
  * AREA_CHANGE

#### 2.5 Valid Status Control
Allowed statuses and their rules:
1. Active (ID: 1)
   - Default initial status for new records
   - Allows modification of all fields
   - Requires CREATED_BY

2. Inactive (ID: 2)
   - Requires DELETION_REASON
   - Requires END_DATE
   - Requires DELETED_BY
   - Does not allow subsequent modifications
   - Must record DELETION_DATE

#### 2.6 Additional Validation Rules
- DISABILITY_PERCENTAGE must be between 0 and 100 when DISABILITY = 'YES'
- If POSITION_CHANGE = 'YES', OBSERVATIONS field is mandatory
- If END_DATE exists, the record must change to Inactive status
- CREATED_BY, MODIFIED_BY, and DELETED_BY must record the user performing the operation
- MODIFICATION_DATE must be automatically updated when modifying any field

### 3. Response Handling

#### 3.1 Base Response Model
```csharp
public class BaseResponse<T>
{
    public bool? Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? Code { get; set; }
    public int? TotalRows { get; set; }
    public string? Exception { get; set; }
}
```

#### 3.2 HTTP Response Codes
- 200 OK: Successful request (GET, PUT/PATCH)
- 400 Bad Request: Validation errors
- 404 Not Found: Resource not found
- 500 Internal Server Error: Unhandled errors

## Deliverables
1. Git repository with complete source code
2. Installation and execution instructions in README

## Evaluation Criteria
- Code structure and organization
- Correct implementation of requested patterns
- Validation and exception handling
- Proper use of AutoMapper
- Clean and properly commented code
- Correct application of Gitflow (Main -> Development -> Feature)

## Delivery Time
- 5 business days from test receipt
- Delivery will be confirmed through creation of a Release PR to main

## Installation Instructions
The candidate must provide clear instructions for:
1. Local PostgreSQL installation
2. Database script execution
3. Running migration with database-first approach using EF with Fluent API
4. Project configuration

## Extras (Not Mandatory)
- Docker

## Delivery Format
- Git repository (GitHub, GitLab, Bitbucket)
- Repository must include this README updated with specific installation instructions for your implementation

---
*Note: For any questions or clarifications about requirements, please create an issue in the repository.*
