IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        AccountID int NOT NULL,
        Login nvarchar(50) NOT NULL,
        PasswordHash char(64) NOT NULL,
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSDATETIME(),
        CONSTRAINT UQ_Users_Login UNIQUE (Login),
        CONSTRAINT UQ_Users_AccountID UNIQUE (AccountID),
        CONSTRAINT FK_Users_Accounts FOREIGN KEY (AccountID) REFERENCES dbo.Accounts(AccountID),
        CONSTRAINT CK_Users_Login CHECK (LEN(LTRIM(RTRIM(Login))) BETWEEN 3 AND 50),
        CONSTRAINT CK_Users_PasswordHash CHECK (PasswordHash NOT LIKE '%[^0-9A-F]%')
    );
END;
GO
