--This is ChatOfAccounts Table and stored procedure
CREATE TABLE ChartOfAccounts (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    AccountName NVARCHAR(100) NOT NULL,
    Code NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200),
    AccountType NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    ParentId UNIQUEIDENTIFIER NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);
GO

CREATE PROCEDURE sp_CreateChartOfAccount
    @AccountName NVARCHAR(100),
    @Code NVARCHAR(50),
    @Description NVARCHAR(200),
    @AccountType NVARCHAR(50),
    @ParentId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ChartOfAccounts (Id, AccountName, Code, Description, AccountType, ParentId, CreatedDate)
    VALUES (NEWID(), @AccountName, @Code, @Description, @AccountType, @ParentId, GETDATE());
END;

