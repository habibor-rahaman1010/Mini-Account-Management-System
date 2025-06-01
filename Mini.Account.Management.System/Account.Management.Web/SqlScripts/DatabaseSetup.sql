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

CREATE PROCEDURE sp_ManageChartOfAccounts
    @Action NVARCHAR(10),
    @Id UNIQUEIDENTIFIER,
    @AccountName NVARCHAR(100) = NULL,
    @Code NVARCHAR(50) = NULL,
    @Description NVARCHAR(200) = NULL,
    @AccountType NVARCHAR(50) = NULL,
    @IsActive BIT = 1,
    @ParentId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'CREATE'
    BEGIN
        INSERT INTO ChartOfAccounts (
            Id,
            AccountName,
            Code,
            Description,
            AccountType,
            IsActive,
            ParentId,
            CreatedDate
        )
        VALUES (
            @Id,
            @AccountName,
            @Code,
            @Description,
            @AccountType,
            @IsActive,
            @ParentId,
            GETDATE()
        );
    END

    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE ChartOfAccounts
        SET
            AccountName = @AccountName,
            Code = @Code,
            Description = @Description,
            AccountType = @AccountType,
            IsActive = @IsActive,
            ParentId = @ParentId,
            ModifiedDate = GETDATE()
        WHERE Id = @Id;
    END

    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM ChartOfAccounts WHERE Id = @Id;
    END
END;
