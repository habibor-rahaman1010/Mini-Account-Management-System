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

    CONSTRAINT FK_ChartOfAccounts_ParentId
        FOREIGN KEY (ParentId)
        REFERENCES ChartOfAccounts(Id)
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


-- Voucher Entry Module Tables

CREATE TABLE VoucherTypes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    TypeName NVARCHAR(50) NOT NULL
);

CREATE TABLE Vouchers (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    VoucherDate DATE NOT NULL,
    ReferenceNo NVARCHAR(100),
    VoucherTypeId UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (VoucherTypeId) REFERENCES VoucherTypes(Id)
);

CREATE TABLE VoucherEntries (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    VoucherId UNIQUEIDENTIFIER NOT NULL,
    AccountId UNIQUEIDENTIFIER NOT NULL, -- ChartOfAccounts.Id
    DebitAmount DECIMAL(18, 2) DEFAULT 0,
    CreditAmount DECIMAL(18, 2) DEFAULT 0,
    Narration NVARCHAR(255),

    FOREIGN KEY (VoucherId) REFERENCES Vouchers(Id),
    FOREIGN KEY (AccountId) REFERENCES ChartOfAccounts(Id)
);
GO

-- stored procedure of VoucherTypes
ALTER PROCEDURE sp_Manage_VoucherType
    @Action NVARCHAR(10),
    @Id UNIQUEIDENTIFIER = NULL,
    @TypeName NVARCHAR(50) = NULL,
    @PageNumber INT = NULL,
    @PageSize INT = NULL,
    @TotalCount INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'CREATE'
    BEGIN
        INSERT INTO VoucherTypes (Id, TypeName)
        VALUES (@Id, @TypeName);
    END

    ELSE IF @Action = 'READ'
    BEGIN
        -- Total Count ber kora
        SELECT @TotalCount = COUNT(*) FROM VoucherTypes;

        -- Pagination apply kore data fetch
        DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize;

        SELECT Id, TypeName
        FROM VoucherTypes
        ORDER BY TypeName
        OFFSET @StartRow ROWS
        FETCH NEXT @PageSize ROWS ONLY;
    END

    ELSE IF @Action = 'READBYID'
    BEGIN
        SELECT Id, TypeName
        FROM VoucherTypes
        WHERE Id = @Id;
    END

    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE VoucherTypes
        SET TypeName = @TypeName
        WHERE Id = @Id;
    END

    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM VoucherTypes
        WHERE Id = @Id;
    END
END;
GO
