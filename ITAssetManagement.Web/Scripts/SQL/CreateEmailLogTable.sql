-- Email Log tablosunu oluştur
CREATE TABLE [dbo].[EmailLogs] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ToEmail] NVARCHAR(255) NOT NULL,
    [Subject] NVARCHAR(255) NOT NULL,
    [Body] NVARCHAR(MAX) NULL,
    [SentDate] DATETIME2 NOT NULL,
    [IsSuccess] BIT NOT NULL,
    [ErrorMessage] NVARCHAR(MAX) NULL,
    [RelatedEntityType] NVARCHAR(50) NULL,
    [RelatedEntityId] INT NULL,
    [RetryCount] INT NULL,
    CONSTRAINT [PK_EmailLogs] PRIMARY KEY ([Id])
);